FROM microsoft/aspnet
WORKDIR /src/InvoiceWebApi
COPY ./InvoiceWebApi/ ./
RUN dnu restore
ENTRYPOINT ["dnx", "web"]
