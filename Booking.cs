using Npgsql;
using System.Runtime.InteropServices;

namespace HolidayMakerGroup6;
public class Booking
{
	public Customer? Customer;
	public Room? Room;
	public Location? Location;
	public Extras Extra;
	public double Total;

	public async Task Add()
	{
		await using var db = NpgsqlDataSource.Create(Database.Url);
		Console.Write("Pick a customer to create booking for");


		// räkna ut priset-metod
		CalculatePrice();
		// Write(); -- insert into bookings och X-table (extra services)
	}

	public async Task Delete()
	{
		List();
	}

	public async Task Edit()
	{
        // Visa alla bookings -> Välj booking att edita

    }

    // callable Method summing the total price (incl. extras) for the booking
    public void CalculatePrice()
	{
		// typ Room.Price + Extras.Price
	}

    public async Task List()
    {
        await using var connection = NpgsqlDataSource.Create(Database.Url);

        await connection.OpenConnectionAsync();

        using var cmd = connection.CreateCommand("SELECT CONCAT(c.first_name, ' ', c.last_name) AS Customer, b.customer_id AS CustomerID, b.id AS BookingID FROM BOOKINGS b JOIN Customers c ON b.customer_id = c.id;");

        using var reader = await cmd.ExecuteReaderAsync();

        Console.WriteLine("List of Bookings:");

        int count = 1;

        while (await reader.ReadAsync())
        {
            string customerName = reader.GetString(reader.GetOrdinal("Customer"));
            int customerId = reader.GetInt32(reader.GetOrdinal("CustomerID"));
            int bookingId = reader.GetInt32(reader.GetOrdinal("BookingID"));

            Console.WriteLine($"{count}. {customerName}, Customer ID: {customerId}, Booking ID: {bookingId}");
            count++;
        }

    }
}



