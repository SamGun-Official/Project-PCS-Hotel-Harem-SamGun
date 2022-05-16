﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hotel_Harem_SamGun
{
    public partial class FormMenuResepsionis : Form
    {
        public static string user_resepsionis;
        public FormMenuResepsionis(string username)
        {
            InitializeComponent();
            user_resepsionis = username;
        }

        private void FormMenuResepsionis_Load(object sender, EventArgs e)
        {
            string query = $"SELECT nama_karyawan FROM karyawan WHERE username = '{user_resepsionis}'";

            try
            {
                MySqlCommand cmd = new MySqlCommand(query, Koneksi.conn);
                lbWelcome.Text = $"Welcome,\n{cmd.ExecuteScalar()}!";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal mendapatkan nama karyawan!");
            }
            
        }

        private void btReservasi_Click(object sender, EventArgs e)
        {
            Hide();
            FormReservasi form = new FormReservasi();
            form.ShowDialog();
            Show();
        }

        private void btPenambahanFasilitasKamar_Click(object sender, EventArgs e)
        {
            Hide();
            FormPenambahanFasilitasDataTamu form = new FormPenambahanFasilitasDataTamu();
            form.ShowDialog();
            Show();
        }

        private void btPemesananMakanan_Click(object sender, EventArgs e)
        {
            Hide();
            FormPemesananMakanan form = new FormPemesananMakanan();
            form.ShowDialog();
            Show();
        }

        private void btCheckInCheckOut_Click(object sender, EventArgs e)
        {
            Hide();
            //FormCheckInCheckOut form = new FormCheckInCheckOut();
            //form.ShowDialog();
            Show();
        }
    }
}