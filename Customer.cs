using Npgsql;
namespace HolidayMakerGroup6;
public class Customer
{
    public int customerID;
    public string? firstName;
    public string? surName;
    public int phoneNumber;
    public string? email;
    public string? DoB;
    public List<Customer> List;

    public async Task Register()
    {
        await using var db = NpgsqlDataSource.Create(Database.Url);

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
        phoneNumber = int.Parse(Console.ReadLine());
        Console.Clear();

        Console.WriteLine("Date of Birth (Like this ->[xxxx-xx-xx]");
        DoB = Console.ReadLine() ?? "DoB";
        Console.Clear();

        await Add();
        // Update-metod som hämtar alla customers och lägger i List<Customer>

        customerID = await GetID(email);

        Console.WriteLine($"Customer with ID {customerID} has successfully been added to the database!");
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }
    public async Task Add()
    {
        await using var db = NpgsqlDataSource.Create(Database.Url);

        using var cmd = db.CreateCommand("INSERT INTO customers (firstname, surname, email, telnumber, date_of_birth) VALUES (@firstname, @surname, @email, @telnumber, @date_of_birth)");
        cmd.Parameters.AddWithValue("@firstname", firstName);
        cmd.Parameters.AddWithValue("@surname", surName);
        cmd.Parameters.AddWithValue("@email", email);
        cmd.Parameters.AddWithValue("@telnumber", phoneNumber);
        cmd.Parameters.AddWithValue("@date_of_birth", DateTime.Parse(DoB));
        cmd.ExecuteNonQuery();
    }

    // Hämtar customer ID where email = email
    public async Task<int> GetID(string email)
    {
        await using var db = NpgsqlDataSource.Create(Database.Url);

        using var cmd = db.CreateCommand("select customer_id from customers where email = @email");
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
}