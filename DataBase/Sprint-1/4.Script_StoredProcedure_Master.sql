use bbqn;
DELIMITER $$
DROP PROCEDURE IF EXISTS GetMasterData;
CREATE PROCEDURE GetMasterData()
BEGIN
SELECT CompanyID,CompanyName,CompanyDescription FROM bbqn.Company;
SELECT RegionID,RegionName FROM bbqn.Regions;
SELECT DepartmentID,DepartmentName,DepartmentDescription FROM bbqn.Departments;
SELECT GradeID,GradeCode,GradeDescription FROM bbqn.Grade;
SELECT RoleID,RoleName,Description FROM bbqn.UserRole;
SELECT EmployeeCategoryID,Name,Description FROM bbqn.EmployeeCategory;
SELECT DivisionID,Name,Description FROM bbqn.Divisions;

END$$
DELIMITER ;

use bbqn;
DELIMITER $$
DROP PROCEDURE IF EXISTS GetSubDepartmentsMasterData;
CREATE PROCEDURE GetSubDepartmentsMasterData(in DepartmentIDval int)
BEGIN
SELECT SubDepartmentID,SubDepartmentName,SubDepartmentDescription,DepartmentID FROM bbqn.SubDepartments
WHERE DepartmentID=DepartmentIDval;
     
END$$
DELIMITER ;

use bbqn;
DELIMITER $$
DROP PROCEDURE IF EXISTS GetOutletsMasterData;
CREATE PROCEDURE GetOutletsMasterData(in RegionID int)
BEGIN
SELECT ol.OutletID,ol.OutletName FROM bbqn.Outlets ol
WHERE ol.RegionID=RegionID;
     
END$$
DELIMITER ;