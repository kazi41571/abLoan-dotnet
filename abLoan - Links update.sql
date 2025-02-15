-- CUSTOMER
ALTER TABLE loanCustomerMaster DISABLE TRIGGER loanCustomerMaster_TR
GO

/*
SELECT *
FROM loanCustomerMaster
WHERE IdNo IN (SELECT LTRIM(STR([الهوية], 25, 0)) FROM urlid4) 
*/

UPDATE loanCustomerMaster
SET Links = (SELECT TRIM([الرابط])
            FROM urlid4
            WHERE LTRIM(STR([الهوية], 25, 0)) = IdNo)
WHERE IdNo IN (SELECT LTRIM(STR([الهوية], 25, 0)) FROM urlid4)
GO

ALTER TABLE loanCustomerMaster ENABLE TRIGGER loanCustomerMaster_TR
GO


-- CONTRACT
ALTER TABLE loanContractMaster DISABLE TRIGGER loanContractMaster_TR
GO

/*
SELECT *
FROM loanContractMaster
WHERE ContractTitle IN (SELECT TRIM([ Contract]) FROM urlid5)
*/

UPDATE loanContractMaster
SET Links = (SELECT TRIM([Contract Links])
            FROM urlid5
            WHERE TRIM([ Contract]) = ContractTitle)
WHERE ContractTitle IN (SELECT TRIM([ Contract]) FROM urlid5)
GO

ALTER TABLE loanContractMaster ENABLE TRIGGER loanContractMaster_TR
GO