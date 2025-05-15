using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

public class DatabaseHelper
{
    private static string connectionString = "server=localhost;database=companydump;user=root;password=;";

    public static MySqlConnection GetConnection()
    {
        return new MySqlConnection(connectionString);
    }
}

