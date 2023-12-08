using Npgsql;

// database
string? dbUri = "Host=localhost;Port=5455;Username=postgres;Password=postgres;Database=holidaymakergroup6";
// connection to database
await using var db = NpgsqlDataSource.Create(dbUri);


// main menu
bool endprogram = false;
do
{
    Console.Clear();
    Console.WriteLine("---- Holidaymaker group 6 ----\n" +
                        "\n1. Register new customer\n" +
                        "2. Bookings\n\n" +
                        "3. End program");
    Console.Write("Write your menuchoice:  ");
    int.TryParse(Console.ReadLine(), out int answer);

    switch (answer)
    {
        default:
            Console.WriteLine("is not in the menu!");
            Console.ReadKey();
            break;
        case 1:
            Console.WriteLine("1. register customer");
            Console.ReadKey();
            break;
        case 2:
            bookingmenu();
            break;
        case 3:
            Console.Clear();
            Console.WriteLine("You have chosen to end the program.\n Press any key to continue!");
            Console.ReadKey();
            endprogram = true;break;
        
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
        Console.Write("Write your menuchoice:  ");
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