using Npgsql;

namespace HolidayMakerGroup6;
public class Extras
{
	public async Task ShowAll()
	{
		await using var db = NpgsqlDataSource.Create(Database.Url);
		const string query = @"select name, price from extra_service;";
		var reader = await db.CreateCommand(query).ExecuteReaderAsync();

		Console.WriteLine("Service              | Price               ");
		Console.WriteLine("*********************|*********************");

		while (await reader.ReadAsync())
		{
			Console.WriteLine($"{reader.GetString(0),-20} | {reader.GetInt32(1),10:C}");
		}
	}
}
