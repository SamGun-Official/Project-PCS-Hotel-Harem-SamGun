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
    public partial class FormDataMenuMakanan : Form
    {
        bool isEdit = false;
        List<String> id_jenis = new List<string>();
        DataTable dtmakanan;
        DataRow pick;
        bool start = false;
        string query = "";
        public string fontName = "Gill Sans MT";
        public float fontSize = 16F;

        public FormDataMenuMakanan()
        {
            InitializeComponent();
        }

        private void FormDataMenuMakanan_Load(object sender, EventArgs e)
        {
            /*Koneksi.openConn();*/
            id_jenis = new List<string>();
            isEdit = false;
            start = false;
            query = @"SELECT
  makanan.id_makanan,
  makanan.nama_makanan,
  makanan.harga_makanan,
CONCAT('Rp ', FORMAT(makanan.harga_makanan, 0)),
  makanan.stok_makanan,
  makanan.status_makanan,
  makanan.id_jenis_makanan,
  jenis_makanan.nama_jenis_makanan
FROM makanan
  INNER JOIN jenis_makanan
    ON makanan.id_jenis_makanan = jenis_makanan.id_jenis_makanan
WHERE makanan.status_makanan != 0
order by 1 asc";
            loadCB();
            loadDatagrid();
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font(fontName, fontSize, FontStyle.Regular, GraphicsUnit.Pixel);
            dataGridView1.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False;
            dataGridView1.DefaultCellStyle.Font = new Font(fontName, fontSize, FontStyle.Regular, GraphicsUnit.Pixel);
            start = true;
        }

        public void loadCB()
        {
            id_jenis = new List<string>();
            cbJenisMakanan.Items.Clear();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = Koneksi.conn;
            cmd.CommandText = @"SELECT id_jenis_makanan, nama_jenis_makanan FROM jenis_makanan order by 1 asc";
            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                cbJenisMakanan.Items.Add(reader.GetString(1));
                id_jenis.Add(reader.GetString(0));

            }
            reader.Close();

            cbJenisMakanan.SelectedIndex = 0;
        }


        // private DataGridView UpdateDataGridViewFont(DataGridView dataGridView, float fontSize)
        // {
        //     dataGridView.Font = new Font(fontName, fontSize, dataGridView.Font.Style, GraphicsUnit.Pixel, ((byte)(0)));

        //     dataGridView.ColumnHeadersDefaultCellStyle.Font = new Font(fontName, fontSize, FontStyle.Regular, GraphicsUnit.Pixel);
        //     dataGridView.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False;

        //     foreach (DataGridViewRow r in dataGridView.Rows)
        //     {
        //         r.DefaultCellStyle.Font = new Font(fontName, fontSize, FontStyle.Regular, GraphicsUnit.Pixel);
        //     }

        //     return dataGridView;
        // }

        public void loadDatagrid()
        {
            try
            {
                dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.White;
                dataGridView1.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.White;
                dataGridView1.EnableHeadersVisualStyles = false;

                MySqlDataAdapter adapter = new MySqlDataAdapter(query, Koneksi.conn);
                dtmakanan = new DataTable();
                adapter.Fill(dtmakanan);

                dataGridView1.DataSource = dtmakanan;
                dataGridView1.Columns[0].HeaderText = "ID Makanan";
                dataGridView1.Columns[1].HeaderText = "Nama Makanan";
                dataGridView1.Columns[2].Visible = false;
                dataGridView1.Columns[3].HeaderText = "Harga Makanan";
                dataGridView1.Columns[4].HeaderText = "Stok Makanan";
                dataGridView1.Columns[5].Visible = false;
                dataGridView1.Columns[6].Visible = false;
                dataGridView1.Columns[7].HeaderText = "Jenis Makanan";

                fitDataGridViewColumn(0, false);
                fitDataGridViewColumn(3, false);
                fitDataGridViewColumn(4, false);
                fitDataGridViewColumn(7, false);

                dataGridView1.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dataGridView1.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void fitDataGridViewColumn(int columnIndex, bool setWidthGrow = false, int minimumWidth = 0)
        {
            if (setWidthGrow)
            {
                dataGridView1.Columns[columnIndex].MinimumWidth = (minimumWidth != 0) ? minimumWidth : dataGridView1.Columns[columnIndex].Width;
            }

            dataGridView1.Columns[columnIndex].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            // Console.WriteLine("Column [Index, Width]: " + columnIndex + ", " + dataGridView1.Columns[columnIndex].Width);
        }

        private void fitDataGridViewColumn(string columnName, bool setWidthGrow = false, int minimumWidth = 0)
        {
            if (setWidthGrow)
            {
                dataGridView1.Columns[columnName].MinimumWidth = (minimumWidth != 0) ? minimumWidth : dataGridView1.Columns[columnName].Width;
            }

            dataGridView1.Columns[columnName].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            // Console.WriteLine("Column [Index, Width]: " + columnName + ", " + dataGridView1.Columns[columnName].Width);
        }

        private void tbHarga_TextChanged(object sender, EventArgs e)
        {
            if (tbHarga.Text.Length > 0)
            {
                char temp = tbHarga.Text[(tbHarga.Text.Length - 1)];
                if (!Char.IsDigit(temp))
                {
                    tbHarga.Text = tbHarga.Text.Remove(tbHarga.Text.Length - 1, 1);
                    tbHarga.SelectionStart = tbHarga.Text.Length;
                }
            }

            if (start)
            {
                if (!isEdit)
                {
                    generateID();
                }
            }
        }

        private void tbStok_TextChanged(object sender, EventArgs e)
        {
            if (tbStok.Text.Length > 0)
            {
                char temp = tbStok.Text[(tbStok.Text.Length - 1)];
                if (!Char.IsDigit(temp))
                {
                    tbStok.Text = tbStok.Text.Remove(tbStok.Text.Length - 1, 1);
                    tbStok.SelectionStart = tbStok.Text.Length;
                }
            }

            if (start)
            {
                if (!isEdit)
                {
                    generateID();
                }
            }
        }

        public void generateID()
        {
            if (tbNama.Text != "" || tbHarga.Text != "" || tbStok.Text != "" || cbJenisMakanan.SelectedIndex != -1)
            {
                MySqlCommand cmd = new MySqlCommand("SELECT MAX(id_makanan) FROM makanan", Koneksi.conn);
                int nextID = Convert.ToInt32(cmd.ExecuteScalar()) + 1;
                tbKode.Text = "" + nextID;
            }
            else
            {
                tbKode.Text = "";
            }
        }

        private void tbNama_TextChanged(object sender, EventArgs e)
        {
            if (start)
            {
                if (!isEdit)
                {
                    generateID();
                }
            }
        }

        private void cbJenisMakanan_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (start)
            {
                if (!isEdit)
                {
                    generateID();
                }
            }
        }

        private void rbTersedia_CheckedChanged(object sender, EventArgs e)
        {
            if (start)
            {
                if (!isEdit)
                {
                    generateID();
                }
            }
        }

        private void rbTidakTersedia_CheckedChanged(object sender, EventArgs e)
        {
            if (start)
            {
                if (!isEdit)
                {
                    generateID();
                }
            }
        }

        private void btnBersihkan_Click(object sender, EventArgs e)
        {
            isEdit = false;
            start = false;
            tbKode.Text = "";
            tbNama.Text = "";
            tbHarga.Text = "";
            tbStok.Text = "";
            cbJenisMakanan.SelectedIndex = 0;
            start = true;
            btnEdit.Enabled = false;
            btnTambah.Enabled = true;
            btnHapus.Enabled = false;
            query = @"SELECT
  makanan.id_makanan,
  makanan.nama_makanan,
  makanan.harga_makanan,
  CONCAT('Rp ', FORMAT(makanan.harga_makanan, 0)),
  makanan.stok_makanan,
  makanan.status_makanan,
  makanan.id_jenis_makanan,
  jenis_makanan.nama_jenis_makanan
FROM makanan
  INNER JOIN jenis_makanan
    ON makanan.id_jenis_makanan = jenis_makanan.id_jenis_makanan
WHERE makanan.status_makanan != 0
order by 1 asc";
            loadDatagrid();
            tbCari.Text = "";
        }

        private void btnTambah_Click(object sender, EventArgs e)
        {
            if (tbNama.Text != "")
            {
                if (tbHarga.Text != "")
                {
                    if (tbStok.Text != "")
                    {
                        if (cbJenisMakanan.SelectedIndex != -1)
                        {
                            if (Convert.ToInt32(tbHarga.Text) >= 0)
                            {
                                if (Convert.ToInt32(tbStok.Text) >= 0)
                                {
                                    MySqlTransaction sqlt = Koneksi.getConn().BeginTransaction();
                                    try
                                    {
                                        // cek nama makanan
                                        // cek status makanan 99

                                        MySqlCommand cmd = new MySqlCommand();
                                        cmd.CommandText = @"SELECT
count(makanan.id_makanan)
FROM makanan
WHERE UPPER(makanan.nama_makanan) like '%" + tbNama.Text.ToUpper() + "%' AND makanan.status_makanan=0";
                                        cmd.Connection = Koneksi.getConn();
                                        int ada;
                                        ada = Convert.ToInt32(cmd.ExecuteScalar().ToString());

                                        if (ada != 0)
                                        {
                                            MySqlCommand cmdid = new MySqlCommand();
                                            cmdid.CommandText = @"SELECT
makanan.id_makanan
FROM makanan
WHERE UPPER(makanan.nama_makanan) like '%" + tbNama.Text.ToUpper() + "%' AND makanan.status_makanan=0";
                                            cmdid.Connection = Koneksi.getConn();
                                            int idtemp;
                                            idtemp = Convert.ToInt32(cmdid.ExecuteScalar().ToString());

                                            MySqlCommand cmd2 = new MySqlCommand();
                                            cmd2.CommandText = "UPDATE makanan SET nama_makanan=@nama_makanan, harga_makanan=@harga_makanan, stok_makanan=@stok_makanan, status_makanan=@status_makanan, id_jenis_makanan=@id_jenis_makanan WHERE id_makanan=@id_makanan";
                                            cmd2.Parameters.AddWithValue("@id_makanan", idtemp);
                                            cmd2.Parameters.AddWithValue("@nama_makanan", tbNama.Text);
                                            cmd2.Parameters.AddWithValue("@harga_makanan", tbHarga.Text);
                                            cmd2.Parameters.AddWithValue("@stok_makanan", tbStok.Text);
                                            cmd2.Parameters.AddWithValue("@status_makanan", "1");
                                            cmd2.Parameters.AddWithValue("@id_jenis_makanan", id_jenis[cbJenisMakanan.SelectedIndex]);

                                            cmd2.Connection = Koneksi.getConn();
                                            cmd2.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            MySqlDataAdapter adapter = new MySqlDataAdapter(@"SELECT
makanan.id_makanan,
makanan.nama_makanan,
makanan.harga_makanan,
makanan.stok_makanan,
makanan.total_terjual,
makanan.status_makanan,
makanan.id_jenis_makanan
FROM makanan
order by 1 asc", Koneksi.getConn());
                                            DataTable dt = new DataTable();
                                            MySqlCommandBuilder builder = new MySqlCommandBuilder(adapter);
                                            adapter.Fill(dt);

                                            DataRow baru = dt.NewRow();
                                            baru["id_makanan"] = tbKode.Text;
                                            baru["nama_makanan"] = tbNama.Text;
                                            baru["harga_makanan"] = tbHarga.Text;
                                            baru["stok_makanan"] = tbStok.Text;
                                            baru["total_terjual"] = "0";
                                            baru["status_makanan"] = "1";
                                            baru["id_jenis_makanan"] = id_jenis[cbJenisMakanan.SelectedIndex];
                                            dt.Rows.Add(baru);

                                            adapter.Update(dt);
                                        }
                                        loadCB();
                                        loadDatagrid();
                                        refreshDataGridView();

                                        isEdit = false;
                                        start = false;
                                        tbKode.Text = "";
                                        tbNama.Text = "";
                                        tbHarga.Text = "";
                                        tbStok.Text = "";
                                        cbJenisMakanan.SelectedIndex = 0;
                                        start = true;
                                        btnEdit.Enabled = false;
                                        btnHapus.Enabled = false;

                                        MessageBox.Show("Berhasil menambah menu makanan baru!", "Berhasil");
                                        sqlt.Commit();
                                    }
                                    catch (MySqlException ex)
                                    {
                                        Console.WriteLine(ex.Message);
                                        sqlt.Rollback();
                                        MessageBox.Show("Gagal menambah menu makanan baru!", "Gagal");
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Stok makanan harus lebih besar sama dengan 0! (>=0)", "Gagal");
                                }
                            }
                            else
                            {
                                MessageBox.Show("Harga makanan harus lebih besar sama dengan 0! (>=0)", "Gagal");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Jenis makanan tidak boleh kosong!", "Gagal");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Stok makanan tidak boleh kosong!", "Gagal");
                    }
                }
                else
                {
                    MessageBox.Show("Harga makanan tidak boleh kosong!", "Gagal");
                }
            }
            else
            {
                MessageBox.Show("Nama makanan tidak boleh kosong!", "Gagal");
            }
        }

        public void refreshDataGridView()
        {
            dataGridView1.DataSource = dtmakanan;
            dataGridView1.Columns[0].HeaderText = "ID Makanan";
            dataGridView1.Columns[1].HeaderText = "Nama Makanan";
            dataGridView1.Columns[2].Visible = false;
            dataGridView1.Columns[3].HeaderText = "Harga Makanan";
            dataGridView1.Columns[4].HeaderText = "Stok Makanan";
            dataGridView1.Columns[5].Visible = false;
            dataGridView1.Columns[6].Visible = false;
            dataGridView1.Columns[7].HeaderText = "Jenis Makanan";
            dataGridView1.ClearSelection();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (tbNama.Text != "")
            {
                if (tbHarga.Text != "")
                {
                    if (tbStok.Text != "")
                    {
                        if (cbJenisMakanan.SelectedIndex != -1)
                        {
                            if (Convert.ToInt32(tbHarga.Text) >= 0)
                            {
                                if (Convert.ToInt32(tbStok.Text) >= 0)
                                {
                                    MySqlTransaction sqlt = Koneksi.getConn().BeginTransaction();
                                    try
                                    {
                                        MySqlCommand cmd2 = new MySqlCommand();
                                        cmd2.CommandText = "UPDATE makanan SET nama_makanan=@nama_makanan, harga_makanan=@harga_makanan, stok_makanan=@stok_makanan, id_jenis_makanan=@id_jenis_makanan WHERE id_makanan=@id_makanan";
                                        cmd2.Parameters.AddWithValue("@id_makanan", tbKode.Text);
                                        cmd2.Parameters.AddWithValue("@nama_makanan", tbNama.Text);
                                        cmd2.Parameters.AddWithValue("@harga_makanan", tbHarga.Text);
                                        cmd2.Parameters.AddWithValue("@stok_makanan", tbStok.Text);
                                        cmd2.Parameters.AddWithValue("@id_jenis_makanan", id_jenis[cbJenisMakanan.SelectedIndex]);
                                        cmd2.Connection = Koneksi.getConn();
                                        cmd2.ExecuteNonQuery();

                                        loadCB();
                                        loadDatagrid();
                                        refreshDataGridView();

                                        isEdit = false;
                                        start = false;
                                        tbKode.Text = "";
                                        tbNama.Text = "";
                                        tbHarga.Text = "";
                                        tbStok.Text = "";
                                        cbJenisMakanan.SelectedIndex = 0;
                                        start = true;
                                        btnEdit.Enabled = false;
                                        btnHapus.Enabled = false;
                                        btnTambah.Enabled = true;

                                        MessageBox.Show("Berhasil ubah data menu makanan!", "Berhasil");
                                        sqlt.Commit();
                                    }
                                    catch (MySqlException ex)
                                    {
                                        Console.WriteLine(ex.Message);
                                        sqlt.Rollback();
                                        MessageBox.Show("Gagal ubah data menu makanan!", "Gagal");
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Stok makanan harus lebih besar sama dengan 0! (>=0)", "Gagal");
                                }
                            }
                            else
                            {
                                MessageBox.Show("Harga makanan harus lebih besar sama dengan 0! (>=0)", "Gagal");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Jenis makanan tidak boleh kosong!", "Gagal");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Stok makanan tidak boleh kosong!", "Gagal");
                    }
                }
                else
                {
                    MessageBox.Show("Harga makanan tidak boleh kosong!", "Gagal");
                }
            }
            else
            {
                MessageBox.Show("Nama makanan tidak boleh kosong!", "Gagal");
            }
        }

        private void btnHapus_Click(object sender, EventArgs e)
        {
            MySqlTransaction sqlt = Koneksi.getConn().BeginTransaction();
            try
            {
                MySqlCommand cmd2 = new MySqlCommand();
                cmd2.CommandText = "UPDATE makanan set status_makanan=@status_makanan WHERE id_makanan=@id_makanan";
                cmd2.Parameters.AddWithValue("@id_makanan", tbKode.Text);
                cmd2.Parameters.AddWithValue("@status_makanan", "0");

                cmd2.Connection = Koneksi.getConn();
                cmd2.ExecuteNonQuery();

                loadCB();
                loadDatagrid();
                refreshDataGridView();

                isEdit = false;
                start = false;
                tbKode.Text = "";
                tbNama.Text = "";
                tbHarga.Text = "";
                tbStok.Text = "";
                cbJenisMakanan.SelectedIndex = 0;
                start = true;
                btnEdit.Enabled = false;
                btnHapus.Enabled = false;
                btnTambah.Enabled = true;

                MessageBox.Show("Berhasil hapus menu makanan!", "Berhasil");
                sqlt.Commit();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                sqlt.Rollback();
                MessageBox.Show("Gagal hapus menu makanan!", "Gagal");
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            /*makanan.id_makanan, 0
            makanan.nama_makanan, 1
            makanan.harga_makanan, 2
            makanan.stok_makanan, 3
            makanan.status_makanan, 4
            makanan.id_jenis_makanan, 5
            jenis_makanan.nama_jenis_makanan 6 */
            isEdit = true;
            btnHapus.Enabled = true;
            btnTambah.Enabled = false;
            btnEdit.Enabled = true;
            pick = dtmakanan.Rows[dataGridView1.CurrentRow.Index];
            tbKode.Text = pick[0].ToString();
            tbNama.Text = pick[1].ToString();
            tbHarga.Text = pick[2].ToString();
            tbStok.Text = pick[4].ToString();
            cbJenisMakanan.SelectedItem = pick[7].ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            query = @"SELECT
  makanan.id_makanan,
  makanan.nama_makanan,
  makanan.harga_makanan,
CONCAT('Rp ', FORMAT(makanan.harga_makanan, 0)),
  makanan.stok_makanan,
  makanan.status_makanan,
  makanan.id_jenis_makanan,
  jenis_makanan.nama_jenis_makanan
FROM makanan
  INNER JOIN jenis_makanan
    ON makanan.id_jenis_makanan = jenis_makanan.id_jenis_makanan
WHERE makanan.status_makanan != 0
AND
makanan.nama_makanan LIKE '%" + tbCari.Text + @"%'
order by 1 asc";
            loadDatagrid();
            tbCari.Text = "";
        }
    }
}
