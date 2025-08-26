INTEGRANTES:

- Simon Rubiano Ortiz
- Santiago Valderrama Laiton

## Paquetes de instalacion

```bash
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Pomelo.EntityFrameworkCore.MySql --version 9.0.0-rc.1.efcore.9.0.0
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.Extensions.Configuration
dotnet add package Microsoft.Extensions.Configuration.Json
dotnet add package Microsoft.Extensions.Configuration.EnvironmentVariables
dotnet add package MySql.Data --version 9.4.0
```

Campus Love es una aplicación de consola en C# que simula un sistema de emparejamiento. El proyecto busca implementar un flujo completo donde los usuarios pueden registrarse, ver perfiles de otras personas, hacer “like” o “dislike” y revisar sus coincidencias (matches). 



﻿﻿Se debe permitir la simulación de interacciones entre usuarios de forma iterativa, implementando estructuras de control, colecciones, patrones de diseño, principios SOLID, y formateo adecuado de datos.



Adicionalmente, se podrá agregar un sistema de "créditos de interacción" que limite la cantidad de likes por día, usando lógica con Math, validaciones y operadores.



Contexto y Especificaciones:



Diseñar una aplicación de consola basada en arquitectura limpia, principios SOLID y patrones de diseño, que simula un sistema de emparejamiento completo entre usuarios.

El sistema debe permitir registrar:

Registro de usuarios (nombre, edad, género, intereses, carrera, frase de perfil).
Visualización de perfiles disponibles (uno por uno) para hacer Like o Dislike.
Ver coincidencias si dos usuarios se dan Like mutuamente.
Listar todas las coincidencias de un usuario.
Limitar la cantidad de likes diarios por usuario con lógica de Math.
Mostrar estadísticas de interacción usando LINQ (por ejemplo, el usuario con más likes recibidos).
Usar CultureInfo y NumberFormat para mostrar créditos, nombres en formato título, etc.


Requisitos funcionales:



Menú en consola con las siguientes opciones:
Registrarse como nuevo usuario
Ver perfiles y dar Like o Dislike
Ver mis coincidencias (matches)
Ver estadísticas del sistema (usuarios con más likes, más matches, etc.)
Salir


Guardar usuarios y sus interacciones en listas o diccionarios
Permitir simulación de múltiples usuarios (modo multicliente ficticio).
Aplicar patrón Factory para creación de usuarios o interacciones.
Usar principios SOLID y separación clara de responsabilidades.


Requisitos no funcionales:



Interacción con la consola de forma amigable, clara y fluida.
Código organizado, con clases separadas (Usuario, Interaccion, MatchService, etc.).
Separación clara entre lógica de validación, lógica de negocio y presentación.
Validación de entrada (edad, texto, género).
Uso de conversiones con int.Parse, TryParse, ToUpper, ToLower, etc.
Manejo adecuado de entrada de datos.
Diagramas



Diagrama de clases: Usuario, Match, GestorUsuarios, Interaccion, etc.


Herramientas y tecnologías:

Lenguaje: C#
Plataforma: .NET Core 8.0
IDE sugerido: VS Code


Sugerencias:

Usa List<Usuario>, Dictionary<int, List<int>> para guardar likes.
Usa LINQ para buscar matches, ordenar por likes, contar usuarios.
Aplica el patrón Strategy para definir reglas de emparejamiento (por intereses, edad, carrera).
Usa Math.Min, Math.Max para controlar cantidad de likes diarios.

## BASE DE DATOS

```cs
DROP DATABASE IF EXISTS DbCampusLove;

CREATE DATABASE DbCampusLove;

USE DbCampusLove;

CREATE TABLE Carreras (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Nombre VARCHAR(100) NOT NULL
);

CREATE TABLE Personas (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Nombre VARCHAR(100) NOT NULL,
    Edad INT NOT NULL,
    Genero CHAR(1) NOT NULL,
    CarreraId INT,
    Frase VARCHAR(255),
    FOREIGN KEY (CarreraId) REFERENCES Carreras(Id)
);

CREATE TABLE Interes (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Nombre VARCHAR(100) NOT NULL
);

CREATE TABLE Persona_Interes (
    PersonaId INT,
    InteresId INT,
    PRIMARY KEY (PersonaId, InteresId),
    FOREIGN KEY (PersonaId) REFERENCES Persona(Id),
    FOREIGN KEY (InteresId) REFERENCES Interes(Id)
);

CREATE TABLE Matches (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Persona1Id INT NOT NULL,
    Persona2Id INT NOT NULL,
    FechaMatch TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    EsMatchMutuo BOOLEAN DEFAULT FALSE,
    FOREIGN KEY (Persona1Id) REFERENCES Persona(Id),
    FOREIGN KEY (Persona2Id) REFERENCES Persona(Id),
    UNIQUE KEY UniqueMatch (Persona1Id, Persona2Id),
    CHECK (Persona1Id < Persona2Id)
);

CREATE TABLE Likes (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    PersonaQueDaLikeId INT NOT NULL,
    PersonaQueRecibeLikeId INT NOT NULL,
    FechaLike TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (PersonaQueDaLikeId) REFERENCES Persona(Id),
    FOREIGN KEY (PersonaQueRecibeLikeId) REFERENCES Persona(Id),
    UNIQUE KEY UniqueLike (PersonaQueDaLikeId, PersonaQueRecibeLikeId),
    CHECK (PersonaQueDaLikeId != PersonaQueRecibeLikeId)
);

INSERT INTO Carreras (Nombre) VALUES 
('Ingeniería de Sistemas'),
('Medicina'),
('Derecho'),
('Administración'),
('Psicología');

INSERT INTO Interes (Nombre) VALUES 
('Deportes'),
('Música'),
('Arte'),
('Tecnología'),
('Lectura'),
('Viajes');
```