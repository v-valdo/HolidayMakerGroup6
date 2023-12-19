using Npgsql;

namespace HolidayMakerGroup6;

public class TablesPopulated
{
    public static async Task Populate()
    {
        await using var db = NpgsqlDataSource.Create(Database.Url);

        const string qInsertLocations = @"
            INSERT INTO locations(name, distance_to_beach, distance_to_city) 
            VALUES  ('tangalooma', 50, 250),
            ('greenside', 100, 200),
            ('rivergate', 150, 150),
            ('azurebliss', 200, 100), 
            ('crystalcove', 250, 50);
        ";

        const string qInsertRooms = @"
            INSERT INTO rooms (size, location_id, price, reviews)
            VALUES (2,5,1550.99,4.4),
            (1,4,1405.99,3.6),
            (2,5,1395.99,3.9),
            (1,5,1564.99,4.6),
            (1,1,1997.99,4.1),
            (3,4,1676.99,3.2),
            (1,1,1134.99,4.7),
            (1,3,1657.99,4.9),
            (1,5,1295.99,4.8),
            (3,5,856.99,4.4),
            (1,4,1170.99,4.8),
            (1,3,1038.99,4.1),
            (2,5,1922.99,4.6),
            (3,1,1342.99,4.5),
            (1,4,1824.99,4.5),
            (3,5,153.99,2.3),
            (1,2,1531.99,5.0),
            (2,3,1970.99,2.5),
            (1,1,703.99,2.7),
            (3,4,1217.99,3.4),
            (2,5,1981.99,4.4),
            (3,5,1149.99,3.6),
            (2,3,726.99,4.0),
            (2,1,809.99,2.7),
            (1,1,877.99,4.0),
            (2,1,1762.99,4.7),
            (1,4,898.99,2.5),
            (1,3,1639.99,2.6),
            (1,5,890.99,2.5),
            (3,5,1350.99,2.7);
        ";

        const string qInsertExtraService = @"
            INSERT INTO extra_service (name, price)
            VALUES ('extrabed', 750.99),
            ('halfboard', 250.50),
            ('fullboard', 500.99),
            ('breakfast', 200.0),
            ('exclusive spa', 2000.0),
            ('gym access', 750.0),
            ('private jet', 12000.0);
        ";

        const string qInsertSearchCriteria = @"
            INSERT INTO search_criteria (name)
            VALUES 
            ('pool'),
            ('restaurant'),
            ('evening entertainment'),
            ('childrens club');
        ";

        const string qInsertCriteraRooms= @"
            INSERT INTO criteria_rooms (criteria_id,room_id)
            VALUES (3,16), (4,13), (1,25),
            (3,2), (3,8), (2,13), (3,11),
            (2,11), (2,19), (4,7),
            (1,2), (1,3), (3,21),
            (1,19), (1,28), (4,25), (4,29),
            (1,14), (4,6), (1,1),
            (2,21), (3,7), (1,23),
            (3,12), (4,19), (2,29),
            (2,9), (4,24), (4,26), (4,28),
            (4,23), (3,4), (4,15), (2,6),
            (3,3), (3,24), (3,6);
        ";

        await db.CreateCommand(qInsertLocations).ExecuteNonQueryAsync();
        await db.CreateCommand(qInsertRooms).ExecuteNonQueryAsync();
        await db.CreateCommand(qInsertExtraService).ExecuteNonQueryAsync();
        await db.CreateCommand(qInsertSearchCriteria).ExecuteNonQueryAsync();
        await db.CreateCommand(qInsertCriteraRooms).ExecuteNonQueryAsync();
    }
}