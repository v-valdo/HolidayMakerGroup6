using Npgsql;

namespace HolidayMakerGroup6;
public class Booking
{
	public int RoomID;
	public double Total;
	public async Task New()
	{
		await using var db = NpgsqlDataSource.Create(Database.Url);

		Booking booking = new();
		Customer customer = new();

		Console.WriteLine($"Press any key to select from all customers");
		Console.ReadKey();
		Console.Clear();


		while (true)
		{
			Console.Clear();

			await customer.ShowAll();

			Console.Write("Pick a customer (ID) to create booking for: ");

			string customerQuery = "select id, first_name, last_name FROM customers where id = @customerID";

			if (int.TryParse(Console.ReadLine(), out int selectedID))
			{
				var cmd = db.CreateCommand(customerQuery);

				cmd.Parameters.AddWithValue("customerID", selectedID);

				var reader = await cmd.ExecuteReaderAsync();

				if (await reader.ReadAsync())
				{
					customer.customerID = reader.GetInt32(0);
					customer.firstName = reader.GetString(1);
					customer.surName = reader.GetString(2);

					Console.WriteLine($"You picked customer \"{customer.firstName} {customer.surName}\" with ID {customer.customerID}\n");

					Console.Write($"Proceed to room selection? (Y/N): ");

					string? input = Console.ReadLine();

					while (true)
					{
						if (input.ToLower() == "y")
						{
							Console.Clear();
							await booking.AssignRoom(booking);
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
			else
			{
				Console.WriteLine("Invalid input");
			}
		}

		// räkna ut priset-metod
		CalculatePrice();
		// Write(); -- insert into bookings och X-table (extra services)
	}

	public async Task AssignDates(int roomId, Booking booking)
	{
		Console.Clear();
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

		bool available = await CheckAvailable(roomId, startDate, endDate);

		if (available)
		{
			Console.Clear();
			Console.WriteLine("Room available!");
			// spara datum i variabel för insert senare
			Console.WriteLine($"Booking dates: {startDate.ToShortDateString()} to {endDate.ToShortDateString()}");
			await booking.Confirm();
		}
		else
		{
			Console.Clear();
			Console.WriteLine($"Room is not available between {startDate} and {endDate}\n Please pick another room");
			await booking.AssignRoom(booking);
		}
	}

	public async Task<bool> CheckAvailable(int roomId, DateTime startDate, DateTime endDate)
	{
		await using var db = NpgsqlDataSource.Create(Database.Url);

		const string query = @"select id from bookings 
								where room_id = @roomId 
								AND start_date <= @endDate 
								AND end_date <= @startDate
";

		var cmd = db.CreateCommand(query);

		cmd.Parameters.AddWithValue("roomid", roomId);
		cmd.Parameters.AddWithValue("startdate", startDate);
		cmd.Parameters.AddWithValue("enddate", endDate);

		var reader = await cmd.ExecuteReaderAsync();

		while (await reader.ReadAsync())
		{
			if (reader.HasRows)
			{
				return false;
			}
			else
			{
				return true;
			}
		}
		return true;
	}

	public async Task Delete()
	{
		await List();
	}

	public async Task AddExtras()
	{
		Extras add = new();
		await add.ShowAll();
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

	public async Task Confirm()
	{
		// skriv in bokning
		Console.WriteLine("Type \"CONFIRM\" to confirm booking");
		Console.ReadLine();
	}

	public async Task AssignRoom(Booking booking)
	{
		Console.Clear();
		await using var db = NpgsqlDataSource.Create(Database.Url);

		Room room = new();

		while (true)
		{
			await room.ViewAll();
			Console.WriteLine();

			Console.Write("Pick a room (ID): ");

			if (int.TryParse(Console.ReadLine(), out int selectedRoom))
			{
				string roomQuery = "select id from rooms where id = @roomid";
				var cmd = db.CreateCommand(roomQuery);

				cmd.Parameters.AddWithValue("roomid", selectedRoom);

				var reader = await cmd.ExecuteReaderAsync();

				if (await reader.ReadAsync())
				{
					Console.Clear();
					Console.WriteLine($"You picked room with ID {selectedRoom}\nProceed to assign dates (Y/N)?");

					string? input = Console.ReadLine();
					while (true)
					{
						if (input.ToLower() == "y")
						{
							Console.Clear();
							await booking.AssignDates(selectedRoom, booking);
							break;
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
					Console.WriteLine($"No room with ID {selectedRoom} found. Press any key to return.");
					Console.ReadKey();
					break;
				}
			}
			else
			{
				Console.WriteLine("Invalid input");
				return;
			}
		}
	}
}
