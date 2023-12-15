using Npgsql;

namespace HolidayMakerGroup6;
public class Extras
{

    private readonly NpgsqlDataSource _db;

    public Extras(NpgsqlDataSource db)
    {
        _db = db;
    }

    public async Task<string> ShowAll()
    {
        string result = string.Empty;
        const string query = @"select name, price from extra_service;";

        await Console.Out.WriteLineAsync("Service              || Price               \n" +
                                        "---------------------||-----------------------");
        var reader = await _db.CreateCommand(query).ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            string service = reader.GetString(0);
            string price = reader.GetDecimal(1).ToString();

            result += " " + reader.GetString(0) + new string(' ', 20 - service.Length);
            result += "||";
            result += " " + reader.GetDecimal(1) + new string(' ', 10 - price.Length);
            result += "\n";
        }
        return result;
    }

    public async Task Add()
    {


    }
}
