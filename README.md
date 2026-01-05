# 🏨 API de Reservas

API REST desarrollada en **ASP.NET Core** para la gestión de reservas de recursos (habitaciones, citas, servicios, etc.), con autenticación JWT, roles, reglas de negocio reales y control de disponibilidad.

---

## 🎯 Objetivo del proyecto

Simular un sistema de reservas real, aplicando:
- reglas de negocio
- seguridad por roles
- ownership de recursos
- validaciones de tiempo
- filtros y paginación

Este proyecto forma parte de un roadmap de formación **Backend .NET** orientado a empleabilidad.

---

## 🧠 Reglas de negocio implementadas

- ❌ No se permiten reservas en fechas pasadas
- ❌ No se permiten reservas solapadas para un mismo recurso
- ❌ Un usuario solo puede modificar sus propias reservas
- ⏳ La cancelación está restringida según el tipo de recurso:
  - Tecnológico → mínimo 24 horas antes
  - Médico → mínimo 10 minutos antes
  - Alimenticio → mínimo 3 días antes
- 🛠️ El administrador puede cancelar reservas sin restricciones de tiempo

---

## 🔐 Autenticación y Seguridad

- Autenticación mediante **JWT**
- Roles soportados:
  - `Admin`
  - `User`
- Protección de endpoints por rol
- Ownership aplicado en todas las operaciones de usuario

---

## 🧱 Entidades principales

- Usuario
- Recurso
- Reserva

---

## ⚙️ Funcionalidades

### 👤 Usuario
- Crear reserva
- Cancelar reserva (con validaciones de tiempo)
- Ver sus reservas

### 🛠️ Administrador
- Crear recursos
- Ver todas las reservas
- Filtrar reservas por:
  - rango de fechas
  - recurso
- Paginación obligatoria en listados

---

## 🔍 Filtros y Paginación

Los listados de reservas permiten:
- paginación (`page`, `pageSize`)
- filtrado por fecha (`fechaInicio`, `fechaFinal`)
- ordenamiento por fecha de creación

---

## 🧩 Arquitectura

El proyecto sigue una arquitectura en capas:

- Controllers → manejo HTTP
- Services → lógica de negocio
- Repositories → acceso a datos
- DTOs → contratos de entrada y salida
- Unit of Work → control de persistencia

Toda la lógica de negocio reside exclusivamente en los **Services**.

---

## 🛠️ Tecnologías usadas

- ASP.NET Core
- Entity Framework Core
- SQL Server
- JWT Authentication
- C#
- LINQ
- TimeSpan / DateTime (reglas temporales)

---

## 📌 Notas finales

Este proyecto prioriza:
- claridad
- mantenibilidad
- reglas realistas
- buenas prácticas backend

No es un CRUD básico, sino un sistema con decisiones de negocio explícitas.

---

## 👨‍💻 Autor

Proyecto desarrollado por **Santiago**  
Backend Developer .NET (en formación)
