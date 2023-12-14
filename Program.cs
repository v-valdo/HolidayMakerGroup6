using HolidayMakerGroup6;
using Npgsql;

await using var db = NpgsqlDataSource.Create(Database.Url);

TableCreation tables = new();
await tables.Create();

// Customer customer = new();

// await customer.ShowAll();

// Console.ReadLine();
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
            Customer customers = new Customer();
            await customers.Register();
            Console.ReadKey();
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
                      "1. Create new booking\n" +
                      "2. Edit booking\n" +
                      "3. Delete booking\n" +
                      "4. View bookings\n\n" +
                      "5. Return to main menu");
        Console.Write("Enter choice:  ");
        int.TryParse(Console.ReadLine(), out int answer);

        switch (answer)
        {
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
            case 5:
                Console.Clear();
                Console.WriteLine("You have chosen to return to main menu.\n Press any key to continue!");
                Console.ReadKey();
                returntomainmenu = true; return;
        }
    } while (!returntomainmenu);
}