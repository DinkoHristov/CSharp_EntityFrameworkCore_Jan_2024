using Microsoft.Data.SqlClient;

string connectionString = "Server=DESKTOP-RS7JOPO;Integrated Security=true;TrustServerCertificate=True;Database=MinionsDB";

using (var connection = new SqlConnection(connectionString))
{
    connection.Open();

    //1.Create Database MinionsDB
    var command = new SqlCommand("CREATE DATABASE MinionsDB", connection);

    command.ExecuteNonQuery();

    //2.Create Tables
    List<string> createTableQueries = GetCreateTableQueries();

    foreach (var query in createTableQueries)
    {
        ExecuteCommand(query, connection);
    }

    //3.Insert Into Tables
    List<string> insertIntoTableQueries = GetInsertIntoTableQueries();

    foreach (var query in insertIntoTableQueries)
    {
        ExecuteCommand(query, connection);
    }
}

void ExecuteCommand(string query, SqlConnection connection)
{
    var command = new SqlCommand(query, connection);

    command.ExecuteNonQuery();
}

List<string> GetCreateTableQueries()
{
    List<string> queries = new List<string>()
    {
        "CREATE TABLE Countries (Id INT PRIMARY KEY IDENTITY, [Name] NVARCHAR(50))",
        "CREATE TABLE Towns(Id INT PRIMARY KEY IDENTITY, [Name] NVARCHAR(50), CountryCode INT REFERENCES Countries(Id))",
        "CREATE TABLE Minions(Id INT PRIMARY KEY IDENTITY, [Name] NVARCHAR(50), Age INT, TownId INT REFERENCES Towns(Id))",
        "CREATE TABLE EvilnessFactors(Id INT PRIMARY KEY IDENTITY, [Name] NVARCHAR(50))",
        "CREATE TABLE Villains(Id INT PRIMARY KEY IDENTITY, [Name] NVARCHAR(50), EvilnessFactorId INT REFERENCES EvilnessFactors(Id))",
        "CREATE TABLE MinionsVillains(MinionId INT REFERENCES Minions(Id), VillainId INT REFERENCES Villains(Id), CONSTRAINT PK_MinionsVillains PRIMARY KEY(MinionId, VillainId))"
    };

    return queries;
}

List<string> GetInsertIntoTableQueries()
{
    List<string> queries = new List<string>()
    {
        "INSERT INTO Countries([Name]) VALUES ('Bulgaria'), ('Norway'), ('Cyprus'), ('Greece'), ('UK')",
        "INSERT INTO Towns([Name], CountryCode) VALUES ('Plovdiv', 1), ('Oslo', 2), ('Larnaca', 3), ('Athens', 4), ('London', 5)",
        "INSERT INTO Minions([Name], Age, TownId) VALUES ('Stoyan', 12, 1), ('George', 22, 2), ('Ivan', 25, 3), ('Kiro', 35, 4), ('Niki', 25, 5)",
        "INSERT INTO EvilnessFactors([Name]) VALUES ('super good'), ('good'), ('bad'), ('evil'), ('super evil')",
        "INSERT INTO Villains([Name], EvilnessFactorId) VALUES ('Gru', 1), ('Ivo', 2), ('Teo', 3), ('Sto', 4), ('Pro', 5)",
        "INSERT INTO MinionsVillains(MinionId, VillainId) VALUES (1, 1), (2, 2), (3, 3), (4, 4), (5, 5)"
    };

    return queries;
}