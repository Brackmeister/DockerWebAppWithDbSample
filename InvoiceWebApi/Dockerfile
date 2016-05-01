FROM microsoft/aspnet
WORKDIR /src
COPY ./ ./
RUN dnu restore
ENTRYPOINT ["dnx", "web"]
