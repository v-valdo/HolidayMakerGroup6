
﻿using Npgsql;
using System.Runtime.InteropServices;
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
        await using var db = NpgsqlDataSource.Create(Database.Url);
        Extras extras = new(db);
        await Console.Out.WriteLineAsync(await extras.Add()); 
	}
		
    public void CalculatePrice()
	{
		
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




