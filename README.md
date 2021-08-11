# Guía para ejecutar localmente la API de la aplicación Su Salud

## Descripción del proyecto:  
El presente proyecto es una API parte de una asignación univesitaria.  Esta API la consume la aplicación móvil [Su Salud](https://github.com/alvifa604/appointments-mobile), la cual es parte de la misma asignación. Este se elaboró utilizando arquitectura limpia, JSON Web Tokens (JWT) para el manejo de la autenticación, y PostgreSQL para el manejo de los datos. 

## Requisitos:
  * Tener instalado [.net 5](https://dotnet.microsoft.com/download)
  * Tener instalado y configurado [PostgreSQL](https://www.postgresql.org/download)
  * Opcional: Tener instalado [Postman](https://www.postman.com/downloads/)

## Pasos para ejecutar el proyecto:
  1. Clonar el proyecto
  2. Cambiar el `ConnectionStrings` en el archivo `appsettings.Development.json` por los parámetros propios de su sistema.
     * Ejemplo: `"DefaultConnection": "Server=localhost; Port=5432; User Id=postgres; Password=password; Database=Example"`
  3. Ejecutar el comando `dotnet run -p api`
  
Una vez ejecutados los pasos anteriores, se realizarán las migraciones automáticamente y podrá comenzar a hacer peticiones a la API utilizando Postman o la herramienta de su preferencia. 
