using Npgsql;
using System.Diagnostics.Metrics;
namespace HolidayMakerGroup6;

public class SearchPage
{

    private readonly NpgsqlDataSource _db;

    public SearchPage(NpgsqlDataSource db)
    {
        _db = db;
    }

    // Room menu option 1
    public async Task<string> RoomsPriceASC()
    {
        string result = string.Empty;
        const string qRoomsPriceSort = @"
            SELECT *
            FROM rooms
            ORDER BY price ASC;
        ";

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
            result += " " + reader.GetDecimal(3) + new string(' ', 12 - price.Length);
            result += "||";
            result += " " + reader.GetDecimal(4) + new string(' ', 12 - reviews.Length);
            result += "\n";
        }
        return result;
    }

    // Room menu option 2
    public async Task<string> RoomsReviewsDESC()
    {
        string result = string.Empty;
        const string qRoomsReviewsSort = @"
            SELECT *
            FROM rooms
            ORDER BY reviews DESC;
        ";

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
            result += " " + reader.GetDecimal(3) + new string(' ', 12 - price.Length);
            result += "||";
            result += " " + reader.GetDecimal(4) + new string(' ', 12 - reviews.Length);
            result += "\n";
        }
        return result;
    }

    // Room menu option 4
    public async Task<string> DistanceToBeach()
    {
        string qDistanceBeach = string.Empty;
        string result = string.Empty;
        string distanceHeader = string.Empty;

        const string qfiftyMToBeach = @"
                    select r.id room_id, l.distance_to_beach, r.size room_size, l.id, r.price, r.reviews
                    from rooms r
                    join locations l on r.location_id = l.id
                    where l.distance_to_beach <= 50
                    order by l.distance_to_beach ASC;";
        const string qhundredMToBeach = @"
                    select r.id room_id, l.distance_to_beach, r.size room_size, l.id, r.price, r.reviews
                    from rooms r
                    join locations l on r.location_id = l.id
                    where l.distance_to_beach <= 100
                    order by l.distance_to_beach ASC;";
        const string qhundredfiftyMToBeach = @"
                    select r.id room_id, l.distance_to_beach, r.size room_size, l.id, r.price, r.reviews
                    from rooms r
                    join locations l on r.location_id = l.id
                    where l.distance_to_beach <= 150
                    order by l.distance_to_beach ASC;";
        const string qtwohundredMToBeach = @"
                    select r.id room_id, l.distance_to_beach, r.size room_size, l.id, r.price, r.reviews
                    from rooms r
                    join locations l on r.location_id = l.id
                    where l.distance_to_beach <= 200
                    order by l.distance_to_beach ASC;";
        const string qtwohundredfiftyMToBeach = @"
                    select r.id room_id, l.distance_to_beach, r.size room_size, l.id, r.price, r.reviews
                    from rooms r
                    join locations l on r.location_id = l.id
                    where l.distance_to_beach <= 200
                    order by l.distance_to_beach ASC;";

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
                    qDistanceBeach = qfiftyMToBeach;
                    distanceHeader = "50 meters";
                    break;
                case ConsoleKey.D2:
                    qDistanceBeach = qhundredMToBeach;
                    distanceHeader = "50 - 100 meters";
                    break;
                case ConsoleKey.D3:
                    qDistanceBeach = qhundredfiftyMToBeach;
                    distanceHeader = "50 - 150 meters";
                    break;
                case ConsoleKey.D4:
                    qDistanceBeach = qtwohundredMToBeach;
                    distanceHeader = "50 - 200 meters";
                    break;
                case ConsoleKey.D5:
                    qDistanceBeach = qtwohundredfiftyMToBeach;
                    distanceHeader = "50 - 200 meters";
                    break;
                case ConsoleKey.D0:
                    Console.Clear();
                    result += " ";
                    validanswer = false;
                    returnToMenu = true; break;
            }

            // Only prints out rooms if they pick an option
            if (validanswer == true)
            {
                Console.Clear();
                await Console.Out.WriteLineAsync("Distance to beach: " + distanceHeader + "\n\n" +
                                                 "Room Number || Distance to Beach || Room Size   || Location ID || Room Price  || Reviews\n" +
                                                 "- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - ");
                using (var reader = await _db.CreateCommand(qDistanceBeach).ExecuteReaderAsync())
                {
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
                        result += " " + reader.GetDecimal(4) + new string(' ', 12 - price.Length);
                        result += "||";
                        result += " " + reader.GetDecimal(5) + new string(' ', 12 - reviews.Length);
                        result += "\n";
                    }

                    returnToMenu = true;
                }
            }
        } while (!returnToMenu);

        return result;
    }
}