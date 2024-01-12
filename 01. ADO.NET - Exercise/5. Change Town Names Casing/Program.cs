using Microsoft.Data.SqlClient;

string connectionString = "Server=DESKTOP-RS7JOPO;Database=MinionsDB;Integrated Security=True;TrustServerCertificate=True";

var connection = new SqlConnection(connectionString);

using (connection)
{
    connection.Open();

    string countryName = Console.ReadLine();

    ExecuteChangeTownNames(countryName, connection);
}

void ExecuteChangeTownNames(string countryName, SqlConnection connection)
{
    string query = "SELECT t.[Name] " +
                    "FROM Countries AS c " +
                    "JOIN Towns AS t ON c.Id = t.CountryCode " +
                    "WHERE c.[Name] = @countryName";

    var command = new SqlCommand(query, connection);

    command.Parameters.Add(new SqlParameter("@countryName", countryName));

    var reader = command.ExecuteReader();

    List<string> townNames = new List<string>();
    List<string> townNamesUpperCase = new List<string>();

    using (reader)
    {
        while(reader.Read())
        {
            townNames.Add((string)reader[0]);
            townNamesUpperCase.Add(((string)reader[0]).ToUpper());
        }
    }

    if (townNames.Any())
    {
        string replaceQuery = "UPDATE Towns SET [Name] = @upperCaseName WHERE [Name] = @currentName";

        var replaceCommand = new SqlCommand(replaceQuery, connection);

        foreach (string name in townNames)
        {
            replaceCommand.Parameters.Add(new SqlParameter("@upperCaseName", name.ToUpper()));
            replaceCommand.Parameters.Add(new SqlParameter("@currentName", name));

            replaceCommand.ExecuteNonQuery();

            replaceCommand.Parameters.Clear();
        }

        Console.WriteLine($"{townNames.Count} town names were affected.\n{string.Join(", ", townNamesUpperCase)}");
    }
    else
    {
        Console.WriteLine("No town names were affected.");
    }
}