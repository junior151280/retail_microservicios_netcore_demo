# Demostración de Microservicios con .NET Core y Docker

Este proyecto sirve como una demostración educativa de una arquitectura de microservicios utilizando .NET Core y Docker. Diseñado para principiantes, ilustra los conceptos fundamentales de microservicios, contenedorización y comunicación entre servicios.

## 🏗️ Estructura del Proyecto

La solución está compuesta por tres componentes principales:

- **Servicio de Catálogo**: Microservicio responsable de la gestión y búsqueda de libros.
- **Servicio de Pedidos**: Microservicio que maneja la creación y gestión de pedidos de libros.
- **Frontend Web**: Aplicación web que consume ambos microservicios y presenta una interfaz de usuario.

## 🧩 Componentes Principales

### 1. Microservicio de Catálogo (`catalogo/Catalogo.API`)
- API RESTful para gestión de libros
- Endpoints para listar, buscar y obtener detalles de libros
- Implementado con ASP.NET Core 9.0

### 2. Microservicio de Pedidos (`pedidos/Pedidos.API`)
- API RESTful para gestión de pedidos de libros
- Endpoints para crear pedidos y consultar el estado de los pedidos
- Implementado con ASP.NET Core 9.0

### 3. Frontend (`frontend/WebSite`)
- Aplicación web desarrollada con ASP.NET Core Razor Pages
- Consume los servicios de Catálogo y Pedidos mediante HttpClient
- Interfaces para buscar libros y gestionar pedidos

## 🐳 Configuración Docker

El proyecto incluye configuración Docker para cada componente:

- **Dockerfiles** individuales para cada servicio
- **Docker Compose** para orquestar todos los servicios
- Red compartida para facilitar la comunicación entre contenedores

### Principales archivos de configuración:

- `docker-compose.yml`: Orquestación de todos los servicios
- `catalogo/Catalogo.API/Dockerfile`: Construcción del servicio de catálogo
- `pedidos/Pedidos.API/Dockerfile`: Construcción del servicio de pedidos
- `frontend/WebSite/Dockerfile`: Construcción del frontend web

## 🔄 Comunicación entre Servicios

Los servicios se comunican entre sí mediante API RESTful:

1. El **Frontend** se comunica con ambos microservicios a través de HttpClient configurado
2. Los microservicios son independientes y no se comunican directamente entre sí
3. En Docker, los servicios utilizan nombres de host de red para encontrarse

## 🚀 Cómo Ejecutar el Proyecto

### Utilizando Docker Compose (Recomendado)

```bash
# Navegar a la raíz del proyecto
cd retail_microservicios_netcore_demo

# Construir y levantar todos los servicios
docker-compose up --build
```

Después de ejecutar los comandos, los servicios estarán disponibles en:
- Frontend: http://localhost:5000
- API de Catálogo: http://localhost:5001/api
- API de Pedidos: http://localhost:5002/api

### Ejecución Local (Desarrollo)

```bash
# Ejecutar el servicio de Catálogo
cd catalogo/Catalogo.API
dotnet run

# Ejecutar el servicio de Pedidos
cd pedidos/Pedidos.API
dotnet run

# Ejecutar el Frontend
cd frontend/WebSite
dotnet run
```

## 📚 Conceptos Clave para Principiantes

### ¿Qué son los Microservicios?

Los microservicios son un enfoque arquitectónico donde una aplicación se compone de pequeños servicios independientes que:
- Se ejecutan en procesos separados
- Se comunican mediante mecanismos ligeros (como HTTP/REST)
- Pueden ser desarrollados, desplegados y escalados independientemente
- Suelen estar organizados en torno a capacidades de negocio

### Ventajas de los Microservicios

1. **Despliegue independiente**: Cada servicio puede ser actualizado sin afectar a otros
2. **Escalabilidad precisa**: Escalar solo los servicios que lo necesitan
3. **Diversidad tecnológica**: Diferentes tecnologías pueden usarse para diferentes servicios
4. **Resiliencia**: El fallo de un servicio no provoca el fallo total del sistema
5. **Equipos autónomos**: Los equipos pueden trabajar de forma independiente

### ¿Qué es Docker?

Docker es una plataforma que permite desarrollar, enviar y ejecutar aplicaciones en contenedores:
- **Contenedores**: Entornos aislados y ligeros que incluyen todo lo necesario para ejecutar una aplicación
- **Imágenes**: Plantillas de solo lectura con instrucciones para crear contenedores
- **Docker Compose**: Herramienta para definir y ejecutar aplicaciones multi-contenedor

## 🔧 Tecnologías Utilizadas

- **.NET Core 9.0**: Framework para el desarrollo de los servicios y la aplicación web
- **ASP.NET Core**: Para el desarrollo de APIs web y la aplicación web
- **Docker**: Para la contenerización de los servicios
- **Razor Pages**: Para el desarrollo del frontend
- **Bootstrap**: Para el diseño responsive del frontend

## 📝 Notas Educativas

- Este proyecto es una simplificación para fines didácticos
- En entornos reales, se incluirían bases de datos, autenticación, y más funcionalidades
- Se han omitido prácticas avanzadas como monitoreo, service discovery, y circuit breakers

## 📄 Licencia

Este proyecto está licenciado bajo la licencia MIT - ver el archivo [LICENSE](LICENSE) para más detalles.