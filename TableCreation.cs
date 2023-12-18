using Npgsql;
namespace HolidayMakerGroup6;

public class TableCreation
{
    public async Task Create()
    {
        await using var db = NpgsqlDataSource.Create(Database.Url);

        const string qCustomers = @"  
            CREATE TABLE IF NOT EXISTS customers(
                id SERIAL NOT NULL PRIMARY KEY,
                first_name TEXT NOT NULL,
                last_name TEXT NOT NULL,
                email VARCHAR(255) NOT NULL,
                telnumber INTEGER NOT NULL,
                date_of_birth DATE NOT NULL
            );
        ";

        const string qLocations = @"
            CREATE TABLE IF NOT EXISTS locations (
                id SERIAL NOT NULL PRIMARY KEY,
                name TEXT NOT NULL,
                distance_to_beach INTEGER NOT NULL,
                distance_to_city INTEGER NOT NULL
            );
        ";

        const string qRooms = @"
            CREATE TABLE IF NOT EXISTS rooms(
                id SERIAL NOT NULL PRIMARY KEY,
                size INTEGER NOT NULL,
                location_id INTEGER REFERENCES locations (id),
                price DECIMAL(8, 2) NOT NULL,
                reviews DECIMAL(2, 1) NULL
            );
        ";

        const string qSearchCriteria = @"
            CREATE TABLE IF NOT EXISTS search_criteria(
                id SERIAL NOT NULL PRIMARY KEY,
                name TEXT NOT NULL
            );
        ";

        const string qCriteriaRooms = @"
            CREATE TABLE IF NOT EXISTS criteria_rooms(
                id SERIAL NOT NULL PRIMARY KEY,
                criteria_id INTEGER NOT NULL REFERENCES search_criteria (id),
                room_id INTEGER NOT NULL REFERENCES rooms (id)
            );
        ";

        const string qBookings = @"
            CREATE TABLE IF NOT EXISTS bookings(
                id SERIAL NOT NULL PRIMARY KEY,
                customer_id INTEGER NOT NULL REFERENCES customers (id),
                start_date DATE NOT NULL,
                end_date DATE NOT NULL,
                room_id INTEGER NOT NULL REFERENCES rooms (id),
                number_of_people INTEGER NOT NULL,
                price DECIMAL (8,2)
            );        
        ";

        const string qExtraService = @"
            CREATE TABLE IF NOT EXISTS extra_service(
                id SERIAL NOT NULL PRIMARY KEY,
                name TEXT NOT NULL,
                price DECIMAL(8, 2) NOT NULL
            );
        ";
        const string qExtraServiceBookings = @"
            CREATE TABLE IF NOT EXISTS extra_service_and_bookings(
                id SERIAL NOT NULL PRIMARY KEY,
                booking_id INTEGER NOT NULL REFERENCES bookings (id),
                extra_service_id INTEGER NOT NULL REFERENCES extra_service (id),
                price DECIMAL (8,2),
                unique (booking_id, extra_service_id)
            );
        ";

        const string qAlterSequences = @"
            ALTER SEQUENCE 
                customers_id_seq RESTART WITH 1;
            ALTER SEQUENCE 
                locations_id_seq RESTART WITH 1;
            ALTER SEQUENCE 
                rooms_id_seq RESTART WITH 1;
            ALTER SEQUENCE 
                criteria_rooms_id_seq RESTART WITH 1;
            ALTER SEQUENCE 
                search_criteria_id_seq RESTART WITH 1;
            ALTER SEQUENCE 
                bookings_id_seq RESTART WITH 1;
            ALTER SEQUENCE 
                extra_service_id_seq RESTART WITH 1;
            ALTER SEQUENCE 
                extra_service_and_bookings_id_seq RESTART WITH 1;        
        ";

        // Creates all the tables
        await db.CreateCommand(qCustomers).ExecuteNonQueryAsync();
        await db.CreateCommand(qLocations).ExecuteNonQueryAsync();
        await db.CreateCommand(qRooms).ExecuteNonQueryAsync();
        await db.CreateCommand(qSearchCriteria).ExecuteNonQueryAsync();
        await db.CreateCommand(qCriteriaRooms).ExecuteNonQueryAsync();
        await db.CreateCommand(qBookings).ExecuteNonQueryAsync();
        await db.CreateCommand(qExtraService).ExecuteNonQueryAsync();
        await db.CreateCommand(qExtraServiceBookings).ExecuteNonQueryAsync();

        // Needs to be inactive after one startup of program
        //await db.CreateCommand(qAlterSequences).ExecuteNonQueryAsync();
    }
}