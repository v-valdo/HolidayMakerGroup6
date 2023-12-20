using Npgsql;
namespace HolidayMakerGroup6;

public class Booking
{
	private int customerId;
	private int roomId;
	private DateTime startDate;
	private DateTime endDate;
	private int numberOfPeople;
	private double price;

	public async Task New()
	{
		await using var db = NpgsqlDataSource.Create(Database.Url);

		Booking booking = new();
		Customer customer = new();

		bool validInput = false;

		while (true)
		{
			Console.Clear();

			await customer.ShowAll();
			Console.WriteLine();
			Console.Write("Pick a customer (ID) to create booking for or enter 0 to exit: ");

			string customerQuery = "select id, first_name, last_name FROM customers where id = $1";

			if (int.TryParse(Console.ReadLine(), out int selectedID))
			{
				if (selectedID == 0)
				{
					Console.Clear();
					Console.WriteLine("Returning to menu");
					return;
				}
				var cmd = db.CreateCommand(customerQuery);

				cmd.Parameters.AddWithValue(selectedID);

				var reader = await cmd.ExecuteReaderAsync();

				if (await reader.ReadAsync())
				{
					customer.customerID = reader.GetInt32(0);
					customer.firstName = reader.GetString(1);
					customer.surName = reader.GetString(2);

					Console.Clear();
					Console.WriteLine($"You picked customer \"{customer.firstName} {customer.surName}\" with ID {customer.customerID}\n");

					Console.Write($"Proceed to room selection? (Y/N): ");

					string? input = Console.ReadLine();

					while (true)
					{
						if (input.ToLower() == "y")
						{
							Console.Clear();

							roomId = await AssignRoom(booking);

							if (roomId == 0 || roomId > 30)
							{
								Console.Clear();
								Console.WriteLine("Valid room must be assigned to the booking. Returning to main menu");
								Console.ReadKey();
								break;
							}
							else
							{
								(startDate, endDate) = await AssignDates(roomId, booking);
								validInput = true;
								break;
							}
						}
						else if (input.ToLower() == "n")
						{
							Console.Clear();
							Console.WriteLine("Booking Cancelled");
							break;
						}
						else
						{
							Console.Clear();
							Console.WriteLine("Invalid input, try again");
							Console.ReadLine();
							await New();
							break;
						}
					}
				}
				else
				{
					Console.Clear();
					Console.WriteLine($"No customer with ID {selectedID} found. Press any key to return.");
					Console.ReadKey();
				}
			}
			else
			{
				Console.Clear();
				Console.WriteLine("Invalid input, try booking again");
				Console.ReadKey();
				await New();
				break;
			}
			break;
		}

		if (validInput)
		{
			while (true)
			{
				Console.Clear();
				Console.WriteLine("Enter amount of residents");
				if (int.TryParse(Console.ReadLine(), out int residents))
				{
					if (residents > 5 || residents < 1)
					{
						Console.WriteLine("Max 5 and minimum 1 resident! Digits only");
						Console.ReadLine();
						continue;
					}
					else
					{
						numberOfPeople = residents;
						break;
					}
				}
			}

			Console.Clear();

			customerId = customer.customerID;
			price = await CalculatePrice(roomId, startDate, endDate);

			Console.WriteLine("Booking summary");
			Console.WriteLine("----------------");
			Console.WriteLine($"Customer ID:          {customerId}");
			Console.WriteLine($"Room ID:              {roomId}");
			Console.WriteLine($"Start Date:           {startDate.ToShortDateString()}");
			Console.WriteLine($"End Date:             {endDate.ToShortDateString()}");
			Console.WriteLine($"People:               {numberOfPeople}");
			Console.WriteLine($"Price (excluding extras): {price:C}");
			Console.WriteLine();

			Console.WriteLine("Please review the booking details and type \"CONFIRM\" to finalize booking\nEnter \"CANCEL\" to cancel.");
			string? input = string.Empty;
			input = Console.ReadLine();

			if (input == "CANCEL")
			{
				Console.Clear();
				Console.WriteLine("Booking cancelled");
				return;
			}
			else if (input == "CONFIRM")
			{
				string qBook = "insert into bookings(customer_id, start_date, end_date, room_id, number_of_people, price) VALUES ($1, $2, $3, $4, $5, $6)";

				using var cmd = db.CreateCommand(qBook);

				cmd.Parameters.AddWithValue(customerId);
				cmd.Parameters.AddWithValue(startDate);
				cmd.Parameters.AddWithValue(endDate);
				cmd.Parameters.AddWithValue(roomId);
				cmd.Parameters.AddWithValue(numberOfPeople);
				cmd.Parameters.AddWithValue(price);

				var insert = cmd.ExecuteNonQueryAsync();

				await insert;

				if (insert.IsCompleted)
				{
					Console.Clear();
					Console.WriteLine("BOOKING CONFIRMED");
					Console.ReadLine();
				}

				if (customerId == 0 || roomId == 0 || price == 0)
				{
					Console.WriteLine("An error has occurred during the booking process - please create a new booking.");
					return;
				}
			}
		}
	}

	public async Task<(DateTime, DateTime)> AssignDates(int roomId, Booking booking)
	{
		Console.Clear();
		while (true)
		{
			Console.Write("Enter start date for booking [YYYY-MM-DD]: ");
			if (DateTime.TryParse(Console.ReadLine(), out DateTime startDate))
			{
				Console.Write("Enter end date [YYYY-MM-DD]: ");
				if (DateTime.TryParse(Console.ReadLine(), out DateTime endDate))
				{
					if (await CheckAvailable(roomId, startDate, endDate) && startDate >= new DateTime(2022, 6, 1) && endDate <= new DateTime(2022, 7, 31))
					{
						Console.WriteLine($"Room {roomId} is available between {startDate.ToShortDateString()} and {endDate.ToShortDateString()}!");
						Console.ReadKey();
						return (startDate, endDate);
					}
					else
					{
						Console.Clear();
						Console.WriteLine("Room is not available for the selected dates. Press any key to try again.\nPlease note rooms are only available between 2022-06-01 and 2022-07-31");
						Console.ReadKey();
					}
				}
				else
				{
					Console.WriteLine("Wrong format for end date. Press any key to try again.");
					Console.ReadKey();
					Console.Clear();
				}
			}
			else
			{
				Console.WriteLine("Wrong format for start date. Press any key to try again.");
				Console.ReadKey();
				Console.Clear();
			}
		}
	}

	public async Task<bool> CheckAvailable(int roomId, DateTime startDate, DateTime endDate)
	{
		await using var db = NpgsqlDataSource.Create(Database.Url);

		const string query = @"select id from bookings 
							where room_id = $1 
							AND start_date >= $2
							AND end_date <= $3
";

		var cmd = db.CreateCommand(query);

		cmd.Parameters.AddWithValue(roomId);
		cmd.Parameters.AddWithValue(startDate);
		cmd.Parameters.AddWithValue(endDate);

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
	public async Task SelectDelete()
	{
		await List();
		Console.Write("Choose a BookingID to delete: ");
		if (int.TryParse(Console.ReadLine(), out int selectedBookingNumber))
		{
			Console.Clear();
			await Delete(selectedBookingNumber);
		}
		else
		{
			Console.WriteLine("Invalid input.");
			Console.Clear();
		}
	}
	public async Task Delete(int bookingNumber)
	{
		await using var connection = NpgsqlDataSource.Create(Database.Url);
		await connection.OpenConnectionAsync();

		using var cmd = connection.CreateCommand("SELECT CONCAT(c.first_name, ' ', c.last_name) AS CustomerName, * FROM BOOKINGS b JOIN Customers c ON b.customer_id = c.id WHERE b.id = $1;");
		cmd.Parameters.AddWithValue(bookingNumber);

		using var reader = await cmd.ExecuteReaderAsync();

		if (await reader.ReadAsync())
		{
			string customerName = reader.GetString(reader.GetOrdinal("CustomerName"));
			int customerId = reader.GetInt32(reader.GetOrdinal("customer_id"));
			DateTime startDate = reader.GetDateTime(reader.GetOrdinal("start_date"));
			DateTime endDate = reader.GetDateTime(reader.GetOrdinal("end_date"));
			int roomId = reader.GetInt32(reader.GetOrdinal("room_id"));
			int numberOfPeople = reader.GetInt32(reader.GetOrdinal("number_of_people"));
			int price = reader.GetInt32(reader.GetOrdinal("price"));

			Console.WriteLine("Customer: " + customerName);
			Console.WriteLine("------------------------------");
			Console.WriteLine($"Booking ID: " + bookingNumber);
			Console.WriteLine("Customer ID: " + customerId);
			Console.WriteLine("Start Date: " + startDate.ToShortDateString());
			Console.WriteLine("End Date: " + endDate.ToShortDateString());
			Console.WriteLine("Room ID: " + roomId);
			Console.WriteLine("Number of People: " + numberOfPeople);
			Console.WriteLine("Price: " + price);


			Console.WriteLine("------------------------------");
			Console.WriteLine("Are you sure you wan to delete this booking? [y/n]");
			string deleteInput = Console.ReadLine().ToLower();

			if (deleteInput == "y")
			{

				using var deleteCmd = connection.CreateCommand("DELETE FROM bookings WHERE id = $1;");
				deleteCmd.Parameters.AddWithValue(bookingNumber);
				await deleteCmd.ExecuteNonQueryAsync();

				Console.Clear();
				Console.WriteLine("Booking with ID " + bookingNumber + " has been successfully deleted. Returning to Booking Menu...");

			}
			else if (deleteInput == "n")
			{
				Console.Clear();
				Console.WriteLine("Nothing has has been removed. Returning to Booking Menu...");
				return;
			}
			else
			{
				Console.Clear();
				Console.WriteLine("Invalid input, answer [y/n]. Returning to Booking Menu...");
			}

		}
		else
		{
			Console.WriteLine($"Booking with ID {bookingNumber} not found.");
		}
	}
	public async Task AddExtras()
	{
		await using var db = NpgsqlDataSource.Create(Database.Url);
		Extras extras = new(db);
		await Console.Out.WriteLineAsync(await extras.Add());
	}

	public async Task<double> CalculatePrice(int roomId, DateTime startDate, DateTime endDate)
	{
		using var db = NpgsqlDataSource.Create(Database.Url);

		const string qGetPrice = @"select price from rooms where id = $1";

		var cmd = db.CreateCommand(qGetPrice);
		cmd.Parameters.AddWithValue(roomId);

		var reader = await cmd.ExecuteReaderAsync();

		while (true)
		{
			while (await reader.ReadAsync())
			{
				if (reader.HasRows)
				{
					int totalDays = (int)(endDate - startDate).TotalDays;
					double price = reader.GetDouble(0) * totalDays;
					return price;
				}
			}
		}
	}

    public async Task List()
    {
        Console.Clear();
        await using var connection = NpgsqlDataSource.Create(Database.Url);

        await connection.OpenConnectionAsync();

        using var cmd = connection.CreateCommand("SELECT CONCAT(c.first_name, ' ', c.last_name) AS Customer, b.customer_id AS CustomerID, b.id AS BookingID, (b.start_date || ' - ' || b.end_date) AS StartdateEndDate, room_id AS RoomID FROM BOOKINGS b JOIN Customers c ON b.customer_id = c.id;");

        using var reader = await cmd.ExecuteReaderAsync();

        Console.WriteLine("List of Bookings:");
        Console.WriteLine(new string('-', 80));
        Console.WriteLine("{0,-12} {1,-20} {2,-12} {3,-25} {4,-10}", "BookingID", "Customer", "CustomerID", "Date", "RoomID");
        Console.WriteLine(new string('-', 80));

        while (await reader.ReadAsync())
        {
            string customerName = reader.GetString(reader.GetOrdinal("Customer"));
            int customerId = reader.GetInt32(reader.GetOrdinal("CustomerID"));
            int bookingId = reader.GetInt32(reader.GetOrdinal("BookingID"));
            int roomID = reader.GetInt32(reader.GetOrdinal("RoomID"));
            string startdateEndDate = reader.GetString(reader.GetOrdinal("StartdateEndDate"));

            Console.WriteLine("{0,-12} {1,-20} {2,-12} {3,-25} {4,-10}", bookingId, customerName, customerId, startdateEndDate, roomID);
        }

        Console.WriteLine(new string('-', 80));
    }
    public async Task SelectEdit()
	{
		await List();
		Console.Write("Choose a BookingID to edit: ");
		if (int.TryParse(Console.ReadLine(), out int selectedBookingNumber))
		{
			Console.Clear();
			await EditBooking(selectedBookingNumber);

		}
		else
		{
			Console.WriteLine("Invalid input.");
			Console.Clear();

		}
	}
	public async Task EditBooking(int bookingNumber)
	{
		await using var connection = NpgsqlDataSource.Create(Database.Url);
		await connection.OpenConnectionAsync();

		using var cmd = connection.CreateCommand("SELECT CONCAT(c.first_name, ' ', c.last_name) AS CustomerName, * FROM BOOKINGS b JOIN Customers c ON b.customer_id = c.id WHERE b.id = $1;");
		cmd.Parameters.AddWithValue(bookingNumber);

		using var reader = await cmd.ExecuteReaderAsync();

		if (await reader.ReadAsync())
		{
			string customerName = reader.GetString(reader.GetOrdinal("CustomerName"));
			int customerId = reader.GetInt32(reader.GetOrdinal("customer_id"));
			DateTime startDate = reader.GetDateTime(reader.GetOrdinal("start_date"));
			DateTime endDate = reader.GetDateTime(reader.GetOrdinal("end_date"));
			int roomId = reader.GetInt32(reader.GetOrdinal("room_id"));
			int numberOfPeople = reader.GetInt32(reader.GetOrdinal("number_of_people"));
			int price = reader.GetInt32(reader.GetOrdinal("price"));

			Console.WriteLine("Editing Customer: " + customerName);
			Console.WriteLine("------------------------------");
			Console.WriteLine($"Booking ID: " + bookingNumber);
			Console.WriteLine("Customer ID: " + customerId);
			Console.WriteLine("Start Date: " + startDate.ToShortDateString());
			Console.WriteLine("End Date: " + endDate.ToShortDateString());
			Console.WriteLine("Room ID: " + roomId);
			Console.WriteLine("Number of People: " + numberOfPeople);
			Console.WriteLine("Price: " + price);


			Console.WriteLine("------------------");
			Console.WriteLine("Choose an option:");
			Console.WriteLine("1. Edit Start Date");
			Console.WriteLine("2. Edit End Date");
			Console.WriteLine("3. Edit Room ID");
			Console.WriteLine("4. Edit Number of People");
			Console.WriteLine("5. Edit Price");

			Console.Write("Enter your choice: ");
			if (int.TryParse(Console.ReadLine(), out int userChoice))
			{
				switch (userChoice)
				{
					case 1:
						Console.Clear();
						var editBookings = new EditBookings();
						await editBookings.ChangeStartDate(bookingNumber);
						break;
					case 2:
						Console.Clear();
						editBookings = new EditBookings();
						await editBookings.ChangeEndDate(bookingNumber);
						break;
					case 3:
						Console.Clear();
						editBookings = new EditBookings();
						await editBookings.ChangeRoomID(bookingNumber);
						break;
					case 4:
						Console.Clear();
						editBookings = new EditBookings();
						await editBookings.ChangeNoP(bookingNumber);
						break;
					case 5:
						Console.Clear();
						editBookings = new EditBookings();
						await editBookings.ChangePrice(bookingNumber);
						break;

					default:
						Console.WriteLine("Invalid input. Returning to main menu.");
						break;
				}
			}
			else
			{
				Console.WriteLine("Invalid input. Returning to main menu.");
			}
		}
		else
		{
			Console.WriteLine($"Booking with ID {bookingNumber} not found.");
		}
	}

	public async Task<int> AssignRoom(Booking booking)
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
				string roomQuery = "select id from rooms where id = $1";

				var cmd = db.CreateCommand(roomQuery);

				cmd.Parameters.AddWithValue(selectedRoom);

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
							return selectedRoom;
						}
						else if (input.ToLower() == "n")
						{
							return 0;
						}
						else
						{
							Console.WriteLine("Invalid input");
							Console.ReadKey();
							break;
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
			}
		}
		return 0;
	}
}