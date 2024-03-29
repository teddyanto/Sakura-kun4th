﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CRUD
{
    public partial class UpdateKomponen : Form
    {
        private Boolean cek = false;
        private string idAlat;

        public UpdateKomponen()
        {
            InitializeComponent();
            txtNama.MaxLength = 30;
            txtAlat.MaxLength = 30;
            txtTempat.MaxLength = 4;
            addSourceData();
            addSourceAlat();
            clear();
        }

        private void addSourceData()
        {
            dgKomponen.Rows.Clear();

            //punya fio
            //string connectionString = ConfigurationSettings.AppSettings["constring1"];
            //punya teddy
            ////punya saya
            string connectionString = Program.getConstring();

            //SqlConnection connection = new SqlConnection(connectionString);

            ////membuat table dengan jumlah data saja
            //SqlDataAdapter adapter = new SqlDataAdapter("select nama_alat from msalatelektronik", connection);
            ////memasukkan ke dataset
            //DataSet msalat = new DataSet();
            //adapter.Fill(msalat);

            //for (int i = 0; i < msalat.Tables[0].Rows.Count; i++)
            //{
            //    collection.Add(msalat.Tables[0].Rows[i][0].ToString());
            //}
            //txtAlat.AutoCompleteCustomSource = collection;
            //txtAlat.AutoCompleteMode = AutoCompleteMode.Suggest;
            //txtAlat.AutoCompleteSource = AutoCompleteSource.CustomSource;
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand myCommand = new SqlCommand("sp_getAllUpdateKomponen", connection);
            myCommand.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter adapter = new SqlDataAdapter();

            DataTable data = new DataTable();
            adapter.SelectCommand = myCommand;
            adapter.Fill(data);

            DataTable dtCloned = data.Clone();
            dtCloned.Columns[3].DataType = typeof(String);
            foreach (DataRow row in data.Rows)
            {
                dtCloned.ImportRow(row);
            }

            //dgKomponen.DataSource = dtCloned;

            for(int i = 0; i < dtCloned.Rows.Count; i++)
            {
                string uang = toRupiah(Convert.ToInt32(dtCloned.Rows[i][3].ToString().Substring(0, dtCloned.Rows[i][3].ToString().Length-5)));
                Object[] n = { (i + 1) + ".", dtCloned.Rows[i][0].ToString(), dtCloned.Rows[i][1].ToString(), dtCloned.Rows[i][2].ToString(), uang, dtCloned.Rows[i][4].ToString(), dtCloned.Rows[i][5].ToString(), };
                dgKomponen.Rows.Add(n);
            }

            foreach (DataGridViewColumn col in dgKomponen.Columns)
            {
                col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                //    col.HeaderCell.Style.Font = new Font("Arial", 9F, FontStyle.Bold, GraphicsUnit.Pixel);
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            this.dgKomponen.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.dgKomponen.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.dgKomponen.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dgKomponen.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells; 
            this.dgKomponen.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.dgKomponen.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dgKomponen.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;

            this.dgKomponen.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            
            //by = 1;

            connection.Close();

        }
        private void addSourceAlat()
        {
            txtAlat.AutoCompleteCustomSource = null;

            AutoCompleteStringCollection collection = new AutoCompleteStringCollection();

            string connectionString = Program.getConstring();

            SqlConnection connection = new SqlConnection(connectionString);

            //membuat table dengan jumlah data saja
            SqlDataAdapter adapter = new SqlDataAdapter("select nama_alat from msalatelektronik", connection);
            //memasukkan ke dataset
            DataSet msdata = new DataSet();
            adapter.Fill(msdata);

            for (int i = 0; i < msdata.Tables[0].Rows.Count; i++)
            {

                collection.Add(msdata.Tables[0].Rows[i][0].ToString());
            }
            txtAlat.AutoCompleteCustomSource = collection;
            txtAlat.AutoCompleteMode = AutoCompleteMode.Suggest;
            txtAlat.AutoCompleteSource = AutoCompleteSource.CustomSource;
        }
        public string toRupiah(Int32 angka)
        {
            return String.Format(CultureInfo.CreateSpecificCulture("id-id"), "{0:N}", angka);
        }

        public int toAngka(string rupiah)
        {
            return int.Parse(Regex.Replace(rupiah, @",.*|\D", ""));
        }
        private void UpdateAlatElektronik_Load(object sender, EventArgs e)
        {
            
        }

        private void btnKembali_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPerbarui_Click(object sender, EventArgs e)
        {
            btnBatal.Enabled = true;
            btnPerbarui.Enabled = false;
            gbUpdate.Enabled = true;
        }

        private void btnBatal_Click(object sender, EventArgs e)
        {
            clear();
        }

        private void clear()
        {
            cek = false;
            //cmbPencarian.SelectedItem = null;
            //cmbPencarian.Text = "--Pilih--";
            //txtPencarian.Text = "";
            lblId.Text = "";
            txtNama.Text = "";
            txtJumlah.Text = "";
            txtHarga.Text = "";
            txtAlat.Text = "";
            txtTempat.Text = "";
            gbUpdate.Enabled = false;
            btnPerbarui.Enabled = false;
            btnBatal.Enabled = false;

            refresh();
        }

        private void btnSegarkan_Click(object sender, EventArgs e)
        {
            refresh();
        }

        private void refresh()
        {
            addSourceData();
        }

        private string converterAlat(String search, Boolean refresh)
        {
            //MessageBox.Show(search.ToString());
            string nama_alat = null;
            try
            {
                //string connectionString = ConfigurationSettings.AppSettings["constring1"];
                //punya teddy
                string connectionString = Program.getConstring();

                SqlConnection connection = new SqlConnection(connectionString);
                SqlCommand myCommand = new SqlCommand("sp_cariAlatElektronik", connection);

                myCommand.CommandType = CommandType.StoredProcedure;

                connection.Open();

                if (refresh)
                {
                    myCommand.Parameters.AddWithValue("id_alat", search);
                }
                else
                {
                    myCommand.Parameters.AddWithValue("id_alat", "xxxx");
                }
                myCommand.Parameters.AddWithValue("nama_alat", search);
                myCommand.Parameters.AddWithValue("id_jenis", search);

                SqlDataAdapter adapter = new SqlDataAdapter(myCommand);
                DataTable data = new DataTable();
                adapter.Fill(data);

                nama_alat = data.Rows[0][1].ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return nama_alat;
        }
        private string getIDAlat()
        {
            //MessageBox.Show(search.ToString());
            string nama_alat = null;
            try
            {
                //string connectionString = ConfigurationSettings.AppSettings["constring1"];
                //punya teddy
                string connectionString = Program.getConstring();

                SqlConnection connection = new SqlConnection(connectionString);
                SqlCommand myCommand = new SqlCommand("sp_cariAlatElektronik", connection);

                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.Parameters.AddWithValue("id_alat", "XXX");
                connection.Open();

                myCommand.Parameters.AddWithValue("nama_alat", txtAlat.Text);
                myCommand.Parameters.AddWithValue("id_jenis", "XXX");

                SqlDataAdapter adapter = new SqlDataAdapter(myCommand);
                DataTable data = new DataTable();
                adapter.Fill(data);

                nama_alat = data.Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return nama_alat;
        }
        private void txtJumlah_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void txtHarga_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

        }
        
        private void btnSimpan_Click(object sender, EventArgs e)
        {
            string checker = null;

            if (txtNama.Text == "")
            {
                cek = false;
                checker += "Kolom nama komponen harap diisi\n";
                MessageBox.Show(checker, "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                cek = true;
            }

            if (txtJumlah.Text == "")
            {
                cek = false;
                checker += "Kolom jumlah harap diisi\n"; MessageBox.Show(checker, "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Warning); return;
            }
            else
            {
                cek = true;
            }

            if (txtHarga.Text == "")
            {
                checker += "Kolom harga jual harap diisi\n";
                MessageBox.Show(checker, "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cek = false; return;
            }
            else
            {
                cek = true;
            }

            if (txtAlat.Text == "")
            {
                checker += "Kolom alat elektronik harap diisi\n";
                MessageBox.Show(checker, "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cek = false; return;
            }
            else
            {
                cek = true;
            }

            if (txtTempat.Text == "")
            {
                checker += "Kolom tempat harap diisi\n"; MessageBox.Show(checker, "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cek = false; return;
            }
            else
            {
                cek = true;
            }

            if (cek)
            {
                DialogResult result = MessageBox.Show("Periksa data sebelum disimpan\n" +
                   "ID Komponen\t\t: " + lblId.Text + "\n" +
                   "Nama Komponen\t\t: " + txtNama.Text + "\n" +
                   "Jumlah\t\t\t: " + txtJumlah.Text + "\n" +
                   "Harga Jual\t\t: " + txtHarga.Text + "\n" +
                   "Komponen Alat Elektronik\t: " + txtAlat.Text + "\n" +
                   "Tempat\t\t\t: " + txtTempat.Text
                   , "Informasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    updateDB();
                }
                else
                {
                    MessageBox.Show("BATAL!", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show(checker, "Data tidak valid!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void updateDB()
        {
            try
            {
                //string connectionString = ConfigurationSettings.AppSettings["constring1"];
                //punya teddy
                string connectionString = Program.getConstring();

                SqlConnection myConnection = new SqlConnection(connectionString);

                myConnection.Open();

                SqlCommand myCommand = new SqlCommand("sp_updateKomponen", myConnection);
                myCommand.CommandType = CommandType.StoredProcedure;

                myCommand.Parameters.AddWithValue("id_komponen", lblId.Text);
                myCommand.Parameters.AddWithValue("nama_komponen", txtNama.Text);
                myCommand.Parameters.AddWithValue("jumlah", int.Parse(txtJumlah.Text));
                myCommand.Parameters.AddWithValue("harga_jual", toAngka(txtHarga.Text));
                myCommand.Parameters.AddWithValue("id_alat", getIDAlat());
                myCommand.Parameters.AddWithValue("tempat", txtTempat.Text);

                myCommand.ExecuteNonQuery();
                myConnection.Close();
                MessageBox.Show("Data berhasil diperbarui!", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                clear();
                btnPerbarui.Enabled = false;
                gbUpdate.Enabled = false;
                btnBatal.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmbPencarian_TextChanged(object sender, EventArgs e)
        {
            //switch (cmbPencarian.Text)
            //{
            //    case "ID Komponen": sourceFilter("id_komponen"); break;
            //    case "Nama Komponen": sourceFilter("nama_komponen"); break;
            //    case "Jumlah": sourceFilter("jumlah"); break;
            //    case "Harga Jual": sourceFilter("harga_jual"); break;
            //    case "Alat Elektronik": sourceFilter("id_alat"); break;
            //    case "Tempat": sourceFilter("tempat"); break;
            //}
        }

        private void sourceFilter(string filter)
        {
            AutoCompleteStringCollection collection = new AutoCompleteStringCollection();
            
            //punya fio
            //string connectionString = ConfigurationSettings.AppSettings["constring1"];
            //punya teddy
            string connectionString = Program.getConstring();

            SqlConnection connection = new SqlConnection(connectionString);

            //membuat table dengan jumlah data saja
            SqlDataAdapter adapter = adapter = new SqlDataAdapter("select " + filter + " from mskomponen", connection);
            //memasukkan ke dataset
            DataSet msalat = new DataSet();
            adapter.Fill(msalat);

            for (int i = 0; i < msalat.Tables[0].Rows.Count; i++)
            {
                collection.Add(msalat.Tables[0].Rows[i][0].ToString());
            }
            //txtPencarian.AutoCompleteCustomSource = collection;
            //txtPencarian.AutoCompleteMode = AutoCompleteMode.Suggest;
            //txtPencarian.AutoCompleteSource = AutoCompleteSource.CustomSource;
        }

        private void txtPencarian_TextChanged(object sender, EventArgs e)
        {
            try
            {
                //string connectionString = ConfigurationSettings.AppSettings["constring1"];
                //punya teddy
                string connectionString = Program.getConstring();

                SqlConnection connection = new SqlConnection(connectionString);

                connection.Open();

                SqlCommand myCommand = new SqlCommand("sp_cariKomponen", connection);
                myCommand.CommandType = CommandType.StoredProcedure;

                string pencarian = "";

                myCommand.Parameters.AddWithValue("id_komponen", pencarian);
                myCommand.Parameters.AddWithValue("nama_komponen", pencarian);
                if (int.TryParse(pencarian, out var value))
                {
                    myCommand.Parameters.AddWithValue("jumlah", value);
                }
                else
                {
                    myCommand.Parameters.AddWithValue("jumlah", value);
                }

                if (int.TryParse(pencarian, out value))
                {
                    myCommand.Parameters.AddWithValue("harga_jual", value);
                }
                else
                {
                    myCommand.Parameters.AddWithValue("harga_jual", value);
                }
                myCommand.Parameters.AddWithValue("id_alat", pencarian);
                myCommand.Parameters.AddWithValue("tempat", pencarian);

                SqlDataAdapter adapter = new SqlDataAdapter(myCommand);
                DataTable data = new DataTable();
                adapter.Fill(data);

                lblId.Text = data.Rows[0][0].ToString();
                txtNama.Text = data.Rows[0][1].ToString();
                txtJumlah.Text = data.Rows[0][2].ToString();
                txtHarga.Text = data.Rows[0][3].ToString();
                txtAlat.Text = converterAlat(data.Rows[0][4].ToString(),false);
                txtTempat.Text = data.Rows[0][5].ToString();

                connection.Close();
                btnPerbarui.Enabled = true;
            }
            catch (Exception ex)
            {
                btnPerbarui.Enabled = false;
                refresh();
                gbUpdate.Enabled = false;
                btnBatal.Enabled = false;
            }
        }

        private void txtAlat_TextChanged(object sender, EventArgs e)
        {
            try
            {
                //string connectionString = ConfigurationSettings.AppSettings["constring1"];
                //punya teddy
                string connectionString = Program.getConstring();

                SqlConnection connection = new SqlConnection(connectionString);
                SqlCommand myCommand = new SqlCommand("sp_cariAlatElektronik", connection);


                myCommand.CommandType = CommandType.StoredProcedure;

                connection.Open();
                string pencarian = txtAlat.Text;

                myCommand.Parameters.AddWithValue("id_alat", pencarian);
                myCommand.Parameters.AddWithValue("nama_alat", pencarian);
                myCommand.Parameters.AddWithValue("id_jenis", pencarian);

                SqlDataAdapter adapter = new SqlDataAdapter(myCommand);
                DataTable data = new DataTable();
                adapter.Fill(data);

                idAlat = "";
                idAlat = data.Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                cek = false;
            }
        }

        private void btnBaru_Click(object sender, EventArgs e)
        {
            GenericIdentity myIdentity = new GenericIdentity(txtAlat.Text);
            string[] myRole = new string[10];

            GenericPrincipal myPrincipal = new GenericPrincipal(myIdentity, myRole);

            Thread.CurrentPrincipal = myPrincipal;

            AlatElektronik alat = new AlatElektronik();

            alat.ShowDialog(this);
            addSourceAlat();
        }

        private void dgKomponen_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            //try
            //{
            //    gbUpdate.Enabled = false;
            //    btnBatal.Enabled = false;
            //    int selectedRow = 0;
            //    Int32 selectedRowCount = dgKomponen.Rows.GetRowCount(DataGridViewElementStates.Selected);
            //    if (selectedRowCount > 0 && selectedRowCount <= 1)
            //    {
            //        btnPerbarui.Enabled = true;

            //        for (int i = 0; i < selectedRowCount; i++)
            //        {
            //            selectedRow = Int32.Parse(dgKomponen.SelectedRows[i].Index.ToString());
            //        }
            //        DataGridViewRow row = dgKomponen.Rows[selectedRow];

            //        lblId.Text = row.Cells[0].Value.ToString();
            //        txtNama.Text = row.Cells[1].Value.ToString();
            //        txtJumlah.Text = row.Cells[2].Value.ToString();
            //        txtHarga.Text = row.Cells[3].Value.ToString();
            //        txtAlat.Text = converterAlat(row.Cells[4].Value.ToString(),false);
            //        txtTempat.Text = row.Cells[5].Value.ToString();

            //        btnPerbarui.Enabled = true;
            //    }
            //    else
            //    {
            //        btnPerbarui.Enabled = false;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("Data tidak valid", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    btnPerbarui.Enabled = false;
            //}
        }

        private void dgKomponen_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int i = e.RowIndex;
            if(i == -1)
            {
                btnPerbarui.Enabled = false;
                return;
            }
            btnPerbarui.Enabled = true;
            dgKomponen.Rows[i].Selected = true;
            DataGridViewRow row = new DataGridViewRow();
            row = dgKomponen.CurrentRow;

            lblId.Text = row.Cells[1].Value.ToString();
            txtNama.Text = row.Cells[2].Value.ToString();
            txtJumlah.Text = row.Cells[3].Value.ToString();
            txtHarga.Text = row.Cells[4].Value.ToString();
            txtAlat.Text = row.Cells[5].Value.ToString();
            txtTempat.Text = row.Cells[6].Value.ToString();
        }
    }
}
