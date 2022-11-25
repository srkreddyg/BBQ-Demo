/* CreateGroupUser*/

use bbqn;
DELIMITER $$
DROP PROCEDURE IF EXISTS CreateGroupUser;
CREATE PROCEDURE CreateGroupUser(IN CreateGroupUser Json, In GroupIDval INT,OUT flag bool)

Begin 
	DECLARE EXIT HANDLER FOR SQLEXCEPTION 
    BEGIN
		SET flag = false;
		ROLLBACK;
    END;    
    
       Start transaction;
    
		SELECT json_extract(CreateGroupUser,'$') into @User_data;
		SELECT json_length(@User_data) into @Userdata_length;
        SELECT json_extract(CreateGroupUser,'$') as '' ;
	
      set @count=0;
        while @count< @Userdata_length do
	    select json_extract(@User_data,concat('$[',@count,']')) into @User_row;
        select json_extract(@User_row,'$.UserID') as '';
        INSERT INTO bbqn.users (UserID,UserLoginID,FirstName,LastName,Gender,CompanyID ,EmailID ,JoiningDate ,LeavingDate,
        MobileNumber ,IsBlock ,IsSuperAdmin ,IsAdmin ,IsActiveStatus ,OutletID ,GradeID ,RoleID ,
        RegionID ,DepartmentID ,SubDepartmentID ,EmployeeCategoryID,DivisionID ,LastModifiedBy,LastModifiedDate) 
			VALUES (json_extract(@User_row,'$.UserID'),json_extract(@User_row,'$.UserLoginID'),json_extract(@User_row,'$.FirstName'),
                    json_extract(@User_row,'$.LastName'),json_extract(@User_row,'$.Gender'),json_extract(@User_row,'$.CompanyID'),
					json_extract(@User_row,'$.EmailID'),CAST(json_extract(@User_row,'$.JoiningDate')AS DATE),CAST(json_extract(@User_row,'$.LeavingDate')AS DATE),
					json_extract(@User_row,'$.MobileNumber'),CASE WHEN json_extract(@User_row,'$.IsBlock') = true
                            THEN 1
                            ELSE 2
                            END,CASE WHEN json_extract(@User_row,'$.IsSuperAdmin') = true
                            THEN 1
                            ELSE 2
                            END,CASE WHEN json_extract(@User_row,'$.IsAdmin') = true
                            THEN 1
                            ELSE 2
                            END,CASE WHEN json_extract(@User_row,'$.IsActiveStatus') = true
                            THEN 1
                            ELSE 2
                            END,
					json_extract(@User_row,'$.OutletID'), json_extract(@User_row,'$.GradeID'),
					json_extract(@User_row,'$.RoleID'),json_extract(@User_row,'$.RegionID'),
					json_extract(@User_row,'$.DepartmentID'),json_extract(@User_row,'$.SubDepartmentID'),
					json_extract(@User_row,'$.EmployeeCategoryID'), json_extract(@User_row,'$.DivisionID'),						
					json_extract(@User_row,'$.LastModifiedBy'),now()); 
                    
				 INSERT INTO groupuser(GroupID,UserID,IsGroupUserDeleted,LastModifiedBy,LastModifiedDate) 
				values (GroupIDval,json_extract(@User_row,'$.UserID'),'0',1,now());
                    
			set @count=@count+1;
		end while;
        
commit;
    SET flag = true;
End $$
DELIMITER ;

----------------------------------------------------------------------------------------------------------------
/* RemoveExistUser*/

use bbqn;
DELIMITER $$
DROP PROCEDURE IF EXISTS RemoveExistUser;
CREATE PROCEDURE RemoveExistUser(in userID int,out isUserRemoved bool)
Begin 
DECLARE EXIT HANDLER FOR SQLEXCEPTION 
 BEGIN
		SET isUserRemoved = false;
		ROLLBACK;
    END;   
 Start transaction;
    UPDATE bbqn.Users SET IsActiveStatus='0',LastModifiedDate =now() where UserId=UserID;
	UPDATE bbqn.groupuser set IsGroupUserDeleted='1',LastModifiedDate =now()  where UserId=UserID;
commit;
 SET isUserRemoved = true;
End $$
DELIMITER ;

----------------------------------------------------------------------------------------------------------------
/* proc_cursor_to_loopAndInsert*/

use bbqn;
DELIMITER $$
CREATE  PROCEDURE proc_cursor_to_loopAndInsert(IN Doj Datetime)
BEGIN
  DECLARE CGroupID INT;
  DECLARE CGroupURL varchar(500);
  Declare CStartDate Datetime DEFAULT NULL;
  Declare CEndDate Datetime DEFAULT NULL;
  Declare CFilterOperator enum('0','1','2','3','4','5');
  DECLARE done INT DEFAULT FALSE;
  DECLARE cursor_GroupEnroll CURSOR FOR SELECT GroupID, GroupURL,StartDate,EndDate,FilterOperator FROM Group_Data;
  DECLARE CONTINUE HANDLER FOR NOT FOUND SET done = TRUE;
  OPEN cursor_GroupEnroll;
  loop_through_rows: LOOP
    FETCH cursor_GroupEnroll INTO CGroupID,CGroupURL,CStartDate,CEndDate,CFilterOperator;
    IF done THEN
      LEAVE loop_through_rows;
    END IF;
    IF(CFilterOperator='0') THEN
    INSERT INTO Group_Data_Enroll(GroupID, GroupURL,StartDate,EndDate,FilterOperator) VALUES(CGroupID,CGroupURL,CStartDate,CEndDate,CFilterOperator);
     END IF;
	IF(CFilterOperator='1' and Doj> CStartDate ) THEN
    INSERT INTO Group_Data_Enroll(GroupID, GroupURL,StartDate,EndDate,FilterOperator) VALUES(CGroupID,CGroupURL,CStartDate,CEndDate,CFilterOperator);
     END IF;

   IF(CFilterOperator='2' and (Doj< CStartDate) ) THEN
        Select CStartDate as '';
    INSERT INTO Group_Data_Enroll(GroupID, GroupURL,StartDate,EndDate,FilterOperator) VALUES(CGroupID,CGroupURL,CStartDate,CEndDate,CFilterOperator);
     END IF;
    IF(CFilterOperator='3' and (Doj>= CStartDate) ) THEN
    INSERT INTO Group_Data_Enroll(GroupID, GroupURL,StartDate,EndDate,FilterOperator) VALUES(CGroupID,CGroupURL,CStartDate,CEndDate,CFilterOperator);
     END IF;
     IF(CFilterOperator='4' and (Doj<= CStartDate) ) THEN
    INSERT INTO Group_Data_Enroll(GroupID, GroupURL,StartDate,EndDate,FilterOperator) VALUES(CGroupID,CGroupURL,CStartDate,CEndDate,CFilterOperator);
     END IF;
      IF(CFilterOperator='5' ) THEN
      IF Exists(Select * From Group_Data where ((Doj between StartDate and EndDate) and groupID=CGroupID) ) then
    INSERT INTO Group_Data_Enroll(GroupID, GroupURL,StartDate,EndDate,FilterOperator) VALUES(CGroupID,CGroupURL,CStartDate,CEndDate,CFilterOperator);
     END IF;
     End IF;
  END LOOP;
  CLOSE cursor_GroupEnroll;
END

----------------------------------------------------------------------------------------------------------------
/* GetMatchedGroupIds*/
use bbqn;
DELIMITER $$
DROP PROCEDURE IF EXISTS GetMatchedGroupIds;
CREATE PROCEDURE GetMatchedGroupIds(IN GetUserFilter Json)
Begin
DROP TABLE IF EXISTS `Group_Data_Enroll`;
 CREATE TEMPORARY TABLE Group_Data_Enroll(
	GroupID int NOT NULL ,
	GroupURL varchar(500) DEFAULT NULL,
	StartDate Datetime  NOT NULL,
	EndDate Datetime NULL,
	FilterOperator enum('0','1','2','3','4','5') Not NULL);
 DROP TABLE IF EXISTS `Group_Data`;
 CREATE TEMPORARY TABLE Group_Data(
	GroupID int NOT NULL ,
	GroupName varchar(100) NOT NULL,
	GroupDescription varchar(100) DEFAULT NULL,
	GroupURL varchar(500) DEFAULT NULL,
	StartDate Datetime  NOT NULL,
	EndDate Datetime NULL,
	FilterOperator enum('0','1','2','3','4','5') Not NULL ,
    CompanyID int NOT NULL,
	RoleID int NOT NULL,
	GradeID int NULL,
	RegionID int NOT NULL,
	OutletID int NOT NULL,
    DepartmentID int NOT NULL ,
    SubDepartmentID int NOT NULL ,
	EmployeeCategoryID int NOT NULL,
	DivisionID int not null,
    IsGroupDeleted ENUM('1', '0') NOT NULL DEFAULT '0',
    IsExEmployeesGroup ENUM('1', '0') NOT NULL DEFAULT '0',
	IsActiveStatus ENUM('1', '0') DEFAULT NULL,
	CreatedBy int NULL,
    LastModifiedBy int NOT NULL,
	LastModifiedDate Datetime NOT NULL DEFAULT CURRENT_TIMESTAMP);
    
  SELECT json_extract(GetUserFilter,'$') into @group_data;
 Select  CAST(json_extract(@group_data,'$.DateofJoining')AS DATE) into @DOJ;
  INSERT INTO Group_Data (
    SELECT g.GroupID,g.GroupName,g.GroupDescription,g.GroupURL,g.StartDate,g.EndDate,g.FilterOperator,g.CompanyID,g.RoleID,g.GradeID,g.RegionID,g.OutletID,
		g.DepartmentID,g.SubDepartmentID,g.EmployeeCategoryID,g.DivisionID,g.IsGroupDeleted,g.IsExEmployeesGroup,g.IsActiveStatus,
		g.CreatedBy,g.LastModifiedBy,g.LastModifiedDate  
		FROM BBQN.groups g
        WHERE g.CompanyID = CASE WHEN json_extract(@group_data,'$.CompanyID') = 0
                            THEN g.CompanyID
                            ELSE CAST(json_extract(@group_data,'$.CompanyID')AS UNSIGNED)
                            END
		    AND g.OutletID = CASE WHEN json_extract(@group_data,'$.OutletID') = 0
						 THEN g.OutletID
						 ELSE CAST(json_extract(@group_data,'$.OutletID')AS UNSIGNED)
						 END
                         AND g.GradeID = CASE WHEN json_extract(@group_data,'$.GradeID') = 0
						 THEN g.GradeID
						 ELSE CAST(json_extract(@group_data,'$.GradeID')AS UNSIGNED)
						 END
                         
		AND g.RoleID = CASE WHEN json_extract(@group_data,'$.RoleID') = 0
						 THEN g.RoleID
						 ELSE CAST(json_extract(@group_data,'$.RoleID')AS UNSIGNED)
						 END
                            
		AND g.RegionID = CASE WHEN json_extract(@group_data,'$.RegionID') = 0
						 THEN g.CompanyID
						 ELSE CAST(json_extract(@group_data,'$.RegionID')AS UNSIGNED)
						 END
                         
		AND g.DepartmentID = CASE WHEN json_extract(@group_data,'$.DepartmentID') = 0
						 THEN g.DepartmentID
						 ELSE CAST(json_extract(@group_data,'$.DepartmentID')AS UNSIGNED)
						 END
                         
		AND g.SubDepartmentID = CASE WHEN json_extract(@group_data,'$.SubDepartmentID') = 0
						 THEN g.SubDepartmentID
						 ELSE CAST(json_extract(@group_data,'$.SubDepartmentID')AS UNSIGNED)
						 END
			AND g.EmployeeCategoryID =CASE WHEN json_extract(@group_data,'$.EmployeeCategoryID') = 0
						 THEN g.EmployeeCategoryID
						 ELSE CAST(json_extract(@group_data,'$.EmployeeCategoryID')AS UNSIGNED)
						 END
                          
		AND g.DivisionID = CASE WHEN json_extract(@group_data,'$.DivisionID') = 0
						 THEN g.DivisionID
						 ELSE CAST(json_extract(@group_data,'$.DivisionID')AS UNSIGNED)
						 END
		AND g.IsAutoEnroled = '1' );
     
    CALL proc_cursor_to_loopAndInsert (@DOJ);
             
	Select * From Group_Data_Enroll;
    
END $$
DELIMITER ;

/* BGServiceAuditLog*/
use bbqn
DELIMITER $$
DROP PROCEDURE IF EXISTS BGServiceAuditLog;
CREATE PROCEDURE BGServiceAuditLog(IN UserIDval int, IN Actiontype Varchar(50), IN Act varchar(1000), in EmployeeIDValue varchar(100))
BEGIN
DECLARE EXIT HANDLER FOR SQLEXCEPTION
    Declare uLogINID int;
     select  UserID into  uLogINID from BBQN.Users where UserLoginID=EmployeeIDValue limit 1;
	Select @urName := FirstName  From bbqn.users where UserID= UserIDval;
	set Act =@urName;
	Insert into BBQN.BGServiceAudit(UserID, ActionTypePerformed, ActionPerformed, LastModifiedBy) 
             Values(UserIDval,Actiontype,Act,uLogINID);
END $$
DELIMITER ;