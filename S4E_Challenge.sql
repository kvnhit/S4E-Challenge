create database S4E_Challenge_

create table T_ASSOC
(
	ID int primary key identity(1000, 1),
	A_NAME varchar(200) not null,
	A_CPF varchar(11) not null,
	A_BIRTH datetime,
	Constraint UQ_CPF unique (A_CPF)
)

create table T_COMP
(
	ID int primary key identity(2000, 1),
	C_NAME varchar(200) NOT NULL,
	C_CNPJ varchar(14) not null,
	Constraint UQ_CNPJ unique (C_CNPJ)
)

create table T_REL
(
	ID int primary key identity(100,1),
	A_ID int,
	C_ID int,
	Constraint FK_REL_ASSOC foreign key (A_ID) references T_ASSOC(ID),
	Constraint FK_REL_COMP foreign key (C_ID) references T_COMP(ID)
)
