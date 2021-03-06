
IF OBJECT_ID ( 'dbp_resto_add_customer', 'P' ) IS NOT NULL 
    DROP PROCEDURE dbp_resto_add_customer ;
GO
CREATE PROCEDURE dbp_resto_add_customer
				@pv_number VARCHAR(100)
				
AS 
SET NOCOUNT ON;

DECLARE @pv_insert_datetime DATETIME,
		@pv_insert_user		VARCHAR(20),
		@pv_insert_process	VARCHAR(20),
		@pv_first_name		VARCHAR(30),
		@pv_email			VARCHAR(30),
		@pv_dob				DATETIME,
		@unique_id			UNIQUEIDENTIFIER

SELECT	@pv_insert_datetime = GETDATE(),
		@pv_insert_user = HOST_NAME(),
		@pv_insert_process = 'WEBAPI',
		@pv_first_name = 'WEBAPI',
		@pv_email = 'webapi@resto.co.nz',
		@pv_dob	  = '19-OCT-2015',
		@unique_id	= CONVERT(UNIQUEIDENTIFIER, @pv_number)

IF ( Len(NULLIF(@pv_number,'')) < 8 OR @unique_id IS NULL)
BEGIN 
	DECLARE @ErrorMessage NVARCHAR(4000);
    DECLARE @ErrorSeverity INT;
    DECLARE @ErrorState INT;

SELECT @ErrorMessage = 'Phone Number or unique numbers is invalid',
	   @ErrorSeverity = 10,
	   @ErrorState  =1
RAISERROR (@ErrorMessage, -- Message text.
               @ErrorSeverity, -- Severity.
               @ErrorState -- State.
               );
	return 
END

IF NOT EXISTS 
(  SELECT 1 FROM customer WheRE unique_id = @unique_id)
BEGIN

INSERT INTO customer 
(		first_name,
		last_name,
		full_name,
		dob,
		email,
		phone_number,
		active,
		insert_datetime,
		insert_user,
		insert_process,
		unique_id)
SELECT	@pv_first_name,
		@pv_first_name,
		@pv_first_name,
		@pv_dob,
		@pv_email,
		'0800000000',
		'Y',
		@pv_insert_datetime,
		@pv_insert_user,
		@pv_insert_process,
		CONVERT(UniqueIDentifier,@pv_number)
END


