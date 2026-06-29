# Modelos de Artefactos de Pruebas

Este documento contiene las plantillas y modelos que se utilizarán para llevar el registro formal de las pruebas de software en el proyecto **Veterinaria Antioquia**. Estos artefactos permiten estandarizar la documentación de la ejecución y los defectos encontrados.

---

## 1. Modelo de Caso de Prueba (Test Case)

Este formato se utiliza para documentar y diseñar cada prueba funcional que se ejecutará manual o automáticamente.

| Campo | Descripción | Ejemplo |
| :--- | :--- | :--- |
| **ID del Caso** | Identificador único del caso de prueba. | `CP-AUTH-001` |
| **Módulo / Componente** | Área funcional que se está probando. | `Autenticación / Login (MAUI)` |
| **Título** | Nombre corto y descriptivo del caso. | `Login exitoso con credenciales válidas` |
| **Precondiciones** | Requisitos que deben cumplirse antes de empezar. | `El usuario debe estar registrado en la BD. El servicio Backend debe estar levantado.` |
| **Pasos de Ejecución** | Lista numerada de acciones que el Tester debe realizar. | `1. Abrir App Android. 2. Ingresar email 'test@vet.com'. 3. Ingresar contraseña '1234'. 4. Presionar "Ingresar".` |
| **Datos de Prueba** | Información de entrada requerida. | `Email: test@vet.com, Pass: 1234` |
| **Resultado Esperado** | El comportamiento que el sistema debería tener si todo funciona bien. | `El sistema valida las credenciales contra la API, recibe token JWT y redirige al usuario a la vista "Dashboard Mascotas".` |
| **Resultado Obtenido** | Lo que realmente ocurrió durante la ejecución de la prueba. | *(A llenar durante la ejecución)* |
| **Estado** | `Aprobado (Pass)`, `Fallido (Fail)`, `Bloqueado (Blocked)` | *(A llenar durante la ejecución)* |

---

## 2. Modelo de Reporte de Incidencia (Bug Report)

Cuando un caso de prueba falla (Estado: `Fallido`), se debe levantar un defecto o "Bug" utilizando este modelo en la herramienta de seguimiento (ej. Jira, GitHub Issues).

| Campo | Descripción | Ejemplo |
| :--- | :--- | :--- |
| **ID del Defecto** | Identificador único del bug. | `BUG-UI-042` |
| **Título** | Resumen conciso del problema. | `Botón de agendar cita no responde en App Android` |
| **Entorno / Plataforma** | Dónde ocurrió el error. | `Android Emulator (API 33), App MAUI v1.0` |
| **Severidad** | Impacto en el sistema (`Crítica`, `Alta`, `Media`, `Baja`). | `Alta` |
| **Pasos para Reproducir** | Secuencia exacta para que un desarrollador pueda recrear el error. | `1. Ir a Mascotas. 2. Seleccionar "Firulais". 3. Tocar "Nueva Cita". 4. El botón parpadea pero no abre el formulario.` |
| **Comportamiento Obtenido** | Qué sucedió erróneamente. | `La UI no reacciona y arroja un error silencioso en consola de MAUI.` |
| **Comportamiento Esperado** | Qué debería haber sucedido. | `Debería abrirse un modal emergente para seleccionar la fecha de la cita.` |
| **Evidencia** | Adjuntos (Capturas de pantalla, videos o logs). | `[Link a video], stacktrace.log` |
| **Asignado a** | Desarrollador responsable de la corrección. | `@DevFrontend` |

---

## 3. Matriz de Trazabilidad (Traceability Matrix)

Documento que asegura que todos los Requerimientos del Sistema (RS) están cubiertos por al menos un Caso de Prueba (CP). Puede llevarse en un archivo Excel/CSV.

| ID Requerimiento | Descripción del Requerimiento | ID Caso(s) de Prueba Asociado(s) | Estado de Cobertura | Estado de Ejecución |
| :--- | :--- | :--- | :--- | :--- |
| `REQ-01` | El sistema debe permitir registro de usuarios. | `CP-AUTH-002`, `CP-AUTH-003` | Cubierto | 50% Pass |
| `REQ-02` | El usuario puede listar sus mascotas registradas. | `CP-MAS-001` | Cubierto | 100% Pass |
| `REQ-03` | Consumo asíncrono de lista de veterinarios disponibles. | `CP-CITAS-005` | Cubierto | No Ejecutado |

---

> **Nota para QA:** Estos modelos pueden ser exportados a herramientas como TestRail, Zephyr, o manejados directamente en hojas de cálculo compartidas durante las fases de ejecución funcionales (Fase 3 y 4 del Plan de Pruebas).
