/* UserPriviledgeAuditLog*/
use bbqn
DELIMITER $$
DROP PROCEDURE IF EXISTS UserPriviledgeAuditLog;
CREATE PROCEDURE UserPriviledgeAuditLog(IN UserIDval int, IN Actiontype Varchar(50), IN Act varchar(1000), in EmployeeIDValue varchar(100))
BEGIN
Declare uLogINID int;
     select  UserID into  uLogINID from BBQN.Users where UserLoginID=EmployeeIDValue limit 1;
	Select @urName := FirstName  From bbqn.users where UserID= UserIDval;
	set Act =@urName;
	Insert into BBQN.PrivilegesUserAudit(UserID, ActionTypePerformed, ActionPerformed, LastModifiedBy) 
             Values(UserIDval,Actiontype,Act,uLogINID);
END $$
DELIMITER ;

/* GetAllPrivileges*/
use bbqn;
DELIMITER $$
DROP PROCEDURE IF EXISTS GetAllPrivileges;
CREATE PROCEDURE GetAllPrivileges()
BEGIN
SELECT PrivilegeID,PrivilegeUniqueKey,Description
    FROM bbqn.privileges     
    Order by PrivilegeID asc;    
END$$
DELIMITER ;;

/* GetAllAdmins*/
use bbqn;
DELIMITER $$
DROP PROCEDURE IF EXISTS GetAllAdmins;
CREATE PROCEDURE GetAllAdmins()
BEGIN
SELECT u.UserID,u.UserLoginID,u.FirstName,u.LastName,u.Gender,u.CompanyID,u.EmailID,u.MobileNumber,u.IsBlock,u.IsSuperAdmin,u.IsPrivilegedUser,
u.IsAdmin,u.IsActiveStatus,u.RegionID,u.OutletID,u.GradeId, u.RoleID,u.DepartmentID,u.SubDepartmentID,u.EmployeeCategoryID,u.DivisionID
    FROM bbqn.users u
    where u.IsAdmin='1'
    Order by u.FirstName asc;    
END$$
DELIMITER ;

/* GetUser*/
use bbqn;
DELIMITER $$
DROP PROCEDURE IF EXISTS GetUser;
CREATE PROCEDURE GetUser(in UserID int)
BEGIN
SELECT u.UserID,U.UserLoginID ,u.FirstName ,U.LastName ,u.CompanyID,u.EmailID,u.JoiningDate,u.LeavingDate,u.MobileNumber,u.IsBlock,u.IsAdmin,u.IsSuperAdmin,u.IsPrivilegedUser ,u.IsActiveStatus,
o.OutletName,gr.GradeDescription,r.RoleName,reg.RegionName,dept.DepartmentName,sub.SubDepartmentName,emp.Name as EmployeeCategoryName ,d.Name as DivisionName,u.LastModifiedBy,u.LastModifiedDate
    FROM BBQN.users u
    Left join bbqn.outlets o on o.OutletID=u.OutletID
    Left join bbqn.grade gr on gr.GradeID=u.GradeID
    Left join bbqn.userrole r on r.RoleID=u.RoleID
    Left join bbqn.regions reg on reg.RegionID=u.RegionID
    Left join bbqn.departments dept on dept.DepartmentID=u.DepartmentID
    Left join bbqn.subdepartments sub on sub.SubDepartmentID=u.SubDepartmentID
    Left join bbqn.employeecategory emp on emp.EmployeeCategoryID=u.EmployeeCategoryID
	Left join bbqn.divisions d on d.DivisionID=u.DivisionID
    where u.UserID=UserID 
	Order by u.FirstName asc; 		
END$$
DELIMITER ;

/* PrivilegeRightsToAdmin*/
use bbqn;
DELIMITER $$
DROP PROCEDURE IF EXISTS PrivilegeRightsToAdmin;
create procedure BBQN.PrivilegeRightsToAdmin(IN PrivilegeIDsval varchar(10),IN UserIDval int,in EmployeeIDValue varchar(100),OUT flag int)
BEGIN
	Declare uLogINID int;
     select  UserID into  uLogINID from BBQN.Users where UserLoginID=EmployeeIDValue limit 1;
    BEGIN
		SET flag = 0;
		ROLLBACK;
    END;
	 Start transaction;
		IF EXISTS (SELECT * FROM BBQN.privilegesuser WHERE UserID=UserIDval) THEN
				BEGIN
					UPDATE BBQN.privilegesuser SET PrivilegeIDs = PrivilegeIDsval, UserID = UserIDval,LastModifiedBy=uLogINID ,LastModifiedDate=now() WHERE UserID=UserIDval;
					Set Flag = 2;
				END;
		ELSE 
				BEGIN
				Insert into BBQN.privilegesuser (PrivilegeIDs,UserID,CreatedBy,LastModifiedBy,LastModifiedDate) values(PrivilegeIDsval,UserIDval,1,uLogINID,now());
                Set Flag = 1;
                END;
		END IF;
  commit;
End $$
DELIMITER ;

/* update admin*/
use bbqn;
DELIMITER $$
DROP PROCEDURE IF EXISTS UpdateAdmin;
create procedure BBQN.UpdateAdmin(in AdminUsers json, OUT flag bool)
Begin	 
	DECLARE EXIT HANDLER FOR SQLEXCEPTION
     BEGIN
	 SET flag = false;
	 ROLLBACK;
     END;  

     Start transaction;

        SELECT json_extract(AdminUsers,'$.userOperations') into @userOperations_data;

        SELECT json_length(@userOperations_data) into @userOperations_length;

        SELECT json_length(@userOperations_data) as '';      

        set @count=0;

        while @count< @userOperations_length do
		SELECT json_extract(@userOperations_data,concat('$[',@count,'].UserID')) as '';
		SELECT json_extract(@userOperations_data,concat('$[',@count,'].action')) into @userOperations_action;
		SELECT json_extract(@userOperations_data,concat('$[',@count,'].UserID')) into @userOperations_user_row;
		if(@userOperations_action = 1) then
		UPDATE bbqn.users SET IsAdmin = '1' WHERE UserID=@userOperations_user_row;
		ELSE
		UPDATE bbqn.users SET IsAdmin = '0' WHERE UserID=@userOperations_user_row;
		End IF;
		set @count=@count+1;
		end while;
commit;
       SET flag = true;
End $$
DELIMITER ;


use bbqn
DELIMITER $$
DROP PROCEDURE IF EXISTS PrivilegeUser;
create procedure BBQN.PrivilegeUser(IN UserIDval int, in isPrivilege bool,in EmployeeIDValue varchar(100),OUT flag bool)
Begin
	Declare uLogINID int;
     select  UserID into  uLogINID from BBQN.Users where UserLoginID=EmployeeIDValue limit 1;
    BEGIN
		SET flag = false;
		ROLLBACK;
    END;
    Start transaction;
    if(isPrivilege=1) then
                UPDATE BBQN.users SET IsPrivilegedUser = '1' , LastModifiedBy=uLogINID ,LastModifiedDate=now() WHERE UserID=UserIDval;
            ELSE
				UPDATE BBQN.users SET IsPrivilegedUser = '0' , LastModifiedBy=uLogINID ,LastModifiedDate=now() WHERE UserID=UserIDval;
			End IF;
   			
	 commit;
    SET flag = true;
End $$
DELIMITER ;
use bbqn;
DELIMITER $$
DROP PROCEDURE IF EXISTS BlokUbblokUser;
create procedure BBQN.BlokUbblokUser(in AdminUsers json,in EmployeeIDValue varchar(100), OUT flag bool)
Begin	 
	Declare uLogINID int;
    select  UserID into  uLogINID from BBQN.Users where UserLoginID=EmployeeIDValue limit 1;
     BEGIN
	 SET flag = false;
	 ROLLBACK;
     END;  

     Start transaction;

        SELECT json_extract(AdminUsers,'$.userOperations') into @userOperations_data;

        SELECT json_length(@userOperations_data) into @userOperations_length;

        SELECT json_length(@userOperations_data) as '';      

        set @count=0;

        while @count< @userOperations_length do
		SELECT json_extract(@userOperations_data,concat('$[',@count,'].UserID')) as '';
		SELECT json_extract(@userOperations_data,concat('$[',@count,'].action')) into @userOperations_action;
		SELECT json_extract(@userOperations_data,concat('$[',@count,'].UserID')) into @userOperations_user_row;
		if(@userOperations_action = 1) then
		UPDATE bbqn.users SET IsBlock = '1' , LastModifiedBy=uLogINID ,LastModifiedDate=now() WHERE UserID=@userOperations_user_row;
		ELSE
		UPDATE bbqn.users SET IsBlock = '0' , LastModifiedBy=uLogINID ,LastModifiedDate=now() WHERE UserID=@userOperations_user_row;
		End IF;
		set @count=@count+1;
		end while;
commit;
       SET flag = true;
End $$
DELIMITER ;

