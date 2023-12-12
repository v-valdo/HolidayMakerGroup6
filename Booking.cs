using Npgsql;

namespace HolidayMakerGroup6;
public class Booking
{
	public Customer? Customer;
	public Room? Room;
	public Location? Location;
	public Extras Extra;
	public double Total;

	public Booking(Customer? customer, Room? room, Location? location, Extras extra, double total)
	{
		Customer = customer;
		Room = room;
		Location = location;
		Extra = extra;
		Total = total;
	}

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
		List();
	}

	// callable Method summing the total price (incl. extras) for the booking
	public void CalculatePrice()
	{
		// typ Room.Price + Extras.Price
	}

	public void List()
	{
		// Visa Alla Bookings
	}
}
