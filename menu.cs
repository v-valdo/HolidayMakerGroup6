// menu

/* Create booking (Kund Reg)
Edit Booking
Delete Booking
View Booking*/
/*
using Npgsql;
using System.Collections.Concurrent;
using System.Runtime.InteropServices;

string? dbUri = "Host=localhost;Port=5455;Username=postgres;Password=postgres;Database=holidaymakergroup6";

await using var db = NpgsqlDataSource.Create(dbUri);


bool endprogram = false;
do
{
    Console.Clear();
    Console.WriteLine(@"---- Holidaymaker group 6 ----" +
                        "1. Register new customer" +
                        "2. Bookings\n\n" +
                        "3. End program");

    // choice "view bookings"
    Console.WriteLine("---- Bookings ----" +
                      "1. Create new booking" +
                      "2. Edit booking" +
                      "3. Delete booking" +
                      "4. View bookings\n\n" +
                      "5. Return to main menu");

    Console.ReadKey();
    endprogram = true;
} while (!endprogram);
*/