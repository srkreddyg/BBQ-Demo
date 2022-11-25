CREATE TABLE BBQN.PrivilegesUserAudit(
	AuditID int NOT NULL Auto_Increment,
	UserID int NOT NULL,
    ActionTypePerformed ENUM('PrivilegeRemoved','PrivilegeAdded', 'PrivilegeUpdated') NOT NULL,
	ActionPerformed varchar(10000) NOT NULL,
	LastModifiedBy int NOT NULL,
	LastModifiedDate Datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
	PRIMARY KEY(AuditID),
	FOREIGN KEY (UserID) references BBQN.users(UserID)
);

CREATE TABLE BBQN.BGServiceAudit(
	AuditID int NOT NULL Auto_Increment,
	UserID int NOT NULL,
    ActionTypePerformed ENUM('UserAdded', 'UserDeleted') NOT NULL,
	ActionPerformed varchar(10000) NOT NULL,
	LastModifiedBy int NOT NULL,
	LastModifiedDate Datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
	PRIMARY KEY(AuditID),
	FOREIGN KEY (UserID) references BBQN.users(UserID)
);