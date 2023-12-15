using HolidayMakerGroup6;
using Npgsql;

await using var db = NpgsqlDataSource.Create(Database.Url);

// Creates all tables for the database
TableCreation tables = new();
await tables.Create();


// main menu
bool endprogram = false;

do
{
	Console.Clear();
	Console.WriteLine("---- HolidayMaker Group 6 ----\n" +
						"\n1. Register new customer\n" +
						"2. Bookings\n\n" +
						"0. Exit");
	Console.Write("\nTo choice menuoption, press key 0 - 2:  ");
	ConsoleKeyInfo keyPressed = Console.ReadKey();

	switch (keyPressed.Key)
	{
		default:
			Console.WriteLine("\nNo such option");
			Console.ReadKey();
			break;
		case ConsoleKey.D1:
			Customer customers = new Customer();
			await customers.Register();
			Console.ReadKey();
			break;
		case ConsoleKey.D2:
			await bookingmenu();
			break;
		case ConsoleKey.D0:
			Console.Clear();
			Console.WriteLine("Good bye!\nPress any key to continue!");
			Console.ReadKey();
			Environment.Exit(0);
			break;
	}
} while (!endprogram);

async Task bookingmenu()
{
    Extras extras = new(db);
    bool returntomainmenu = false;
	do
	{
		Booking booking = new();

		Console.Clear();
		// choice "view bookings"
		Console.WriteLine("---- Bookings ----\n\n" +
					  "1. Create new booking\n" +
					  "2. Edit booking\n" +
					  "3. Delete booking\n" +
					  "4. View bookings\n" +
					  "5. Rooms\n" +
                      "6. Add extras to booking\n" +
                      "7. View extras\n\n" +
                      "0. Return to main menu");
		Console.Write("\nTo choice menuoption, press key 0 - 5: ");
		ConsoleKeyInfo keyPressed = Console.ReadKey();

		switch (keyPressed.Key)
		{
			default:
				Console.WriteLine("\nNo such option");
				Console.ReadKey();
				break;
			case ConsoleKey.D1:
				Console.Clear();
				await booking.New();
				Console.ReadKey();
				break;
			case ConsoleKey.D2:
				await booking.Edit();
				Console.ReadKey();
				break;
			case ConsoleKey.D3:
				await booking.List();
				Console.ReadKey();
				break;
			case ConsoleKey.D4:
				await booking.Delete();
				Console.ReadKey();
				break;
			case ConsoleKey.D5:
				await searchpagemenu();
				break;
			case ConsoleKey.D6:
				Console.Clear();
				Console.WriteLine(await extras.Add());
				Console.ReadKey();
				break;
            case ConsoleKey.D7:
                Console.Clear();
                Console.WriteLine(await extras.ShowAll());
                Console.ReadKey();
                break;
			case ConsoleKey.D0:
				Console.Clear();
				Console.WriteLine("You have chosen to return to main menu.\nPress any key to continue!");
				Console.ReadKey();
				returntomainmenu = true; return;
		}
	} while (!returntomainmenu);
}

async Task searchpagemenu()
{
	SearchPage sort = new(db);
	bool returntomainmenu = false;
	do
	{
		Console.Clear();
		Console.WriteLine("|----------- Rooms ------------|\n" +
						  "| 1. Sort by price ASC         |\n" +
						  "| 2. Sort by reviews DESC      |\n" +
						  "| 3. Sort by distance to city  |\n" +
						  "| 4. Sort by distance to beach |\n" +
						  "|                              |\n" +
						  "| 0. Return to bookings menu   |\n" +
						  "|------------------------------|\n");
		Console.Write("\nTo choice menuoption, press key 0 - 4:  ");
		ConsoleKeyInfo keyPressed = Console.ReadKey();

		switch (keyPressed.Key)
		{
			default:
				Console.WriteLine("\nNo such option in menu");
				Console.ReadKey();
				break;
			case ConsoleKey.D1:
				Console.Clear();
				Console.WriteLine("Rooms sorted by: Price in ascending order\n" +
								  " Room Number || Room Size   || Location ID || Room Price  || Reviews\n" +
								  "- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -\n" +
								  await sort.RoomsPriceASC() +
								  "- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -\n" +
								  "\nPress any key to return to booking menu!");
				Console.ReadKey();
				break;
			case ConsoleKey.D2:
				Console.Clear();
				Console.WriteLine("Rooms sorted by: Reviews in descending order\n\n" +
								 " Room Number || Room Size   || Location ID || Room Price  || Reviews\n" +
								 "- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -\n" +
								 await sort.RoomsReviewsDESC() +
								 "- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -\n" +
								 "\nPress any key to return to booking menu!");
				Console.ReadKey();
				break;
			case ConsoleKey.D3:
				Console.Clear();
				Console.WriteLine(await sort.DistanceToCity() +
								 "- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - \n" +
								 "\nPress any key to return to booking menu!");
				Console.ReadKey();
				break;
			case ConsoleKey.D4:
				Console.Clear();
				Console.WriteLine(await sort.DistanceToBeach() +
								 "- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - \n" +
								 "\nPress any key to return to booking menu!");
				Console.ReadKey();
				break;
			case ConsoleKey.D0:
				Console.Clear();
				Console.WriteLine("\nReturing to booking menu...");
				await Task.Delay(2500);
				returntomainmenu = true; return;
		}
	} while (!returntomainmenu);
}
