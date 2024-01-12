using Microsoft.Data.SqlClient;

string connectionString = "Server=DESKTOP-RS7JOPO;Database=MinionsDB;Integrated Security=True;TrustServerCertificate=True";

var connection = new SqlConnection(connectionString);

using (connection)
{
    connection.Open();

    ExecutePrintAllMinionsNames(connection);
}

void ExecutePrintAllMinionsNames(SqlConnection connection)
{
    string query = "SELECT [Name] FROM Minions";

    var command = new SqlCommand(query, connection);

    List<string> minionNames = new List<string>();

    var reader = command.ExecuteReader();

    using (reader)
    {
        while(reader.Read())
        {
            minionNames.Add((string)reader[0]);
        }
    }

    List<string> orderedNames = new List<string>();

    int length = minionNames.Count / 2;

    if (minionNames.Count % 2 == 0)
    {
        for (int i = 0; i < length; i++)
        {
            orderedNames.Add(minionNames[i]);
            orderedNames.Add(minionNames[minionNames.Count - i - 1]);
        }
    }
    else
    {
        for (int i = 0; i <= length; i++)
        {
            if (i == length)
            {
                orderedNames.Add(minionNames[i]);
                break;
            }

            orderedNames.Add(minionNames[i]);
            orderedNames.Add(minionNames[minionNames.Count - i - 1]);
        }
    }

    Console.WriteLine(string.Join("\n", orderedNames));
}