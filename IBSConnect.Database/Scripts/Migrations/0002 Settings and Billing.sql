create table `settings` (
	`Id` int not null auto_increment,
	`Name` varchar(100) not null,
	`Value` varchar(100) null,
	constraint `uc_settings_name` unique (`Name`),
	primary key (`Id`)
) engine=innodb;

insert into settings (`Name`, `Value`)
values ('Rate', '0.50'), ('DefaultTime', '1200'), ('DefaultPassword', 'password123');

create table member_payments (
	`Id` int not null auto_increment,
	`MemberId` int not null,
	`Minutes` int not null,
	`Rate` decimal(5,2) not null,
	`Amount` decimal(7,2) not null,
	`UserId` int not null,
	`CreatedDate` timestamp not null,
	foreign key (`MemberId`) references `members`(`Id`),
	foreign key (`UserId`) references `users`(`Id`),
	primary key (`Id`)
) engine=innodb;


GRANT INSERT, UPDATE, DELETE, SELECT, DROP, CREATE, ALTER, REFERENCES on ibs_connect.* TO 'ibsuser' WITH GRANT OPTION;

