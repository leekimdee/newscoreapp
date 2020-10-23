ALTER PROC RemoveRolesOfUser
	@userId VARCHAR(250),--uniqueidentifier
	@roles NVARCHAR(MAX)
AS
BEGIN
		  DECLARE @MyCursor CURSOR;
		  DECLARE @MyField NVARCHAR(250);
		  DECLARE @roleId VARCHAR(250);

		  SET @MyCursor = CURSOR FOR
          select value from STRING_SPLIT(@roles, ',')

		  OPEN @MyCursor 
          FETCH NEXT FROM @MyCursor 
          INTO @MyField

		  WHILE @@FETCH_STATUS = 0
		  BEGIN
			set @roleId = (select Id from AppRoles where Name = @MyField)

		  	delete AppUserRoles where UserId = @userId and RoleId = @roleId

		  	FETCH NEXT FROM @MyCursor 
		  	INTO @MyField 
		  END; 
		  
		  CLOSE @MyCursor ;
		  DEALLOCATE @MyCursor;
END

--EXEC dbo.RemoveRolesOfUser @userId = '63ec39a6-80d9-4f39-755a-08d84a5e63a1', @roles = 'Customer'