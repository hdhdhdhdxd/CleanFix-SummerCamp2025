// See https://aka.ms/new-console-template for more information
using System.Drawing;
using System.Reflection.Emit;
using Dominio;
using Dominio.Maintenance;
using static System.Net.Mime.MediaTypeNames;

Console.WriteLine("Hello, World!");

var company = new Company();

/*
 
- Nombre de la aplicación: Dificultad I()

   [-] Como [Usuario] quiero [var el nombre de la aplicacion] para [poder redonocer la aplicacion que estoy usando]

     ///// Criterio de Aceptación:
        - En la pantalla debe aparecer el nomre de la aplicación "CleanFix"
        - Debe aparecer el nombre situado arriba a la izquierda semicentrado

- Concepto de diseño/Interfaz (Angular): Dificultad IIIII

   [-] Como [Usuario] quiero [ver diseño de la aplicación] para [poder usar la aplicación]
    ///// Criterio de Aceptación:
          - Debe tener un estilo similar al mockup de la aplicación
          - Debe mostrar el logo y nombre de la aplicación
          - Debe mostrar un botón que daría acceso a un formulario de busqueda de empresas
          - Debe mostrar una lista de empresas con sus datos básicos (nombre, dirección, teléfono, email)

- Lógica de funcionamiento / Simulacro con grupo de empresas pequeño (5/10 empresas): Dificultad III

   [-] Como [Usuario] quiero [poder gestionar las empresas de mantenimiento] para [poder realizar tareas de mantenimiento]
///// Criterio de Aceptación:
          - Debe haber una clase empresa con propiedades basicas (nombre, dirección, teléfono, email, tipo, coste, tiempo de trabajo)
          - Debe haber constructores acordes a cada una de las empresas
          - Debe ser posible introducir un tipo de problema y mostrar una lista de empresas que pueden solucionar ese problema
 */


