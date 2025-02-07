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
    public partial class FormDetailReservasi : Form
    {
        bool inputingData;
        int deposito, dp, subtotal;

        DataTable dtDetailReservasi, dtAvailableKodeReservasi, dtNamaTamu, dtJenisKamar, dtNomorKamar;
        MySqlDataAdapter da;

        public FormDetailReservasi()
        {
            InitializeComponent();
        }

        private void FormDetailReservasi_Load(object sender, EventArgs e)
        {
            refresh_cbCari_dgvDetailReservasi();
            loadNamaTamu();
            loadJenisKamar();
            changeMode(1);
            dgvDetailReservasi.ColumnHeadersDefaultCellStyle.Font = new Font("Gill Sans MT", 12, FontStyle.Regular);
            dgvDetailReservasi.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False;
            dgvDetailReservasi.DefaultCellStyle.Font = new Font("Gill Sans MT", 12, FontStyle.Regular);
        }

        private void fillDT(DataTable dt, string query)
        {
            try
            {
                dgvDetailReservasi.ColumnHeadersDefaultCellStyle.BackColor = Color.White;
                dgvDetailReservasi.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.White;
                dgvDetailReservasi.EnableHeadersVisualStyles = false;
                da = new MySqlDataAdapter(query, Koneksi.conn);
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void loadDetailReservasi(int status)
        {
            dgvDetailReservasi.ColumnHeadersDefaultCellStyle.BackColor = Color.White;
            dgvDetailReservasi.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.White;
            dgvDetailReservasi.EnableHeadersVisualStyles = false;
            dtDetailReservasi = new DataTable();
            string query = @"
                SELECT CONCAT(t.nama_tamu, ' - ', dr.kode_reservasi) AS nama_kode, dr.kode_reservasi, t.nama_tamu, jk.nama_jenis_kamar, km.nomor_kamar, dr.jumlah_penghuni_kamar, CONCAT('Rp ', FORMAT(down_payment, 0, 'de_DE')), down_payment, CONCAT('Rp ', FORMAT(deposito, 0, 'de_DE')), deposito, DATE_FORMAT(jadwal_check_in, '%W, %d %M %Y'), jadwal_check_in, DATE_FORMAT(jadwal_check_out, '%W, %d %M %Y'), jadwal_check_out, IFNULL(DATE_FORMAT(tanggal_check_in, '%W, %d %M %Y'), '-'), tanggal_check_in, IFNULL(DATE_FORMAT(tanggal_check_out, '%W, %d %M %Y'), '-'), tanggal_check_out, CONCAT('Rp ', FORMAT(dr.subtotal_biaya_reservasi, 0, 'de_DE')), dr.subtotal_biaya_reservasi, dr.status_detail_reservasi
                FROM detail_reservasi dr
                JOIN header_reservasi hr ON hr.kode_reservasi = dr.kode_reservasi
                JOIN tamu t ON t.kode_tamu = hr.kode_tamu
                JOIN kamar km ON km.kode_kamar = dr.kode_kamar
                JOIN jenis_kamar jk ON jk.id_jenis_kamar = km.id_jenis_kamar";
            if (status == 0)
            {
                // SELESAI
                query += " WHERE status_detail_reservasi = 0";
            }
            else if (status == 1)
            {
                // DIPESAN
                query += " WHERE status_detail_reservasi = 1";
            }
            else if (status == 2)
            {
                // CHECK IN
                query += " WHERE status_detail_reservasi = 2";
            }
            else if (status == 3)
            {
                // DIBATALKAN
                query += " WHERE status_detail_reservasi = 99";
            }
            query += " ORDER BY dr.id_detail_reservasi";
            fillDT(dtDetailReservasi, query);
        }

        private void refresh_cbCari_dgvDetailReservasi(int status = -1)
        {
            dgvDetailReservasi.ColumnHeadersDefaultCellStyle.BackColor = Color.White;
            dgvDetailReservasi.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.White;
            dgvDetailReservasi.EnableHeadersVisualStyles = false;
            loadDetailReservasi(status);
            cbCari.DataSource = dtDetailReservasi;
            cbCari.DisplayMember = "nama_kode";
            cbCari.ValueMember = "kode_reservasi";
            cbCari.SelectedIndex = -1;

            dgvDetailReservasi.DataSource = dtDetailReservasi;
            dgvDetailReservasi.Columns[0].Visible = false;
            dgvDetailReservasi.Columns[1].HeaderText = "Kode Reservasi";
            dgvDetailReservasi.Columns[2].HeaderText = "Nama Tamu";
            dgvDetailReservasi.Columns[3].HeaderText = "Jenis Kamar";
            dgvDetailReservasi.Columns[4].HeaderText = "Nomor Kamar";
            dgvDetailReservasi.Columns[5].HeaderText = "Jumlah Penghuni Kamar";
            dgvDetailReservasi.Columns[6].HeaderText = "Down Payment";
            dgvDetailReservasi.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvDetailReservasi.Columns[7].Visible = false;
            dgvDetailReservasi.Columns[8].HeaderText = "Deposito";
            dgvDetailReservasi.Columns[8].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvDetailReservasi.Columns[9].Visible = false;
            dgvDetailReservasi.Columns[10].HeaderText = "Jadwal Check In";
            dgvDetailReservasi.Columns[11].Visible = false;
            dgvDetailReservasi.Columns[12].HeaderText = "Jadwal Check Out";
            dgvDetailReservasi.Columns[13].Visible = false;
            dgvDetailReservasi.Columns[14].HeaderText = "Tanggal Check In";
            dgvDetailReservasi.Columns[15].Visible = false;
            dgvDetailReservasi.Columns[16].HeaderText = "Tanggal Check Out";
            dgvDetailReservasi.Columns[17].Visible = false;
            dgvDetailReservasi.Columns[18].HeaderText = "Subtotal Biaya Reservasi";
            dgvDetailReservasi.Columns[18].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvDetailReservasi.Columns[19].Visible = false;
            dgvDetailReservasi.Columns[20].Visible = false;

            //DISABLE SORT
            foreach (DataGridViewColumn col in dgvDetailReservasi.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            //COLOR
            foreach (DataGridViewRow row in dgvDetailReservasi.Rows)
            {

                if (row.Cells[20].Value.ToString() == "0")
                {
                    row.DefaultCellStyle.BackColor = Color.White;
                }
                else if (row.Cells[20].Value.ToString() == "1")
                {
                    row.DefaultCellStyle.BackColor = Color.Yellow;
                }
                else if (row.Cells[20].Value.ToString() == "2")
                {
                    row.DefaultCellStyle.BackColor = Color.LightGreen;
                }
                else
                {
                    row.DefaultCellStyle.BackColor = Color.DarkGray;
                }
            }

            dgvDetailReservasi.ClearSelection();

            loadAvailableKodeReservasi();
        }

        private void loadAvailableKodeReservasi()
        {
            dtAvailableKodeReservasi = new DataTable();
            string query = @"
                SELECT CONCAT(kode_reservasi, ' - ', nama_tamu) AS kode_nama, kode_reservasi, nama_tamu
                FROM header_reservasi hr
                JOIN tamu t ON t.kode_tamu = hr.kode_tamu
                WHERE status_header_reservasi = 1";
            fillDT(dtAvailableKodeReservasi, query);
            cbKodeReservasi.DataSource = dtAvailableKodeReservasi;
            cbKodeReservasi.DisplayMember = "kode_nama";
            cbKodeReservasi.ValueMember = "kode_reservasi";
            cbKodeReservasi.SelectedIndex = -1;
        }

        private void loadNamaTamu()
        {
            dtNamaTamu = new DataTable();
            string query = "SELECT kode_tamu, nama_tamu FROM tamu WHERE status_tamu = 1";
            fillDT(dtNamaTamu, query);
            cbNamaTamu.DataSource = dtNamaTamu;
            cbNamaTamu.DisplayMember = "nama_tamu";
            cbNamaTamu.ValueMember = "kode_tamu";
            cbNamaTamu.SelectedIndex = -1;
        }

        private void loadJenisKamar()
        {
            dtJenisKamar = new DataTable();
            string query = "SELECT id_jenis_kamar, nama_jenis_kamar, harga_jenis_kamar FROM jenis_kamar WHERE status_jenis_kamar = 1";
            fillDT(dtJenisKamar, query);
            cbJenisKamar.DataSource = dtJenisKamar;
            cbJenisKamar.DisplayMember = "nama_jenis_kamar";
            cbJenisKamar.ValueMember = "id_jenis_kamar";
        }

        private void loadNomorKamar()
        {
            dtNomorKamar = new DataTable();
            string query = $"SELECT kode_kamar, nomor_kamar FROM kamar WHERE id_jenis_kamar = {Convert.ToInt32(dtJenisKamar.Rows[cbJenisKamar.SelectedIndex][0])} AND (status_kamar = 0 OR status_kamar = 1)";
            fillDT(dtNomorKamar, query);
            cbNomorKamar.DataSource = dtNomorKamar;
            cbNomorKamar.DisplayMember = "nomor_kamar";
            cbNomorKamar.ValueMember = "kode_kamar";
        }

        private void changeMode(int mode = -1)
        {
            inputingData = false;
            dtpJadwalCheckIn.Enabled = false;
            dtpJadwalCheckOut.Enabled = false;
            cbJenisKamar.Enabled = false;
            cbNomorKamar.Enabled = false;

            if (mode == 1)
            {
                // RESERVASI
                inputingData = true;

                cbKodeReservasi.SelectedIndex = -1;
                cbNamaTamu.SelectedIndex = -1;
                cbStatus.SelectedIndex = -1;
                cbCari.SelectedIndex = -1;

                lbKodeReservasi.Text = "-";
                nudJumlahPenghuniKamar.Value = 1;
                dtpJadwalCheckIn.Value = DateTime.Today;
                dtpJadwalCheckOut.Value = DateTime.Today.AddDays(1);
                cbJenisKamar.SelectedIndex = -1;
                cbNomorKamar.SelectedIndex = -1;
                lbDownPayment.Text = "-";
                lbDeposito.Text = "-";
                lbSubtotalBiaya.Text = "-";

                cbReservasiBaru.Checked = false;
                cbReservasiBaru.Enabled = true;
                cbKodeReservasi.Enabled = true;
                nudJumlahPenghuniKamar.Enabled = true;
                dtpJadwalCheckIn.Enabled = true;
                dtpJadwalCheckOut.Enabled = true;
                cbJenisKamar.Enabled = true;
                cbNomorKamar.Enabled = true;

                btReservasi.Enabled = true;
                btBatal.Enabled = false;
            }
            else if (mode == 2)
            {
                // BATAL
                cbReservasiBaru.Enabled = false;
                cbKodeReservasi.Enabled = false;
                nudJumlahPenghuniKamar.Enabled = false;

                btReservasi.Enabled = false;
                btBatal.Enabled = true;
            }
            else
            {
                // NOTHING TO DO
                cbReservasiBaru.Enabled = false;
                cbKodeReservasi.Enabled = false;
                nudJumlahPenghuniKamar.Enabled = false;

                btReservasi.Enabled = false;
                btBatal.Enabled = false;
            }
        }

        private void dgvReservasi_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                fillInput("dgv");
            }
        }

        private void cbStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            refresh_cbCari_dgvDetailReservasi(cbStatus.SelectedIndex);
        }

        private void btBersihkan_Click(object sender, EventArgs e)
        {
            dgvDetailReservasi.ClearSelection();
            changeMode(1);
        }

        private void btCari_Click(object sender, EventArgs e)
        {
            if (cbCari.SelectedIndex > -1)
            {
                fillInput("cari");
            }
        }

        private void fillInput(string asal)
        {
            if (asal == "cari")
            {
                dgvDetailReservasi.Rows[cbCari.SelectedIndex].Selected = true;
            }

            int status = Convert.ToInt32(dgvDetailReservasi.SelectedRows[0].Cells[20].Value);
            if (status == 1)
            {
                // DIPESAN -> BATAL
                changeMode(2);
            }
            else
            {
                // SELESAI / CHECK IN / BATAL -> NOTHING TO DO
                changeMode();
            }

            lbKodeReservasi.Text = dgvDetailReservasi.SelectedRows[0].Cells[1].Value.ToString();
            cbKodeReservasi.SelectedIndex = -1;
            for (int i = 0; i < cbNamaTamu.Items.Count; i++)
            {
                if (dtNamaTamu.Rows[i][1].ToString() == dgvDetailReservasi.SelectedRows[0].Cells[2].Value.ToString())
                {
                    cbNamaTamu.SelectedIndex = i;
                    break;
                }
            }
            nudJumlahPenghuniKamar.Value = Convert.ToInt32(dgvDetailReservasi.SelectedRows[0].Cells[5].Value);
            dtpJadwalCheckIn.Value = (DateTime)dgvDetailReservasi.SelectedRows[0].Cells[11].Value;
            dtpJadwalCheckOut.Value = (DateTime)dgvDetailReservasi.SelectedRows[0].Cells[13].Value;
            for (int i = 0; i < cbJenisKamar.Items.Count; i++)
            {
                if (dgvDetailReservasi.SelectedRows[0].Cells[3].Value.ToString() == dtJenisKamar.Rows[i][1].ToString())
                {
                    cbJenisKamar.SelectedIndex = i;
                    break;
                }
            }
            for (int i = 0; i < cbNomorKamar.Items.Count; i++)
            {
                if (dgvDetailReservasi.SelectedRows[0].Cells[4].Value.ToString() == dtNomorKamar.Rows[i][1].ToString())
                {
                    cbNomorKamar.SelectedIndex = i;
                    break;
                }
            }
            lbDownPayment.Text = $"{dgvDetailReservasi.SelectedRows[0].Cells[6].Value}";
            lbDeposito.Text = $"{dgvDetailReservasi.SelectedRows[0].Cells[8].Value}";
            lbSubtotalBiaya.Text = $"{dgvDetailReservasi.SelectedRows[0].Cells[18].Value}";
        }

        private void btKembali_Click(object sender, EventArgs e)
        {
            Close();
        }

        private bool bisaReservasi()
        {
            DataTable dtListedReservasi = new DataTable();
            //FIND NOMOR KAMAR & STATUS_DETAIL 1 / 2
            string query = $"SELECT jadwal_check_in, jadwal_check_out FROM detail_reservasi dr JOIN kamar k ON k.kode_kamar = dr.kode_kamar WHERE nomor_kamar = {Convert.ToInt32(dtNomorKamar.Rows[cbNomorKamar.SelectedIndex][1])} AND (status_detail_reservasi = 1 OR status_detail_reservasi = 2)";
            fillDT(dtListedReservasi, query);

            if (dtListedReservasi.Rows.Count > 0)
            {
                //CHECK JADWAL CHECK IN DAN JADWAL CHECK OUT
                for (int i = 0; i < dtListedReservasi.Rows.Count; i++)
                {
                    DateTime tempCheckIn = dtpJadwalCheckIn.Value;
                    DateTime tempCheckOut = dtpJadwalCheckOut.Value;
                    while (tempCheckIn < tempCheckOut)
                    {
                        if (tempCheckIn >= (DateTime)dtListedReservasi.Rows[i][0] && tempCheckIn < (DateTime)dtListedReservasi.Rows[i][1])
                        {
                            return false;
                        }
                        if (tempCheckOut > (DateTime)dtListedReservasi.Rows[i][0] && tempCheckOut <= (DateTime)dtListedReservasi.Rows[i][1])
                        {
                            return false;
                        }

                        tempCheckIn = tempCheckIn.AddDays(1);
                        tempCheckOut = tempCheckOut.AddDays(-1);
                        Console.WriteLine(tempCheckIn);
                        Console.WriteLine(tempCheckOut);
                    }
                }
            }

            return true;
        }

        private void btReservasi_Click(object sender, EventArgs e)
        {
            // TIDAK BOLEH ADA DATA KOSONG
            if (cbNamaTamu.SelectedIndex == -1 || cbJenisKamar.SelectedIndex == -1 || cbNomorKamar.SelectedIndex == -1)
            {
                MessageBox.Show("Pastikan semua data terisi dengan benar!", "Gagal");
                return;
            }

            // PENGECEKAN TANGGAL
            if (!bisaReservasi())
            {
                MessageBox.Show("Kamar sudah direservasi/ditempati!", "Gagal");
                return;
            }

            // DIPESAN -> 1
            string query;
            int total_biaya = deposito + dp + subtotal;
            MySqlTransaction trans = Koneksi.conn.BeginTransaction();
            MySqlCommand cmd;
            try
            {
                if (cbReservasiBaru.Checked)
                {
                    // INSERT HEADER
                    query = @"INSERT INTO header_reservasi VALUES (@IdHeader, @KodeReservasi, @KodeTamu, @TotalBiaya, @KodeKaryawan, @StatusHeader)";
                    cmd = new MySqlCommand(query, Koneksi.conn);
                    cmd.Parameters.AddWithValue("@IdHeader", 0);
                    cmd.Parameters.AddWithValue("@KodeReservasi", lbKodeReservasi.Text);
                    cmd.Parameters.AddWithValue("@KodeTamu", dtNamaTamu.Rows[cbNamaTamu.SelectedIndex][0].ToString());
                    cmd.Parameters.AddWithValue("@TotalBiaya", total_biaya);
                    cmd.Parameters.AddWithValue("@KodeKaryawan", FormLogin.dtKaryawan.Rows[0][0].ToString());
                    cmd.Parameters.AddWithValue("@StatusHeader", 1);
                    cmd.ExecuteNonQuery();
                }
                else
                {
                    // CARI total_biaya_reservasi
                    query = @"SELECT total_biaya_reservasi FROM header_reservasi WHERE kode_reservasi = @KodeReservasi";
                    cmd = new MySqlCommand(query, Koneksi.conn);
                    cmd.Parameters.AddWithValue("@KodeReservasi", lbKodeReservasi.Text);
                    int total_biaya_sebelum = Convert.ToInt32(cmd.ExecuteScalar());
                    total_biaya += total_biaya_sebelum;

                    // UPDATE HEADER
                    query = @"UPDATE header_reservasi SET total_biaya_reservasi = @TotalBiaya WHERE kode_reservasi = @KodeReservasi";
                    cmd = new MySqlCommand(query, Koneksi.conn);
                    cmd.Parameters.AddWithValue("@TotalBiaya", total_biaya);
                    cmd.Parameters.AddWithValue("@KodeReservasi", lbKodeReservasi.Text);
                    cmd.ExecuteNonQuery();
                }

                //INSERT DETAIL
                query = @"INSERT INTO detail_reservasi (id_detail_reservasi, kode_reservasi, kode_kamar, jumlah_penghuni_kamar, down_payment, deposito, jadwal_check_in, jadwal_check_out, subtotal_biaya_reservasi, status_detail_reservasi) VALUES (@IdDetail, @KodeReservasi, @KodeKamar, @JumlahPenghuniKamar, @DownPayment, @Deposito, @JadwalCheckIn, @JadwalCheckOut, @SubtotalBiaya, @StatusDetail)";
                cmd = new MySqlCommand(query, Koneksi.conn);
                cmd.Parameters.AddWithValue("@IdDetail", 0);
                cmd.Parameters.AddWithValue("@KodeReservasi", lbKodeReservasi.Text);
                cmd.Parameters.AddWithValue("@KodeKamar", dtNomorKamar.Rows[cbNomorKamar.SelectedIndex][0].ToString());
                cmd.Parameters.AddWithValue("@JumlahPenghuniKamar", Convert.ToInt32(nudJumlahPenghuniKamar.Value));
                cmd.Parameters.AddWithValue("@DownPayment", dp);
                cmd.Parameters.AddWithValue("@Deposito", deposito);
                cmd.Parameters.AddWithValue("@JadwalCheckIn", dtpJadwalCheckIn.Value.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@JadwalCheckOut", dtpJadwalCheckOut.Value.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@SubtotalBiaya", subtotal);
                cmd.Parameters.AddWithValue("@StatusDetail", 1);
                cmd.ExecuteNonQuery();

                trans.Commit();
                MessageBox.Show("Berhasil melakukan reservasi!", "Berhasil");

                refresh_cbCari_dgvDetailReservasi();
                changeMode(1);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                Console.WriteLine(ex.Message);
                MessageBox.Show("Gagal melakukan reservasi!", "Gagal");
            }
        }

        private void btBatal_Click(object sender, EventArgs e)
        {
            createTriggerCheckStatus();

            // BATAL -> 99
            string query = @"UPDATE detail_reservasi SET status_detail_reservasi = 99 WHERE kode_reservasi = @KodeReservasi";
            MySqlTransaction trans = Koneksi.conn.BeginTransaction();
            try
            {
                MySqlCommand cmd = new MySqlCommand(query, Koneksi.conn);
                cmd.Parameters.AddWithValue("@KodeReservasi", lbKodeReservasi.Text);
                cmd.ExecuteNonQuery();

                trans.Commit();
                MessageBox.Show("Berhasil membatalkan reservasi!", "Berhasil");

                refresh_cbCari_dgvDetailReservasi();
                changeMode(1);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                Console.WriteLine(ex.Message);
                MessageBox.Show("Gagal membatalkan reservasi!", "Gagal");
            }
        }

        private void createTriggerCheckStatus()
        {
            // ADD TRIGGER
            MySqlCommand cmd = new MySqlCommand();
            try
            {
                cmd.Connection = Koneksi.conn;
                cmd.CommandText = @"
                     CREATE OR REPLACE TRIGGER update_status_totalBiaya_header_reservasi
                     AFTER UPDATE ON detail_reservasi FOR EACH ROW
                     BEGIN
	                     DECLARE jumlah INT DEFAULT 0;
	                     DECLARE total_biaya INT;
	
	                     SELECT total_biaya_reservasi INTO total_biaya FROM header_reservasi WHERE kode_reservasi = NEW.kode_reservasi;
	                     SET total_biaya = total_biaya - (NEW.subtotal_biaya_reservasi + NEW.deposito + (0.5 * NEW.down_payment));
	                     UPDATE header_reservasi SET total_biaya_reservasi = total_biaya WHERE kode_reservasi = NEW.kode_reservasi;
	
	                     # JIKA SEMUA STATUS DETAIL = 99 MAKA STATUS HEADER 99
	                     SELECT COUNT(*) INTO jumlah FROM detail_reservasi WHERE status_detail_reservasi != 99 AND kode_reservasi = NEW.kode_reservasi;
	
	                     IF (jumlah = 0) THEN
		                     UPDATE header_reservasi SET status_header_reservasi = 99 WHERE kode_reservasi = NEW.kode_reservasi;
	                     END IF;
                     END";
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void dtpJadwalCheckIn_ValueChanged(object sender, EventArgs e)
        {
            if (inputingData)
            {
                if (dtpJadwalCheckIn.Value < DateTime.Today)
                {
                    MessageBox.Show("Jadwal check-in tidak boleh kurang dari jadwal saat ini!", "Gagal");
                    dtpJadwalCheckIn.Value = DateTime.Today;
                }
                if (dtpJadwalCheckOut.Value <= dtpJadwalCheckIn.Value)
                {
                    dtpJadwalCheckOut.Value = dtpJadwalCheckIn.Value.AddDays(1);
                }
            }
        }

        private void dtpJadwalCheckOut_ValueChanged(object sender, EventArgs e)
        {
            if (inputingData)
            {
                if (dtpJadwalCheckOut.Value <= dtpJadwalCheckIn.Value)
                {
                    MessageBox.Show("Jadwal check-out tidak boleh kurang dari sama dengan jadwal check-in!", "Gagal");
                    dtpJadwalCheckOut.Value = dtpJadwalCheckIn.Value.AddDays(1);
                }
            }
        }

        private void btTambahTamu_Click(object sender, EventArgs e)
        {
            Hide();
            FormPencatatanDataTamu form = new FormPencatatanDataTamu();
            form.ShowDialog();
            loadNamaTamu();
            Show();
        }

        private void generateKodeReservasi()
        {
            // ADD & EXECUTE PROCEDURE
            MySqlCommand cmd = new MySqlCommand();
            try
            {
                cmd.Connection = Koneksi.conn;
                cmd.CommandText = @"
                    CREATE OR REPLACE PROCEDURE generateKodeTamu(IN nama VARCHAR(255), IN nomor INT, OUT kode VARCHAR(15))
                    BEGIN
	                    SET kode = CONCAT('RSV', DATE_FORMAT(NOW(), '%d%m%y'));

                            IF INSTR(nama, ' ') = 0 THEN
                                    SET kode = CONCAT(kode, SUBSTR(nama, 1, 2));
                            ELSE
                                    SET kode = CONCAT(kode, CONCAT(SUBSTR(nama, 1, 1), SUBSTR(nama, INSTR(nama, ' ') + 1, 1)));
                            END IF;
        
                            SET kode = UPPER(CONCAT(kode, LPAD(nomor, 4, '0')));
                    END";
                cmd.ExecuteNonQuery();

                cmd.CommandText = "generateKodeTamu";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@nama", dtNamaTamu.Rows[cbNamaTamu.SelectedIndex][1].ToString());
                cmd.Parameters["@nama"].Direction = ParameterDirection.Input;

                cmd.Parameters.AddWithValue("@nomor", dtNomorKamar.Rows[cbNomorKamar.SelectedIndex][1].ToString());
                cmd.Parameters["@nomor"].Direction = ParameterDirection.Input;

                cmd.Parameters.Add("@kode", MySqlDbType.VarChar);
                cmd.Parameters["@kode"].Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();

                lbKodeReservasi.Text = cmd.Parameters["@kode"].Value.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void generate_DownPayment_Deposito()
        {
            dp = (int)(Convert.ToInt32(dtJenisKamar.Rows[cbJenisKamar.SelectedIndex][2]) * 0.5);
            deposito = 500000;
            subtotal = 0;

            lbDownPayment.Text = $"Rp {string.Format("{0:#,0}", dp).Replace(",", ".")}";
            lbDeposito.Text = $"Rp {string.Format("{0:#,0}", deposito).Replace(",", ".")}";
            lbSubtotalBiaya.Text = $"Rp {string.Format("{0:#,0}", subtotal).Replace(",", ".")}";
        }

        private void cbNamaTamu_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbNamaTamu.SelectedIndex > -1 && cbReservasiBaru.Checked && cbNomorKamar.SelectedIndex > -1)
            {
                generateKodeReservasi();
            }
        }

        private void cbJenisKamar_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbJenisKamar.SelectedIndex > -1)
            {
                loadNomorKamar();
            }
            else
            {
                cbNomorKamar.DataSource = null;
                cbNomorKamar.Items.Clear();
                cbNomorKamar.Items.Add("");
            }
        }

        private void cbNomorKamar_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbNomorKamar.Items[0].ToString() != "" && cbNomorKamar.SelectedIndex > -1)
            {
                generate_DownPayment_Deposito();

                if (cbNamaTamu.SelectedIndex > -1 && cbReservasiBaru.Checked)
                {
                    generateKodeReservasi();
                }
            }
        }

        private void cbReservasiBaru_CheckedChanged(object sender, EventArgs e)
        {
            lbKodeReservasi.Text = "-";
            cbKodeReservasi.Enabled = true;
            cbKodeReservasi.SelectedIndex = -1;
            cbNamaTamu.SelectedIndex = -1;
            cbNamaTamu.Enabled = false;
            if (cbReservasiBaru.Checked)
            {
                cbKodeReservasi.Enabled = false;
                cbNamaTamu.Enabled = true;
            }
        }

        private void cbKodeReservasi_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbKodeReservasi.SelectedIndex > -1)
            {
                lbKodeReservasi.Text = dtAvailableKodeReservasi.Rows[cbKodeReservasi.SelectedIndex][1].ToString();
                for (int i = 0; i < cbNamaTamu.Items.Count; i++)
                {
                    if (dtNamaTamu.Rows[i][1].ToString() == dtAvailableKodeReservasi.Rows[cbKodeReservasi.SelectedIndex][2].ToString())
                    {
                        cbNamaTamu.SelectedIndex = i;
                        break;
                    }
                }
            }
        }
    }
}
