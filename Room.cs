using Npgsql;

namespace HolidayMakerGroup6;
public class Room
{
	public async Task Criterias()
	{
		const string qCriterias = "select criteria_rooms.room_id, string_agg(search_criteria.name, ', ') from criteria_rooms JOIN search_criteria ON criteria_rooms.criteria_id = search_criteria.id GROUP BY criteria_rooms.room_id order by criteria_rooms.room_id";

		await using var db = NpgsqlDataSource.Create(Database.Url);
		var cmd = db.CreateCommand(qCriterias);
		var reader = await cmd.ExecuteReaderAsync();

		Console.WriteLine("-------------------------------------------------");
		Console.WriteLine("| ROOM ID  |              CRITERIA                ");
		Console.WriteLine("-------------------------------------------------");

		while (await reader.ReadAsync())
		{
			Console.WriteLine($"| {reader.GetInt32(0),-8} | {reader.GetString(1),-50}");
		}
	}
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
