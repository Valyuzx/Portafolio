#  Portafolio — Full Stack App

> **Estado:**  En desarrollo activo

Aplicación web full stack para gestionar y mostrar proyectos de portafolio personal. Permite registrar proyectos, categorizarlos, asociarles tecnologías y controlar su visibilidad de publicación. Cuenta con autenticación segura mediante JWT con soporte de Refresh Tokens.

---

##  Stack Tecnológico

| Capa            | Tecnología                                       |
|-----------------|--------------------------------------------------|
| Backend         | .NET 10 (ASP.NET Core Web API)                   |
| ORM             | Entity Framework Core 10 + Npgsql               |
| Base de Datos   | PostgreSQL 17+                                   |
| Autenticación   | JWT Bearer + Refresh Tokens + BCrypt             |
| Logging         | Serilog (consola + archivos con rotación diaria) |
| Frontend        | Angular 22 (Standalone Components)              |
| UI Library      | Angular Material 22 + Angular CDK               |
| Package Manager | pnpm 11                                          |
| Testing FE      | Vitest                                           |
| Formato         | Prettier                                         |

---

##  Estructura del Repositorio

```
PORTAFOLIO/
├── BACKEND/               # ASP.NET Core Web API
│   ├── BuissnesLayer/     # Lógica de negocio (interfaces + implementaciones)
│   ├── Controllers/       # Endpoints HTTP
│   ├── DTOs/              # Data Transfer Objects (RequestDTOs / ResponseDTOs)
│   ├── Exceptions/        # Excepciones personalizadas
│   ├── Helpers/           # Servicios auxiliares (TokenService, RefreshTokenService, etc.)
│   ├── Middlewares/       # GlobalExceptionHandler (RFC 7807)
│   ├── Migrations/        # Migraciones de EF Core
│   ├── Models/            # Entidades del dominio
│   └── Program.cs         # Composición raíz del servicio
│
└── FRONTEND/              # Angular 22 SPA
    └── src/
        ├── app/
        │   ├── core/       # Guards, interceptors, seguridad, constantes
        │   ├── features/   # Módulos por funcionalidad (auth, home, project)
        │   └── shared/     # Componentes, pipes y directivas reutilizables
        ├── assets/
        └── enviroment/     # Variables de entorno por ambiente
```

---

## Backend — Buenas Prácticas Implementadas

### Autenticación y Seguridad

- **JWT con Access Token + Refresh Token**: El Access Token expira en **10 minutos**; el Refresh Token en **1 hora**, minimizando la superficie de ataque ante robo de tokens.
- **Refresh Token Rotation**: Cada vez que se usa un Refresh Token, se genera uno nuevo e invalida el anterior.
- **BlackList de Tokens**: Los tokens revocados al hacer logout se almacenan en base de datos y son validados en cada request mediante un Middleware dedicado (`BlackListMiddleware`), ejecutado inmediatamente después de `UseAuthentication`.
- **Limpieza automática de BlackList**: Un `IHostedService` (`BlackListCleanupService`) se ejecuta en segundo plano eliminando periódicamente los tokens ya expirados.
- **Hashing de contraseñas**: Uso de `BCrypt.Net-Next` para hash seguro con sal aleatoria.

###  Arquitectura en Capas

```
Controllers → Business Layer (BL) → Data (DbContext)
```

- Las **interfaces** (`IAccountBL`, `IProjectBL`, `ICategoryBL`, `ITechnologyBL`) desacoplan la implementación de los controladores, facilitando el testing y el intercambio de implementaciones.
- **DTOs separados por dirección**: Carpetas `RequestDTOs` y `ResponseDTOs` para mantener contratos de entrada/salida claros y explícitos.
- 
###  API y Estándares

- **Versionado de API**: Todos los endpoints siguen el patrón `/api/v1/...`
- **Manejo global de excepciones**: Implementación de `IExceptionHandler` (nativo .NET 8+) que serializa los errores siguiendo el estándar **RFC 7807** (`ProblemDetails`), garantizando respuestas de error consistentes en toda la API.
- **Swagger/OpenAPI**: Disponible en entorno de desarrollo con soporte de autenticación Bearer integrado en la UI.
- **CORS configurado** para los orígenes del frontend (`localhost:4200`, `localhost:5012`, `localhost:3000`).

### Logging con Serilog

- Configurado **enteramente desde `appsettings.json`**, sin código de configuración manual en `Program.cs` (`configuration.ReadFrom.Configuration(...)`).
- Salida dual: **consola** (para desarrollo) y **archivos con rotación diaria** en `Logs/log-.txt` (para auditoría).
- Formato **JSON compacto** en archivos, listo para integración con herramientas como Seq, Elasticsearch o Azure Monitor.
- Niveles filtrados por namespace para reducir ruido: e.g., `Microsoft.EntityFrameworkCore.Database.Command` → `Warning`.

###  Autorización

- **Políticas declarativas** registradas en `Program.cs`:
  - `UserPolicy`: Requiere usuario autenticado.
  - `AdminPolicy`: Requiere rol `Admin`.
- Fácilmente extensibles para nuevos roles o requerimientos específicos.

---

## Frontend — Buenas Prácticas Implementadas

### Arquitectura Angular Moderna

- **Standalone Components** (sin NgModule): Cada componente declara sus propias dependencias importadas, reduciendo el acoplamiento y mejorando el tree-shaking.
- **Lazy Loading por feature**: Todas las rutas cargan sus componentes con `loadComponent()`, reduciendo el bundle inicial de la aplicación.
- **Separación por capas**:
  - `core/` → Lógica transversal (guards, interceptors, servicios de seguridad, constantes)
  - `features/` → Funcionalidades de negocio organizadas por dominio (auth, home, project)
  - `shared/` → Componentes, pipes y directivas compartidas entre features

### Seguridad en el Cliente

- **`authGuard`**: Protege rutas que requieren sesión activa; redirige a `/login` si no existe token válido.
- **`noAuthGuard`**: Evita que usuarios ya autenticados accedan a `/login` o `/register`, redirigiendo al dashboard.
- **`auth.interceptor`**: Adjunta automáticamente el JWT en el header `Authorization: Bearer <token>` de cada petición HTTP saliente, centralizando la lógica sin repetirla en cada servicio.

### Herramientas de Desarrollo

- **pnpm** como gestor de paquetes: más rápido y eficiente en espacio en disco que npm/yarn gracias a su store centralizado.
- **Prettier** para formateo de código uniforme y consistente en todo el equipo.
- **Vitest** para pruebas unitarias: más rápido que el stack tradicional Karma + Jasmine al usar Vite como bundler.
- **Variables de entorno** separadas por ambiente en `src/enviroment/` para un fácil manejo de configuración entre desarrollo y producción.

---

## Modelo de Datos
<img width="795" height="586" alt="imagen" src="https://github.com/user-attachments/assets/c014172f-4707-4d38-89f4-18720759f3e2" />

```
User ──< UserRole >── Role
Project ──< ProjectTechnology >── Technology
Project >── Category
User ──< RefreshToken
BlackList  (tokens revocados)
ContactMessage  (mensajes de contacto)
```

| Entidad             | Descripción                                          |
|---------------------|------------------------------------------------------|
| `User`              | Usuarios del sistema                                 |
| `Role`              | Roles disponibles (`Admin`, `User`, etc.)            |
| `UserRole`          | Tabla pivote usuario ↔ rol                           |
| `Project`           | Proyectos del portafolio                             |
| `Category`          | Categorías de proyectos (ej. Web, Mobile, Backend)   |
| `Technology`        | Tecnologías usadas (ej. Angular, .NET, PostgreSQL)   |
| `ProjectTechnology` | Tabla pivote proyecto ↔ tecnología                   |
| `RefreshToken`      | Refresh Tokens activos por usuario                   |
| `BlackList`         | Tokens JWT revocados (logout / expirados)            |
| `ContactMessage`    | Mensajes enviados desde el formulario de contacto    |

---

## Configuración y Levantamiento Local

### Requisitos Previos

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Node.js 22+](https://nodejs.org/)
- [pnpm](https://pnpm.io/) → `npm install -g pnpm`
- [PostgreSQL 17+](https://www.postgresql.org/download/)
- [dotnet-ef](https://learn.microsoft.com/ef/core/cli/dotnet) → `dotnet tool install --global dotnet-ef`

---

### Backend

#### 1. Crear base de datos PostgreSQL

```sql
CREATE DATABASE "DBPortafolio";
ALTER USER postgres PASSWORD 'postgres';
```

#### 2. Revisar `appsettings.Development.json`

El archivo ya tiene una configuración por defecto lista para desarrollo local:

```json
{
  "ConnectionStrings": {
    "ApplicationDbContext": "Host=localhost;Port=5432;Database=DBPortafolio;Username=postgres;Password=postgres;"
  },
  "Jwt": {
    "Secret": "EstaEsUnaClaveSuperSecretaYMuyLarga...",
    "Issuers": "https://localhost:5012",
    "Audience": "portfolio-client",
    "AccessTokenExpiryMinutes": 10,
    "RefreshTokenExpiryHours": 1
  }
}
```
EN OTRO CASO GENERAR UN SERVICIO PARA LAS CREDENDIALES O USAR VAULT O AZURE
>  **Importante**: Nunca expongas la clave JWT real en el repositorio. En producción, usa variables de entorno o `dotnet user-secrets`.

#### 3. Aplicar migraciones y ejecutar

```bash
cd BACKEND

# Aplicar migraciones a la base de datos
dotnet ef database update

# Levantar en modo desarrollo
dotnet run

```
<img width="1861" height="1070" alt="imagen" src="https://github.com/user-attachments/assets/3e362324-3c09-43f5-a5a5-4fe125a46f48" />

El API quedará disponible en:
- **API Base**: `https://localhost:5012`
- **Swagger UI**: `https://localhost:5012/swagger`

---

### Frontend

```bash
cd FRONTEND

# Instalar dependencias
pnpm install

# Levantar servidor de desarrollo
pnpm start
```

La aplicación estará disponible en: `http://localhost:4200`

---

## Endpoints Principales

| Método   | Endpoint                      | Descripción                        | Acceso       |
|----------|-------------------------------|------------------------------------|--------------|
| `POST`   | `/api/v1/auth/register`       | Registro de nuevo usuario          | Público      |
| `POST`   | `/api/v1/auth/login`          | Login (devuelve JWT + Refresh)     | Público      |
| `POST`   | `/api/v1/auth/refresh`        | Renovar Access Token               | Público      |
| `POST`   | `/api/v1/auth/logout`         | Revocar tokens activos             | Autenticado  |
| `GET`    | `/api/v1/projects`            | Listar proyectos                   | Autenticado  |
| `POST`   | `/api/v1/projects`            | Crear proyecto                     | Admin        |
| `PUT`    | `/api/v1/projects/{id}`       | Actualizar proyecto                | Admin        |
| `DELETE` | `/api/v1/projects/{id}`       | Eliminar proyecto                  | Admin        |
| `GET`    | `/api/v1/categories`          | Listar categorías                  | Autenticado  |
| `GET`    | `/api/v1/technologies`        | Listar tecnologías                 | Autenticado  |
| `POST`   | `/api/v1/contact`             | Enviar mensaje de contacto         | Público      |

> 📖 Documentación completa e interactiva disponible en Swagger al levantar el backend.

---

## Rutas del Frontend

| Ruta                      | Descripción                        | Guard          |
|---------------------------|------------------------------------|----------------|
| `/login`                  | Inicio de sesión                   | `noAuthGuard`  |
| `/register`               | Registro de usuario                | `noAuthGuard`  |
| `/home`                   | Dashboard principal                | `authGuard`    |
| `/home/projects`          | Listado de proyectos               | `authGuard`    |
| `/home/projects/create`   | Formulario de creación             | `authGuard`    |

---

## Pendientes / Roadmap

### Backend
- [ ] Endpoint para actualizar perfil de usuario
- [ ] Paginación en listados (proyectos, tecnologías, categorías)
- [ ] Upload de imágenes (integración con almacenamiento en nube)
- [ ] Endpoint de contacto con envío real de email (SMTP / SendGrid)
- [ ] Rate Limiting en endpoints públicos
- [ ] Pruebas unitarias e integración

### Frontend
- [ ] Vista pública del portafolio (sin autenticación requerida)
- [ ] Gestión de categorías y tecnologías desde la UI admin
- [ ] Interceptor con lógica de Refresh Token automático (retry en 401)
- [ ] Componente de carga/previsualización de imágenes
- [ ] Soporte modo oscuro / claro con Angular Material theming
- [ ] Pruebas unitarias con Vitest

---

##  Contribución

Este proyecto es de desarrollo personal, pero las sugerencias son bienvenidas. 
---
