﻿using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace Hotel_Harem_SamGun
{
    public partial class FormLogin : Form
    {
        public static DataTable dtKaryawan;

        public FormLogin()
        {
            InitializeComponent();
        }

        private void FormLogin_Load(object sender, EventArgs e)
        {
            label4.ForeColor = System.Drawing.ColorTranslator.FromHtml("#f7a13e");
            tbUsername.BackColor = System.Drawing.ColorTranslator.FromHtml("#f6f6f6");
            tbPassword.BackColor = System.Drawing.ColorTranslator.FromHtml("#f6f6f6");
            btLogin.BackColor = System.Drawing.ColorTranslator.FromHtml("#f7a13e");
            btLogin.ForeColor = System.Drawing.Color.White;
            btLogin.FlatAppearance.BorderSize = 0;

            Koneksi.openConn();
            if (!Koneksi.isValid)
            {
                MessageBox.Show("Gagal terhubung ke database!", "Gagal");
                return;
            }
        }

        private bool getUsernamePasswordRoles()
        {
            dtKaryawan = new DataTable();

            try
            {
                string query = $"SELECT kode_karyawan, nama_karyawan, username, password, roles FROM `karyawan` WHERE `username`=@Username AND `password`=@Password";
                MySqlDataAdapter da = new MySqlDataAdapter(query, Koneksi.conn);
                da.SelectCommand.Parameters.AddWithValue("@Username", tbUsername.Text);
                da.SelectCommand.Parameters.AddWithValue("@Password", tbPassword.Text);
                da.Fill(dtKaryawan);

                if (dtKaryawan.Rows.Count == 1) return true;
                else return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }

            return false;
        }

        private void btLogin_Click(object sender, EventArgs e)
        {
            //CEK FIELD TEXTBOX
            if (tbUsername.Text.Trim() == "" || tbPassword.Text.Trim() == "")
            {
                MessageBox.Show("Tidak boleh ada field kosong!", "Gagal");
                return;
            }

            if (!getUsernamePasswordRoles())
            {
                MessageBox.Show("Username dan password tidak ditemukan!", "Gagal");
                return;
            }

            //CEK USERNAME
            if (tbUsername.Text.Trim() != dtKaryawan.Rows[0][2].ToString())
            {
                MessageBox.Show("Username salah! Harap cek kembali!", "Gagal");
                return;
            }

            //CEK PASSWORD
            if (tbPassword.Text.Trim() != dtKaryawan.Rows[0][3].ToString())
            {
                MessageBox.Show("Password salah! Harap cek kembali!", "Gagal");
                tbPassword.Text = "";
                return;
            }

            string roles = dtKaryawan.Rows[0][4].ToString();

            //GANTI FORM
            tbUsername.Text = "";
            tbPassword.Text = "";

            this.Hide();

            if (roles == "Admin")
            {
                FormMenuAdmin form = new FormMenuAdmin();
                form.ShowDialog();
            }
            else
            {
                FormMenuResepsionis form = new FormMenuResepsionis();
                form.ShowDialog();
            }

            this.Show();
        }
    }
}
