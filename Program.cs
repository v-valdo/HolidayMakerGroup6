using HolidayMakerGroup6;
using Npgsql;

await using var db = NpgsqlDataSource.Create(Database.Url);

Extras x = new();
x.ShowAll();

Console.ReadKey();

// main menu
do
{
	Console.Clear();
	Console.WriteLine("---- HolidayMaker Group 6 ----" +
						"\n1. Bookings\n" +
						"2. Customers\n" +
						"3. Exit\n");
	Console.Write("Enter choice:  ");
	int.TryParse(Console.ReadLine(), out int answer);

	switch (answer)
	{
		default:
			Console.WriteLine("No such option");
			Console.ReadKey();
			break;
		case 2:
			Console.Clear();
			Customer customer = new();
			await customer.Register();
			break;
		case 1:
			Console.Clear();
			Booking booking = new();
			await booking.Menu();
			break;
		case 3:
			Console.Clear();
			Console.WriteLine("Good bye!\n Press any key to continue!");
			Console.ReadKey();
			Environment.Exit(0);
			break;
	}
} while (true);