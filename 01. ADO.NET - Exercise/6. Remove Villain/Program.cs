using Microsoft.Data.SqlClient;

string connectionString = "Server=DESKTOP-RS7JOPO;Database=MinionsDB;Integrated Security=True;TrustServerCertificate=True";

var connection = new SqlConnection(connectionString);

using (connection)
{
    connection.Open();

    int villainId = int.Parse(Console.ReadLine());

    ExecuteDeleteVillain(villainId, connection);
}

void ExecuteDeleteVillain(int villainId, SqlConnection connection)
{
    // Check if villain exists.
    string searchVillain = "SELECT [Name] FROM Villains WHERE Id = @villainId";

    var searchCommand = new SqlCommand(searchVillain, connection);

    searchCommand.Parameters.Add(new SqlParameter("@villainId", villainId));

    string searchedName = (string)searchCommand.ExecuteScalar();

    if (searchedName == null)
    {
        Console.WriteLine("No such villain was found.");

        return;
    }

    // Get minions count
    string countMinions = "SELECT COUNT(mv.MinionId) " +
                            "FROM Villains AS v " +
                            "JOIN MinionsVillains AS mv ON mv.VillainId = v.Id " +
                            "WHERE v.Id = @villainId";

    var countCommand = new SqlCommand(countMinions, connection);

    countCommand.Parameters.Add(new SqlParameter("@villainId", villainId));

    int minionsCount = (int)countCommand.ExecuteScalar();

    // Delete villain
    string query = "DELETE MinionsVillains " +
                   "WHERE VillainId IN (SELECT Id FROM Villains WHERE Id = @villainId) " +
                   "DELETE Villains " +
                   "WHERE Id = @villainId";

    var command = new SqlCommand(query, connection);

    command.Parameters.Add(new SqlParameter("@villainId", villainId));

    command.ExecuteNonQuery();

    Console.WriteLine($"{searchedName} was deleted.");
    Console.WriteLine($"{minionsCount} minions were released.");
}