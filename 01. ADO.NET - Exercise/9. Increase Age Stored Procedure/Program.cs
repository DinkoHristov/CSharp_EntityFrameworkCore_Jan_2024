using Microsoft.Data.SqlClient;

string connectionString = "Server=DESKTOP-RS7JOPO;Database=MinionsDB;Integrated Security=True;TrustServerCertificate=True";

var connection = new SqlConnection(connectionString);

using (connection)
{
    connection.Open();

    int minionId = int.Parse(Console.ReadLine());

    ExecuteCreateAndExecuteProcedure(minionId, connection);

    ExecutePrintMinion(minionId, connection);
}

void ExecutePrintMinion(int minionId, SqlConnection connection)
{
    string query = "SELECT [Name], Age FROM Minions WHERE Id = @inputMinionId";

    var command = new SqlCommand(query, connection);

    command.Parameters.Add(new SqlParameter("@inputMinionId", minionId));

    var reader = command.ExecuteReader();

    using (reader)
    {
        reader.Read();

        Console.WriteLine($"{reader[0]} - {reader[1]} years old");
    }
}

void ExecuteCreateAndExecuteProcedure(int minionId, SqlConnection connection)
{
    // Create Procedure
    string query = "CREATE OR ALTER PROCEDURE usp_GetOlder(@minionId INT) " +
                    "AS " +
                    "BEGIN " +
                    "UPDATE Minions " +
                    "SET Age += 1 " +
                    "WHERE Id = @minionId " +
                    "END; ";

    var command = new SqlCommand(query, connection);

    command.ExecuteNonQuery();

    // Execute Procedure
    string executeQuery = "EXECUTE usp_GetOlder @minionId";

    var executeCommand = new SqlCommand(executeQuery, connection);

    executeCommand.Parameters.Add(new SqlParameter("@minionId", minionId));

    executeCommand.ExecuteNonQuery();
}