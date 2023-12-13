using Npgsql;

namespace HolidayMakerGroup6;
public class Booking
{
	public Room? Room;
	public Extras Extra;
	public double Total;

	public async Task Menu()
	{
		while (true)
		{
			Console.WriteLine("---- Bookings ----\n" +
						  "1. Create new booking\n" +
						  "2. Edit booking\n" +
						  "3. Delete booking\n" +
						  "4. View bookings\n" +
						  "5. Return to main menu\n");
			Console.Write("Enter choice:  ");
			int.TryParse(Console.ReadLine(), out int answer);

			switch (answer)
			{
				case 1:
					Console.Clear();
					await New();
					break;
				case 2:
					Console.Clear();
					await Edit();
					break;
				case 3:
					Console.Clear();
					await List();
					break;
				case 4:
					Console.Clear();
					Console.ReadKey();
					break;
				case 5:
					Console.Clear();
					return;
			}
		}
	}
	public async Task New()
	{
		await using var db = NpgsqlDataSource.Create(Database.Url);

		Booking booking = new();
		Customer customer = new();

		Console.WriteLine($"Press any key to select from all customers");
		Console.ReadKey();

		await customer.ShowAll();

		while (true)
		{
			Console.Write("Pick a customer (ID) to create booking for: ");

			string query = "select id, first_name, last_name FROM customers where id = @customerID";

			if (int.TryParse(Console.ReadLine(), out int selectedID))
			{
				var cmd = db.CreateCommand(query);

				cmd.Parameters.AddWithValue("customerID", selectedID);

				var reader = await cmd.ExecuteReaderAsync();

				if (await reader.ReadAsync())
				{
					customer.customerID = reader.GetInt32(0);
					customer.firstName = reader.GetString(1);
					customer.surName = reader.GetString(2);

					Console.Clear();
					Console.WriteLine($"You picked customer \"{customer.firstName} {customer.surName}\" with ID {customer.customerID}\n");
					Console.Write($"Proceed? (Y/N): ");

					string? input = Console.ReadLine();
					while (input == "")
					{
						if (input.ToLower() == "y")
						{
							Console.WriteLine("Pick a room");

						}
						else if (input.ToLower() == "n")
						{
							break;
						}
						else
						{
							Console.WriteLine("Invalid input");
						}
					}
				}
				else
				{
					Console.WriteLine($"No customer with ID {selectedID} found. Press any key to return.");
					Console.ReadKey();
					break;
				}
			}
		}

		// räkna ut priset-metod
		CalculatePrice();
		// Write(); -- insert into bookings och X-table (extra services)
	}

	public async Task AssignDates(int roomId)
	{
		Console.Write("Enter start date for booking [YYYY-MM-DD]: ");
		if (!DateTime.TryParse(Console.ReadLine(), out DateTime startDate))
		{
			Console.WriteLine("Wrong format\n Press any key to try again");
			Console.ReadKey();
			Console.Clear();
			return;
		}

		Console.Write("Enter end date [YYYY-MM-DD]: ");
		if (!DateTime.TryParse(Console.ReadLine(), out DateTime endDate))
		{
			Console.WriteLine("Wrong format\n Press any key to try again");
			Console.ReadKey();
			Console.Clear();
			return;
		}

		if (startDate < new DateTime(2022, 06, 01) || endDate > new DateTime(2022, 07, 31))
		{
			Console.WriteLine("rooms are only available between 2022-06-01 and 2022-07-31.");
			return;
		}

		// bool available = await IsRoomAvailable(roomId, startDate, endDate);

		/* if (available)
		{
			Console.WriteLine("Room available!");
			// spara datum i variabel för insert senare
			Console.WriteLine($"Booking dates: {startDate} to {endDate}");
		}
		else
		{
			Console.WriteLine($"Room is not available between {startDate} and {endDate}");
		} */
	}

	public async Task Delete()
	{
		await List();
	}

	public async Task AddExtras()
	{

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

	}

	public async Task Insert()
	{
		// skriv in bokning
	}
}
