FROM microsoft/aspnet
WORKDIR /src
COPY ./InvoiceWebApi/ ./
RUN dnu restore
ENTRYPOINT ["dnx", "web"]
