using Npgsql;
using System.Threading.Tasks;
namespace HolidayMakerGroup6;
public class Customer
{
    public int customerID;
    public string? firstName;
    public string? surName;
    public int phoneNumber { get; set; }
    public string? email;
    public string? DoB;
    public List<Customer> List;

    public async Task Register()
    {
        await using var db = NpgsqlDataSource.Create(Database.Url);
        Console.Clear();
        Console.Write("Firstname: ");
        firstName = Console.ReadLine() ?? "Not Specified";
        Console.Clear();

        Console.Write("Surname: ");
        surName = Console.ReadLine() ?? "Not Specified";
        Console.Clear();

        Console.Write("Email: ");
        email = Console.ReadLine() ?? "Not Specified";
        Console.Clear();

        Console.Write("Phone number: ");
        var phoneNumberInput = Console.ReadLine();

        if (phoneNumberInput.Length != 10 || !int.TryParse(phoneNumberInput, out var parsedPhoneNumber))
        {
            Console.WriteLine("Invalid phone number. Please enter a 10-digit numeric phone number. Press Enter to continue...");
            Console.ReadKey();
            return;
        }

        phoneNumber = parsedPhoneNumber;
        Console.Clear();

        Console.WriteLine("Date of Birth (Like this ->[xxxx-xx-xx]");
        DoB = Console.ReadLine() ?? "DoB";
        Console.Clear();

        await Add();


    }
    public async Task Add()
    {
        await using var db = NpgsqlDataSource.Create(Database.Url);

        try
        {
            if (await CustomerExists(email, phoneNumber))
            {
                Console.WriteLine("A customer with the same email or phone number already exists. Press Enter to continue...");
                Console.ReadKey();
                return;
            }

            using var cmd = db.CreateCommand("INSERT INTO customers (first_name, last_name, email, telnumber, date_of_birth) VALUES (@first_name, @last_name, @email, @telnumber, @date_of_birth)");
            cmd.Parameters.AddWithValue("@first_name", firstName);
            cmd.Parameters.AddWithValue("@last_name", surName);
            cmd.Parameters.AddWithValue("@email", email);
            cmd.Parameters.AddWithValue("@telnumber", phoneNumber);
            cmd.Parameters.AddWithValue("@date_of_birth", DateTime.Parse(DoB));
            await cmd.ExecuteNonQueryAsync();

            customerID = await GetID(email);

            Console.WriteLine($"Customer {firstName + " " + surName} with CustomerID {customerID} has successfully been added to the database!");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error has occurred while adding the customer: {ex.Message}");
            Console.ReadKey();
        }
    }

    // Hämtar customer ID where email = email
    public async Task<int> GetID(string email)
    {
        await using var db = NpgsqlDataSource.Create(Database.Url);

        using var cmd = db.CreateCommand("select id from customers where email = @email");
        cmd.Parameters.AddWithValue("@email", email);

        var customerID = await cmd.ExecuteScalarAsync();
        return Convert.ToInt32(customerID);

    }

    // fungerande lista från databasen
    public async Task ShowAll()
    {
        await using var db = NpgsqlDataSource.Create(Database.Url);

        const string query = "select first_name, last_name from customers;";
        var reader = await db.CreateCommand(query).ExecuteReaderAsync();

        List<string> allcustomers = new();

        while (await reader.ReadAsync())
        {
            allcustomers.Add($"{reader.GetString(0)} {reader.GetString(1)}");
        }

        int indexer = 1;
        foreach (var item in allcustomers)
        {
            Console.WriteLine($"{indexer}. {item}");
            indexer++;
        }

    }
    public async Task<bool> CustomerExists(string email, int phoneNumber)
    {
        await using var db = NpgsqlDataSource.Create(Database.Url);

        using var cmd = db.CreateCommand("SELECT COUNT(*) FROM customers WHERE email = @email OR telnumber = @telnumber");
        cmd.Parameters.AddWithValue("@email", email);
        cmd.Parameters.AddWithValue("@telnumber", phoneNumber);

        var count = await cmd.ExecuteScalarAsync();
        return Convert.ToInt32(count) > 0;
    }
}
