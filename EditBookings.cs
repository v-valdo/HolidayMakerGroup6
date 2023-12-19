using Npgsql;

namespace HolidayMakerGroup6;

public class EditBookings
{
	public async Task ChangeStartDate(int bookingId)
	{
		Console.Write("Enter the new start date (yyyy-mm-dd): ");
		if (DateTime.TryParse(Console.ReadLine(), out DateTime newStartDate))
		{
			await using var connection = NpgsqlDataSource.Create(Database.Url);
			await connection.OpenConnectionAsync();

			using var cmd = connection.CreateCommand("UPDATE BOOKINGS SET start_date = $1 WHERE id = $2;");
			cmd.Parameters.AddWithValue(newStartDate);
			cmd.Parameters.AddWithValue(bookingId);

			int rowsAffected = await cmd.ExecuteNonQueryAsync();

			if (rowsAffected > 0)
			{
				Console.WriteLine($"Start date updated successfully for Booking ID {bookingId}.");
			}
			else
			{
				Console.WriteLine($"Failed to update start date for Booking ID {bookingId}.");
			}
		}
		else
		{
			Console.WriteLine("Invalid date format. Please enter the date in the format yyyy-mm-dd.");
		}
	}
	public async Task ChangeEndDate(int bookingId)
	{
		Console.Write("Enter the new end date (yyyy-mm-dd): ");
		if (DateTime.TryParse(Console.ReadLine(), out DateTime newEndDate))
		{
			await using var connection = NpgsqlDataSource.Create(Database.Url);
			await connection.OpenConnectionAsync();

			using var cmd = connection.CreateCommand("UPDATE BOOKINGS SET end_date = $1 WHERE id = $2;");
			cmd.Parameters.AddWithValue(newEndDate);
			cmd.Parameters.AddWithValue(bookingId);

			int rowsAffected = await cmd.ExecuteNonQueryAsync();

			if (rowsAffected > 0)
			{
				Console.WriteLine($"End date updated successfully for Booking ID {bookingId}.");
			}
			else
			{
				Console.WriteLine($"Failed to update end date for Booking ID {bookingId}.");
			}
		}
		else
		{
			Console.WriteLine("Invalid date format. Please enter the date in the format yyyy-mm-dd.");
		}
	}
	public async Task ChangeRoomID(int bookingId)
	{
		Console.Write("Enter the new Room ID: ");
		if (int.TryParse(Console.ReadLine(), out int newRoomId))
		{
			await using var connection = NpgsqlDataSource.Create(Database.Url);
			await connection.OpenConnectionAsync();

			using var cmd = connection.CreateCommand("UPDATE BOOKINGS SET room_id = $1 WHERE id = $2;");
			cmd.Parameters.AddWithValue(newRoomId);
			cmd.Parameters.AddWithValue(bookingId);

			int rowsAffected = await cmd.ExecuteNonQueryAsync();

			if (rowsAffected > 0)
			{
				Console.WriteLine($"Room ID updated successfully for Booking ID {bookingId}.");
			}
			else
			{
				Console.WriteLine($"Failed to update Room ID for Booking ID {bookingId}.");
			}
		}
		else
		{
			Console.WriteLine("Invalid input. Please enter a valid Room ID.");
		}
	}
	public async Task ChangeNoP(int bookingId)
	{
		Console.Write("Enter the new Number of People: ");
		if (int.TryParse(Console.ReadLine(), out int newNumberOfPeople))
		{
			await using var connection = NpgsqlDataSource.Create(Database.Url);
			await connection.OpenConnectionAsync();

			using var cmd = connection.CreateCommand("UPDATE BOOKINGS SET number_of_people = $1 WHERE id = $2;");
			cmd.Parameters.AddWithValue(newNumberOfPeople);
			cmd.Parameters.AddWithValue(bookingId);

			int rowsAffected = await cmd.ExecuteNonQueryAsync();

			if (rowsAffected > 0)
			{
				Console.WriteLine($"Number of People updated successfully for Booking ID {bookingId}.");
			}
			else
			{
				Console.WriteLine($"Failed to update Number of People for Booking ID {bookingId}.");
			}
		}
		else
		{
			Console.WriteLine("Invalid input. Please enter a numeric value.");
		}
	}
	public async Task ChangePrice(int bookingId)
	{
		Console.Write("Enter the new Price: ");
		if (int.TryParse(Console.ReadLine(), out int newPrice))
		{
			await using var connection = NpgsqlDataSource.Create(Database.Url);
			await connection.OpenConnectionAsync();

			using var cmd = connection.CreateCommand("UPDATE BOOKINGS SET price = $1 WHERE id = $2;");
			cmd.Parameters.AddWithValue(newPrice);
			cmd.Parameters.AddWithValue(bookingId);

			int rowsAffected = await cmd.ExecuteNonQueryAsync();

			if (rowsAffected > 0)
			{
				Console.WriteLine($"Price updated successfully for Booking ID {bookingId}.");
			}
			else
			{
				Console.WriteLine($"Failed to update Price for Booking ID {bookingId}.");
			}
		}
		else
		{
			Console.WriteLine("Invalid input. Please enter a valid Price.");
		}
	}
}
