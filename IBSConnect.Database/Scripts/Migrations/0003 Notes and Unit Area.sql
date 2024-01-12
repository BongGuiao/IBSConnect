alter table `members`
add `Notes` varchar(8000) null;


create table `unit_areas` (
	`Id` int not null auto_increment,
	`Name` varchar(100) not null,
	constraint uc_unitareas_name unique (`Name`),
	primary key (`Id`)
) engine=innodb;

alter table `member_sessions`
add `UnitAreaId` int null,
add foreign key (`UnitAreaId`) references `unit_areas`(`Id`);


alter table `categories`
add `IsFreeTier` tinyint(1) not null;

