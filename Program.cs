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
                        "3. Exit");
    Console.Write("Enter choice:  ");
    int.TryParse(Console.ReadLine(), out int answer);

    switch (answer)
    {
        default:
            Console.WriteLine("No such option");
            Console.ReadKey();
            break;
        case 1:
            Console.WriteLine("1. Register New Customer");
            Console.ReadKey();

            //Customer customer = new Customer();
            //await customer.Register();
            break;
        case 2:
            bookingmenu();
            break;
        case 3:
            Console.Clear();
            Console.WriteLine("Good bye!\n Press any key to continue!");
            Console.ReadKey();
            Environment.Exit(0);
            break;
    }
} while (!endprogram);

void bookingmenu()
{
    bool returntomainmenu = false;
    do
    {
        Console.Clear();
        // choice "view bookings"
        Console.WriteLine("---- Bookings ----\n" +
                      "6. Rooms\n" +
                      "1. Create new booking\n" +
                      "2. Edit booking\n" +
                      "3. Delete booking\n" +
                      "4. View bookings\n\n" +
                      "5. Return to main menu");
        Console.Write("Enter choice:  ");
        int.TryParse(Console.ReadLine(), out int answer);

        switch (answer)
        {
            default:
                Console.WriteLine("No such option");
                Console.ReadKey();
                break;
            case 1:
                Console.WriteLine("1. create new booking");
                Console.ReadKey();
                break;
            case 2:
                Console.WriteLine("edit booking");
                Console.ReadKey();
                break;
            case 3:
                Console.WriteLine("view booking");
                Console.ReadKey();
                break;
            case 4:
                Console.WriteLine("delete booking");
                Console.ReadKey();
                break;
            case 6:
                Task search = searchpagemenu();
                break;
            case 5:
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
        Console.WriteLine("--- Rooms ---\n" +
                          "1. Sort by price ASC\n" +
                          "2. Sort by reviews DESC\n\n" +
                          "3. Return to bookings menu");
        Console.Write("Enter choice:  ");
        int.TryParse(Console.ReadLine(), out int answer);

        switch (answer)
        {
            default:
                Console.WriteLine("No such option");
                Console.ReadKey();
                break;
            case 1:
                Console.Clear();
                Console.WriteLine(await sort.RoomsPriceASC());
                Console.ReadKey();
                break;
            case 2:
                Console.Clear();
                Console.WriteLine(await sort.RoomsReviewsDESC()); 
                Console.ReadKey();
                break;
            case 3:
                Console.Clear();
                Console.WriteLine("You have chosen to return to main menu.\n Press any key to continue!");
                Console.ReadKey();
                returntomainmenu = true; return;
        }
    } while (!returntomainmenu);
}