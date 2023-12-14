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
            string price = reader.GetInt32(3).ToString();
            string reviews = reader.GetInt32(4).ToString();

            result += " " + reader.GetInt32(0) + new string(' ', 12 - nr.Length);
            result += "||";
            result += " " + reader.GetInt32(1) + new string(' ', 12 - size.Length);
            result += "||";
            result += " " + reader.GetInt32(2) + new string(' ', 12 - locationID.Length);
            result += "||";
            result += " " + reader.GetInt32(3) + new string(' ', 12 - price.Length);
            result += "||";
            result += " " + reader.GetInt32(4) + new string(' ', 12 - reviews.Length);
            
            result += "\n";
        }
        return result;
    }

    public async Task<string> RoomsReviewsDESC()
    {
        Console.Clear();
        string result = string.Empty;
        const string qRoomsReviewsSort = @"
            SELECT *
            FROM rooms
            ORDER BY reviews DESC;
        ";

        var reader = await _db.CreateCommand(qRoomsReviewsSort).ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            result += reader.GetInt32(0);
            result += " ||   ";
            result += reader.GetInt32(1);
            result += "  ||      ";
            result += reader.GetInt32(2);
            result += "      ||  ";
            result += reader.GetDecimal(3);
            result += "  ||   ";
            result += reader.GetDecimal(4);
            result += "\n";
        }
        return result;
    }

}
