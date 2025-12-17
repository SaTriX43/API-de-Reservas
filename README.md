# API de Reservas (Citas / Turnos)

API REST desarrollada en **ASP.NET Core Web API (.NET 8)** para la gestión de reservas con validación de disponibilidad, control de acceso por roles y reglas de negocio reales.

Este proyecto forma parte de un portafolio backend enfocado a roles **Backend .NET Trainee / Junior**.

---

## 🎯 Objetivo del proyecto

Implementar un sistema de reservas que permita:

* Gestionar usuarios, recursos y reservas
* Evitar solapamiento de horarios
* Validar fechas pasadas
* Aplicar autorización basada en roles y ownership
* Mantener una arquitectura limpia y mantenible

---

## 🧱 Arquitectura

Arquitectura por capas:

* **Controllers** → Exponen endpoints HTTP
* **Services** → Reglas de negocio y validaciones
* **Repositories (DALs)** → Acceso a datos (EF Core)
* **DTOs** → Contratos de entrada y salida
* **Models** → Entidades del dominio
* **Middleware** → Manejo global de errores

---

## 🛠️ Tecnologías utilizadas

* .NET 8 – ASP.NET Core Web API
* Entity Framework Core
* SQL Server
* JWT Authentication
* BCrypt (hash de contraseñas)
* Serilog (logging)
* Swagger / OpenAPI

---

## 🔐 Autenticación y Autorización

* Autenticación mediante **JWT**
* Claims incluidos:

  * UserId
  * Email
  * Rol
* Roles disponibles:

  * **User**
  * **Admin**

### Reglas de acceso

* Un **User**:

  * Solo puede ver y cancelar sus propias reservas
* Un **Admin**:

  * Puede ver todas las reservas
  * Puede cancelar cualquier reserva

---

## 📦 Entidades principales

### Usuario

* Id
* Nombre
* Email
* PasswordHash
* Rol
* FechaCreacion

### Recurso

* Id
* Nombre
* Descripción
* Tipo (Enum)
* Activo
* FechaCreacion

### Reserva

* Id
* UsuarioId
* RecursoId
* FechaInicio
* FechaFinal
* Estado (Activo, Cancelada, etc.)
* FechaCreacion

---

## 📋 Endpoints principales

### Autenticación

* `POST /api/autenticacion/registro`
* `POST /api/autenticacion/login`

### Recursos

* `POST /api/recurso/crear-recurso`

### Reservas

* `POST /api/reservas/crear-reserva`
* `PUT /api/reservas/cancelar-reserva/{reservaId}`
* `GET /api/reservas/obtener-reservas-usuario/{usuarioId}`
* `GET /api/reservas/obtener-reservas-recurso/{recursoId}` (solo Admin)

---

## 🧠 Reglas de negocio implementadas

### Validaciones de fechas

* No se permiten reservas en fechas pasadas
* Fecha final no puede ser anterior a fecha inicio
* Se aplica tolerancia de tiempo para evitar errores por latencia

### Disponibilidad

* No se permiten reservas si el recurso ya está ocupado
* Se valida solapamiento de horarios
* Reservas canceladas no bloquean horarios

---

## ⚠️ Manejo de errores

Middleware global de errores:

* Captura excepciones no controladas
* Retorna respuesta estándar JSON
* Loguea errores con Serilog

---

## ▶️ Cómo ejecutar el proyecto

1. Clonar el repositorio
2. Configurar la cadena de conexión en `appsettings.json`
3. Ejecutar migraciones:

```bash
dotnet ef database update
```

4. Ejecutar el proyecto:

```bash
dotnet run
```

5. Acceder a Swagger:

```
https://localhost:{puerto}/swagger
```

---

## 📌 Estado del proyecto

✔ Funcional
✔ Reglas de negocio completas
✔ Autorización implementada
✔ Arquitectura clara

Próximas mejoras:

* Refresh Tokens (rotación y revocación)
* Paginación en listados
* Tests unitarios

---

## 👨‍💻 Autor

**Santiago González**
Backend .NET Trainee / Junior
Ecuador – LATAM
