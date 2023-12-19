using Npgsql;

namespace HolidayMakerGroup6;
public class Room
{
	public async Task ViewAll()
	{
		string result = string.Empty;

		await using var db = NpgsqlDataSource.Create(Database.Url);

		const string? query = @"select rooms.id, rooms.size, locations.name, rooms.price from rooms
								join locations
								on rooms.location_id = locations.id;";

		var cmd = db.CreateCommand(query);

		var reader = await cmd.ExecuteReaderAsync();

		Console.WriteLine("Room ID    | Room Size       | Location       | Price");
		Console.WriteLine("***********|*****************|************************");

		while (await reader.ReadAsync())
		{
			// result += $"{reader.GetInt32(0)}";
			Console.WriteLine($"{reader.GetInt32(0),-10} | {reader.GetInt32(1),-15} | {reader.GetString(2),-15} | {reader.GetInt32(3),-15:C}");
		}
	}
}
