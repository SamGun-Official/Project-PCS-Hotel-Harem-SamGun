﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hotel_Harem_SamGun
{
    class Koneksi
    {
        public static MySqlConnection conn;
        public static string server;
        public static string uid;
        public static string database;
        public static bool koneksiValid;

        public static void openConn()
        {
            server = "localhost";
            uid = "root";
            database = "db_hotel_samgun";

            conn = new MySqlConnection($"server = {server}; uid = {uid}; database = {database}");

            try
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();
                }

                koneksiValid = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal terhubung ke database!", "Failed");
                Console.WriteLine(ex.Message);

                koneksiValid = false;
            }
        }

        public static void closeConn()
        {
            conn.Close();
        }
    }
}
