using Npgsql;

namespace HolidayMakerGroup6;
public class Booking
{
	public Customer? Customer;
	public Room? Room;
	public Location? Location;
	public Extras Extra;
	public double total;

	public async Task Add()
	{
		await using var db = NpgsqlDataSource.Create(Database.Url);


		// räkna ut priset-metod
		CalculatePrice();
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
        
    }
}
