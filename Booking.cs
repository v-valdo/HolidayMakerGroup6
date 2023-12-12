using Npgsql;

namespace HolidayMakerGroup6;
public class Booking
{
	public Customer? Customer;
	public Room? Room;
	public Location? Location;
	public Extras Extra;
	public double Total;

	//public Booking(Customer? customer, Room? room, Location? location, Extras extra, double total)
	//{
	//	Customer = customer;
	//	Room = room;
	//	Location = location;
	//	Extra = extra;
	//	Total = total;
	//}

	public async Task Add()
	{
		await using var db = NpgsqlDataSource.Create(Database.Url);

		Customer customer = new();
		await customer.ShowAll();
		Console.Write("Pick a customer (ID) to create booking for: ");

		const string query = "select id from customers";
		var reader = await db.CreateCommand(query).ExecuteReaderAsync();

		if (int.TryParse(Console.ReadLine(), out int c))
		{
			while (await reader.ReadAsync())
			{
				if (reader.GetInt32(0) == c)
				{
					Console.WriteLine("found");
					break;
				}
			}
			Console.ReadKey();
		}

		// räkna ut priset-metod
		CalculatePrice();
		// Write(); -- insert into bookings och X-table (extra services)
	}

	public async Task Delete()
	{
		await List();
	}

	public async Task Edit()
	{
		// Visa alla bookings -> Välj booking att edita
		await List();
	}

	// callable Method summing the total price (incl. extras) for the booking
	public void CalculatePrice()
	{
		// typ Room.Price + Extras.Price
	}
	public async Task List()
	{
		Customer customer = new();
		await customer.ShowAll();
	}
}
