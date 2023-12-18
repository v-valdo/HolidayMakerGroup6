using Npgsql;

namespace HolidayMakerGroup6;
public class Extras
{
    public int bookingid;
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

    public async Task<string> Add()
    {

        try
        {
            Console.Clear();
            Console.WriteLine("Booking id: ");
            var bookingidinput = Console.ReadLine();
            Console.Write("Extras id: ");
            var extraserviceidinput = Console.ReadLine();

            Console.Clear();

            if (!int.TryParse(bookingidinput, out var parsedbookingid) || !int.TryParse(extraserviceidinput, out var parsedextraserviceid))
            {
                string answer = "Invalid input. Try again";

                Console.ReadKey();
                return answer;
            }

            using var cmd = _db.CreateCommand($"INSERT INTO extra_service_and_bookings (booking_id, extra_service_id) VALUES ({parsedbookingid}, {parsedextraserviceid})");

            await cmd.ExecuteNonQueryAsync();
            return "Record added successfully!";
        }
        catch (Exception ex)
        {
            return $"An error has occurred while adding the extra service to booking:\n\n{ex.Message}";
        }

    }

}