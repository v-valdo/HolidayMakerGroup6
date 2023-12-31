using Npgsql;
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
        string heading = "REGISTER CUSTOMER";

        Console.WriteLine("╔══════════════════════════════════════╗");
        Console.WriteLine($"║           {heading}          ║");
        Console.WriteLine("╚══════════════════════════════════════╝");

        Console.Write("Firstname: ");
		firstName = Console.ReadLine() ?? "Not Specified";
		Console.Clear();

		if (firstName.Length < 2)
		{
			Console.WriteLine("Firstname need to be atleast 2 letters.");
			return;
		}

        Console.WriteLine("╔══════════════════════════════════════╗");
        Console.WriteLine($"║           {heading}          ║");
        Console.WriteLine("╚══════════════════════════════════════╝");

        Console.Write("Surname: ");
		surName = Console.ReadLine() ?? "Not Specified";
		Console.Clear();

		if (surName.Length < 2)
		{
			Console.WriteLine("Surname need to be atleast 2 letters.");
			return;
		}

        Console.WriteLine("╔══════════════════════════════════════╗");
        Console.WriteLine($"║           {heading}          ║");
        Console.WriteLine("╚══════════════════════════════════════╝");

        Console.Write("Email: ");
		email = Console.ReadLine() ?? "Not Specified";
		Console.Clear();

		if (email.Length < 10 || !email.Contains("@"))
		{
			Console.WriteLine("Email need to be atleast 10 letters and include @.");
			return;
		}

        Console.WriteLine("╔══════════════════════════════════════╗");
        Console.WriteLine($"║           {heading}          ║");
        Console.WriteLine("╚══════════════════════════════════════╝");

        Console.Write("Phone number: ");
		var phoneNumberInput = Console.ReadLine();

		if (phoneNumberInput?.Length != 10 || !int.TryParse(phoneNumberInput, out var parsedPhoneNumber))
		{
			Console.WriteLine("Invalid phone number. Please enter a 10-digit numeric phone number. Press Enter to continue...");
			Console.ReadKey();
			return;
		}

		phoneNumber = parsedPhoneNumber;
		Console.Clear();

        Console.WriteLine("╔══════════════════════════════════════╗");
        Console.WriteLine($"║           {heading}          ║");
        Console.WriteLine("╚══════════════════════════════════════╝");

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

			using var cmd = db.CreateCommand("INSERT INTO customers (first_name, last_name, email, telnumber, date_of_birth) VALUES ($1, $2, $3, $4, $5)");
			cmd.Parameters.AddWithValue(firstName);
			cmd.Parameters.AddWithValue(surName);
			cmd.Parameters.AddWithValue(email);
			cmd.Parameters.AddWithValue(phoneNumber);
			cmd.Parameters.AddWithValue(DateTime.Parse(DoB));

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

	public async Task<int> GetID(string email)
	{
		await using var db = NpgsqlDataSource.Create(Database.Url);

		using var cmd = db.CreateCommand("select id from customers where email = $1");
		cmd.Parameters.AddWithValue(email);

		var customerID = await cmd.ExecuteScalarAsync();
		return Convert.ToInt32(customerID);

	}

	public async Task ShowAll()
	{
		await using var db = NpgsqlDataSource.Create(Database.Url);

		const string query = "select * from customers;";
		var reader = await db.CreateCommand(query).ExecuteReaderAsync();

		Console.WriteLine("ID    | Firstname       | Surname         ");
		Console.WriteLine("******|*****************|*************");

		while (await reader.ReadAsync())
		{
			Console.WriteLine($"{reader.GetInt32(0),-5} | {reader.GetString(1),-15} | {reader.GetString(2),-15}");
		}
	}

	public async Task<bool> CustomerExists(string email, int phoneNumber)
	{
		await using var db = NpgsqlDataSource.Create(Database.Url);

		using var cmd = db.CreateCommand("SELECT COUNT(*) FROM customers WHERE email = $1 OR telnumber = $2");
		cmd.Parameters.AddWithValue(email);
		cmd.Parameters.AddWithValue(phoneNumber);

		var count = await cmd.ExecuteScalarAsync();
		return Convert.ToInt32(count) > 0;
	}
}