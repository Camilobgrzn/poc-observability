# Proyecto de Gestión de Inventario, Facturación y Órdenes

Este proyecto es una solución que incluye tres servicios principales: Inventario, Facturación y Órdenes. Cada servicio está implementado como un proyecto ASP.NET Core Web API.

## Proyectos

1. **Inventario**
   - Ubicación: `Inventario/`
   - Descripción: Servicio para la gestión de inventarios.
   - Puerto: 5001

2. **Facturación**
   - Ubicación: `Facturacion/`
   - Descripción: Servicio para la gestión de facturación.
   - Puerto: 5002

3. **Órdenes**
   - Ubicación: `Ordenes/`
   - Descripción: Servicio para la gestión de órdenes.
   - Puerto: 5003

## Requisitos

- .NET 6.0
- Visual Studio 2022 o superior
- Docker (opcional, para despliegue en contenedores)

## Configuración

Cada proyecto tiene su propio archivo de configuración `appsettings.json` donde puedes ajustar las configuraciones específicas del servicio.

## Ejecución

Para ejecutar los proyectos, puedes usar Visual Studio o la línea de comandos.

### Usando Visual Studio

1. Abre la solución en Visual Studio.
2. Selecciona el proyecto que deseas ejecutar como proyecto de inicio.
3. Presiona `F5` para iniciar el proyecto.

### Usando la Línea de Comandos

Navega al directorio del proyecto que deseas ejecutar y usa el siguiente comando:

dotnet run


## Endpoints

### Inventario

- `GET /WeatherForecast`: Obtiene el pronóstico del tiempo.

### Facturación

- `GET /WeatherForecast`: Obtiene el pronóstico del tiempo.

### Órdenes

- `GET /WeatherForecast`: Obtiene el pronóstico del tiempo.

## Swagger

Cada servicio tiene Swagger configurado para la documentación de la API. Puedes acceder a la documentación de Swagger en las siguientes URLs cuando los servicios están en ejecución:

- Inventario: `http://localhost:5001/swagger`
- Facturación: `http://localhost:5002/swagger`
- Órdenes: `http://localhost:5003/swagger`

## Docker

Cada proyecto está configurado para ser desplegado en contenedores Docker. Puedes construir y ejecutar los contenedores usando los siguientes comandos:

docker build -t inventario ./Inventario docker build -t facturacion ./Facturacion docker build -t ordenes ./Ordenes
docker run -d -p 5001:80 inventario docker run -d -p 5002:80 facturacion docker run -d -p 5003:80 ordenes


## Contribuciones

Las contribuciones son bienvenidas. Por favor, abre un issue o un pull request para discutir cualquier cambio que te gustaría hacer.

## Licencia

Este proyecto está licenciado bajo la Licencia MIT. Consulta el archivo `LICENSE` para más detalles.
