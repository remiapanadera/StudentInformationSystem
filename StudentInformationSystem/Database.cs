using MySql.Data.MySqlClient;

namespace StudentInformationSystem
{
    public class Database
    {
        private readonly string connectionString =
            "server=localhost;database=student_information_system;user=root;password=remia2006;";

        public MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }
    }
}