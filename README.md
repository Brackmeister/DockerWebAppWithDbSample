Description
===========

This is a (work in progress) example of and end to end docker infrastructure with Database, Web API (ASP.NET 5) and a Single Page Application as client.

How to run the example
----------------------

    # sensitive data is located in a file that needs to be sourced
    source environmentSettings.sh

    # create a volume to hold the data
    docker create -v /dbdata --name pgdata postgres /bin/true
    # create a container running the DB,
    # passing the $POSTGRES_PASSWORD environment variable defined in environmentSettings.sh
    docker run --name pgdb -e POSTGRES_PASSWORD -e PGDATA=/dbdata --volumes-from pgdata -d postgres
    # if you have psql on your machine, you can run it directly
    # otherwise run it from an interactive container
    docker run -it --link pgdb --rm -v $PWD/Database:/src postgres psql -h pgdb -p 5432 -U postgres

    ## inside psgl run the following line and exit again:
    \i /src/initializeDb.sql
    \q

    # while developing/debugging, run the web api interactively,
    # passing the $WEBAPI_CONNECTIONSTRING environment variable defined in environmentSettings.sh
    docker run -it --name webapi --link pgdb -v $PWD/InvoiceWebApi:/src -e WEBAPI_CONNECTIONSTRING -p 8080:5000 microsoft/aspnet /bin/bash

    ## inside the container change to the correct directory,
    ## do a package restore and start the web api
    cd src/
    dnu restore
    dnx web
 
    ## on your docker host check that it is working
    http://localhost:8080/api/invoices

    # when all is ok, create an image using the provided Dockerfile in the WebAPI folder
    ## the file should contain something like this
    FROM microsoft/aspnet
    WORKDIR /src
    COPY ./InvoiceWebApi/ ./
    RUN dnu restore
    ENTRYPOINT ["dnx", "web"]

    # on the docker host, build the image with
    docker build -t webapi InvoiceWebApi

    # then run your image,
    # passing the $WEBAPI_CONNECTIONSTRING environment variable defined in environmentSettings.sh
    docker run -d -p 8080:5000 --link pgdb -e WEBAPI_CONNECTIONSTRING --name webapi webapi

    # if the image doesn't work as expected, override the entrypoint of the Dockerfile
    # with an interactive shell and have a look what's wrong
    docker run -it -p 8080:5000 --link pgdb -e WEBAPI_CONNECTIONSTRING --name webapi --entrypoint=/bin/bash webapi

    # when the webapi image works, create the javascript files from typescript
    cd SinglePageApplication
    tsc invoicesController.ts app.ts
    cd ..

    # now, check that the Single Page Application runs correctly when run in an nginx container
    # use the sources from a mapped folder
    docker run -d -v $PWD/SinglePageApplication:/usr/share/nginx/html -p 8888:80 --name web nginx

    ## on your docker host check that it is working
    http://localhost:8888/
    
    # when all is ok, create an image using the provided Dockerfile in the SPA folder
    ## the file should contain something like this
    FROM nginx
    COPY ./index.html /usr/share/nginx/html/
    COPY ./*.js /usr/share/nginx/html/
    COPY ./*.css /usr/share/nginx/html/
    COPY ./images/ /usr/share/nginx/html/

    # on the docker host, build the image with
    docker build -t web SinglePageApplication
    
    # then, run the new image
    docker run -d -p 8888:80 --name web web

    ## on your docker host check that it is working
    http://localhost:8888/
    

The environmentSettings.sh file
-------------------------------

It has to look like this

    export POSTGRES_PASSWORD='<Choose a password here>'
    export WEBAPI_CONNECTIONSTRING="Host=pgdb;Username=postgres;Password=$POSTGRES_PASSWORD"
