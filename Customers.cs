using Npgsql;
using System.Security.Cryptography.X509Certificates;

namespace HolidayMakerGroup6
{
    public class Customers
    {
        public string fullName;
        public int phoneNumber;
        public string email;
        public string DoB;

        public async Task RegisterCustomer()
        {

            string dbUri = "Host=localhost;Port=5455;Username=postgres;Password=postgres;Database=holidaymakergroup6";

            using var db = NpgsqlDataSource.Create(dbUri);

            Console.Write("Fullname: ");
            fullName = Console.ReadLine();
            Console.Clear();

            Console.Write("Email: ");
            email = Console.ReadLine();
            Console.Clear();

            Console.Write("Phone number: ");
            phoneNumber = int.Parse(Console.ReadLine());
            Console.Clear();

            Console.WriteLine("Date of Birth (Like this ->[xxxx-xx-xx]");
            DoB = Console.ReadLine();
            Console.Clear();


            using (var cmd = db.CreateCommand("INSERT INTO customers (name, email, telnumber, date_of_birth) VALUES (@name, @email, @telnumber, @date_of_birth)"))
            {
                cmd.Parameters.AddWithValue("@name", fullName);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@telnumber", phoneNumber);
                cmd.Parameters.AddWithValue("@date_of_birth", DateTime.Parse(DoB));
                cmd.ExecuteNonQuery();

            }

        }
    }
}
