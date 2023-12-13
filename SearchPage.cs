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
        while (await reader.ReadAsync()) // så länge det finns något att läsa
        {
            result += reader.GetInt32(0);
            result += " || ";
            result += reader.GetInt32(1);
            result += " || ";
            result += reader.GetInt32(2);
            result += " || ";
            result += reader.GetDecimal(3);
            result += " || ";
            result += reader.GetDecimal(4);
            result += "\n";
        }
        return result;
    }

    public async Task RoomsReviewsDESC()
    {
        await using var db = NpgsqlDataSource.Create(Database.Url);
        const string qRoomsReviewsSort = @"
            SELECT *
            FROM rooms
            ORDER BY reviews DESC;
        ";

        await db.CreateCommand(qRoomsReviewsSort).ExecuteReaderAsync();
    }

}
