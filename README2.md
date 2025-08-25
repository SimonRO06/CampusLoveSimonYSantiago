# 🧠 Apuntes de Estudio --- C#, Entity Framework y iTextSharp 7

------------------------------------------------------------------------

## 1. Configuración de Git

``` bash
nano ~/.gitconfig
git config --global core.editor "code --wait"
git config --global init.defaultBranch main
git config --global user.name "tu nombre"
git config --global user.email "tuemail"
```

## 2. Instalación de Paquetes (.NET CLI)

``` bash
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Pomelo.EntityFrameworkCore.MySql --version 9.0.0-rc.1.efcore.9.0.0
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.Extensions.Configuration
dotnet add package Microsoft.Extensions.Configuration.Json
dotnet add package Microsoft.Extensions.Configuration.EnvironmentVariables
dotnet add package MySql.Data --version 9.4.0
dotnet add package itext7
```

## 3. Usings Requeridos

``` csharp
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

using ApuntesCS.Modules.Entidad.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
```

## 4. Conexión Manual a MySQL

``` csharp
string connStr = "Server=localhost;Database=ExamenDB;User=campus2023;Password=campus2023;";
using var conn = new MySqlConnection(connStr);
conn.Open();
```

## 5. Operaciones CRUD con ADO.NET

### INSERT

``` csharp
string insert = "INSERT INTO usuarios (nombre, email) VALUES (@nombre, @correo)";
using var cmd = new MySqlCommand(insert, conn);
cmd.Parameters.AddWithValue("@nombre", "Simón");
cmd.Parameters.AddWithValue("@correo", "simon@mail.com");
cmd.ExecuteNonQuery();
```

### UPDATE

``` csharp
string update = "UPDATE usuarios SET email=@correo WHERE id=@id";
using var cmd = new MySqlCommand(update, conn);
cmd.Parameters.AddWithValue("@correo", "nuevo@mail.com");
cmd.Parameters.AddWithValue("@id", 1);
cmd.ExecuteNonQuery();
```

### DELETE

``` csharp
string delete = "DELETE FROM usuarios WHERE id=@id";
using var cmd = new MySqlCommand(delete, conn);
cmd.Parameters.AddWithValue("@id", 1);
cmd.ExecuteNonQuery();
```

### SELECT

``` csharp
public List<Usuario> GetUsuarios()
{
    var lista = new List<Usuario>();
    string query = "SELECT id, nombre, email FROM usuarios";
    using var cmd = new MySqlCommand(query, conn);
    using var reader = cmd.ExecuteReader();
    while (reader.Read())
    {
        lista.Add(new Usuario {
            Id = reader.GetInt32("id"),
            Nombre = reader.GetString("nombre"),
            Email = reader.GetString("email")
        });
    }
    return lista;
}
```

## 6. Configuración de Propiedades en EF Core (Fluent API)

La "e" es la tabla.

``` csharp
builder.Property(e => e.Atributo)
    .IsRequired()
    .HasMaxLength(100)
    .HasColumnName("nombre")
    .HasDefaultValue("N/A")
    .HasColumnType("TEXT")
    .HasColumnType("DATETIME")
    .HasDefaultValueSql("CURRENT_TIMESTAMP")
    .HasColumnType("TINYINT(1)")
    .HasDefaultValue(true)
    .HasColumnType("DECIMAL(10,2)")
    .HasDefaultValue(0.00m)
    .HasMaxLength(20)
    .IsUnicode(false)
    .IsRequired(false);
```

## 7. Uso de Enums

``` csharp
public enum NombreEntidad
{
    ZemZem = 1,
    Zemen = 2
}

// De número a enum
int valor = 2;
NombreEntidad nombre = (NombreEntidad)valor;

// De enum a número
int valor2 = (int)NombreEntidad.Zemen;

// De texto a enum
string texto = "ZemZem";
NombreEntidad nombre3 = Enum.Parse<NombreEntidad>(texto);
```

## 8. Entity Framework Core --- Modelos, Migraciones y LINQ

### 8.1 DbContext y Modelos

``` csharp
public class ApplicationDbContext : DbContext
{
    public DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseMySql("Server=localhost;Database=ExamenDB;User=campus2023;Password=campus2023;",
            new MySqlServerVersion(new Version(8, 0, 29)));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Usuario>(ConfigureUsuario);
    }

    private void ConfigureUsuario(EntityTypeBuilder<Usuario> builder)
    {
        builder.HasKey(u => u.Id);
        builder.Property(u => u.Nombre).IsRequired().HasMaxLength(50);
        builder.Property(u => u.Email).IsRequired().HasMaxLength(100);
    }
}
```

### 8.2 Comandos de Migración

``` bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### 8.3 Consultas con LINQ

``` csharp
using var context = new ApplicationDbContext();

var todos = context.Usuarios.ToList();

var filtro = context.Usuarios
    .Where(u => u.Nombre.Contains("Simón"))
    .ToList();

var proyeccion = context.Usuarios
    .Select(u => new { u.Nombre, u.Email })
    .ToList();

var ordenados = context.Usuarios
    .OrderByDescending(u => u.Email)
    .ToList();

var nuevo = new Usuario { Nombre = "Ana", Email = "ana@mail.com" };
context.Usuarios.Add(nuevo);
context.SaveChanges();
```

## 9. Crear PDF con iTextSharp 7

### 9.1 Paquete Requerido

``` bash
dotnet add package itext7
```

### 9.2 Código para Generar PDF

``` csharp
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

public class PdfGenerator
{
    public void CrearPdf(string ruta)
    {
        using var writer = new PdfWriter(ruta);
        using var pdf = new PdfDocument(writer);
        using var document = new Document(pdf);

        document.Add(new Paragraph("Hola, este es un PDF generado con iText 7!"));
        document.Add(new Paragraph("Generado el: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));

        var table = new Table(2)
            .AddCell("Nombre")
            .AddCell("Correo")
            .AddCell("Simón")
            .AddCell("simon@mail.com");
        document.Add(table);

        var lista = new List()
            .Add(new ListItem("Elemento 1"))
            .Add(new ListItem("Elemento 2"))
            .Add(new ListItem("Elemento 3"));
        document.Add(lista);
    }
}
```

### 9.3 Usar el Generador

``` csharp
var generador = new PdfGenerator();
generador.CrearPdf("reporte.pdf");
```
## Controlar que un int no sea string

```cs
Console.Write("Ingresa un número: ");
int numero;
string? entrada = Console.ReadLine();

// Intentar convertir a int
if (int.TryParse(Console.ReadLine(), out int numero)) // Verificar que sea un número
{
    Console.WriteLine($"Tu numero es {numero}");
}
else
{
    Console.WriteLine("Numero inválida.");
}
```

## Enlazar atributos con variables

```cs
using System;

class Program
{
    static void Main(string[] args)
    {
        Entidad nuevoUsuario = new Entidad(); // Crear instancia de la entidad

        Console.Write("Ingresa el nombre: ");
        nuevoUsuario.Nombre = Console.ReadLine(); // Asignar al atributo Nombre

        Console.Write("Ingresa la edad: ");
        if (int.TryParse(Console.ReadLine(), out int edad)) // Verificar que sea un número
        {
            nuevoUsuario.Edad = edad;
        }
        else
        {
            Console.WriteLine("Edad inválida, se asigna 0 por defecto.");
            nuevoUsuario.Edad = 0;
        }

        // Mostrar el resultado
        Console.WriteLine($"\nUsuario creado: {nuevoUsuario.Nombre} {nuevoUsuario.Apellido}, {nuevoUsuario.Edad} años");
    }
}
```