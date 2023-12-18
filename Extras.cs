using Npgsql;

namespace HolidayMakerGroup6;
public class Extras
{

    private readonly NpgsqlDataSource _db;

    public Extras(NpgsqlDataSource db)
    {
        _db = db;
    }

    public async Task <string> ShowAll()
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
            result += " " + reader.GetDecimal(1) + " kr" + new string(' ', 10 - price.Length);
            result += "\n";
        }
        return result;
    }

    public async Task <string> Add()
    {
        string result = string.Empty;

        Console.Clear();
        Console.Write("Write booking ID:  ");
        var bookingIdInput = Console.ReadLine();

        Console.Write("Enter extra service ID:  ");
        var extraIdInput = Console.ReadLine();

        if (!int.TryParse(extraIdInput, out int parsedExtraServiceId) || !int.TryParse(bookingIdInput, out int parsedBookingId))
        {
            result += "Invalid input";
            Console.ReadKey();
            return result;
        }

        using var cmd = _db.CreateCommand ("insert into extra_service_and_bookings (booking_id, extra_service_id) values ($0, $1)");
        cmd.Parameters.AddWithValue("$0", parsedBookingId);
        cmd.Parameters.AddWithValue("$1", parsedExtraServiceId);
        await cmd.ExecuteNonQueryAsync();

        result += "Completed"; 
        return result;

    }
}
