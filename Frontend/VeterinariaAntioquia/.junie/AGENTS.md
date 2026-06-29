# Project Guidelines

Actúa como un Arquitecto de Software y Desarrollador Fullstack Senior. Vamos a construir el sistema de gestión para una Veterinaria.

**Objetivo de Arquitectura:**
Busco la máxima reutilización de código. Escribir la interfaz una vez y desplegarla en Web, Móvil y Escritorio.
- **Frontend / Cliente:** C# con Blazor Hybrid (.NET MAUI Blazor App).
- **Backend / API:** Node.js (con Express o el framework que consideres más ágil).
- **IDE Principal:** JetBrains Rider / WebStorm.

**Identidad Visual y Diseño (UI/UX):**
- Diseño "Mobile-First" y "App-like" (componentes grandes, bordes redondeados, adaptables a escritorio).
- **Color Primario:** `#00bf63` (Verde vibrante para botones de acción principal, FABs).
- **Color Secundario:** `#7ed957` (Verde suave para fondos de tarjetas destacadas, acentos).

**Alcance Inicial (Fase 1):**
Necesito estructurar los siguientes módulos visuales en Blazor (usando datos falsos/mocks por ahora, sin conectar a la API aún):
1. **Login y Registro:** Formularios limpios y modernos.
2. **Dashboard del Cliente:**
    - Saludo al usuario.
    - Lista horizontal de "Tus Mascotas".
    - Tarjeta destacada de "Próxima Cita" (usando el color secundario).
    - Barra de navegación inferior (para móvil) y menú lateral adaptable (para escritorio).
    - Botón Flotante de Acción (FAB) con el color primario que abra un modal/bottom sheet para "Pedir Cita".

**Reglas de Código:**
1. Todo el código debe estar comentado como un profesional.
2. Estructura el proyecto siguiendo buenas prácticas y separación de responsabilidades.

**Primera acción requerida:**
Dame el paso a paso exacto para inicializar este proyecto Frontend (Blazor MAUI) y el Backend (Node.js) desde la terminal o Rider. Luego, generaremos los primeros componentes visuales.
