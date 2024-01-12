
create table `applications` (
	`Id` int not null auto_increment,
	`Name` varchar(100) not null,
	constraint uc_applications_name unique (`Name`),
	primary key (`Id`)
) engine=innodb;

create table `categories` (
	`Id` int not null auto_increment,
	`Name` varchar(100) not null,
	constraint uc_categories_name unique (`Name`),
	primary key (`Id`)
)engine=innodb;

create table `years` (
	`Id` int not null auto_increment,
	`Name` varchar(20) not null,
	constraint uc_years_name unique (`Name`),
	primary key (`Id`)
)engine=innodb;

create table `courses` (
	`Id` int not null auto_increment,
	`Name` varchar(100) not null,
	constraint uc_courses_name unique (`Name`),
	primary key (`Id`)
)engine=innodb;

create table `sections` (
	`Id` int not null auto_increment,
	`Name` varchar(20) not null,
	primary key (`Id`)
)engine=innodb;

create table `colleges` (
    `Id` int not null auto_increment,
    `Name` varchar(100) not null,
	constraint uc_colleges_name unique (`Name`),
    primary key (`Id`)
)  engine=innodb;

create table `users` (
	`Id` int not null auto_increment,
	`Username` varchar(50) not null,
	`FirstName` varchar(100) not null,
	`LastName` varchar(100) not null,
	`MiddleName` varchar(100) not null,
	`PasswordHash` varchar(100) not null,
	`CreatedDate` timestamp not null,
    `IsActive` tinyint not null,
	key `idx_users_username` (`username`) using btree,
	constraint uc_users_username unique (`Username`),
	primary key (`id`)
) engine=innodb;


create table `members` (
	`Id` int not null auto_increment,
	`IdNo` varchar(50) not null,
	`FirstName` varchar(100) not null,
	`MiddleName` varchar(100) not null,
	`LastName` varchar(100) not null,
	`Age` int not null,

	`Picture` varchar(100) null,
	`MimeType` varchar(50) null,

	`Section` varchar(20) not null,

	`CategoryId` int not null,
	`CollegeId` int not null,
	`CourseId` int not null,
	`YearId` int not null,

	`Remarks` varchar(300) null,

    `IsActive` tinyint not null,
	`PasswordHash` varchar(100) not null,
	`CreatedDate` timestamp not null,

	constraint `uc_members_idno` unique (`IdNo`),
	foreign key (`CategoryId`) references `categories`(`Id`),
	foreign key (`CollegeId`) references `colleges`(`Id`),
	foreign key (`CourseId`) references `courses`(`Id`),
	foreign key (`YearId`) references `years`(`Id`),

	primary key (`Id`)
) engine=innodb;

create table `member_credits` (
	`Id` int not null auto_increment,
	`MemberId` int not null,
	`Credit` int not null,
	`CreatedDate` timestamp not null,
	foreign key (`MemberId`) references `members`(`Id`),
	primary key (`Id`)
) engine=innodb;

create table `member_sessions` (
	`Id` int not null auto_increment,
	`MemberId` int not null,
	`StartTime` timestamp not null,
	`EndTime` timestamp null,
	foreign key (`MemberId`) references `members`(`Id`),
	primary key (`Id`)
) engine=innodb;


create table `session_applications` (
	`Id` int not null auto_increment,
	`SessionId` int not null,
	`ApplicationId` int not null,
	foreign key (`SessionId`) references `member_sessions`(`Id`),
	foreign key (`ApplicationId`) references `applications`(`Id`),
	primary key (`Id`)
) engine=innodb;

DROP USER if exists 'ibsuser';

CREATE USER 'ibsuser' IDENTIFIED BY 'tz2YyXE?e&366sXJ';


GRANT INSERT, UPDATE, DELETE, SELECT, DROP, CREATE, ALTER, REFERENCES on ibs_connect.* TO 'ibsuser' WITH GRANT OPTION;


insert into `users` (
	`Username`, 
	`FirstName`, 
	`LastName`,
	`MiddleName`,
	`PasswordHash`,
	`CreatedDate`,
    `IsActive`
)
values (
	'admin',
	'Admin',
	'',
	'',
	'$2a$11$5L4RoZ9ZOJttNtHVh7BhBeXaDQruhfZ2CwmTQmGAVReS4LlAiHLSe',
	now(),
	1
)

#Username: admin
#Password: password123