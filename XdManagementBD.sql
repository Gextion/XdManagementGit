USE [XdManagementBD]
GO
/****** Object:  Table [dbo].[AccionesAhorro]    Script Date: 13/03/2018 8:52:05 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AccionesAhorro](
	[AccionAhorroID] [int] IDENTITY(1,1) NOT NULL,
	[DescripcionAccionAhorro] [nvarchar](60) NOT NULL,
	[TipoEquipoID] [int] NOT NULL,
 CONSTRAINT [PK_AccionesAhorro] PRIMARY KEY CLUSTERED 
(
	[AccionAhorroID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[AccionesAhorroSedes]    Script Date: 13/03/2018 8:52:05 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AccionesAhorroSedes](
	[AccionSedeID] [int] IDENTITY(1,1) NOT NULL,
	[InventarioSedeID] [int] NOT NULL,
	[TipoEquipoID] [int] NOT NULL,
	[AccionAhorroID] [int] NOT NULL,
 CONSTRAINT [PK_AccionesAhorroSede] PRIMARY KEY CLUSTERED 
(
	[AccionSedeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[Años]    Script Date: 13/03/2018 8:52:05 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Años](
	[AñoID] [int] NOT NULL,
	[Año] [int] NOT NULL,
 CONSTRAINT [PK_Años] PRIMARY KEY CLUSTERED 
(
	[AñoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[ApplicationRoleGroups]    Script Date: 13/03/2018 8:52:05 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ApplicationRoleGroups](
	[RoleId] [nvarchar](128) NOT NULL,
	[GroupId] [int] NOT NULL,
 CONSTRAINT [PK_dbo.ApplicationRoleGroups] PRIMARY KEY CLUSTERED 
(
	[RoleId] ASC,
	[GroupId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[ApplicationUserGroups]    Script Date: 13/03/2018 8:52:06 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ApplicationUserGroups](
	[UserId] [nvarchar](128) NOT NULL,
	[GroupId] [int] NOT NULL,
 CONSTRAINT [PK_dbo.ApplicationUserGroups] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[GroupId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[Asesores]    Script Date: 13/03/2018 8:52:06 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Asesores](
	[AsesorID] [int] IDENTITY(1,1) NOT NULL,
	[Codigo] [nvarchar](12) NOT NULL,
	[NombreAsesor] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Asesores] PRIMARY KEY CLUSTERED 
(
	[AsesorID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[AspNetRoles]    Script Date: 13/03/2018 8:52:06 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoles](
	[Id] [nvarchar](128) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Discriminator] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[AspNetUserLogins]    Script Date: 13/03/2018 8:52:06 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserLogins](
	[UserId] [nvarchar](128) NOT NULL,
	[LoginProvider] [nvarchar](128) NOT NULL,
	[ProviderKey] [nvarchar](128) NOT NULL,
	[IdentityUser_Id] [nvarchar](128) NULL,
 CONSTRAINT [PK_dbo.AspNetUserLogins] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[LoginProvider] ASC,
	[ProviderKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[AspNetUserRoles]    Script Date: 13/03/2018 8:52:07 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserRoles](
	[UserId] [nvarchar](128) NOT NULL,
	[RoleId] [nvarchar](128) NOT NULL,
	[IdentityRole_Id] [nvarchar](128) NULL,
	[IdentityUser_Id] [nvarchar](128) NULL,
 CONSTRAINT [PK_dbo.AspNetUserRoles] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[AspNetUsers]    Script Date: 13/03/2018 8:52:07 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUsers](
	[Id] [nvarchar](128) NOT NULL,
	[Email] [nvarchar](max) NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[LockoutEndDateUtc] [datetime] NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
	[UserName] [nvarchar](max) NOT NULL,
	[FirstName] [nvarchar](max) NULL,
	[LastName] [nvarchar](max) NULL,
	[Discriminator] [nvarchar](128) NOT NULL,
	[LogoUrl] [nvarchar](max) NULL,
	[EmpresaID] [int] NULL,
 CONSTRAINT [PK_dbo.AspNetUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[AuthenticationAudit]    Script Date: 13/03/2018 8:52:07 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AuthenticationAudit](
	[AuditID] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](128) NOT NULL,
	[LoginIP] [nvarchar](50) NULL,
	[LoginBrowser] [nvarchar](150) NULL,
	[LoginBrowserVersion] [nvarchar](150) NULL,
	[LoginPlatform] [nvarchar](150) NULL,
	[LoginDate] [datetime] NOT NULL,
 CONSTRAINT [PK_AuthenticationAudit] PRIMARY KEY CLUSTERED 
(
	[AuditID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[Ciudades]    Script Date: 13/03/2018 8:52:07 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Ciudades](
	[CiudadID] [int] IDENTITY(1,1) NOT NULL,
	[Ciudad] [nvarchar](max) NOT NULL,
	[NivelTermicoID] [int] NOT NULL,
 CONSTRAINT [PK_dbo.Ciudades] PRIMARY KEY CLUSTERED 
(
	[CiudadID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[Consumos]    Script Date: 13/03/2018 8:52:08 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Consumos](
	[ConsumoID] [int] IDENTITY(1,1) NOT NULL,
	[DispositivoID] [int] NOT NULL,
	[FechaInicial] [datetime] NOT NULL,
	[FechaFinal] [datetime] NOT NULL,
	[LineaBase] [decimal](18, 2) NOT NULL,
	[ConsumoPeriodo] [decimal](18, 2) NOT NULL,
	[Valor] [decimal](18, 2) NOT NULL,
	[ValorUnitario] [decimal](18, 2) NOT NULL,
 CONSTRAINT [PK_dbo.Consumos] PRIMARY KEY CLUSTERED 
(
	[ConsumoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[Dispositivos]    Script Date: 13/03/2018 8:52:08 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Dispositivos](
	[DispositivoID] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](40) NOT NULL,
	[LineaBase] [decimal](18, 2) NOT NULL,
	[SedeID] [int] NOT NULL,
	[FuenteID] [int] NOT NULL,
	[PeriodoFacturacionID] [int] NOT NULL,
 CONSTRAINT [PK_dbo.Dispositivos] PRIMARY KEY CLUSTERED 
(
	[DispositivoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[Empresas]    Script Date: 13/03/2018 8:52:08 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Empresas](
	[EmpresaID] [int] IDENTITY(1,1) NOT NULL,
	[Codigo] [nvarchar](30) NOT NULL,
	[Nit] [bigint] NOT NULL,
	[Nombre] [nvarchar](200) NOT NULL,
	[RepresentanteLegal] [nvarchar](120) NULL,
	[CiudadID] [int] NOT NULL,
	[SectorEconomicoID] [int] NOT NULL,
	[Direccion] [nvarchar](120) NOT NULL,
	[Telefono] [bigint] NULL,
	[Celular] [bigint] NULL,
	[Email] [nvarchar](100) NULL,
	[SitioWeb] [nvarchar](300) NULL,
	[LogoUrl] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.Empresas] PRIMARY KEY CLUSTERED 
(
	[EmpresaID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[Equipos]    Script Date: 13/03/2018 8:52:09 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Equipos](
	[EquipoID] [int] IDENTITY(1,1) NOT NULL,
	[DescripcionEquipo] [nvarchar](60) NOT NULL,
	[TipoEquipoID] [int] NOT NULL,
	[Consumo] [decimal](18, 4) NOT NULL,
	[Watt] [decimal](18, 2) NOT NULL,
 CONSTRAINT [PK_Equipos] PRIMARY KEY CLUSTERED 
(
	[EquipoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[FuentesEnergeticas]    Script Date: 13/03/2018 8:52:09 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FuentesEnergeticas](
	[FuenteID] [int] IDENTITY(1,1) NOT NULL,
	[Fuente] [nvarchar](max) NOT NULL,
	[UnidadMedida] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_dbo.FuentesEnergeticas] PRIMARY KEY CLUSTERED 
(
	[FuenteID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[Groups]    Script Date: 13/03/2018 8:52:09 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Groups](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_dbo.Groups] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[IdentityUserClaims]    Script Date: 13/03/2018 8:52:09 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[IdentityUserClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](max) NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
	[IdentityUser_Id] [nvarchar](128) NULL,
 CONSTRAINT [PK_dbo.IdentityUserClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[Inventario]    Script Date: 13/03/2018 8:52:09 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Inventario](
	[InventarioSedeID] [int] IDENTITY(1,1) NOT NULL,
	[EmpresaID] [int] NOT NULL,
	[SedeID] [int] NOT NULL,
	[AsesorID] [int] NULL,
	[Fecha] [smalldatetime] NOT NULL,
	[PrestadoraServicio] [nvarchar](50) NULL,
	[Consumo] [decimal](18, 2) NULL,
	[ValorKWH] [decimal](18, 2) NOT NULL,
	[MetrosCuadrados] [smallint] NOT NULL,
	[HorasSemana] [smallint] NOT NULL,
	[HorasSabado] [smallint] NOT NULL,
	[HorasDomingo] [smallint] NOT NULL,
 CONSTRAINT [PK_InventarioEquipos] PRIMARY KEY CLUSTERED 
(
	[InventarioSedeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[InventarioEquipos]    Script Date: 13/03/2018 8:52:10 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InventarioEquipos](
	[InventarioEquipoID] [int] IDENTITY(1,1) NOT NULL,
	[InventarioSedeID] [int] NOT NULL,
	[EquipoID] [int] NOT NULL,
	[Cantidad] [smallint] NOT NULL,
	[ConsumoSemana] [decimal](18, 2) NULL,
	[ConsumoSabado] [decimal](18, 2) NULL,
	[ConsumoDomingo] [decimal](18, 2) NULL,
 CONSTRAINT [PK_InventarioEquipos_1] PRIMARY KEY CLUSTERED 
(
	[InventarioEquipoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[Meses]    Script Date: 13/03/2018 8:52:10 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Meses](
	[MesID] [int] NOT NULL,
	[Mes] [nvarchar](15) NOT NULL,
 CONSTRAINT [PK_Meses] PRIMARY KEY CLUSTERED 
(
	[MesID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[NivelesTermicos]    Script Date: 13/03/2018 8:52:10 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NivelesTermicos](
	[NivelTermicoID] [int] IDENTITY(1,1) NOT NULL,
	[NombreNivelTermico] [nvarchar](30) NOT NULL,
 CONSTRAINT [PK_NivelesTermicos] PRIMARY KEY CLUSTERED 
(
	[NivelTermicoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[PeriodosFacturacion]    Script Date: 13/03/2018 8:52:11 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PeriodosFacturacion](
	[PeriodoFacturacionID] [int] IDENTITY(1,1) NOT NULL,
	[Periodo] [nvarchar](max) NOT NULL,
	[Dias] [smallint] NOT NULL,
 CONSTRAINT [PK_dbo.PeriodosFacturacion] PRIMARY KEY CLUSTERED 
(
	[PeriodoFacturacionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[Peticiones]    Script Date: 13/03/2018 8:52:11 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Peticiones](
	[PeticionID] [int] IDENTITY(1,1) NOT NULL,
	[FechaRegistro] [datetime] NOT NULL,
	[Titulo] [nvarchar](60) NOT NULL,
	[Descripcion] [nvarchar](600) NOT NULL,
	[TipoPeticion] [int] NOT NULL,
	[FechaSolucion] [datetime] NULL,
	[Solucion] [nvarchar](max) NULL,
	[ResueltaPor] [nvarchar](max) NULL,
	[UserID] [nvarchar](max) NULL,
	[EmpresaID] [int] NOT NULL,
 CONSTRAINT [PK_dbo.Peticiones] PRIMARY KEY CLUSTERED 
(
	[PeticionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[SectoresEconomicos]    Script Date: 13/03/2018 8:52:11 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SectoresEconomicos](
	[SectorEconomicoID] [int] IDENTITY(1,1) NOT NULL,
	[SectorEconomico] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.SectoresEconomicos] PRIMARY KEY CLUSTERED 
(
	[SectorEconomicoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[Sedes]    Script Date: 13/03/2018 8:52:11 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sedes](
	[SedeID] [int] IDENTITY(1,1) NOT NULL,
	[NombreSede] [nvarchar](200) NOT NULL,
	[Responsable] [nvarchar](120) NULL,
	[Estrato] [int] NOT NULL,
	[CiudadID] [int] NOT NULL,
	[Direccion] [nvarchar](120) NOT NULL,
	[Telefono] [bigint] NULL,
	[Celular] [bigint] NULL,
	[Email] [nvarchar](100) NULL,
	[EmpresaID] [int] NOT NULL,
 CONSTRAINT [PK_dbo.Sedes] PRIMARY KEY CLUSTERED 
(
	[SedeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[TiposEquipos]    Script Date: 13/03/2018 8:52:12 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TiposEquipos](
	[TipoEquipoID] [int] IDENTITY(1,1) NOT NULL,
	[NombreTipoEquipo] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_TiposEquipos] PRIMARY KEY CLUSTERED 
(
	[TipoEquipoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
ALTER TABLE [dbo].[Ciudades] ADD  CONSTRAINT [DF_Ciudades_NivelTermicoID]  DEFAULT ((1)) FOR [NivelTermicoID]
GO
ALTER TABLE [dbo].[Equipos] ADD  CONSTRAINT [DF_Equipos_Watts]  DEFAULT ((0)) FOR [Watt]
GO
ALTER TABLE [dbo].[Inventario] ADD  CONSTRAINT [DF_Inventario_Consumo]  DEFAULT ((0)) FOR [Consumo]
GO
ALTER TABLE [dbo].[Inventario] ADD  CONSTRAINT [DF_Inventario_MetrosCuadrados]  DEFAULT ((0)) FOR [MetrosCuadrados]
GO
ALTER TABLE [dbo].[AccionesAhorro]  WITH CHECK ADD  CONSTRAINT [FK_AccionesAhorro_TiposEquipos] FOREIGN KEY([TipoEquipoID])
REFERENCES [dbo].[TiposEquipos] ([TipoEquipoID])
GO
ALTER TABLE [dbo].[AccionesAhorro] CHECK CONSTRAINT [FK_AccionesAhorro_TiposEquipos]
GO
ALTER TABLE [dbo].[AccionesAhorroSedes]  WITH CHECK ADD  CONSTRAINT [FK_AccionesAhorroSedes_AccionesAhorro] FOREIGN KEY([AccionAhorroID])
REFERENCES [dbo].[AccionesAhorro] ([AccionAhorroID])
GO
ALTER TABLE [dbo].[AccionesAhorroSedes] CHECK CONSTRAINT [FK_AccionesAhorroSedes_AccionesAhorro]
GO
ALTER TABLE [dbo].[AccionesAhorroSedes]  WITH CHECK ADD  CONSTRAINT [FK_AccionesAhorroSedes_InventarioEquipos] FOREIGN KEY([InventarioSedeID])
REFERENCES [dbo].[Inventario] ([InventarioSedeID])
GO
ALTER TABLE [dbo].[AccionesAhorroSedes] CHECK CONSTRAINT [FK_AccionesAhorroSedes_InventarioEquipos]
GO
ALTER TABLE [dbo].[AccionesAhorroSedes]  WITH CHECK ADD  CONSTRAINT [FK_AccionesAhorroSedes_TiposEquipos] FOREIGN KEY([TipoEquipoID])
REFERENCES [dbo].[TiposEquipos] ([TipoEquipoID])
GO
ALTER TABLE [dbo].[AccionesAhorroSedes] CHECK CONSTRAINT [FK_AccionesAhorroSedes_TiposEquipos]
GO
ALTER TABLE [dbo].[ApplicationRoleGroups]  WITH CHECK ADD  CONSTRAINT [FK_dbo.ApplicationRoleGroups_dbo.AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ApplicationRoleGroups] CHECK CONSTRAINT [FK_dbo.ApplicationRoleGroups_dbo.AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[ApplicationRoleGroups]  WITH CHECK ADD  CONSTRAINT [FK_dbo.ApplicationRoleGroups_dbo.Groups_GroupId] FOREIGN KEY([GroupId])
REFERENCES [dbo].[Groups] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ApplicationRoleGroups] CHECK CONSTRAINT [FK_dbo.ApplicationRoleGroups_dbo.Groups_GroupId]
GO
ALTER TABLE [dbo].[ApplicationUserGroups]  WITH CHECK ADD  CONSTRAINT [FK_dbo.ApplicationUserGroups_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ApplicationUserGroups] CHECK CONSTRAINT [FK_dbo.ApplicationUserGroups_dbo.AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[ApplicationUserGroups]  WITH CHECK ADD  CONSTRAINT [FK_dbo.ApplicationUserGroups_dbo.Groups_GroupId] FOREIGN KEY([GroupId])
REFERENCES [dbo].[Groups] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ApplicationUserGroups] CHECK CONSTRAINT [FK_dbo.ApplicationUserGroups_dbo.Groups_GroupId]
GO
ALTER TABLE [dbo].[AspNetUserLogins]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserLogins_dbo.AspNetUsers_IdentityUser_Id] FOREIGN KEY([IdentityUser_Id])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[AspNetUserLogins] CHECK CONSTRAINT [FK_dbo.AspNetUserLogins_dbo.AspNetUsers_IdentityUser_Id]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetRoles_IdentityRole_Id] FOREIGN KEY([IdentityRole_Id])
REFERENCES [dbo].[AspNetRoles] ([Id])
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetRoles_IdentityRole_Id]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetUsers_IdentityUser_Id] FOREIGN KEY([IdentityUser_Id])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetUsers_IdentityUser_Id]
GO
ALTER TABLE [dbo].[AspNetUsers]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUsers_Empresas] FOREIGN KEY([EmpresaID])
REFERENCES [dbo].[Empresas] ([EmpresaID])
GO
ALTER TABLE [dbo].[AspNetUsers] CHECK CONSTRAINT [FK_AspNetUsers_Empresas]
GO
ALTER TABLE [dbo].[AuthenticationAudit]  WITH CHECK ADD  CONSTRAINT [FK_AuthenticationAudit_AspNetUsers] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AuthenticationAudit] CHECK CONSTRAINT [FK_AuthenticationAudit_AspNetUsers]
GO
ALTER TABLE [dbo].[Ciudades]  WITH CHECK ADD  CONSTRAINT [FK_Ciudades_NivelesTermicos] FOREIGN KEY([NivelTermicoID])
REFERENCES [dbo].[NivelesTermicos] ([NivelTermicoID])
GO
ALTER TABLE [dbo].[Ciudades] CHECK CONSTRAINT [FK_Ciudades_NivelesTermicos]
GO
ALTER TABLE [dbo].[Consumos]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Consumos_dbo.Dispositivos_DispositivoID] FOREIGN KEY([DispositivoID])
REFERENCES [dbo].[Dispositivos] ([DispositivoID])
GO
ALTER TABLE [dbo].[Consumos] CHECK CONSTRAINT [FK_dbo.Consumos_dbo.Dispositivos_DispositivoID]
GO
ALTER TABLE [dbo].[Dispositivos]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Dispositivos_dbo.FuentesEnergeticas_FuenteID] FOREIGN KEY([FuenteID])
REFERENCES [dbo].[FuentesEnergeticas] ([FuenteID])
GO
ALTER TABLE [dbo].[Dispositivos] CHECK CONSTRAINT [FK_dbo.Dispositivos_dbo.FuentesEnergeticas_FuenteID]
GO
ALTER TABLE [dbo].[Dispositivos]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Dispositivos_dbo.PeriodosFacturacion_PeriodoFacturacionID] FOREIGN KEY([PeriodoFacturacionID])
REFERENCES [dbo].[PeriodosFacturacion] ([PeriodoFacturacionID])
GO
ALTER TABLE [dbo].[Dispositivos] CHECK CONSTRAINT [FK_dbo.Dispositivos_dbo.PeriodosFacturacion_PeriodoFacturacionID]
GO
ALTER TABLE [dbo].[Dispositivos]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Dispositivos_dbo.Sedes_SedeID] FOREIGN KEY([SedeID])
REFERENCES [dbo].[Sedes] ([SedeID])
GO
ALTER TABLE [dbo].[Dispositivos] CHECK CONSTRAINT [FK_dbo.Dispositivos_dbo.Sedes_SedeID]
GO
ALTER TABLE [dbo].[Empresas]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Empresas_dbo.Ciudades_CiudadID] FOREIGN KEY([CiudadID])
REFERENCES [dbo].[Ciudades] ([CiudadID])
GO
ALTER TABLE [dbo].[Empresas] CHECK CONSTRAINT [FK_dbo.Empresas_dbo.Ciudades_CiudadID]
GO
ALTER TABLE [dbo].[Empresas]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Empresas_dbo.SectoresEconomicos_SectorEconomicoID] FOREIGN KEY([SectorEconomicoID])
REFERENCES [dbo].[SectoresEconomicos] ([SectorEconomicoID])
GO
ALTER TABLE [dbo].[Empresas] CHECK CONSTRAINT [FK_dbo.Empresas_dbo.SectoresEconomicos_SectorEconomicoID]
GO
ALTER TABLE [dbo].[Equipos]  WITH CHECK ADD  CONSTRAINT [FK_Equipos_TiposEquipos] FOREIGN KEY([TipoEquipoID])
REFERENCES [dbo].[TiposEquipos] ([TipoEquipoID])
GO
ALTER TABLE [dbo].[Equipos] CHECK CONSTRAINT [FK_Equipos_TiposEquipos]
GO
ALTER TABLE [dbo].[IdentityUserClaims]  WITH CHECK ADD  CONSTRAINT [FK_dbo.IdentityUserClaims_dbo.AspNetUsers_IdentityUser_Id] FOREIGN KEY([IdentityUser_Id])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[IdentityUserClaims] CHECK CONSTRAINT [FK_dbo.IdentityUserClaims_dbo.AspNetUsers_IdentityUser_Id]
GO
ALTER TABLE [dbo].[Inventario]  WITH CHECK ADD  CONSTRAINT [FK_Inventario_Asesores] FOREIGN KEY([AsesorID])
REFERENCES [dbo].[Asesores] ([AsesorID])
GO
ALTER TABLE [dbo].[Inventario] CHECK CONSTRAINT [FK_Inventario_Asesores]
GO
ALTER TABLE [dbo].[Inventario]  WITH CHECK ADD  CONSTRAINT [FK_Inventario_Empresas] FOREIGN KEY([EmpresaID])
REFERENCES [dbo].[Empresas] ([EmpresaID])
GO
ALTER TABLE [dbo].[Inventario] CHECK CONSTRAINT [FK_Inventario_Empresas]
GO
ALTER TABLE [dbo].[Inventario]  WITH CHECK ADD  CONSTRAINT [FK_Inventario_Sedes] FOREIGN KEY([SedeID])
REFERENCES [dbo].[Sedes] ([SedeID])
GO
ALTER TABLE [dbo].[Inventario] CHECK CONSTRAINT [FK_Inventario_Sedes]
GO
ALTER TABLE [dbo].[InventarioEquipos]  WITH CHECK ADD  CONSTRAINT [FK_InventarioEquipos_Equipos] FOREIGN KEY([EquipoID])
REFERENCES [dbo].[Equipos] ([EquipoID])
GO
ALTER TABLE [dbo].[InventarioEquipos] CHECK CONSTRAINT [FK_InventarioEquipos_Equipos]
GO
ALTER TABLE [dbo].[InventarioEquipos]  WITH CHECK ADD  CONSTRAINT [FK_InventarioEquipos_Inventario] FOREIGN KEY([InventarioSedeID])
REFERENCES [dbo].[Inventario] ([InventarioSedeID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[InventarioEquipos] CHECK CONSTRAINT [FK_InventarioEquipos_Inventario]
GO
ALTER TABLE [dbo].[Peticiones]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Peticiones_dbo.Empresas_EmpresaID] FOREIGN KEY([EmpresaID])
REFERENCES [dbo].[Empresas] ([EmpresaID])
GO
ALTER TABLE [dbo].[Peticiones] CHECK CONSTRAINT [FK_dbo.Peticiones_dbo.Empresas_EmpresaID]
GO
ALTER TABLE [dbo].[Sedes]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Sedes_dbo.Ciudades_CiudadID] FOREIGN KEY([CiudadID])
REFERENCES [dbo].[Ciudades] ([CiudadID])
GO
ALTER TABLE [dbo].[Sedes] CHECK CONSTRAINT [FK_dbo.Sedes_dbo.Ciudades_CiudadID]
GO
ALTER TABLE [dbo].[Sedes]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Sedes_dbo.Empresas_EmpresaID] FOREIGN KEY([EmpresaID])
REFERENCES [dbo].[Empresas] ([EmpresaID])
GO
ALTER TABLE [dbo].[Sedes] CHECK CONSTRAINT [FK_dbo.Sedes_dbo.Empresas_EmpresaID]
GO
/****** Object:  StoredProcedure [dbo].[spDashboard]    Script Date: 13/03/2018 8:52:12 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spDashboard]

	--declare 

	@Año int,
	@Mes int,
	@EmpresaID int,
	@SedeID int,
	@DispositivoID int,
	@FuenteID int

AS

SELECT      
			--,Empresas.Nombre AS Empresa
			--,Sedes.NombreSede AS Sede
			--,Dispositivos.Nombre AS Dispositivo
			--YEAR(Consumos.FechaInicial) AS Ano
			FuentesEnergeticas.Fuente
			,Meses.MesID
			,Meses.Mes
            ,SUM(Consumos.LineaBase) AS LineaBase
            ,SUM(Consumos.ConsumoPeriodo) AS ConsumoPeriodo
			,SUM(Consumos.Valor) AS Valor
			,SUM(Consumos.ValorUnitario) AS ValorUnitario
			,AVG(Consumos.ValorUnitario) AS ValorUnitarioPromedio
			--,AVG(Consumos.ConsumoPeriodo) AS PromedioConsumoPeriodo
			--,AVG(Consumos.Valor) AS Valor, AVG(Consumos.Valor) AS PromedioValor
FROM        Empresas 
			INNER JOIN Sedes ON Empresas.EmpresaID = Sedes.EmpresaID 
			INNER JOIN Dispositivos ON Sedes.SedeID = Dispositivos.SedeID 
			INNER JOIN FuentesEnergeticas ON FuentesEnergeticas.FuenteID = Dispositivos.FuenteID
			INNER JOIN Consumos ON Dispositivos.DispositivoID = Consumos.DispositivoID
			INNER JOIN Meses ON MONTH(Consumos.FechaInicial) = Meses.MesID
WHERE		(@Año = 0 OR YEAR(Consumos.FechaInicial) = @Año)
			AND (@Mes = 0  OR MONTH(Consumos.FechaInicial) = @Mes) 
			AND (@EmpresaID = 0 OR Empresas.EmpresaID = @EmpresaID) 
			AND (@SedeID = 0 OR Sedes.SedeID = @SedeID) 
			AND (@DispositivoID = 0 OR Dispositivos.DispositivoID = @DispositivoID)
			AND (@FuenteID = 0 OR FuentesEnergeticas.FuenteID = @FuenteID)
GROUP BY Meses.MesID, Meses.Mes, FuentesEnergeticas.Fuente -- Empresas.Nombre, Sedes.NombreSede, Dispositivos.Nombre, 
ORDER BY 1, Meses.MesID




GO
/****** Object:  StoredProcedure [dbo].[spDashboard_AhorroEnergia]    Script Date: 13/03/2018 8:52:12 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spDashboard_AhorroEnergia]

	@Año int,
	@Mes int,
	@EmpresaID int,
	@SedeID int,
	@DispositivoID int,
	@FuenteID int

AS

WITH cetAhorro (Fuente, MesID, Mes, LineaBase, LineaBaseFull, ConsumoPeriodo, Diferencia, AhorroPeriodo)
AS
(
	SELECT      
				--,Empresas.Nombre AS Empresa
				--,Sedes.NombreSede AS Sede
				--,Dispositivos.Nombre AS Dispositivo
				--YEAR(Consumos.FechaInicial) AS Ano
				FuentesEnergeticas.Fuente
				,Meses.MesID
				,Meses.Mes
				,SUM(Consumos.LineaBase * 0.15) AS LineaBase
				,SUM(Consumos.LineaBase) AS LineaBaseFull
				,SUM(Consumos.ConsumoPeriodo) AS ConsumoPeriodo
				,SUM(Consumos.LineaBase - Consumos.ConsumoPeriodo) AS Diferencia
				,CASE WHEN SUM(Consumos.LineaBase - Consumos.ConsumoPeriodo) > 0 THEN SUM(Consumos.LineaBase - Consumos.ConsumoPeriodo) ELSE 0 END AS AhorroPeriodo
				--,SUM(Consumos.Valor) AS Valor
				--,SUM(Consumos.ValorUnitario) AS ValorUnitario
				--,AVG(Consumos.ValorUnitario) AS ValorUnitarioPromedio
				--,AVG(Consumos.ConsumoPeriodo) AS PromedioConsumoPeriodo
				--,AVG(Consumos.Valor) AS Valor, AVG(Consumos.Valor) AS PromedioValor
	FROM        Empresas 
				INNER JOIN Sedes ON Empresas.EmpresaID = Sedes.EmpresaID 
				INNER JOIN Dispositivos ON Sedes.SedeID = Dispositivos.SedeID 
				INNER JOIN FuentesEnergeticas ON FuentesEnergeticas.FuenteID = Dispositivos.FuenteID
				INNER JOIN Consumos ON Dispositivos.DispositivoID = Consumos.DispositivoID
				INNER JOIN Meses ON MONTH(Consumos.FechaInicial) = Meses.MesID
	WHERE		(Empresas.EmpresaID = @EmpresaID) 
				AND (@Año = 0 OR YEAR(Consumos.FechaInicial) = @Año)
				AND (@Mes = 0  OR MONTH(Consumos.FechaInicial) = @Mes) 
				AND (@SedeID = 0 OR Sedes.SedeID = @SedeID) 
				AND (@DispositivoID = 0 OR Dispositivos.DispositivoID = @DispositivoID)
				AND (@FuenteID = 0 OR FuentesEnergeticas.FuenteID = @FuenteID)
	GROUP BY Meses.MesID, Meses.Mes, FuentesEnergeticas.Fuente -- Empresas.Nombre, Sedes.NombreSede, Dispositivos.Nombre, 
)
SELECT *, SUM(AhorroPeriodo) OVER(PARTITION BY Fuente, MesID ORDER BY Fuente, MesID ROWS UNBOUNDED PRECEDING) AS AhorroPeriodoAcumulado FROM cetAhorro
ORDER BY Fuente, MesID


GO
/****** Object:  StoredProcedure [dbo].[spDashboard_ConsumoVsLineaBase]    Script Date: 13/03/2018 8:52:12 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spDashboard_ConsumoVsLineaBase]

	--declare 

	@Año int,
	@Mes int,
	@EmpresaID int,
	@SedeID int,
	@DispositivoID int,
	@FuenteID int

AS

SELECT      
			FuentesEnergeticas.Fuente
			,FuentesEnergeticas.UnidadMedida
			,Meses.MesID
			,Meses.Mes
            ,SUM(Consumos.LineaBase) AS LineaBase
            ,SUM(Consumos.ConsumoPeriodo) AS ConsumoPeriodo
			,SUM(Consumos.Valor) AS Valor
			,SUM(Consumos.ValorUnitario) AS ValorUnitario
			,AVG(Consumos.ValorUnitario) AS ValorUnitarioPromedio
			--,AVG(Consumos.ConsumoPeriodo) AS PromedioConsumoPeriodo
			--,AVG(Consumos.Valor) AS Valor, AVG(Consumos.Valor) AS PromedioValor
FROM        Empresas 
			INNER JOIN Sedes ON Empresas.EmpresaID = Sedes.EmpresaID 
			INNER JOIN Dispositivos ON Sedes.SedeID = Dispositivos.SedeID 
			INNER JOIN FuentesEnergeticas ON FuentesEnergeticas.FuenteID = Dispositivos.FuenteID
			INNER JOIN Consumos ON Dispositivos.DispositivoID = Consumos.DispositivoID
			INNER JOIN Meses ON MONTH(Consumos.FechaInicial) = Meses.MesID
WHERE		(Empresas.EmpresaID = @EmpresaID) 
			AND (@Año = 0 OR YEAR(Consumos.FechaInicial) = @Año)
			AND (@Mes = 0  OR MONTH(Consumos.FechaInicial) = @Mes) 
			AND (@SedeID = 0 OR Sedes.SedeID = @SedeID) 
			AND (@DispositivoID = 0 OR Dispositivos.DispositivoID = @DispositivoID)
			AND (@FuenteID = 0 OR FuentesEnergeticas.FuenteID = @FuenteID)
GROUP BY Meses.MesID, Meses.Mes, FuentesEnergeticas.Fuente, FuentesEnergeticas.UnidadMedida -- Empresas.Nombre, Sedes.NombreSede, Dispositivos.Nombre, 
ORDER BY Meses.MesID


GO
/****** Object:  StoredProcedure [dbo].[spDashboardIndicadores]    Script Date: 13/03/2018 8:52:12 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spDashboardIndicadores]

--declare 
	@Año int,
	@Mes int,
	@EmpresaID int,
	@SedeID int,
	@DispositivoID int,
	@FuenteID int

AS

	declare @ConsumoMes decimal
	declare @ConsumoAnual decimal
	declare @LineaBaseMes decimal
	declare @LineaBaseAnual decimal
	declare @ValorConsumoMes decimal
	declare @ValorConsumoAnual decimal
	declare @AhorroMes float
	declare @AhorroAnual decimal
	declare @Mes1 int
	declare @fecha date

	declare @FuenteEnergetica nvarchar(30)
	declare @UnidadMedida nvarchar(15)

	IF @Mes = 0
	BEGIN
		SELECT	@Fecha = MAX(Consumos.FechaInicial)
		FROM	Consumos 
				INNER JOIN Dispositivos ON Dispositivos.DispositivoID = Consumos.DispositivoID
				INNER JOIN FuentesEnergeticas ON FuentesEnergeticas.FuenteID = Dispositivos.FuenteID
				INNER JOIN Sedes ON Sedes.SedeID = Dispositivos.SedeID 
				INNER JOIN Empresas ON Empresas.EmpresaID = Sedes.EmpresaID 
		WHERE	(Empresas.EmpresaID = @EmpresaID)
				AND (@Año = 0 OR YEAR(Consumos.FechaInicial) = @Año)
				AND (@SedeID = 0 OR Sedes.SedeID = @SedeID) 
				AND (@FuenteID = 0 OR FuentesEnergeticas.FuenteID = @FuenteID)
				AND (@DispositivoID = 0 OR Dispositivos.DispositivoID = @DispositivoID)

		SET @Año = YEAR(@fecha)
		SET @Mes1 = MONTH(@fecha)
	END
	ELSE
		SET @Mes1 = @Mes

	SELECT	@FuenteEnergetica = FuentesEnergeticas.Fuente,
			@UnidadMedida = FuentesEnergeticas.UnidadMedida,
			@ConsumoMes = SUM(ConsumoPeriodo),
			@LineaBaseMes = SUM(Consumos.LineaBase)
	FROM	Consumos 
			INNER JOIN Dispositivos ON Dispositivos.DispositivoID = Consumos.DispositivoID
			INNER JOIN FuentesEnergeticas ON FuentesEnergeticas.FuenteID = Dispositivos.FuenteID
			INNER JOIN Sedes ON Sedes.SedeID = Dispositivos.SedeID 
			INNER JOIN Empresas ON Empresas.EmpresaID = Sedes.EmpresaID 
	WHERE	(Empresas.EmpresaID = @EmpresaID)
			AND (@Año = 0 OR YEAR(Consumos.FechaInicial) = @Año)
			AND (@Mes1 = 0 OR MONTH(Consumos.FechaInicial) = @Mes1) 
			AND (@SedeID = 0 OR Sedes.SedeID = @SedeID) 
			AND (@FuenteID = 0 OR FuentesEnergeticas.FuenteID = @FuenteID)
			AND (@DispositivoID = 0 OR Dispositivos.DispositivoID = @DispositivoID)
	GROUP BY FuentesEnergeticas.Fuente, FuentesEnergeticas.UnidadMedida

	IF (@LineaBaseMes - @ConsumoMes) > 0 
		SET @AhorroMes = (@LineaBaseMes - @ConsumoMes)
	ELSE
		SET @AhorroMes = 0
		
	SELECT	@ConsumoAnual = SUM(ConsumoPeriodo),
			@LineaBaseAnual = SUM(Consumos.LineaBase)
	FROM	Consumos 
			INNER JOIN Dispositivos ON Dispositivos.DispositivoID = Consumos.DispositivoID
			INNER JOIN FuentesEnergeticas ON FuentesEnergeticas.FuenteID = Dispositivos.FuenteID
			INNER JOIN Sedes ON Sedes.SedeID = Dispositivos.SedeID 
			INNER JOIN Empresas ON Empresas.EmpresaID = Sedes.EmpresaID 
	WHERE	(Empresas.EmpresaID = @EmpresaID)
			AND (@Año = 0 OR YEAR(Consumos.FechaInicial) = @Año)
			AND (@Mes = 0 OR MONTH(Consumos.FechaInicial) <= @Mes) 
			AND (@SedeID = 0 OR Sedes.SedeID = @SedeID) 
			AND (@FuenteID = 0 OR FuentesEnergeticas.FuenteID = @FuenteID)
			AND (@DispositivoID = 0 OR Dispositivos.DispositivoID = @DispositivoID)

	IF (@LineaBaseAnual - @ConsumoAnual)  > 0
		IF ((@LineaBaseAnual * 0.15) - (@LineaBaseAnual - @ConsumoAnual)) > 0 
			SET @AhorroAnual = (@LineaBaseAnual * 0.15) - (@LineaBaseAnual - @ConsumoAnual)
		ELSE
			SET @AhorroAnual = 0
	ELSE
		SET @AhorroAnual = 0

SELECT 
	@FuenteEnergetica FuenteEnergetica,
	@UnidadMedida UnidadMedida,
	ISNULL(@ConsumoMes, 0)			ConsumoMes,
	ISNULL(@ConsumoAnual, 0)		ConsumoAnual,
	ISNULL(@LineaBaseMes, 0)		LineaBaseMes,
	ISNULL(@LineaBaseAnual, 0)		LineaBaseAnual,
	--ISNULL(@ValorConsumoMes, 0)		ValorConsumoMes,
	--ISNULL(@ValorConsumoAnual, 0)	ValorConsumoAnual,
	ISNULL(@AhorroMes, 0)			AhorroMes,
	ISNULL(@AhorroAnual, 0)			AhorroAnual


GO
/****** Object:  StoredProcedure [dbo].[spReporteAhorroAcumulado]    Script Date: 13/03/2018 8:52:12 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spReporteAhorroAcumulado]

	@EmpresaID int,
	@SedeID int,
	@DispositivoID int,
	@FuenteID int,
	@Desde smalldatetime,
	@Hasta smalldatetime

AS

WITH cetAhorro (Codigo, Empresa, Sede, Dispositivo, Ano, Fuente, MesID, Mes, LineaBase, LineaBaseFull, ConsumoPeriodo, Diferencia, AhorroPeriodo, Valor, ValorUnitario)
AS
(
	SELECT      
				Empresas.Codigo
				,Empresas.Nombre
				,Sedes.NombreSede
				,Dispositivos.Nombre
				,YEAR(Consumos.FechaInicial)
				,FuentesEnergeticas.Fuente
				,Meses.MesID
				,Meses.Mes
				,Consumos.LineaBase * 0.15 AS LineaBase
				,Consumos.LineaBase AS LineaBaseFull
				,Consumos.ConsumoPeriodo AS ConsumoPeriodo
				,Consumos.LineaBase - Consumos.ConsumoPeriodo AS Diferencia
				,CASE WHEN Consumos.LineaBase - Consumos.ConsumoPeriodo > 0 THEN Consumos.LineaBase - Consumos.ConsumoPeriodo ELSE 0 END AS AhorroPeriodo
				,Consumos.Valor AS Valor
				,Consumos.ValorUnitario AS ValorUnitario
	FROM        Empresas 
				INNER JOIN Sedes ON Empresas.EmpresaID = Sedes.EmpresaID 
				INNER JOIN Dispositivos ON Sedes.SedeID = Dispositivos.SedeID 
				INNER JOIN FuentesEnergeticas ON FuentesEnergeticas.FuenteID = Dispositivos.FuenteID
				INNER JOIN Consumos ON Dispositivos.DispositivoID = Consumos.DispositivoID
				INNER JOIN Meses ON MONTH(Consumos.FechaInicial) = Meses.MesID
	WHERE	   (@EmpresaID = 0 OR Empresas.EmpresaID = @EmpresaID) 
				AND (@SedeID = 0 OR Sedes.SedeID = @SedeID) 
				AND (@DispositivoID = 0 OR Dispositivos.DispositivoID = @DispositivoID)
				AND (@FuenteID = 0 OR FuentesEnergeticas.FuenteID = @FuenteID)
				AND (Consumos.FechaInicial BETWEEN @Desde AND @Hasta)
	--GROUP BY Empresas.Nombre, Sedes.NombreSede, Dispositivos.Nombre, YEAR(Consumos.FechaInicial), Meses.MesID, Meses.Mes, FuentesEnergeticas.Fuente
),
cetAhorroAcumulado (Codigo, Empresa, Sede, Dispositivo, Ano, Fuente, MesID, Mes, LineaBase, LineaBaseFull, ConsumoPeriodo, Diferencia, AhorroPeriodo, Valor, ValorUnitario, AhorroPeriodoAcumulado)
AS
(
SELECT *, SUM(AhorroPeriodo) OVER(PARTITION BY Empresa, Sede, Dispositivo, Ano, Fuente ORDER BY Fuente, MesID ROWS UNBOUNDED PRECEDING) AS AhorroPeriodoAcumulado FROM cetAhorro
--ORDER BY Empresa, Sede, Dispositivo, Ano, Fuente, MesID
)
SELECT *, CASE WHEN LineaBaseFull > 0 THEN AhorroPeriodoAcumulado / LineaBaseFull ELSE 0 END AS Indicador FROM cetAhorroAcumulado

GO
