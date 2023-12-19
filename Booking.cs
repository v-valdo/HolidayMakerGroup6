
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
							Console.WriteLine("Invalid input");
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
				Console.WriteLine("Invalid input");
				Console.ReadKey();
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
				return;
			}
			else if (input == "CONFIRM")
			{
				string qBook = "insert into bookings(customer_id, start_date, end_date, room_id, number_of_people, price) VALUES (@customerId, @startDate, @endDate, @roomId, @numberOfPeople, @price)";

				using var cmd = db.CreateCommand(qBook);

				cmd.Parameters.AddWithValue("@customerId", customerId);
				cmd.Parameters.AddWithValue("@startDate", startDate);
				cmd.Parameters.AddWithValue("@endDate", endDate);
				cmd.Parameters.AddWithValue("@roomId", roomId);
				cmd.Parameters.AddWithValue("@numberOfPeople", numberOfPeople);
				cmd.Parameters.AddWithValue("@price", price);

				var insert = cmd.ExecuteNonQueryAsync();

				await insert;

				if (insert.IsCompleted)
				{
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
					if (await CheckAvailable(roomId, startDate, endDate))
					{
						Console.WriteLine($"Room {roomId} is available between {startDate.ToShortDateString()} and {endDate.ToShortDateString()}!");
						Console.ReadKey();
						return (startDate, endDate);
					}
					else
					{
						Console.WriteLine("Room is not available for the selected dates. Press any key to try again.");
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

		await using var db = NpgsqlDataSource.Create(Database.Url);

		const string qDelete = "delete from bookings where id = @bookingID";

		const string qCheck = "select * from bookings where id = @bookingID";

		await using var cmdCheck = db.CreateCommand(qCheck);


		Console.WriteLine("\n- - - - - - - - - - - - - - - - - - - - -");
		Console.WriteLine("Select which booking to delete from system");

		while (true)
		{
			if (int.TryParse(Console.ReadLine(), out int selectedID))
			{
				cmdCheck.Parameters.AddWithValue("@bookingID", selectedID);

				var reader = await cmdCheck.ExecuteReaderAsync();
				while (await reader.ReadAsync())
				{
					if (reader.HasRows)
					{
						Console.Clear();
						string loader = ".....";
						foreach (char c in loader)
						{
							Console.Write(c);
							Thread.Sleep(250);
						}
						Console.Clear();
						await using var cmdDelete = db.CreateCommand(qDelete);
						cmdDelete.Parameters.AddWithValue("@bookingID", selectedID);

						await cmdDelete.ExecuteNonQueryAsync();

						Console.WriteLine($"Successfully deleted booking with ID {selectedID}");
						Console.ReadKey();
						return;
					}
				}
				Console.Clear();
				Console.WriteLine($"Booking with ID {selectedID} does not exist.");
				Console.ReadKey();
				return;
			}
			else
			{
				Console.WriteLine("Invalid ID, try again");
				Console.ReadKey();
			}
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

		const string qGetPrice = @"select price from rooms where id = @roomId";

		var cmd = db.CreateCommand(qGetPrice);
		cmd.Parameters.AddWithValue("roomId", roomId);

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

		int count = 1;

		while (await reader.ReadAsync())
		{
			string customerName = reader.GetString(reader.GetOrdinal("Customer"));
			int customerId = reader.GetInt32(reader.GetOrdinal("CustomerID"));
			int bookingId = reader.GetInt32(reader.GetOrdinal("BookingID"));
			int roomID = reader.GetInt32(reader.GetOrdinal("RoomID"));
			string startdateEndDate = reader.GetString(reader.GetOrdinal("StartdateEndDate"));


			Console.WriteLine($"BookingID: {bookingId} Name: {customerName}, Customer ID: {customerId}, Date: {startdateEndDate}, RoomID: {roomID}");
			count++;
		}
	}
	public async Task SelectEdit()
	{
		await List();
		Console.WriteLine("---------------------------------------------------------------------------------------------");
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

		using var cmd = connection.CreateCommand("SELECT CONCAT(c.first_name, ' ', c.last_name) AS CustomerName, * FROM BOOKINGS b JOIN Customers c ON b.customer_id = c.id WHERE b.id = @BookingID;");
		cmd.Parameters.AddWithValue("BookingID", bookingNumber);

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

	public async Task Confirm()
	{
		// skriv in bokning
		Console.WriteLine("Type \"CONFIRM\" to confirm booking");
		Console.ReadLine();
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
							return selectedRoom;
						}
						else if (input.ToLower() == "n")
						{
							return 0;
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
			}
		}
		return 0;
	}
}
