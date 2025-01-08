# Sistema de Gestión de Turnos para Profesionales 

Este proyecto es un sistema de gestión turnos desarrollado en .NET Framework (Front con ASP y Back con C#). Permite gestionar turnos, disponibilidad, profesionales, especialidades y pacientes. El sistema incluye funcionalidades de autenticación con Google Auth, ABMs (Altas, Bajas, Modificaciones) y vistas diferenciadas para administración y usuarios.

# Indice 
- [Funcionalidades y roles principales]
- [Requisitos]
- [Instalación]
- [Credenciales]
- [Desarrolladores]

---

# Funcionalidades y roles principales

Administrador:
- **Gestión de Profesionales**: Alta, baja, modificación, asociación de profesionales con Especialidades y su disponibilidades y visualización de profesionales activos e inactivos.
- **Gestión de Especialidades**: Alta, baja, modificación y visualización de especialidades activas e inactivas.
- **Gestión de Disponibilidad**: Alta, baja, modificación, de disponibilidad de profesionales dentro de horarios especificos y visualización.
- **Gestión de Pacientes**: Alta, baja, modificación, asociaciando a estos con Turnos y visualización de pacientes activos e inactivos.
- **Gestión de Turnos**: Alta, baja, modificación, asociación con especialidad, profesionales, respetando la disponibildad, el estado de los turnos es "pendiente", hasta que el administrador los confirme y visualización de turnos pendientes, confirmados y cancelados.
- **Sistema de Autenticación**: con credenciales especiales para administrador y con Google Auth para pacientes.


Paciente: 
- **Turnos**: visualiza un buscador de turnos donde podra ver por el DNI deseado.
- **Turnos del Paciente**: visualiza un listado del historial de turnos del paciente.
- **Perfil**: visualiza detalles de su cuenta.

---

# Requisitos
Para ejecutar este proyecto, necesitarás:
- **.NET**: Versión 8.0 o superior.
- **Base de Datos**: SQL Server Management Studio 20 o superior.
- **Google Auth**: Configuración para integración de autenticación con Google.
---

# Instalación

1. **Clonar el Repositorio**:
Clonar el repositorio en la ubicación deseada. git clone https://github.com/alanfrancoa/TurneraWeb.git
  

2. Configurar la Base de Datos:
Crea una base de datos en SQL Server y ajusta las cadenas de conexión en los archivos de configuración de la aplicación.

3.Configurar Variables de Entorno: 
Edita el archivo appsettings.json:

 {
  "ConnectionStrings": {
    "DefaultConnection": " "ConnectionString": "Server=tcp:servidor-unlz-tpturnera.database.windows.net,1433;Initial Catalog=turnera_db;Persist Security Info=False;User ID=adminturnera;Password=Admin1234;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;""
  },

  (Claves provistas en Google Console Cloud)
  "GoogleAuth": {
    "ClientId": "your-client-id", 
    "ClientSecret": "your-client-secret"
  },
  "AppUrl": "http://localhost:5000" (URL del localhost de tu proyecto)
}

4. Configurar el Servidor: Asegurate de que tu servidor web apunte a la carpeta de inicio de tu proyecto.

5. Iniciar el Proyecto:
Abre el proyecto en Visual Studio, presiona F5 para ejecutar el proyecto.

6. Accede a la aplicación desde tu navegador en:
http://localhost:5000

---

# Credenciales

Para acceder a las funciones de administración, utiliza las siguientes credenciales:

Usuario: admin
Contraseña: 123456

