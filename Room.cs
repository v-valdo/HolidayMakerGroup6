using Npgsql;

namespace HolidayMakerGroup6;
public class Room
{
	public double Size;
	public double Price;
	public Location? Location;
	public double Review;

	// exempel
	public async Task ViewAll()
	{
		await using var db = NpgsqlDataSource.Create(Database.Url);
		const string? query = @"select rooms.id, rooms.size, location.name, rooms.price from rooms
								join location
								on rooms.location_id = location.id;";

		var cmd = db.CreateCommand(query);

		var reader = await cmd.ExecuteReaderAsync();

		Console.WriteLine("Room ID    | Room Size       | Location       | Price");
		Console.WriteLine("***********|*****************|************************");

		while (await reader.ReadAsync())
		{
			Console.WriteLine($"{reader.GetInt32(0),-10} | {reader.GetInt32(1),-15} | {reader.GetString(2),-15} | {reader.GetInt32(3),-15:C}");
		}
	}
}
