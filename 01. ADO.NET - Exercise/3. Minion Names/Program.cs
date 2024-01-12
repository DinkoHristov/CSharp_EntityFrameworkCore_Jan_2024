using Microsoft.Data.SqlClient;

string connectionString = "Server=DESKTOP-RS7JOPO;Database=MinionsDB;Integrated Security=True;TrustServerCertificate=True";

var connection = new SqlConnection(connectionString);

using (connection)
{
    connection.Open();

    int villainId = int.Parse(Console.ReadLine());

    string? villainName = FindVillainName(villainId, connection);

    if (villainName != null)
    {
        List<string> minions = FindAllMinions(villainId ,connection);

        if (minions.Any())
        {
            Console.WriteLine($"Villain: {villainName}");
            Console.WriteLine(string.Join("\n", minions));
        }
        else
        {
            Console.WriteLine("(no minions)");
        }
    }
    else
    {
        Console.WriteLine($"No villain with ID {villainId} exists in the database.");
    }
}

List<string> FindAllMinions(int villainId, SqlConnection connection)
{
    List<string> minions = new List<string>();

    string query = "SELECT m.Name, m.Age " +
                    "FROM Minions AS m " +
                    "JOIN MinionsVillains AS mv ON m.Id = mv.MinionId " +
                    "JOIN Villains AS v ON mv.VillainId = v.Id " +
                    "WHERE v.Id = @VillainId";

    var command = new SqlCommand(query ,connection);

    command.Parameters.Add(new SqlParameter("@VillainId", villainId));

    var reader = command.ExecuteReader();

    using (reader)
    {
        int count = 1;

        while (reader.Read()) 
        {
            string minionNameAge = $"{count}. {reader[0]} {reader[1]}";

            minions.Add(minionNameAge);
        }
    }

    return minions;
}

string? FindVillainName(int villainId, SqlConnection connection)
{
    string query = "SELECT [Name] FROM Villains WHERE Id = @VillainId";

    var command = new SqlCommand(query, connection);

    command.Parameters.Add(new SqlParameter("@VillainId", villainId));

    string villainName = (string)command.ExecuteScalar();

    return villainName;
}

