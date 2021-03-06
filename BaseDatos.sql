USE [TestBanco]
GO
/****** Object:  Table [dbo].[Cliente]    Script Date: 29/3/2022 23:09:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cliente](
	[clienteId] [int] IDENTITY(1,1) NOT NULL,
	[documentoIdentificacion] [varchar](25) NOT NULL,
	[contrasena] [varchar](50) NOT NULL,
	[estado] [bit] NOT NULL,
 CONSTRAINT [PK_Cliente] PRIMARY KEY CLUSTERED 
(
	[clienteId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Cuenta]    Script Date: 29/3/2022 23:09:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cuenta](
	[idCuenta] [int] IDENTITY(1,1) NOT NULL,
	[clienteId] [int] NOT NULL,
	[numeroCuenta] [varchar](25) NOT NULL,
	[tipoCuenta] [varchar](1) NOT NULL,
	[saldoInicial] [decimal](18, 2) NOT NULL,
	[estadoCuenta] [int] NOT NULL,
 CONSTRAINT [PK_Cuenta] PRIMARY KEY CLUSTERED 
(
	[idCuenta] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Movimiento]    Script Date: 29/3/2022 23:09:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Movimiento](
	[idMovimiento] [int] IDENTITY(1,1) NOT NULL,
	[idCuenta] [int] NOT NULL,
	[fechaMovimiento] [datetime] NOT NULL,
	[tipoMovimiento] [varchar](10) NOT NULL,
	[saldoMovimiento] [decimal](18, 2) NOT NULL,
	[valorMovimiento] [decimal](18, 2) NOT NULL,
	[saldoDisponibleMovimiento] [decimal](18, 2) NOT NULL,
	[descripcionMovimiento] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Movimiento_1] PRIMARY KEY CLUSTERED 
(
	[idMovimiento] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Persona]    Script Date: 29/3/2022 23:09:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Persona](
	[documentoIdentificacion] [varchar](25) NOT NULL,
	[nombreCompleto] [varchar](50) NOT NULL,
	[genero] [varchar](1) NOT NULL,
	[edad] [int] NOT NULL,
	[direccion] [varchar](255) NOT NULL,
	[telefono] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Persona] PRIMARY KEY CLUSTERED 
(
	[documentoIdentificacion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Cliente]  WITH CHECK ADD  CONSTRAINT [FK_Cliente_Persona] FOREIGN KEY([documentoIdentificacion])
REFERENCES [dbo].[Persona] ([documentoIdentificacion])
GO
ALTER TABLE [dbo].[Cliente] CHECK CONSTRAINT [FK_Cliente_Persona]
GO
/****** Object:  StoredProcedure [dbo].[sp_ActualizarCliente]    Script Date: 29/3/2022 23:09:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Author: Dikson Calderon
-- Create date: 27-03-2022 17:12 pm
-- Description:	Actualiza los Datos de una Persona
-- =============================================
CREATE PROCEDURE [dbo].[sp_ActualizarCliente] 
	@documentoIdentificacion VARCHAR(25),
	@nombreCompleto VARCHAR(50),
	@genero VARCHAR(1),
	@edad int,
	@direccion VARCHAR(255),
	@telefono VARCHAR(50),
	@contrasena VARCHAR(50), 
	@estado INT,
	@mensaje_control VARCHAR(400) OUTPUT,
	@error INT OUTPUT

AS

BEGIN TRY
	BEGIN TRANSACTION
		--Recomendable un Merge
		IF EXISTS (SELECT * FROM dbo.Persona WHERE documentoIdentificacion=  @documentoIdentificacion)
		BEGIN
			UPDATE  Persona
			SET 
				nombreCompleto = @nombreCompleto, 
				genero= @genero, 
				edad= @edad, 
				direccion= @direccion,
				telefono = @telefono
			WHERE documentoIdentificacion= @documentoIdentificacion


			UPDATE Cliente 
			SET 
				documentoIdentificacion= @documentoIdentificacion, 
				contrasena=@contrasena, 
				estado= @estado
			WHERE documentoIdentificacion= @documentoIdentificacion

			SELECT @mensaje_control ='Proceso ejecutado de forma satisfactoria'
			SELECT @error =0
		END
		ELSE
			IF NOT EXISTS (SELECT * FROM dbo.Cliente WHERE documentoIdentificacion=  @documentoIdentificacion)
			BEGIN
				UPDATE Cliente 
				SET 
					documentoIdentificacion= @documentoIdentificacion, 
					contrasena=@contrasena, 
					estado= @estado
				WHERE documentoIdentificacion= @documentoIdentificacion
				SELECT @mensaje_control ='Proceso ejecutado de forma satisfactoria'
				SELECT @error =0
			END
			
			ELSE
			BEGIN
				SELECT @mensaje_control ='Persona no encontrada'
				SELECT @error =0
			END 
	
	COMMIT TRANSACTION

END TRY
BEGIN CATCH
	if @@TRANCOUNT > 0
		ROLLBACK TRANSACTION
	SELECT @mensaje_control ='Proceso ejecutado con errores'
	SET @error =@@PROCID
END CATCH
GO
/****** Object:  StoredProcedure [dbo].[sp_ActualizarCuenta]    Script Date: 29/3/2022 23:09:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Author: Dikson Calderon
-- Create date: 27-03-2022 17:12 pm
-- Description:	Actualiza los Datos de una Persona
-- =============================================
CREATE PROCEDURE [dbo].[sp_ActualizarCuenta] 
	
	@numeroCuenta VARCHAR(25),
	@tipoCuenta VARCHAR(1),
	@saldoInicial DECIMAL(18,2),
	@estadoCuenta INT,
	@mensaje_control VARCHAR(400) OUTPUT,
	@error INT OUTPUT

AS

BEGIN TRY
	BEGIN TRANSACTION
		--Recomendable un Merge
		IF EXISTS (SELECT * FROM dbo.Cuenta WHERE numeroCuenta=  @numeroCuenta)
		BEGIN
			UPDATE  Cuenta
			SET 
				tipoCuenta = @tipoCuenta, 
				saldoInicial= @saldoInicial, 
				estadoCuenta= @estadoCuenta
			WHERE numeroCuenta=  @numeroCuenta


			SELECT @mensaje_control ='Proceso ejecutado de forma satisfactoria'
			SELECT @error =0
		END
		ELSE
			
			BEGIN
				SELECT @mensaje_control ='Nro.de Cuenta no encontrada!'
				SELECT @error =0
			END 
	
	COMMIT TRANSACTION

END TRY
BEGIN CATCH
	if @@TRANCOUNT > 0
		ROLLBACK TRANSACTION
	SELECT @mensaje_control ='Proceso ejecutado con errores'
	SET @error =@@PROCID
END CATCH
GO
/****** Object:  StoredProcedure [dbo].[sp_ActualizarMovimiento]    Script Date: 29/3/2022 23:09:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Author: Dikson Calderon
-- Create date: 27-03-2022 17:12 pm
-- Description:	Actualiza los Movimientos de las Cuentas de los Clientes
-- =============================================
CREATE PROCEDURE [dbo].[sp_ActualizarMovimiento] 
	@idMovimiento INT,	
	@fechaMovimiento DATETIME,
	@numeroCuenta VARCHAR(25),
	@tipoMovimiento VARCHAR(10),
	@saldoInicial DECIMAL(18,2),
	@valorMovimiento DECIMAL(18,2),
	@descripcionMovimiento VARCHAR(50),
	@mensaje_control VARCHAR(400) OUTPUT,
	@error INT OUTPUT

AS

BEGIN TRY
	BEGIN TRANSACTION
		--Recomendable un Merge
		
		DECLARE @saldoDisponibleAnterior DECIMAL(18,2)
		DECLARE @saldoMovimiento DECIMAL(18,2)
		DECLARE @valorMovimientoDiario DECIMAL(18,2)
		DECLARE @LimiteDiario DECIMAL(18,2)
		DECLARE @idCuenta INT
		DECLARE @fecha VARCHAR(23)
		DECLARE @grabarRegistro BIT
		
		SELECT  @fecha = FORMAT(@fechaMovimiento,'yyyy-MM-dd 00:00:00.000')
		SET @grabarRegistro= 1

		SELECT 
			@idCuenta = idCuenta 
		FROM dbo.Cuenta
		WHERE numeroCuenta= @numeroCuenta
		

		IF EXISTS (SELECT * FROM dbo.Movimiento WHERE idCuenta =  @idCuenta)
		BEGIN
			SELECT TOP 1 
				@saldoDisponibleAnterior= saldoDisponibleMovimiento
				--saldoDisponibleMovimiento
			FROM dbo.Movimiento 
			WHERE idCuenta =  @idCuenta
			ORDER BY  idMovimiento DESC

			IF (@valorMovimiento >=0)
			BEGIN
				SET @saldoMovimiento= @valorMovimiento  + @saldoDisponibleAnterior
				
			END
			ELSE
			BEGIN
				--BUSCAR LIMITE DIARIO
				EXEC sp_LimiteDiarioMovimientoCuenta  @numeroCuenta, @valorMovimientoDiario = @valorMovimientoDiario OUTPUT, @error= @error OUTPUT;
				IF @valorMovimientoDiario >= @LimiteDiario
				BEGIN
					SET @mensaje_control= 'Cupo diario Excedido'
					SET @grabarRegistro= 0
				END
				ELSE
				BEGIN
					SET @saldoMovimiento= @saldoDisponibleAnterior + @valorMovimiento   
					IF @saldoMovimiento <=0
						SET @mensaje_control= 'Saldo no disponible'
					ELSE	
						IF @grabarRegistro= 1
						BEGIN
							UPDATE  Movimiento
							SET 
								fechaMovimiento   = @fechaMovimiento, 
								tipoMovimiento    = @tipoMovimiento, 
								valorMovimiento   = @valorMovimiento,
								saldoDisponibleMovimiento = @saldoMovimiento
							WHERE idMovimiento = @idMovimiento
							SELECT @mensaje_control ='Proceso ejecutado de forma satisfactoria'
						END
				END
			END
			SELECT @error =0
		END
		ELSE
			
			BEGIN
				SELECT @mensaje_control ='Nro.de Cuenta no encontrada!'
				SELECT @error =0
			END 
	
	COMMIT TRANSACTION

END TRY
BEGIN CATCH
	if @@TRANCOUNT > 0
		ROLLBACK TRANSACTION
	SELECT @mensaje_control ='Proceso ejecutado con errores'
	SET @error =@@PROCID
END CATCH
GO
/****** Object:  StoredProcedure [dbo].[sp_ActualizarPersona]    Script Date: 29/3/2022 23:09:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Author: Dikson Calderon
-- Create date: 27-03-2022 17:12 pm
-- Description:	Actualiza los Datos de una Persona
-- =============================================
CREATE PROCEDURE [dbo].[sp_ActualizarPersona] 
	@documentoIdentificacion VARCHAR(25),
	@nombreCompleto VARCHAR(50),
	@genero VARCHAR(1),
	@edad int,
	@direccion VARCHAR(255),
	@telefono VARCHAR(50),
	@mensaje_control VARCHAR(400) OUTPUT,
	@error INT OUTPUT

AS

BEGIN TRY
	BEGIN TRANSACTION
		--Recomendable un Merge
		IF EXISTS (SELECT * FROM dbo.Persona WHERE documentoIdentificacion=  @documentoIdentificacion)
		BEGIN
			UPDATE  Persona
			SET nombreCompleto = @nombreCompleto, 
				genero= @genero, 
				edad= @edad, 
				direccion= @direccion,
				telefono = @telefono
			WHERE documentoIdentificacion= @documentoIdentificacion
			SELECT @mensaje_control ='Proceso ejecutado de forma satisfactoria'
			SELECT @error =0
		END
		ELSE
		BEGIN
			SELECT @mensaje_control ='Persona no encontrada'
			SELECT @error =0
		END 
	
	COMMIT TRANSACTION

END TRY
BEGIN CATCH
	if @@TRANCOUNT > 0
		ROLLBACK TRANSACTION
	SELECT @mensaje_control ='Proceso ejecutado con errores'
	SET @error =@@PROCID
END CATCH
GO
/****** Object:  StoredProcedure [dbo].[sp_ConsultarCliente]    Script Date: 29/3/2022 23:09:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Author:		Dikson Calderon
-- Create date: 27-3-2022
-- Description:	Consultas Cliente
-- =============================================
CREATE PROCEDURE [dbo].[sp_ConsultarCliente] 
	@documentoIdentificacion VARCHAR(25)
AS
BEGIN TRY
	BEGIN TRANSACTION

    
	SELECT  
		cl.clienteId, 
		cl.documentoIdentificacion, 
		cl.contrasena, 
		cl.estado, 
		pe.nombreCompleto, 
		pe.genero,
		pe.edad, 
		pe.direccion, 
		pe.telefono
		FROM dbo.Cliente cl INNER JOIN dbo.persona pe ON pe.documentoIdentificacion = cl.documentoIdentificacion
		WHERE cl.documentoIdentificacion= @documentoIdentificacion

	COMMIT TRANSACTION
END TRY
BEGIN CATCH
	if @@TRANCOUNT > 0
		ROLLBACK TRANSACTION
	
END CATCH
GO
/****** Object:  StoredProcedure [dbo].[sp_ConsultarCuenta]    Script Date: 29/3/2022 23:09:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Author:		Dikson Calderon
-- Create date: 27-3-2022
-- Description:	Consultas Cuenta
-- =============================================
CREATE PROCEDURE [dbo].[sp_ConsultarCuenta] 
	@numeroCuenta VARCHAR(25)
AS
BEGIN TRY
	BEGIN TRANSACTION
	    
		SELECT  
			cta.numeroCuenta,
			cta.tipoCuenta,
			cta.saldoInicial,
			cta.estadoCuenta,
			pe.documentoIdentificacion,
			pe.nombreCompleto 
		
			FROM dbo.Cuenta cta 
			INNER JOIN dbo.cliente cli ON cli.clienteId= cta.clienteId
			INNER JOIN dbo.persona pe ON cli.documentoIdentificacion = pe.documentoIdentificacion
			WHERE cta.numeroCuenta =  @numeroCuenta

	COMMIT TRANSACTION
END TRY
BEGIN CATCH
	if @@TRANCOUNT > 0
		ROLLBACK TRANSACTION
	
END CATCH
GO
/****** Object:  StoredProcedure [dbo].[sp_ConsultarMovimiento]    Script Date: 29/3/2022 23:09:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Author:		Dikson Calderon
-- Create date: 28-3-2022
-- Description:	Consultas Movimiento
-- =============================================
CREATE PROCEDURE [dbo].[sp_ConsultarMovimiento] 
	@idMovimiento VARCHAR(25)
AS
BEGIN TRY
	BEGIN TRANSACTION

    
	SELECT  
		
		mov.idMovimiento,
		mov.fechaMovimiento,
		pe.nombreCompleto, 
		cta.idCuenta,
		cta.numeroCuenta,
		cta.tipoCuenta,
		cta.saldoInicial,
		cta.estadoCuenta,
		mov.valorMovimiento,
		mov.saldoMovimiento,
		mov.tipoMovimiento,
		mov.descripcionMovimiento
		FROM dbo.Movimiento mov 
		INNER JOIN dbo.Cuenta cta ON cta.idCuenta= mov.idCuenta
		INNER JOIN dbo.Cliente cli ON cta.clienteId = cli.clienteId
		INNER JOIN dbo.Persona pe ON pe.documentoIdentificacion= cli.documentoIdentificacion
		WHERE mov.idMovimiento= @idMovimiento

	COMMIT TRANSACTION
END TRY
BEGIN CATCH
	if @@TRANCOUNT > 0
		ROLLBACK TRANSACTION
	
END CATCH
GO
/****** Object:  StoredProcedure [dbo].[sp_ConsultarPersona]    Script Date: 29/3/2022 23:09:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_ConsultarPersona] 
	@documentoIdentificacion VARCHAR(25)
AS
BEGIN
	
	SELECT * 
	FROM dbo.Persona
	WHERE documentoIdentificacion= @documentoIdentificacion
END
GO
/****** Object:  StoredProcedure [dbo].[sp_CrearCliente]    Script Date: 29/3/2022 23:09:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Author: Dikson Calderon
-- Create date: 27-03-2022 17:12 pm
-- Description:	Actualiza los Datos de una Persona
-- =============================================
CREATE PROCEDURE [dbo].[sp_CrearCliente] 
	@documentoIdentificacion VARCHAR(25),
	@nombreCompleto VARCHAR(50),
	@genero VARCHAR(1),
	@edad INT,
	@direccion VARCHAR(255),
	@telefono VARCHAR(50),
	@clienteId INT,
	@contrasena VARCHAR(50),
	@estado INT,
	@mensaje_control VARCHAR(400) OUTPUT,
	@error INT OUTPUT

AS

BEGIN TRY
	BEGIN TRANSACTION
		--Recomendable un Merge
		IF NOT EXISTS (SELECT * FROM dbo.Persona WHERE documentoIdentificacion=  @documentoIdentificacion)
		BEGIN
			
			INSERT INTO Persona ( documentoIdentificacion, nombreCompleto, genero, edad, direccion, telefono  )
			VALUES (@documentoIdentificacion, @nombreCompleto, @genero, @edad, @direccion, @telefono)
			
			INSERT INTO Cliente (documentoIdentificacion, contrasena, estado)
			VALUES ( @documentoIdentificacion, @contrasena,@estado)

			SELECT @mensaje_control ='Proceso ejecutado de forma satisfactoria'
			SELECT @error =0
		END
		ELSE
		BEGIN
			IF NOT EXISTS (SELECT * FROM dbo.Cliente WHERE documentoIdentificacion=  @documentoIdentificacion)
			BEGIN
				INSERT INTO Cliente (documentoIdentificacion, contrasena, estado)
				VALUES ( @documentoIdentificacion, @contrasena,@estado)

				SELECT @mensaje_control ='Proceso ejecutado de forma satisfactoria'
				SELECT @error =0
			END
			ELSE
			BEGIN
				SELECT @mensaje_control ='Persona y Cliente,  ya existe'
				SELECT @error =0
			END

		END 
	COMMIT TRANSACTION

END TRY
BEGIN CATCH
	if @@TRANCOUNT > 0
		ROLLBACK TRANSACTION
	SELECT @mensaje_control ='Proceso ejecutado con errores'
	SET @error =@@ERROR
END CATCH
GO
/****** Object:  StoredProcedure [dbo].[sp_CrearCuenta]    Script Date: 29/3/2022 23:09:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Author: Dikson Calderon
-- Create date: 27-03-2022 17:12 pm
-- Description:	Actualiza los Datos de una Persona
-- =============================================
CREATE PROCEDURE [dbo].[sp_CrearCuenta] 
	@clienteId INT,
	@numeroCuenta VARCHAR(25),
	@tipoCuenta VARCHAR(1),
	@saldoInicial DECIMAL(18,2),
	@estadoCuenta INT,
	@mensaje_control VARCHAR(400) OUTPUT,
	@error INT OUTPUT

AS

BEGIN TRY
	BEGIN TRANSACTION
		--Recomendable un Merge
		IF EXISTS (SELECT * FROM dbo.Cliente WHERE clienteId=  @clienteId)
		BEGIN
			IF NOT EXISTS	( SELECT  * FROM dbo.Cuenta WHERE clienteId=  @clienteId AND numeroCuenta= @numeroCuenta)
			BEGIN
				INSERT INTO Cuenta ( clienteId, numeroCuenta, tipoCuenta, saldoInicial, estadoCuenta  )
				VALUES (@clienteId,  @numeroCuenta, @tipoCuenta, @saldoInicial, @estadoCuenta )
			END
			ELSE
			BEGIN
				UPDATE Cuenta
				SET 
					tipoCuenta= @tipoCuenta , 
					saldoInicial= @saldoInicial, 
					estadoCuenta = @estadoCuenta
				WHERE clienteId =  @clienteId AND numeroCuenta= @numeroCuenta
			END
			
			SELECT @mensaje_control ='Proceso ejecutado de forma satisfactoria'
			SELECT @error =0
		END
		ELSE
		BEGIN
			SELECT @mensaje_control ='Cliente no existe!, debe crearlo antes'
			SELECT @error =0
		END 
	COMMIT TRANSACTION

END TRY
BEGIN CATCH
	if @@TRANCOUNT > 0
		ROLLBACK TRANSACTION
	SELECT @mensaje_control ='Proceso ejecutado con errores'
	SET @error =@@ERROR
END CATCH
GO
/****** Object:  StoredProcedure [dbo].[sp_CrearMovimiento]    Script Date: 29/3/2022 23:09:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Author: Dikson Calderon
-- Create date: 28-03-2022 17:12 pm
-- Description:	Crea un Movimiento en Cuenta de los Cliente
-- =============================================
CREATE PROCEDURE [dbo].[sp_CrearMovimiento] 
	
	@fechaMovimiento DATETIME,
	@numeroCuenta VARCHAR(25),
	@tipoMovimiento VARCHAR(10),
	@saldoInicial DECIMAL(18,2),
	@valorMovimiento DECIMAL(18,2),
	@descripcionMovimiento VARCHAR(50),
	@mensaje_control VARCHAR(400) OUTPUT,
	@error INT OUTPUT

AS

BEGIN TRY
	BEGIN TRANSACTION
			
		DECLARE @saldoDisponibleAnterior DECIMAL(18,2)
		DECLARE @saldoMovimiento DECIMAL(18,2)
		DECLARE @valorMovimientoDiario DECIMAL(18,2)
		DECLARE @LimiteDiario DECIMAL(18,2)
		DECLARE @errorSP INT
		DECLARE @idCuenta INT
		DECLARE @fecha VARCHAR(23)
		DECLARE @grabarRegistro BIT
		SELECT  @fecha = FORMAT(GETDATE(),'yyyy-MM-dd 00:00:00.000')	
		SET @LimiteDiario = 1000.00
		SET @grabarRegistro= 1
		SET @saldoMovimiento= 0

		IF EXISTS (SELECT * FROM dbo.Cuenta WHERE numeroCuenta = @numeroCuenta)
		BEGIN
			SELECT TOP 1 
				@saldoDisponibleAnterior= saldoDisponibleMovimiento
			FROM dbo.Movimiento 
			ORDER BY  idMovimiento DESC

			SELECT 
				@idCuenta= idCuenta
			FROM dbo.Cuenta
			WHERE  numeroCuenta = @numeroCuenta

			IF (@valorMovimiento >=0)
			BEGIN
				SET @saldoMovimiento= @valorMovimiento  + @saldoDisponibleAnterior
			END
			ELSE
			BEGIN
				--BUSCAR LIMITE DIARIO
				EXEC sp_LimiteDiarioMovimientoCuenta  @numeroCuenta, @valorMovimientoDiario = @valorMovimientoDiario OUTPUT, @error= @error OUTPUT;
				
				IF @valorMovimientoDiario >= @LimiteDiario
				BEGIN
					SET @mensaje_control= 'Cupo diario Excedido'
					SET @grabarRegistro= 0
				END
				ELSE
				BEGIN
					SET @saldoMovimiento=  @saldoDisponibleAnterior + @valorMovimiento  
					IF @saldoMovimiento <=0
						SET @mensaje_control= 'Saldo no disponible'
				END
				
			END
				IF @grabarRegistro= 1
				BEGIN
					INSERT INTO  Movimiento (idCuenta, fechaMovimiento, saldoMovimiento, tipoMovimiento, valorMovimiento , saldoDisponibleMovimiento, descripcionMovimiento )  
					VALUES (@idCuenta, @fecha, @saldoDisponibleAnterior, @tipoMovimiento, @valorMovimiento, @saldoMovimiento, @descripcionMovimiento)
					SELECT @mensaje_control ='Proceso ejecutado de forma satisfactoria'
					SELECT @error =0
				END 
		END
		ELSE
			
			BEGIN
				SELECT @mensaje_control ='Nro.de Movimiento no encontrado!'
				SELECT @error =0
			END 
	
	COMMIT TRANSACTION

END TRY
BEGIN CATCH
	if @@TRANCOUNT > 0
		ROLLBACK TRANSACTION
	SELECT @mensaje_control ='Proceso ejecutado con errores'
	SET @error =@@PROCID
END CATCH
GO
/****** Object:  StoredProcedure [dbo].[sp_CrearPersona]    Script Date: 29/3/2022 23:09:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Author: Dikson Calderon
-- Create date: 27-03-2022 17:12 pm
-- Description:	Actualiza los Datos de una Persona
-- =============================================
CREATE PROCEDURE [dbo].[sp_CrearPersona] 
	@documentoIdentificacion VARCHAR(25),
	@nombreCompleto VARCHAR(50),
	@genero VARCHAR(1),
	@edad int,
	@direccion VARCHAR(255),
	@telefono VARCHAR(50),
	@mensaje_control VARCHAR(400) OUTPUT,
	@error INT OUTPUT

AS

BEGIN TRY
	BEGIN TRANSACTION
		--Recomendable un Merge
		IF NOT EXISTS (SELECT * FROM dbo.Persona WHERE documentoIdentificacion=  @documentoIdentificacion)
		BEGIN
			INSERT INTO Persona ( documentoIdentificacion, nombreCompleto, genero, edad, direccion, telefono  )
			VALUES (@documentoIdentificacion, @nombreCompleto, @genero, @edad, @direccion, @telefono)
			
			SELECT @mensaje_control ='Proceso ejecutado de forma satisfactoria'
			SELECT @error =0
		END
		ELSE
		BEGIN
			SELECT @mensaje_control ='Persona ya existe'
			SELECT @error =0

		END 
	COMMIT TRANSACTION

END TRY
BEGIN CATCH
	if @@TRANCOUNT > 0
		ROLLBACK TRANSACTION
	SELECT @mensaje_control ='Proceso ejecutado con errores'
	SET @error =@@PROCID
END CATCH
GO
/****** Object:  StoredProcedure [dbo].[sp_EliminarCliente]    Script Date: 29/3/2022 23:09:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Author:		Dikson Calderon
-- Create date: 27/3/2022
-- Description:	Eliminar un Cliente
-- =============================================
CREATE  PROCEDURE [dbo].[sp_EliminarCliente] 
	@documentoIdentificacion VARCHAR(25),
	@mensaje_control VARCHAR(400) OUTPUT,
	@error INT OUTPUT
AS
BEGIN TRY
	BEGIN TRANSACTION
		IF EXISTS (SELECT * FROM dbo.Cliente WHERE documentoIdentificacion=  @documentoIdentificacion)
		BEGIN
			DELETE  
			FROM dbo.Cliente
			WHERE documentoIdentificacion=  @documentoIdentificacion
			SELECT @mensaje_control ='Proceso de borrado ejecutado de forma satisfactoria!'
			SELECT @error =0
		END
		ELSE
		BEGIN
			SELECT @mensaje_control ='Cliente no encontrado!'
			SELECT @error =0
		END
COMMIT TRANSACTION
END TRY
BEGIN CATCH
	if @@TRANCOUNT > 0
		ROLLBACK TRANSACTION
		SELECT @mensaje_control ='Proceso ejecutado con errores'
	SET @error =@@PROCID
END CATCH
GO
/****** Object:  StoredProcedure [dbo].[sp_EliminarCuenta]    Script Date: 29/3/2022 23:09:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Author:		Dikson Calderon
-- Create date: 27/3/2022
-- Description:	Eliminar una Persona
-- =============================================
CREATE  PROCEDURE [dbo].[sp_EliminarCuenta] 
	@numeroCuenta VARCHAR(25),
	@mensaje_control VARCHAR(400) OUTPUT,
	@error INT OUTPUT
AS
BEGIN TRY
	BEGIN TRANSACTION
		IF EXISTS (SELECT * FROM dbo.Cuenta WHERE numeroCuenta =  @numeroCuenta)
		BEGIN
			DELETE  
			FROM dbo.Cuenta
			WHERE numeroCuenta=  @numeroCuenta
			SELECT @mensaje_control ='Proceso ejecutado de forma satisfactoria'
			SELECT @error =0
		END
		ELSE
		BEGIN
			SELECT @mensaje_control ='Cuenta no encontrada!'
			SELECT @error =0
		END
COMMIT TRANSACTION
END TRY
BEGIN CATCH
	if @@TRANCOUNT > 0
		ROLLBACK TRANSACTION
		SELECT @mensaje_control ='Proceso ejecutado con errores'
	SET @error =@@PROCID
END CATCH
GO
/****** Object:  StoredProcedure [dbo].[sp_EliminarMovimiento]    Script Date: 29/3/2022 23:09:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Author:		Dikson Calderon
-- Create date: 28/3/2022
-- Description:	Eliminar un Movimiento
-- =============================================
CREATE  PROCEDURE [dbo].[sp_EliminarMovimiento] 
	@idMovimiento INT,
	@mensaje_control VARCHAR(400) OUTPUT,
	@error INT OUTPUT
AS
BEGIN TRY
	BEGIN TRANSACTION
		IF EXISTS (SELECT * FROM dbo.Movimiento WHERE idMovimiento=  @idMovimiento)
		BEGIN
			DELETE  
			FROM dbo.Movimiento
			WHERE idMovimiento=  @idMovimiento
			SELECT @mensaje_control ='Proceso de borrado ejecutado de forma satisfactoria!'
			SELECT @error =0
		END
		ELSE
		BEGIN
			SELECT @mensaje_control ='Nro. de Movimiento de la cuenta no encontrado!'
			SELECT @error =0
		END
COMMIT TRANSACTION
END TRY
BEGIN CATCH
	if @@TRANCOUNT > 0
		ROLLBACK TRANSACTION
		SELECT @mensaje_control ='Proceso ejecutado con errores'
	SET @error =@@PROCID
END CATCH
GO
/****** Object:  StoredProcedure [dbo].[sp_EliminarPersona]    Script Date: 29/3/2022 23:09:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Author:		Dikson Calderon
-- Create date: 27/3/2022
-- Description:	Eliminar una Persona
-- =============================================
CREATE  PROCEDURE [dbo].[sp_EliminarPersona] 
	@documentoIdentificacion VARCHAR(25),
	@mensaje_control VARCHAR(400) OUTPUT,
	@error INT OUTPUT
AS
BEGIN TRY
	BEGIN TRANSACTION
		IF EXISTS (SELECT * FROM dbo.Persona WHERE documentoIdentificacion=  @documentoIdentificacion)
		BEGIN
			DELETE  
			FROM dbo.Persona
			WHERE documentoIdentificacion=  @documentoIdentificacion
			SELECT @mensaje_control ='Proceso ejecutado de forma satisfactoria'
			SELECT @error =0
		END
		ELSE
		BEGIN
			SELECT @mensaje_control ='Persona no encontrada'
			SELECT @error =0
		END
COMMIT TRANSACTION
END TRY
BEGIN CATCH
	if @@TRANCOUNT > 0
		ROLLBACK TRANSACTION
		SELECT @mensaje_control ='Proceso ejecutado con errores'
	SET @error =@@PROCID
END CATCH
GO
/****** Object:  StoredProcedure [dbo].[sp_LimiteDiarioMovimientoCuenta]    Script Date: 29/3/2022 23:09:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Author: Dikson Calderon
-- Create date: 28-03-2022 17:12 pm
-- Description:	Limite Diario de los Movimientos de las Cuentas de los Clientes
-- =============================================
CREATE PROCEDURE [dbo].[sp_LimiteDiarioMovimientoCuenta] 
	@numeroCuenta VARCHAR(25),
	@valorMovimientoDiario DECIMAL(18,2) OUTPUT,
	@error INT OUTPUT

AS

BEGIN TRY
	BEGIN TRANSACTION
		
		DECLARE @idCuenta INT
		DECLARE @valorMovimiento DECIMAL
		DECLARE @FechaMovimiento DATETIME
		DECLARE @fecha VARCHAR(23)
		SELECT  @fecha = FORMAT(GETDATE(),'yyyy-MM-dd 00:00:00.000')

		IF EXISTS (SELECT * FROM dbo.Cuenta WHERE numeroCuenta =  @numeroCuenta)
		BEGIN
			SELECT 
				@idCuenta= idCuenta
			FROM dbo.Cuenta
			WHERE numeroCuenta =  @numeroCuenta
			
			SELECT 
				@valorMovimientoDiario= SUM(valorMovimiento),
				@FechaMovimiento= fechaMovimiento
			FROM dbo.Movimiento 
			WHERE idCuenta = @idCuenta AND fechaMovimiento= @fecha AND tipoMovimiento= 'D'
			GROUP BY fechaMovimiento 
			SET @valorMovimientoDiario= @valorMovimientoDiario
			
		END
		ELSE
			
			BEGIN
				SELECT @error =0
			END 
	
	COMMIT TRANSACTION

END TRY
BEGIN CATCH
	if @@TRANCOUNT > 0
		ROLLBACK TRANSACTION
		SET @error =@@PROCID
END CATCH
GO
/****** Object:  StoredProcedure [dbo].[sp_ListaClientes]    Script Date: 29/3/2022 23:09:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Dikson Calderon
-- Create date: 27/3/2022
-- Description:	Listar los Clientes
-- =============================================
CREATE PROCEDURE [dbo].[sp_ListaClientes] 
	
AS
BEGIN TRY
	BEGIN TRANSACTION

		SELECT  
			cl.clienteId, 
			cl.documentoIdentificacion, 
			cl.contrasena, 
			cl.estado, 
			pe.nombreCompleto, 
			pe.genero,
			pe.edad, 
			pe.direccion, 
			pe.telefono
		FROM dbo.Cliente cl INNER JOIN dbo.persona pe ON pe.documentoIdentificacion = cl.documentoIdentificacion

	COMMIT TRANSACTION
END TRY
BEGIN CATCH
	if @@TRANCOUNT > 0
		ROLLBACK TRANSACTION
	
END CATCH
GO
/****** Object:  StoredProcedure [dbo].[sp_ListaCuentas]    Script Date: 29/3/2022 23:09:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Dikson Calderon
-- Create date: 27/3/2022
-- Description:	Listar las Cuentas
-- =============================================
CREATE PROCEDURE [dbo].[sp_ListaCuentas] 
	
AS
BEGIN TRY
	BEGIN TRANSACTION

		SELECT  
			cl.clienteId,
			cl.documentoIdentificacion, 
			pe.nombreCompleto, 
			cta.idCuenta,
			cta.numeroCuenta,
			cta.tipoCuenta,
			cta.saldoInicial,
			cta.estadoCuenta
		FROM dbo.Cliente cl 
		INNER JOIN dbo.persona pe ON pe.documentoIdentificacion = cl.documentoIdentificacion
		INNER JOIN dbo.Cuenta cta ON cl.clienteId= cta.clienteId
		    

	COMMIT TRANSACTION
END TRY
BEGIN CATCH
	if @@TRANCOUNT > 0
		ROLLBACK TRANSACTION
	
END CATCH
GO
/****** Object:  StoredProcedure [dbo].[sp_ListaEstadoCuenta]    Script Date: 29/3/2022 23:09:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Dikson Calderon
-- Create date: 29/3/2022
-- Description:	Listado Estado de Cuenta
-- =============================================
CREATE PROCEDURE [dbo].[sp_ListaEstadoCuenta] 
	@documentoIdentificacion VARCHAR(50),
	@fechaIni DATETIME,
	@fechaFin DATETIME
AS
BEGIN TRY
	BEGIN TRANSACTION
	DECLARE @fechaDesde VARCHAR(23)
	DECLARE @fechaHasta VARCHAR(23)
	SELECT  @fechaDesde = FORMAT(@fechaIni,'yyyy-MM-dd 00:00:00.000')
	SELECT  @fechaHasta = FORMAT(@fechaFin,'yyyy-MM-dd 00:00:00.000')

		SELECT  
	
			mov.fechaMovimiento Fecha,
			pe.nombreCompleto Cliente,
			cta.numeroCuenta NumeroCuenta,
			cta.tipoCuenta Tipo,
			cta.saldoInicial SaldoInicial,
			cta.estadoCuenta Estado,
			mov.valorMovimiento Movimiento,
			mov.saldoMovimiento SaldoDisponible
						
		FROM dbo.Cliente cl 
		INNER JOIN dbo.persona pe ON pe.documentoIdentificacion = cl.documentoIdentificacion
		INNER JOIN dbo.Cuenta cta ON cl.clienteId= cta.clienteId
		INNER JOIN dbo.Movimiento mov ON mov.idCuenta= cta.idCuenta
		WHERE mov.fechaMovimiento between @fechaDesde AND @fechaHasta
		AND pe.documentoIdentificacion= @documentoIdentificacion
		ORDER BY cta.numeroCuenta,mov.idMovimiento, mov.fechaMovimiento
		    

	COMMIT TRANSACTION
END TRY
BEGIN CATCH
	if @@TRANCOUNT > 0
		ROLLBACK TRANSACTION
	
END CATCH
GO
/****** Object:  StoredProcedure [dbo].[sp_ListaMovimientos]    Script Date: 29/3/2022 23:09:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Dikson Calderon
-- Create date: 27/3/2022
-- Description:	Listar los Movimientos
-- =============================================
CREATE PROCEDURE [dbo].[sp_ListaMovimientos] 
	
AS
BEGIN TRY
	BEGIN TRANSACTION

		SELECT  
			mov.idMovimiento,
			cta.idCuenta,
			cta.numeroCuenta,
			cta.tipoCuenta,
			cta.saldoInicial,
			mov.valorMovimiento,
			mov.saldoMovimiento,
			mov.descripcionMovimiento,
			mov.fechaMovimiento,
			mov.tipoMovimiento
		FROM dbo.Cliente cl 
		INNER JOIN dbo.persona pe ON pe.documentoIdentificacion = cl.documentoIdentificacion
		INNER JOIN dbo.Cuenta cta ON cl.clienteId= cta.clienteId
		INNER JOIN dbo.Movimiento mov ON mov.idCuenta= cta.idCuenta
		    

	COMMIT TRANSACTION
END TRY
BEGIN CATCH
	if @@TRANCOUNT > 0
		ROLLBACK TRANSACTION
	
END CATCH
GO
/****** Object:  StoredProcedure [dbo].[sp_ListaPersonas]    Script Date: 29/3/2022 23:09:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Author:		Dikson Calderon
-- Create date: 27/3/2022
-- Description:	<Description,,>
-- =============================================
CREATE  PROCEDURE [dbo].[sp_ListaPersonas] 

AS
BEGIN TRY
	BEGIN TRANSACTION

		SELECT * 
		FROM dbo.Persona

	COMMIT TRANSACTION
END TRY
BEGIN CATCH
	if @@TRANCOUNT > 0
		ROLLBACK TRANSACTION
	
END CATCH
GO
