using Npgsql;
namespace HolidayMakerGroup6;

public class SearchPage
{

	private readonly NpgsqlDataSource _db;

	public SearchPage(NpgsqlDataSource db)
	{
		_db = db;
	}

	public async Task<string> RoomsPriceASC()
	{
		string result = string.Empty;
		const string qRoomsPriceSort = @"select * from rooms order by price asc;";

		var reader = await _db.CreateCommand(qRoomsPriceSort).ExecuteReaderAsync();
		while (await reader.ReadAsync())
		{
			string nr = reader.GetInt32(0).ToString();
			string size = reader.GetInt32(1).ToString();
			string locationID = reader.GetInt32(2).ToString();
			string price = reader.GetDecimal(3).ToString();
			string reviews = reader.GetDecimal(4).ToString();

			result += " " + reader.GetInt32(0) + new string(' ', 12 - nr.Length);
			result += "||";
			result += " " + reader.GetInt32(1) + new string(' ', 12 - size.Length);
			result += "||";
			result += " " + reader.GetInt32(2) + new string(' ', 12 - locationID.Length);
			result += "||";
			result += " " + reader.GetDecimal(3) + " kr" + new string(' ', 12 - price.Length);
			result += "||";
			result += " " + reader.GetDecimal(4) + new string(' ', 12 - reviews.Length);
			result += "\n";
		}
		return result;
	}

	public async Task<string> RoomsReviewsDESC()
	{
		string result = string.Empty;
		const string qRoomsReviewsSort = @"select * from rooms order by reviews desc;";

		var reader = await _db.CreateCommand(qRoomsReviewsSort).ExecuteReaderAsync();
		while (await reader.ReadAsync())
		{
			string nr = reader.GetInt32(0).ToString();
			string size = reader.GetInt32(1).ToString();
			string locationID = reader.GetInt32(2).ToString();
			string price = reader.GetDecimal(3).ToString();
			string reviews = reader.GetDecimal(4).ToString();

			result += " " + reader.GetInt32(0) + new string(' ', 12 - nr.Length);
			result += "||";
			result += " " + reader.GetInt32(1) + new string(' ', 12 - size.Length);
			result += "||";
			result += " " + reader.GetInt32(2) + new string(' ', 12 - locationID.Length);
			result += "||";
			result += " " + reader.GetDecimal(3) + " kr" + new string(' ', 12 - price.Length);
			result += "||";
			result += " " + reader.GetDecimal(4) + new string(' ', 12 - reviews.Length);
			result += "\n";
		}
		return result;
	}

	public async Task<string> DistanceToBeach()
	{
		string result = string.Empty;
		string distanceHeader = string.Empty;
        string distance = string.Empty;

		bool validanswer = true;
		bool returnToMenu = false;
		do
		{
			Console.Clear();
			Console.WriteLine("|----- Distance to beach -----|\n" +
							  "|                             |\n" +
							  "| 1. 50 meters                |\n" +
							  "| 2. 50 - 100 meters          |\n" +
							  "| 3. 50 - 150 meters          |\n" +
							  "| 4. 50 - 200 meters          |\n" +
							  "| 5. 50 - 250 meters          |\n" +
							  "|                             |\n" +
							  "| 0. Return to bookings menu  |\n" +
							  "|-----------------------------|");
			Console.Write("\n\nTo choice menuoption, press key 0 - 5:  ");
			ConsoleKeyInfo keyPressed = Console.ReadKey();

			switch (keyPressed.Key)
			{
				default:
					Console.WriteLine("\nNo such option in menu");
					Console.ReadKey();
					validanswer = false; break;
				case ConsoleKey.D1:
					distance = "50";
					distanceHeader = "50 meters";
					break;
				case ConsoleKey.D2:
					distance = "100";
					distanceHeader = "50 - 100 meters";
					break;
				case ConsoleKey.D3:
					distance = "150";
					distanceHeader = "50 - 150 meters";
					break;
				case ConsoleKey.D4:
					distance = "200";
					distanceHeader = "50 - 200 meters";
					break;
				case ConsoleKey.D5:
					distance = "250";
					distanceHeader = "50 - 200 meters";
					break;
				case ConsoleKey.D0:
					Console.Clear();
					result += " ";
					validanswer = false;
					returnToMenu = true; break;
			}

			if (validanswer == true)
			{
                string qDistanceBeach = @$"
                    select r.id room_id, l.distance_to_beach, r.size room_size, l.id, r.price, r.reviews
                    from rooms r
                    join locations l on r.location_id = l.id
                    where l.distance_to_beach <= {distance}
                    order by l.distance_to_beach ASC;"
				;

                Console.Clear();
				await Console.Out.WriteLineAsync("Distance to beach: " + distanceHeader + "\n\n" +
												 "Room Number || Distance to Beach || Room Size   || Location ID || Room Price     || Reviews\n" +
												 "- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - ");
				using var reader = await _db.CreateCommand(qDistanceBeach).ExecuteReaderAsync();
				while (await reader.ReadAsync())
				{
					string nr = reader.GetInt32(0).ToString();
					string distanceBeach = reader.GetInt32(1).ToString();
					string size = reader.GetInt32(2).ToString();
					string locationID = reader.GetInt32(3).ToString();
					string price = reader.GetDecimal(4).ToString();
					string reviews = reader.GetDecimal(5).ToString();

					result += " " + reader.GetInt32(0) + new string(' ', 11 - nr.Length);
					result += "||";
					result += " " + reader.GetInt32(1) + new string(' ', 18 - distanceBeach.Length);
					result += "||";
					result += " " + reader.GetInt32(2) + new string(' ', 12 - size.Length);
					result += "||";
					result += " " + reader.GetInt32(3) + new string(' ', 12 - locationID.Length);
					result += "||";
					result += " " + reader.GetDecimal(4) + " kr"+ new string(' ', 12 - price.Length);
					result += "||";
					result += " " + reader.GetDecimal(5) + new string(' ', 12 - reviews.Length);
					result += "\n";
				}

				returnToMenu = true;
			}
		} while (!returnToMenu);

		return result;
	}

	public async Task<string> DistanceToCity()
	{
		string result = string.Empty;
		string distanceHeader = string.Empty;
		string distance = string.Empty;

		bool validanswer = true;
		bool returnToMenu = false;
		do
		{
			Console.Clear();
			Console.WriteLine("|----- Distance to city  -----|\n" +
							  "|                             |\n" +
							  "| 1. 50 meters                |\n" +
							  "| 2. 50 - 100 meters          |\n" +
							  "| 3. 50 - 150 meters          |\n" +
							  "| 4. 50 - 200 meters          |\n" +
							  "| 5. 50 - 250 meters          |\n" +
							  "|                             |\n" +
							  "| 0. Return to bookings menu  |\n" +
							  "|-----------------------------|");
			Console.Write("\n\nTo choice menuoption, press key 0 - 5:  ");
			ConsoleKeyInfo keyPressed = Console.ReadKey();

			switch (keyPressed.Key)
			{
				default:
					Console.WriteLine("\nNo such option in menu");
					Console.ReadKey();
					validanswer = false; break;
				case ConsoleKey.D1:
					distance = "50";
					distanceHeader = "50 meters";
					break;
				case ConsoleKey.D2:
					distance = "100";
					distanceHeader = "50 - 100 meters";
					break;
				case ConsoleKey.D3:
					distance = "150";
					distanceHeader = "50 - 150 meters";
					break;
				case ConsoleKey.D4:
					distance = "200";
					distanceHeader = "50 - 200 meters";
					break;
				case ConsoleKey.D5:
					distance = "250";
					distanceHeader = "50 - 200 meters";
					break;
				case ConsoleKey.D0:
					Console.Clear();
					result += " ";
					validanswer = false;
					returnToMenu = true; break;
			}

			if (validanswer == true)
			{
                string qDistanceCity = @$"
                    select r.id room_id, l.distance_to_city, r.size room_size, l.id, r.price, r.reviews
                    from rooms r
                    join locations l on r.location_id = l.id
                    where l.distance_to_city <= {distance}
                    order by l.distance_to_city ASC;"
				;

                Console.Clear();
				await Console.Out.WriteLineAsync("Distance to city: " + distanceHeader + "\n\n" +
												 "Room Number || Distance to City  || Room Size   || Location ID || Room Price     || Reviews\n" +
												 "- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - ");
				using var reader = await _db.CreateCommand(qDistanceCity).ExecuteReaderAsync();
				while (await reader.ReadAsync())
				{
					string nr = reader.GetInt32(0).ToString();
					string distanceCity = reader.GetInt32(1).ToString();
					string size = reader.GetInt32(2).ToString();
					string locationID = reader.GetInt32(3).ToString();
					string price = reader.GetDecimal(4).ToString();
					string reviews = reader.GetDecimal(5).ToString();

					result += " " + reader.GetInt32(0) + new string(' ', 11 - nr.Length);
					result += "||";
					result += " " + reader.GetInt32(1) + new string(' ', 18 - distanceCity.Length);
					result += "||";
					result += " " + reader.GetInt32(2) + new string(' ', 12 - size.Length);
					result += "||";
					result += " " + reader.GetInt32(3) + new string(' ', 12 - locationID.Length);
					result += "||";
					result += " " + reader.GetDecimal(4) + " kr" + new string(' ', 12 - price.Length);
					result += "||";
					result += " " + reader.GetDecimal(5) + new string(' ', 12 - reviews.Length);
					result += "\n";
				}

				returnToMenu = true;
			}
		} while (!returnToMenu);

		return result;
	}
}