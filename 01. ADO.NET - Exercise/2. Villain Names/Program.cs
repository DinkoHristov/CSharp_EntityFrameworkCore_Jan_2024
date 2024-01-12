using Microsoft.Data.SqlClient;

string connectionString = "Server=DESKTOP-RS7JOPO;Database=MinionsDB;Integrated Security=True;TrustServerCertificate=True";

var connection = new SqlConnection(connectionString);

using (connection)
{
    connection.Open();

    var command = new SqlCommand("SELECT v.Name, COUNT(m.Id) AS MinionsCount " +
                                    "FROM Villains AS v " +
                                    "JOIN MinionsVillains AS mv ON v.Id = mv.VillainId " +
                                    "JOIN Minions AS m ON mv.MinionId = m.Id " +
                                    "GROUP BY v.Name " +
                                    "ORDER BY MinionsCount DESC", connection);

    using (var reader = command.ExecuteReader())
    {
        while (reader.Read())
        {
            Console.WriteLine($"{reader[0]} - {reader[1]}");
        }
    }
}