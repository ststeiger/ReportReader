/*
DECLARE @in_mandant varchar(3) 
DECLARE @in_sprache varchar(2) 
DECLARE @in_gebaeude varchar(MAX)
DECLARE @in_geschoss varchar(MAX)
DECLARE @in_groups varchar(MAX) 
DECLARE @in_multisel int 
DECLARE @in_stichtag datetime 



SET @in_mandant = 0 
SET @in_sprache = 'DE'
SET @in_groups  = '0000'
SET @in_multisel = 1 
SET @in_stichtag = '10.12.2015'


DECLARE @in_raum nvarchar(MAX) 
SET @in_raum = N'DECLARE @tblWorkaround TABLE
(
  MultiSelectParameter uniqueidentifier  
) 

-- INSERT INTO @tblWorkaround(MultiSelectParameter) VALUES (''53694174-A9C2-4681-A3AF-143E170662FF''); 
-- INSERT INTO @tblWorkaround(MultiSelectParameter) VALUES (''19DD7DEC-8C05-4272-9E5D-1FA2230657DB''); 
-- INSERT INTO @tblWorkaround(MultiSelectParameter) VALUES (''1DE87CD0-5513-45B9-ABE2-4ACD6F5096E5''); 

SELECT CAST(GS_GB_UID AS varchar(36))  + ''_'' + CAST(GS_GST_UID AS varchar(36)) + ''_'' + RIGHT(''00'' + GS_Nr, 2) FROM T_AP_Geschoss
' 


SET @in_gebaeude = @in_raum
SET @in_geschoss = @in_raum
*/

DECLARE @tStandortkategorie GuidTableType 
DECLARE @tStandort GuidTableType 
DECLARE @tGebaeude GuidTableType 
DECLARE @tGeschoss GuidTableType 
DECLARE @tTrakt GuidTableType 
DECLARE @tNutzungsart GuidTableType 
DECLARE @tOrganisationseinheit GuidTableType 



INSERT INTO @tStandortkategorie(GUIDValue) 
EXECUTE(@in_standortkategorie) -- dynamic query 

INSERT INTO @tStandort(GUIDValue) 
EXECUTE(@in_standort) -- dynamic query 

INSERT INTO @tGebaeude(GUIDValue) 
EXECUTE(@in_gebaeude) -- dynamic query 

INSERT INTO @tGeschoss(GUIDValue) 
EXECUTE(@in_geschoss) -- dynamic query 

INSERT INTO @tTrakt(GUIDValue) 
EXECUTE(@in_trakt) -- dynamic query 

INSERT INTO @tNutzungsart(GUIDValue) 
EXECUTE(@in_nutzungsart) -- dynamic query 

INSERT INTO @tOrganisationseinheit(GUIDValue) 
EXECUTE(@in_oe_stufe1) -- dynamic query 


DECLARE @tRealGeschoss GuidTableType 
INSERT INTO @tRealGeschoss(GUIDValue) 
SELECT GS_UID 
FROM tfu_RPT_PARA_MSEL_GeschossUidTable(@tGeschoss, @in_stichtag) 
;


-- DECLARE @tGeschoss GeschossIdentifierTableType
-- INSERT INTO @tGeschoss(GeschossIdentifierValue) 

-- INSERT INTO @tGeschoss(GeschossIdentifierValue) 
-- EXECUTE(@in_geschoss) --dynamic query 






SELECT 
	 RPT_UID
	,RPT_Name
	,RPT_Sort
FROM tfu_RPT_MSEL_Trakt_Fast2(@in_mandant, @in_sprache, @tStandort, @tGebaeude, @tGeschoss, @in_groups, @in_multisel, @in_stichtag) 
ORDER BY RPT_Sort, RPT_Name 

-- ,@in_standort GUIDTableType readonly
-- ,@in_gebaeude GUIDTableType readonly
-- ,@in_geschoss GeschossIdentifierTableType readonly


