using Npgsql;
namespace HolidayMakerGroup6;

public class Extras
{
	private readonly NpgsqlDataSource _db;

	public Extras(NpgsqlDataSource db)
	{
		_db = db;
	}

    public async Task <string> ShowAllExtras()
    {
        string result = string.Empty;
        const string query = @"select id, name, price from extra_service;";

        await Console.Out.WriteLineAsync("---------------------------------------------------\n" +
                                         "ID     || Service             || Price               \n" +
                                         "-------||---------------------||-------------------");
        var reader = await _db.CreateCommand(query).ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            string id = reader.GetInt32(0).ToString();
            string service = reader.GetString(1);
            string price = reader.GetDecimal(2).ToString();

            result += " " + reader.GetInt32(0) + new string(' ', 6 - id.Length);
            result += "||";
            result += " " + reader.GetString(1) + new string(' ', 20 - service.Length);
            result += "||";
            result += " " + reader.GetDecimal(2) + " kr" + new string(' ', 10 - price.Length);
            result += "\n";
        }
        return result;
    }

    public async Task<string> Add()
    {
        const string qPriceUpdateExtraBooking = @" 
            UPDATE extra_service_and_bookings eb
            SET price = (select e.price * EXTRACT(days from AGE(b.end_date, b.start_date)) +1
					            from rooms r, extra_service e, bookings b
					            WHERE b.room_id = r.id AND eb.booking_id = b.id AND e.id = eb.extra_service_id)
            FROM rooms r, extra_service e, bookings b
            WHERE b.room_id = r.id AND eb.booking_id = b.id AND e.id = eb.extra_service_id;
        ";
        try
        {
            Console.Clear();
            Booking booking = new();
            await booking.List();
            await Console.Out.WriteAsync("\nInsert booking id: ");
            var bookingidinput = Console.ReadLine();
            await Console.Out.WriteAsync(await ShowAllExtras());
            await Console.Out.WriteLineAsync("---------------------------------------------------");
            await Console.Out.WriteAsync("Insert extras id: ");
            var extraserviceidinput = Console.ReadLine();

			Console.Clear();

			if (!int.TryParse(bookingidinput, out var parsedbookingid) || !int.TryParse(extraserviceidinput, out var parsedextraserviceid))
			{
				string answer = "Invalid input. Try again";

				Console.ReadKey();
				return answer;
			}

			using var cmd = _db.CreateCommand("INSERT INTO extra_service_and_bookings (booking_id, extra_service_id) VALUES ($1, $2)");

            cmd.Parameters.AddWithValue(parsedbookingid);
            cmd.Parameters.AddWithValue(parsedextraserviceid);

            await cmd.ExecuteNonQueryAsync();
            await _db.CreateCommand(qPriceUpdateExtraBooking).ExecuteNonQueryAsync();
            return "Record added successfully!";
        }
        catch (Exception ex)
        {
            return $"An error has occurred while adding the extra service to booking:\n\n{ex.Message}";
        }

    }
    public async Task<string> ViewBookingExtras()
    {
        string result = string.Empty;
        const string qBookingExtras = @"select eb.booking_id, string_agg(e.name, ', ')
            from extra_service_and_bookings eb 
            join extra_service e on eb.extra_service_id = e.id
            group by eb.booking_id
            order by eb.booking_id
        ;";

        await Console.Out.WriteLineAsync("Booking ID    || Service             \n" +
                                         "--------------||---------------------");
        var reader = await _db.CreateCommand(qBookingExtras).ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            string id = reader.GetInt32(0).ToString();
            string service = reader.GetString(1);

            result += " " + reader.GetInt32(0) + new string(' ', 13 - id.Length);
            result += "||";
            result += " " + reader.GetString(1) + new string(' ', 80 - service.Length);
            result += "\n";
        }
        return result;
    }
}