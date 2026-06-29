# Plan de Pruebas de Software

**Proyecto Veterinaria Antioquia**
**Fecha: 26/06/2026**

---

## Tabla de contenido
- [Historial de Versiones](#historial-de-versiones)
- [Información del Proyecto](#información-del-proyecto)
- [Aprobaciones](#aprobaciones)
- [Resumen Ejecutivo](#resumen-ejecutivo)
- [Alcance de las Pruebas](#alcance-de-las-pruebas)
  - [Elementos de Pruebas](#elementos-de-pruebas)
  - [Nuevas Funcionalidades a Probar](#nuevas-funcionalidades-a-probar)
  - [Pruebas de Regresión](#pruebas-de-regresión)
  - [Funcionalidades a No Probar](#funcionalidades-a-no-probar)
- [Enfoque de Pruebas (Estrategia)](#enfoque-de-pruebas-estrategia)
- [Criterios de Aceptación o Rechazo](#criterios-de-aceptación-o-rechazo)
  - [Criterios de Aceptación o Rechazo](#criterios-de-aceptación-o-rechazo-1)
  - [Criterios de Suspensión](#criterios-de-suspensión)
  - [Criterios de Reanudación](#criterios-de-reanudación)
- [Entregables](#entregables)
- [Recursos](#recursos)
  - [Requerimientos de Entornos – Hardware](#requerimientos-de-entornos--hardware)
  - [Requerimientos de Entornos – Software](#requerimientos-de-entornos--software)
  - [Herramientas de Pruebas Requeridas](#herramientas-de-pruebas-requeridas)
  - [Personal](#personal)
  - [Entrenamiento](#entrenamiento)
- [Planificación y Organización](#planificación-y-organización)
  - [Procedimientos para las Pruebas](#procedimientos-para-las-pruebas)
  - [Matriz de Responsabilidades](#matriz-de-responsabilidades)
  - [Cronograma](#cronograma)
  - [Premisas](#premisas)
  - [Dependencias y Riesgos](#dependencias-y-riesgos)
- [Referencias](#referencias)
- [Glosario](#glosario)

---

## Historial de Versiones
| Fecha | Versión | Autor | Organización | Descripción |
|---|---|---|---|---|
| 26/06/2026 | 1.0 | Analista de QA | Veterinaria Antioquia | Versión inicial del plan de pruebas enfocado en el backend Node.js, frontend Web (Blazor) y frontend multiplataforma (MAUI). |
| 26/06/2026 | 1.1 | Analista de QA | Veterinaria Antioquia | Inclusión del proyecto móvil y de escritorio multiplataforma (.NET MAUI). |

## Información del Proyecto
| Campo | Detalle |
|---|---|
| **Empresa / Organización** | Veterinaria Antioquia |
| **Proyecto** | Sistema Integral de Gestión Veterinaria |
| **Fecha de preparación** | 26/06/2026 |
| **Cliente** | Usuarios internos y clientes de la veterinaria |
| **Patrocinador principal** | Dirección de la Veterinaria |
| **Gerente / Líder de Proyecto** | [Nombre del Gerente] |
| **Gerente / Líder de Pruebas de Software** | [Nombre del Líder QA] |

## Aprobaciones
| Nombre y Apellido | Cargo | Departamento u Organización | Fecha | Firma |
|---|---|---|---|---|
| [Nombre] | Gerente de Proyecto | Tecnología | | |
| [Nombre] | Líder QA | Aseguramiento de Calidad | | |

## Resumen Ejecutivo
El propósito de este Plan de Pruebas de Software es detallar el enfoque, los recursos y la planificación para probar el ecosistema de Veterinaria Antioquia. Este documento abarca la fase de pruebas del Backend (Node.js/Express) y los Frontends: aplicación Web (C# .NET Blazor) y aplicación multiplataforma de escritorio/móvil (.NET MAUI para Android, iOS y Windows). Se busca garantizar la calidad del software, asegurando la integración correcta entre la API de backend, la base de datos MySQL y todas las interfaces de usuario de los clientes (Web, Windows y Móviles).

## Alcance de las Pruebas

### Elementos de Pruebas
1. **Backend (`ApiVeterinaria`):**
   - Endpoints de la API REST para los distintos módulos.
   - Conexión, consultas y persistencia en la Base de Datos MySQL.
   - Lógica de autenticación y seguridad mediante JWT y `bcrypt`.
2. **Frontend Web (`VeterinariaAntioquia.Web` y `Shared`):**
   - Componentes interactivos de interfaz de usuario en Blazor Web (Framework .NET).
   - Consumo asíncrono de los servicios (API) expuestos por el backend.
   - Enrutamiento seguro y restricción de vistas por roles (`Microsoft.AspNetCore.Components.Authorization`).
3. **Frontend Móvil y Escritorio (`VeterinariaAntioquia` / .NET MAUI):**
   - Funcionamiento e instalación de la App nativa en **Windows 10/11**, **Android** (API 24+) e **iOS** (15.0+).
   - Interfaz de usuario adaptable y responsiva alojada mediante componentes Web (Blazor Hybrid / WebView).
   - Interacción nativa del dispositivo, manejo de conectividad y estado de sesión multiplataforma.

### Nuevas Funcionalidades a Probar
*(Estos elementos deben ajustarse a la iteración en desarrollo)*
- Proceso completo de Autenticación, Registro y Login sincronizado entre Web y Móvil/Escritorio.
- Módulos CRUD de Mascotas, Clientes y Citas reflejados en todos los canales.
- Validaciones de formularios e interfaces en tiempo real.
- Instalación, apertura, suspensión y cierre de la aplicación MAUI en los distintos dispositivos.
- Pruebas de responsividad y experiencia de usuario en tamaños de pantalla de teléfonos, tablets y monitores de PC.

### Pruebas de Regresión
- Verificación de módulos existentes que consumen la base de datos ante cambios estructurales.
- Ejecución completa de la suite de pruebas automatizadas al realizar un nuevo Pull Request al repositorio Git.
- Flujo básico de inicio de sesión tras añadir cualquier middleware nuevo al backend o actualizar paquetes de MAUI.

### Funcionalidades a No Probar
- Pasarelas de pago de terceros o servicios externos a la API nativa del proyecto (a menos que existan mocks habilitados).
- Compatibilidad con versiones de Android inferiores a la 7.0 (API 24) o iOS inferiores a la 15.0.
- Rendimiento extremo (pruebas de estrés de altísima concurrencia fuera del alcance funcional base).

## Enfoque de Pruebas (Estrategia)
Se adoptará un enfoque de pruebas en múltiples capas:
1. **Pruebas Unitarias:** 
   - **Backend:** Uso de **Jest** para verificar lógica aislada y funciones core.
   - **Frontend (Web/MAUI/Shared):** Uso de **xUnit** para verificar la lógica compartida en `.NET` y componentes de estado.
2. **Pruebas de Integración (API):**
   - **Backend:** Se usará **Supertest** en conjunto con Jest para simular las peticiones HTTP y validar estados e integridad JSON.
3. **Pruebas Funcionales y Manuales (UI y Móvil):**
   - **Web:** Navegación en navegadores de escritorio (Chrome, Edge, Firefox).
   - **MAUI (Multiplataforma):** Ejecución de casos de prueba manuales mediante emuladores (Android Studio Emulator, simuladores iOS en Mac) y directamente en dispositivos físicos para validar experiencia real táctil y de hardware. Validación de la aplicación instalada nativamente en Windows (`net9.0-windows10.0.19041.0`).
4. **Pruebas de Seguridad (Básicas):** Validación de rutas protegidas asegurando que los Endpoints requieran token JWT en todos los canales de acceso (Web/App).

## Criterios de Aceptación o Rechazo

### Criterios de Aceptación o Rechazo
- **Aceptación:** Las suites automatizadas completan su ejecución de forma exitosa sin fallos (100% test pass rate en la rama principal). Todos los flujos funcionales críticos funcionan según lo esperado tanto en web como en las compilaciones de Windows, iOS y Android.
- **Rechazo:** Existencia de "Bugs Bloqueantes" que impidan flujos core o la compilación (build) y despliegue del proyecto MAUI en alguna de las plataformas objetivo.

### Criterios de Suspensión
- Indisponibilidad prolongada del entorno de pruebas o caída de la base de datos MySQL.
- Errores graves en la actualización de librerías .NET MAUI que impidan la generación de los APK/IPA o ejecutables en Windows.

### Criterios de Reanudación
- Despliegue de un hotfix que restaure los servicios principales o estabilice las compilaciones de MAUI.

## Entregables
- Plan de Pruebas de Software (este documento).
- Scripts y suites de automatización (Jest/Supertest y xUnit).
- Reportes de defectos en las pruebas de Emuladores/Dispositivos físicos.
- Certificación final de QA para paso a producción de API, Web y binarios (Android/iOS/Windows).

## Recursos

### Requerimientos de Entornos – Hardware
- Servidor / Contenedor local o cloud con Node.js para despliegue de la API.
- Servidor de base de datos para hospedar MySQL.
- Máquinas de testing/desarrollo (PC Windows) capaces de compilar C#, montar el servidor de desarrollo y ejecutar emuladores Android.
- Entorno Mac (Hardware Apple) obligatorio si se planea compilar, firmar y probar exhaustivamente el proyecto para iOS/MacCatalyst.
- Dispositivos móviles físicos Android e iOS de prueba.

### Requerimientos de Entornos – Software
- **Backend:** Node.js, Express, Jest, Supertest, librería de mysql2, `bcrypt`, `jsonwebtoken`.
- **Frontend / Móvil:** SDK .NET 9.0, Blazor Web, .NET MAUI Workloads (para Android, iOS, maccatalyst, windows), framework `xUnit` + `coverlet`.
- **General:** Android SDK & Emulators, Xcode (para simuladores iOS), Windows 10 SDK (mínimo 10.0.17763.0), Git, Postman/Swagger.

### Herramientas de Pruebas Requeridas
- **Jest y Supertest:** Para automatizar las validaciones de backend.
- **xUnit:** Para automatización de C#.
- Herramienta de gestión de bugs (Jira, GitHub Issues, etc).

### Personal
- **1 Líder de QA:** Encargado del plan de pruebas, seguimiento y aprobación.
- **1 o 2 Testers Manuales/Funcionales:** Orientados a pruebas UI en Blazor y pruebas en emuladores / dispositivos físicos.
- **Desarrolladores (Roles compartidos):** Creación de las pruebas unitarias y configuración de los pipelines multiplataforma.

### Entrenamiento
- Familiarización del equipo de QA con la instalación y ejecución de emuladores Android/iOS.
- Comprensión de las diferencias de UI y navegación en aplicaciones Híbridas (Blazor dentro de WebView MAUI) frente a la Web tradicional.

## Planificación y Organización

### Procedimientos para las Pruebas
1. Revisión de especificaciones del proyecto.
2. Diseño de casos de prueba considerando matrices de dispositivos y navegadores.
3. Ejecución de pruebas unitarias/integración (Backend y Shared de C#).
4. Despliegue en ambiente local / staging:
   - Acceso Web vía navegador.
   - Compilación y ejecución de `.APK` en Android y Windows App en local.
5. Ejecución simultánea de casos de uso funcionales en Web y Móvil/Escritorio.
6. Reporte de hallazgos especificando Plataforma (Ej: "Falla en Login solo en App de Android").
7. Retesting posterior a correcciones.

### Matriz de Responsabilidades
| Tarea | Líder QA | Analista / Tester | Desarrollo |
|---|:---:|:---:|:---:|
| Creación de este Plan | R | C | I |
| Desarrollo Pruebas Unitarias | C | C | R |
| Ejecución Pruebas Manuales (Web y App) | A | R | I |
| Configuración de Emuladores de Prueba | A | R | C |
| Reporte de Bugs y Dispositivos afectados | C | R | I |
| Corrección de Defectos | I | I | R |

*(R=Responsable, A=Aprobador, C=Consultado, I=Informado)*

### Cronograma
*(Fechas sugeridas a adaptar)*
- **Fase 1:** Definición del Plan y configuración del entorno (Emuladores, Node, .NET MAUI).
- **Fase 2:** Pruebas y estabilización de API (Supertest + Jest).
- **Fase 3:** Pruebas funcionales Web Blazor y Lógica compartida.
- **Fase 4:** Pruebas multiplataforma MAUI en Emuladores y Windows nativo.
- **Fase 5:** Pruebas de Regresión E2E y Certificación.

### Premisas
- El código se maneja eficientemente mediante ramas y Pull Requests en Git.
- El hardware de los Testers es lo suficientemente robusto para soportar emuladores Android/iOS corriendo al mismo tiempo que la API.
- Existirá hardware Apple disponible para probar adecuadamente la compilación de iOS.

### Dependencias y Riesgos
- **Riesgo:** Inconsistencias de interfaz de usuario entre Android, iOS y Windows, ya que cada WebView puede renderizar de forma ligeramente distinta.  
  **Mitigación:** Asegurar que los componentes de Blazor sean altamente responsivos y se realicen pruebas tempranas en todos los entornos objetivo.
- **Dependencia:** El frontend y la App dependen de los contratos (JSON) del backend; cualquier cambio romperá el ecosistema en C#.

## Referencias
- Código fuente del Backend Node.js (`package.json`, `index.js`).
- Código fuente del Frontend C# .NET (`VeterinariaAntioquia.sln`, `VeterinariaAntioquia.Web.csproj`, `VeterinariaAntioquia.Shared.csproj`, `VeterinariaAntioquia.Tests.csproj`).
- Código fuente del App Multiplataforma MAUI (`VeterinariaAntioquia.csproj`).
- Documentación PMOInformática de la cual deriva esta plantilla.

## Glosario
- **Blazor:** Framework Web C# de Microsoft que permite crear UI de cliente robusta.
- **MAUI (.NET Multi-platform App UI):** Framework multiplataforma de Microsoft para crear aplicaciones nativas móviles y de escritorio (Android, iOS, macOS, Windows) con C#.
- **Jest:** Framework de Testing ágil y potente para JavaScript/Node.js.
- **Supertest:** Librería que permite simular llamadas HTTP para testear APIs REST.
- **xUnit:** Herramienta de pruebas unitarias para la plataforma .NET.
- **JWT (JSON Web Token):** Estándar abierto usado en el sistema para la seguridad y sesiones.
