using Microsoft.Data.SqlClient;

string connectionString = "Server=DESKTOP-RS7JOPO;Database=MinionsDB;Integrated Security=True;TrustServerCertificate=True";

var connection = new SqlConnection(connectionString);

using (connection)
{
    connection.Open();

    int[] minionIds = Console.ReadLine().Split(" ").Select(int.Parse).ToArray();

    foreach (int id in minionIds)
    {
        ExecuteIncreaseAgeByOne(id, connection);
    }
}

void ExecuteIncreaseAgeByOne(int id, SqlConnection connection)
{
    // Increase the salary by 1
    string query = "UPDATE Minions SET Age += 1 WHERE Id = @minionId";

    var command = new SqlCommand(query, connection);

    command.Parameters.Add(new SqlParameter("@minionId", id));

    command.ExecuteNonQuery();

    command.Parameters.Clear();

    // Find minion name by given id
    string nameQuery = "SELECT [Name] FROM Minions WHERE Id = @minionId";

    var nameCommand = new SqlCommand(nameQuery, connection);

    nameCommand.Parameters.Add(new SqlParameter("@minionId", id));

    string minionName = (string)nameCommand.ExecuteScalar();

    nameCommand.Parameters.Clear();

    minionName = char.ToUpper(minionName[0]) + minionName.Substring(1);

    // Update minion name
    string changeNameQuery = "UPDATE Minions SET [Name] = @minionName WHERE Id = @minionId";

    var changeNameCommand = new SqlCommand(changeNameQuery, connection);

    changeNameCommand.Parameters.Add(new SqlParameter("@minionName", minionName));
    changeNameCommand.Parameters.Add(new SqlParameter("@minionId", id));

    changeNameCommand.ExecuteNonQuery();

    changeNameCommand.Parameters.Clear();
}