# Proyecto de Gesti�n de Inventario, Facturaci�n y �rdenes

Este proyecto es una soluci�n que incluye tres servicios principales: Inventario, Facturaci�n y �rdenes. Cada servicio est� implementado como un proyecto ASP.NET Core Web API.

## Proyectos

1. **Inventario**
   - Ubicaci�n: `Inventario/`
   - Descripci�n: Servicio para la gesti�n de inventarios.
   - Puerto: 5001

2. **Facturaci�n**
   - Ubicaci�n: `Facturacion/`
   - Descripci�n: Servicio para la gesti�n de facturaci�n.
   - Puerto: 5002

3. **�rdenes**
   - Ubicaci�n: `Ordenes/`
   - Descripci�n: Servicio para la gesti�n de �rdenes.
   - Puerto: 5003

## Requisitos

- .NET 6.0
- Visual Studio 2022 o superior
- Docker (opcional, para despliegue en contenedores)

## Configuraci�n

Cada proyecto tiene su propio archivo de configuraci�n `appsettings.json` donde puedes ajustar las configuraciones espec�ficas del servicio.

## Ejecuci�n

Para ejecutar los proyectos, puedes usar Visual Studio o la l�nea de comandos.

### Usando Visual Studio

1. Abre la soluci�n en Visual Studio.
2. Selecciona el proyecto que deseas ejecutar como proyecto de inicio.
3. Presiona `F5` para iniciar el proyecto.

### Usando la L�nea de Comandos

Navega al directorio del proyecto que deseas ejecutar y usa el siguiente comando:

dotnet run


## Endpoints

### Inventario

- `GET /WeatherForecast`: Obtiene el pron�stico del tiempo.

### Facturaci�n

- `GET /WeatherForecast`: Obtiene el pron�stico del tiempo.

### �rdenes

- `GET /WeatherForecast`: Obtiene el pron�stico del tiempo.

## Swagger

Cada servicio tiene Swagger configurado para la documentaci�n de la API. Puedes acceder a la documentaci�n de Swagger en las siguientes URLs cuando los servicios est�n en ejecuci�n:

- Inventario: `http://localhost:5001/swagger`
- Facturaci�n: `http://localhost:5002/swagger`
- �rdenes: `http://localhost:5003/swagger`

## Docker

Cada proyecto est� configurado para ser desplegado en contenedores Docker. Puedes construir y ejecutar los contenedores usando los siguientes comandos:

docker build -t inventario ./Inventario docker build -t facturacion ./Facturacion docker build -t ordenes ./Ordenes
docker run -d -p 5001:80 inventario docker run -d -p 5002:80 facturacion docker run -d -p 5003:80 ordenes


## Contribuciones

Las contribuciones son bienvenidas. Por favor, abre un issue o un pull request para discutir cualquier cambio que te gustar�a hacer.

## Licencia

Este proyecto est� licenciado bajo la Licencia MIT. Consulta el archivo `LICENSE` para m�s detalles.
