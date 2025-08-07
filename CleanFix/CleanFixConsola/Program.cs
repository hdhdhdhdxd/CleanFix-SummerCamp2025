// See https://aka.ms/new-console-template for more information
using Dominio.Maintenance;

string titulo = "CleanFix";
int anchoConsola = Console.WindowWidth;
int posicionX1 = (anchoConsola - titulo.Length) / 2;

Console.Clear();

// Animación: mostrar letra por letra con color
Console.ForegroundColor = ConsoleColor.Cyan;
Console.SetCursorPosition(posicionX1, Console.CursorTop);
foreach (char c in titulo)
{
    Console.Write(c);
    Thread.Sleep(150); // Pausa para animación
}
Console.ResetColor();

// Línea en blanco después del título
Console.WriteLine("\n");

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

// crear empresas

Empresa Empresa1 = new Empresa()
{
    Id = 1,
    Nombre = "Electricidad y Más",
    Direccion = "Calle de la Luz, 123",
    Telefono = "123-456-7890",
    Email = "ElectricidadyMás@gmail.com",
    Tipo = "Electrical",
    Coste = 50.00m, // Coste por hora
    TiempoTrabajo = new TimeSpan(2, 0, 0) // 2 horas de trabajo
};

Empresa Empresa2 = new Empresa()
{
    Id = 2,
    Nombre = "Fontanería Rápida",
    Direccion = "Avenida del Agua, 456",
    Telefono = "987-654-3210",
    Email = "FontaneríaRápida@gmail.com",
    Tipo = "Plumbing",
    Coste = 40.00m, // Coste por hora
    TiempoTrabajo = new TimeSpan(1, 30, 0) // 1 hora y 30 minutos de trabajo
};

Empresa Empresa3 = new Empresa()
{
    Id = 3,
    Nombre = "Carpenterizacion Total",
    Direccion = "Plaza del Clima, 789",
    Telefono = "555-123-4567",
    Email = "CarpenterizacionTotal@gmail.com",
    Tipo = "Carpentry",
    Coste = 60.00m, // Coste por hora
    TiempoTrabajo = new TimeSpan(3, 0, 0) // 3 horas de trabajo

};

Empresa Empresa4 = new Empresa()
{
    Id= 4,
    Nombre = "Pinturas y Más",
    Direccion = "Calle del Color, 321",
    Telefono = "444-555-6666",
    Email = "PinturasyMás@gmail.com",
    Tipo = "Painting",
    Coste = 45.00m, // Coste por hora
    TiempoTrabajo = new TimeSpan(2, 30, 0) // 2 horas y 30 minutos de trabajo
};

Empresa Empresa5 = new Empresa()
{
    Id = 5,
    Nombre = "Suelo Perfecto",
    Direccion = "Avenida del Suelo, 654",
    Telefono = "333-222-1111",
    Email = "SueloPerfecto@gmail.com",
    Tipo = "Flooring",
    Coste = 55.00m, // Coste por hora
    TiempoTrabajo = new TimeSpan(4, 0, 0) // 4 horas de trabajo

};
Empresa Empresa6 = new Empresa()
{
    Id = 6,
    Nombre = "Limpieza Total",
    Direccion = "Calle de la Limpieza, 987",
    Telefono = "222-333-4444",
    Email = "LimpiezaTotal@gmail.com",
    Tipo = "Cleaning",
    Coste = 30.00m, // Coste por hora
    TiempoTrabajo = new TimeSpan(1, 0, 0) // 1 hora de trabajo
};

Empresa Empresa7 = new Empresa()
{
    Id = 7,
    Nombre = "Fontanería Bros",
    Direccion = "Calle de las Reparaciones, 159",
    Telefono = "111-222-3333",
    Email = "ReparacionesGenerales@gmail.com",
    Tipo = "Ready",
    Coste = 70.00m, // Coste por hora
    TiempoTrabajo = new TimeSpan(5, 0, 0) // 5 horas de trabajo
};

Empresa Empresa8 = new Empresa()
{
    Id=8,
    Nombre = "Electricidad Express",
    Direccion = "Calle de la Electricidad, 852",
    Telefono = "888-777-6666",
    Email = "ElectricidadExpress@gmail.com",
    Tipo = "Electrical",
    Coste = 65.00m, // Coste por hora
    TiempoTrabajo = new TimeSpan(2, 15, 0) // 2 horas y 15 minutos de trabajo
};

Empresa Empresa9 = new Empresa()
{
    Id = 9,
    Nombre = "Carpintería Creativa",
    Direccion = "Calle de la Madera, 753",
    Telefono = "999-888-7777",
    Email = "CarpinteríaCreativa@gmail.com",
    Tipo = "Carpentry",
    Coste = 75.00m, // Coste por hora
    TiempoTrabajo = new TimeSpan(3, 30, 0) // 3 horas y 30 minutos de trabajo
};

// Añadimos todas las empresas a una lista para facilitar su gestión
List<Empresa> Empresas = new List<Empresa>
    {
        Empresa1,
        Empresa2,
        Empresa3,
        Empresa4,
        Empresa5,
        Empresa6,
        Empresa7,
        Empresa8,
        Empresa9
    };

// Método para buscar empresas por tipo de problema

Console.WriteLine("Ingrese el número del tipo de problema: (introduzca 's' Para Salir) ");
Console.WriteLine();

// Mostrar título animado (puedes reutilizar el código anterior aquí)

// Colores para los issues
ConsoleColor[] colores = {
            ConsoleColor.Yellow,
            ConsoleColor.Green,
            ConsoleColor.Magenta,
            ConsoleColor.Red
        };

foreach (IssueType estado in Enum.GetValues(typeof(IssueType)))
{
    if (estado == IssueType.Ready)
    {
        continue; // Saltar el estado "Ready"
    }

    string estadoTexto = estado.ToString();
    int posicionX2 = (anchoConsola - estadoTexto.Length) / 10;

    Console.ForegroundColor = colores[(int)estado % colores.Length];
    Console.SetCursorPosition(posicionX2, Console.CursorTop);
    Console.WriteLine($"{estadoTexto}: {(int)estado}");
    Console.ResetColor();

    // Separación visual entre issues
    Console.WriteLine();
    Thread.Sleep(200); // Animación opcional
}

var respuesta = Console.ReadLine();



while (respuesta != "s")
{
    switch (respuesta)
    {
        case "0":
            Console.WriteLine("Empresas de Electricidad:");
            foreach (var empresa in Empresas.Where(e => e.Tipo == "Electrical"))
            {
                MostrarRespuesta(empresa.MostrarInformacion(), ConsoleColor.Yellow);
            }
            break;
        case "1":
            Console.WriteLine("Empresas de Fontanería:");
            foreach (var empresa in Empresas.Where(e => e.Tipo == "Plumbing"))
            {
                MostrarRespuesta(empresa.MostrarInformacion(), ConsoleColor.Yellow);
            }
            break;
        case "2":
            Console.WriteLine("Empresas de Carpintería:");
            foreach (var empresa in Empresas.Where(e => e.Tipo == "Carpentry"))
            {
                MostrarRespuesta(empresa.MostrarInformacion(), ConsoleColor.Yellow);
            }
            break;
        case "3":
            Console.WriteLine("Empresas de Pintura:");
            foreach (var empresa in Empresas.Where(e => e.Tipo == "Painting"))
            {
                MostrarRespuesta(empresa.MostrarInformacion(), ConsoleColor.Yellow);
            }
            break;
        case "4":
            Console.WriteLine("Empresas de Suelos:");
            foreach (var empresa in Empresas.Where(e => e.Tipo == "Flooring"))
            {
                MostrarRespuesta(empresa.MostrarInformacion(), ConsoleColor.Yellow);
            }
            break;
        case "5":
            Console.WriteLine("Empresas de Limpieza:");
            foreach (var empresa in Empresas.Where(e => e.Tipo == "Cleaning"))
            {
                MostrarRespuesta(empresa.MostrarInformacion(), ConsoleColor.Yellow);
            }
            break;
        default:
            MostrarRespuesta("Tipo de problema no válido. Por favor, ingrese un número válido.(introduzca 's' Para Salir)", ConsoleColor.Red);
            break;
    }

    Console.WriteLine("Ingrese el número del tipo de problema: (introduzca 's' Para Salir) ");
    respuesta = Console.ReadLine();

}

MostrarRespuesta("Gracias por usar CleanFix. ¡Hasta luego!", ConsoleColor.Cyan);
Thread.Sleep(1000);
Environment.Exit(0);

static void MostrarRespuesta(string mensaje, ConsoleColor color)
{
    Console.WriteLine(); // Línea en blanco antes del mensaje
    int anchoConsola = Console.WindowWidth;
    int posicionX = (anchoConsola - mensaje.Length) / 2;
    Console.ForegroundColor = color;

    foreach (char c in mensaje)
    {
        Console.Write(c);
        Thread.Sleep(5); // Animación letra por letra
    }
    Console.ResetColor();
    Console.WriteLine("\n");
}





