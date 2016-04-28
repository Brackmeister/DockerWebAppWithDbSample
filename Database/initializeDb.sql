create table Invoices (Id int primary key, Description varchar(100), Customer varchar(100) );
insert into Invoices values (1, 'Environment Test', 'Monsanto');
insert into Invoices values (2, 'Product Test', 'Mattel');
select * from Invoices;
