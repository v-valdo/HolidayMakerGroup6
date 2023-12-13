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
		const string? query = @"select rooms.id, rooms.size, locations.name from rooms
join;";

		var cmd = db.CreateCommand(query);

		var reader = await cmd.ExecuteReaderAsync();

		while (await reader.ReadAsync())
		{
			Console.WriteLine($"{reader.GetInt32(0)} + {reader.GetInt32(1)}");
		}
	}
}
