﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CRUD.Baru;
using System.Data.SqlClient;

namespace CRUD
{
    public partial class UCMasuk : UserControl
    {
        private String nama;
        private String id;
        public UCMasuk()
        {
            InitializeComponent();
        }
        private void btnMasuk_Click(object sender, EventArgs e)
        {
            if (!getAkun())
            {
                MessageBox.Show("Username atau password tidak dapat ditemukan","",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                return;
            }
            Menu_Utama home = new Menu_Utama(nama, id);
            this.Visible = false;
            home.ShowDialog(this);
            this.Visible = true;

        }
        private bool getAkun()
        {
            try
            {
                string connectionString = "integrated security = true; data source = localhost; initial catalog = SakuraData";

                SqlConnection connection = new SqlConnection(connectionString);
                SqlCommand myCommand = new SqlCommand("sp_cariAkunPelayan", connection);


                myCommand.CommandType = CommandType.StoredProcedure;

                connection.Open();


                myCommand.Parameters.AddWithValue("@username", txtUsername.Text);
                myCommand.Parameters.AddWithValue("@password", txtPassword.Text);

                SqlDataAdapter adapter = new SqlDataAdapter(myCommand);
                DataTable data = new DataTable();
                adapter.Fill(data);

                nama = data.Rows[0][1].ToString();
                id = data.Rows[0][0].ToString();
                MessageBox.Show(nama);
                if (!nama.Equals(""))
                {
                    return true;
                }
                return false;

                //by = 1;

                //btnUpdate.Enabled = true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Error getAkun : " + ex.ToString());

                return false;
                //btnUpdate.Enabled = false;
                //clear();
            }
        }

        private void btnLupa_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Fungsi belum tersedia!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}