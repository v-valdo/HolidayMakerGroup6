using HolidayMakerGroup6;
using Npgsql;
using System.Runtime.InteropServices;

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
    Console.Write("Enter choice:  ");
    ConsoleKeyInfo keyPressed = Console.ReadKey();

    switch (keyPressed.Key)
    {
        default:
            Console.WriteLine("No such option");
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
        case ConsoleKey.Enter:
            Console.WriteLine("No such option");
            Console.ReadKey();
            break;
        case ConsoleKey.D0:
            Console.Clear();
            Console.WriteLine("Good bye!\n Press any key to continue!");
            Console.ReadKey();
            Environment.Exit(0);
            break;
    }
} while (!endprogram);

async Task bookingmenu()
{
    bool returntomainmenu = false;
    do
    {
        Console.Clear();
        // choice "view bookings"
        Console.WriteLine("---- Bookings ----\n\n" +
                      "1. Create new booking\n" +
                      "2. Edit booking\n" +
                      "3. Delete booking\n" +
                      "4. View bookings\n" +
                      "5. Rooms\n\n" +
                      "0. Return to main menu");
        Console.Write("Enter choice:  ");
        ConsoleKeyInfo keyPressed = Console.ReadKey();

        switch (keyPressed.Key)
        {
            default:
                Console.WriteLine("No such option");
                Console.ReadKey();
                break;
            case ConsoleKey.D1:
                Console.Clear();
                Console.WriteLine("1. create new booking");
                Console.ReadKey();
                break;
            case ConsoleKey.D2:
                Console.WriteLine("edit booking");
                Console.ReadKey();
                break;
            case ConsoleKey.D3:
                Console.WriteLine("view booking");
                Console.ReadKey();
                break;
            case ConsoleKey.D4:
                Console.WriteLine("delete booking");
                Console.ReadKey();
                break;
            case ConsoleKey.D5:
                await searchpagemenu();
                break;
            case ConsoleKey.Enter:
                Console.WriteLine("No such option");
                Console.ReadKey();
                break;
            case ConsoleKey.D0:
                Console.Clear();
                Console.WriteLine("You have chosen to return to main menu.\n Press any key to continue!");
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
        Console.WriteLine("---- Rooms ----\n\n" +
                          "1. Sort by price ASC\n" +
                          "2. Sort by reviews DESC\n" +
                          "3. Sort by distance to city\n" +
                          "4. Sort by distance to beach\n\n" +
                          "0. Return to bookings menu");
        Console.Write("Enter choice:  ");
        ConsoleKeyInfo keyPressed = Console.ReadKey();

        switch (keyPressed.Key)
        {
            default:
                Console.WriteLine("No such option");
                Console.ReadKey();
                break;
            case ConsoleKey.D1:
                Console.Clear();
                Console.WriteLine(await sort.RoomsPriceASC());
                Console.ReadKey();
                break;
            case ConsoleKey.D2:
                Console.Clear();
                Console.WriteLine("Rooms sorted by reviews in desc ordning\nNr || Size || Location ID || Price || Reviews\n" + await sort.RoomsReviewsDESC()); 
                Console.ReadKey();
                break;
            case ConsoleKey.D3:
                Console.Clear();
                Console.WriteLine("city");
                Console.ReadKey();
                break;
            case ConsoleKey.D4:
                Console.Clear();
                Console.WriteLine("beach");
                Console.ReadKey();
                break;
            case ConsoleKey.Enter:
                Console.WriteLine("No such option");
                Console.ReadKey();
                break;
            case ConsoleKey.D0:
                Console.Clear();
                Console.WriteLine("You have chosen to return to main menu.\n Press any key to continue!");
                Console.ReadKey();
                returntomainmenu = true; return;
        }
    } while (!returntomainmenu);
}