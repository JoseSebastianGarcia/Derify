# Derify v3.1

  ----------------
  ENGLISH README
  ----------------

## Derify v3.1 released

Derify is a .NET middleware that automatically generates an interactive
Entity-Relationship Diagram (ERD) from your SQL Server database schema
and displays it directly in your browser.

Derify's mission is simple but powerful: Turn complex databases into
clear, explorable diagrams in seconds.

### How to use

1. Add Derify to your application:
builder.Services.AddDerify(connectionString);
2. Enable the middleware:
app.UseDerify();
3. Run your application (you will need a running SQL Server instance) and
navigate to: https://yourdomain.com/derify/
4. That's it. Derify will automatically scan your database and render a
live, interactive ERD in your browser.
![image](https://github.com/JoseSebastianGarcia/Derify/assets/94945762/25f4aa40-5026-433e-a354-a464d2e5ae0a)

### Diagramming engine

The diagramming engine was built manually from scratch to ensure full
technical control, performance, and accuracy. AI assistance was used to
refine the UI/UX --- improving clarity, navigation, and visual
consistency without compromising engineering quality.

### Key Features (v3.1)
-   Inverse table and schema toggle
-   Theme selection
-   Table drag selection
-   Table pick (ctrl + left click)

### Key Features (v3)

-   Built for .NET 10+
-   Improved and more visible search for schemas, tables, and columns
-   Toggleable minimap
-   Smooth Zoom In and Zoom Out
-   Zoom-to-fit for the entire diagram
-   Automatic table layout
-   Save snapshots of table positions and visibility
-   Load snapshots to restore previous views
-   Improved relationship and cardinality visualization
-   Print-ready diagrams
-   Export the diagram as an image
-   Show and hide schemas and tables
-   Tables display their column count
-   Light and Dark theme toggle


## Snapshot
<img width="959" height="506" alt="image" src="https://github.com/user-attachments/assets/0e073e8f-311a-4bb5-a31b-67f0d94017b3" />




¡See you inside! 
Made with ❤️ by Sebastian Garcia


  ----------------
  ESPAÑOL README
  ----------------

## Derify v3.1 ya está disponible

Derify es un middleware para .NET que genera automáticamente un diagrama
entidad-relación (ERD) a partir del esquema de tu base de datos SQL
Server y lo muestra de forma interactiva en el navegador.

El objetivo de Derify es claro y práctico: Convertir bases de datos
complejas en diagramas claros y explorables en segundos.

### Cómo usarlo

1. Agregá Derify a tu aplicación:
builder.Services.AddDerify(connectionString);
2. Activá el middleware:
app.UseDerify();
3. Ejecutá tu solución (necesitás una instancia activa de SQL Server) y
abrí en el navegador: https://tudominio.com/derify/
4. Listo. Derify analizará tu base de datos y generará automáticamente un
ERD interactivo en pantalla.
![image](https://github.com/JoseSebastianGarcia/Derify/assets/94945762/25f4aa40-5026-433e-a354-a464d2e5ae0a)

### Motor de diagramación

El motor de visualización fue desarrollado manualmente desde cero para
garantizar precisión, rendimiento y control técnico total. Se utilizó
asistencia de IA para refinar la interfaz y la experiencia de usuario
--- mejorando claridad, navegación y coherencia visual sin perder
calidad de ingeniería.

### Características principales (v3.1)
-   Seleccion inversa de tablas y esquemas.
-   Selección de temas
-   Selección de tablas por arrastre del mouse
-   Selección múltiple de tablas (ctrl + left click)

### Características principales (v3)

-   Compatible con .NET 10+
-   Búsqueda de esquemas, tablas y campos mejorada y más visible
-   Minimap activable y desactivable
-   Zoom In y Zoom Out fluido
-   Zoom para ajustar todo el diagrama
-   Auto-layout de tablas
-   Guardar snapshots de posición y visibilidad de tablas
-   Cargar snapshots de visualizaciones previas
-   Relaciones y cardinalidades mejoradas
-   Opción de impresión
-   Exportar el diagrama como imagen
-   Mostrar y ocultar esquemas y tablas
-   Las tablas muestran cantidad de columnas
-   Modo claro y oscuro

## Snapshot
<img width="959" height="506" alt="image" src="https://github.com/user-attachments/assets/0e073e8f-311a-4bb5-a31b-67f0d94017b3" />




¡Nos vemos dentro! 
Hecho con ❤️ por Sebastián Garcia
