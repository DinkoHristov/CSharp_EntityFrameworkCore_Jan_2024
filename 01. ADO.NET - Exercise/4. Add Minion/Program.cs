using Microsoft.Data.SqlClient;

string connectionString = "Server=DESKTOP-RS7JOPO;Database=MinionsDB;Integrated Security=True;TrustServerCertificate=True";

var connection = new SqlConnection(connectionString);

using (connection)
{
    connection.Open();

    string[] minionInput = Console.ReadLine().Split(" ").TakeLast(3).ToArray();
    string minionName = minionInput[0];
    string minionAge = minionInput[1];
    string townName = minionInput[2];

    ExecuteCreateTown(townName, connection);

    ExecuteCreateMinion(minionName, minionAge, townName, connection);

    string[] villainInfo = Console.ReadLine().Split(" ").TakeLast(1).ToArray();
    string villainName = villainInfo[0];

    ExecuteCreateVillain(villainName, connection);

    ExecuteAddMinionToVillan(minionName, villainName, connection);
}

void ExecuteAddMinionToVillan(string minionName, string villainName, SqlConnection connection)
{
    // Find minionId
    string minionQuery = "SELECT Id FROM Minions WHERE [Name] = @minionName";

    var minionCommand = new SqlCommand(minionQuery, connection);

    minionCommand.Parameters.Add(new SqlParameter("@minionName", minionName));

    int minionId = (int)minionCommand.ExecuteScalar();

    // Find villainId
    string villainQuery = "SELECT Id FROM Villains WHERE [Name] = @villainName";

    var villainCommand = new SqlCommand(villainQuery, connection);

    villainCommand.Parameters.Add(new SqlParameter("@villainName", villainName));

    int villainId = (int)villainCommand.ExecuteScalar();

    // Is created
    string search = "SELECT COUNT(*) FROM MinionsVillains WHERE MinionId = @minionId AND VillainId = @villainId";

    var searchCommand = new SqlCommand(search, connection);

    searchCommand.Parameters.Add(new SqlParameter("@minionId", minionId));
    searchCommand.Parameters.Add(new SqlParameter("@villainId", villainId));

    int foundCount = (int)searchCommand.ExecuteScalar();

    if (foundCount > 0)
    {
        return;
    }

    // Insert Into MinionsVillains
    string query = "INSERT INTO MinionsVillains (MinionId, VillainId) VALUES (@minionId, @villainId)";

    var command = new SqlCommand(query, connection);

    command.Parameters.Add(new SqlParameter("@minionId", minionId));
    command.Parameters.Add(new SqlParameter("@villainId", villainId));

    command.ExecuteNonQuery();

    Console.WriteLine($"Successfully added {minionName} to be minion of {villainName}");
}

void ExecuteCreateVillain(string villainName, SqlConnection connection)
{
    string search = "SELECT [Name] FROM Villains WHERE [Name] = @villainName";

    var searchCommand = new SqlCommand(search, connection);

    searchCommand.Parameters.Add(new SqlParameter("@villainName", villainName));

    string foundName = (string)searchCommand.ExecuteScalar();

    if (foundName != null)
    {
        return;
    }

    string query = "INSERT INTO Villains([Name]) VALUES (@villainName)";

    var command = new SqlCommand(query, connection);

    command.Parameters.Add(new SqlParameter("@villainName", villainName));

    command.ExecuteNonQuery();

    Console.WriteLine($"Villain {villainName} was added to the database.");
}

void ExecuteCreateMinion(string minionName, string minionAge, string townName, SqlConnection connection)
{
    string search = "SELECT [Name] FROM Minions WHERE [Name] = @minionName";

    var searchCommand = new SqlCommand(search, connection);

    searchCommand.Parameters.Add(new SqlParameter("@minionName", minionName));

    string foundName = (string)searchCommand.ExecuteScalar();

    if (foundName != null)
    {
        return;
    }

    string townQuery = "SELECT Id FROM Towns WHERE [Name] = @townName";

    var townCommand = new SqlCommand(townQuery, connection);

    townCommand.Parameters.Add(new SqlParameter("@townName", townName));

    int townId = (int)townCommand.ExecuteScalar();

    string minionQuery = "INSERT INTO Minions([Name], Age, TownId) VALUES (@minionName, @minionAge, @townId)";

    var minionCommand = new SqlCommand(minionQuery, connection);

    minionCommand.Parameters.Add(new SqlParameter("@minionName", minionName));
    minionCommand.Parameters.Add(new SqlParameter("@minionAge", minionAge));
    minionCommand.Parameters.Add(new SqlParameter("@townId", townId));

    minionCommand.ExecuteNonQuery();
}

void ExecuteCreateTown(string townName, SqlConnection connection)
{
    string search = "SELECT [Name] FROM Towns WHERE [Name] = @townName";

    var searchCommand = new SqlCommand(search, connection);

    searchCommand.Parameters.Add(new SqlParameter("@townName", townName));

    string foundName = (string)searchCommand.ExecuteScalar();

    if (foundName != null)
    {
        return;
    }

    string query = "INSERT INTO Towns([Name]) VALUES (@townName)";

    var command = new SqlCommand(query ,connection);

    command.Parameters.Add(new SqlParameter("@townName", townName));

    command.ExecuteNonQuery();

    Console.WriteLine($"Town {townName} was added to the database.");
}