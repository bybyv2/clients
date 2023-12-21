


using System.Data;
using System.Runtime.Serialization.Formatters;
using System.Security.Cryptography;
using System.Text;
using MySqlConnector;

(int left, int top) = Console.GetCursorPosition();
var option = 1;
var decorator = "  -->";
string space = new string(' ', decorator.Length);
ConsoleKeyInfo key;
bool isSelected = false;
while (!isSelected)
{
    Console.SetCursorPosition(left, top);
    Console.WriteLine("+------------------------------+");
    Console.WriteLine($"{(option == 1 ? decorator : space)}Clients|");
    Console.WriteLine($"{(option == 2 ? decorator : space)}Produits|");
    Console.WriteLine($"{(option == 3 ? decorator : space)}Commandes|");
    Console.WriteLine("+------------------------------+");
    key = Console.ReadKey(false);
    switch (key.Key)
    {
        case ConsoleKey.UpArrow:
            option = option == 1 ? 3 : option - 1;
            break;
        case ConsoleKey.DownArrow:
            option = option == 3 ? 1 : option + 1;
            break;
        case ConsoleKey.Enter:
            isSelected = true;
            break;
    }
}

if (option == 1)
{




    string m_strMySQLConnectionString;
    m_strMySQLConnectionString = "server=localhost;userid=root;password=123;database=clients";

    using (var mysqlconnection = new MySqlConnection(m_strMySQLConnectionString))
    {
        mysqlconnection.Open();
        bool premiereLigne = true;
        int i = 0;
        Random random = new Random();
        foreach (string line in System.IO.File.ReadAllLines(@"D:\Programmation\clients\src\csharp\data\clients.csv"))
        {
            string pw = "";
           
            if (premiereLigne==false)
            {
                for (int j = 0; j < 32; j++)
                {
                    pw = pw + random.Next(0, 9).ToString();
                    
                }
                string password =sha256(pw);
                
                var line2 = line.Replace("\"", "'");
                var columns = line2.Split(",");
                string first_name = columns[0];
                string last_name = columns[1];
                string company_name = columns[2];
                string address = columns[3];
                string city = columns[4];
                string county = columns[5];
                string state = columns[6];
                string zip = columns[7];
                string phone1 = columns[8];
                string phone2 = columns[9];
                string email = columns[10];
                string web = columns[11];

                int horrible = Math.Min(first_name.Length-1, 4);
                
                string login = "," + first_name.Substring(0, horrible) + last_name.Substring(1, last_name.Length-2) + "'";

                string sql = $"INSERT INTO clientsLogin(first_name, last_name, company_name, address, city, county, state, zip, phone1, phone2, email, web, login, password) VALUES ({line2 + login +",'" + password+"'"})\n";
                using (MySqlCommand cmd = mysqlconnection.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = 300;
                    cmd.CommandText = sql;
                    try
                    {
                        i += cmd.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(sql+"\n"+e.Message);
                    }
                    

                }
            }
            premiereLigne = false;
        }
        mysqlconnection.Close();
        Console.WriteLine($"{i} lignes ajoutees");
    }
}













string sha256(string input)
{
    using (var hashGenerator = SHA256.Create())
    {
        var hash = hashGenerator.ComputeHash(Encoding.Default.GetBytes(input));
        return BitConverter.ToString(hash);
    }
}