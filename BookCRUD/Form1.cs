using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace BookCRUD
{
    public partial class Form1 : Form
    {
        // w odwolaniach dodalem mysql.data.dll, dodalem biblioteke do using. 
        // aby polaczyc sie z baza wystarczy utworzyc connection string wg. wzoru ponizej 
        // i wykorzystywac go do polaczenia sie przy pomocy klasy mysqlconnection
        string connectionString = @"Server=localhost;Database=bookdb;Uid=root;Pwd=;";
        int book_id = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            using (MySqlConnection mysqlCon = new MySqlConnection(connectionString))
            {
                mysqlCon.Open();
                MySqlDataAdapter sqlDa = new MySqlDataAdapter("book_search_by_value", mysqlCon);
                sqlDa.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDa.SelectCommand.Parameters.AddWithValue("_search_value", txtSearch.Text);
                DataTable dtblBook = new DataTable();
                sqlDa.Fill(dtblBook);
                dgvBook.DataSource = dtblBook;
                dgvBook.Columns[0].Visible = false;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            using (MySqlConnection mysqlCon = new MySqlConnection(connectionString))
            {
                mysqlCon.Open();
                MySqlCommand mySqlCmd = new MySqlCommand("book_add_or_edit", mysqlCon);
                mySqlCmd.CommandType = CommandType.StoredProcedure;
                mySqlCmd.Parameters.AddWithValue("_book_id", book_id);
                mySqlCmd.Parameters.AddWithValue("_book_name", txtName.Text.Trim());
                mySqlCmd.Parameters.AddWithValue("_author", txtAuthor.Text.Trim());
                mySqlCmd.Parameters.AddWithValue("_description", txtDescription.Text.Trim());
                mySqlCmd.ExecuteNonQuery();
                MessageBox.Show("Submitted Sucessfully");
                Clear();
                GridFill();
            }
        }

        void GridFill()
        {
            using (MySqlConnection mysqlCon = new MySqlConnection(connectionString))
            {
                mysqlCon.Open();
                MySqlDataAdapter sqlDa = new MySqlDataAdapter("book_view_all", mysqlCon);
                sqlDa.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dtblBook = new DataTable();
                sqlDa.Fill(dtblBook);
                dgvBook.DataSource = dtblBook;
                dgvBook.Columns[0].Visible = false;
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            Clear();
            GridFill();
        }
        void Clear()
        {
            txtName.Text = txtAuthor.Text = txtDescription.Text = txtSearch.Text = "";
            book_id = 0;
            btnSave.Text = "Save";
            btnDelete.Enabled = false;
        }
        private void dgvBook_DoubleClick(object sender, EventArgs e)
        {
            if (dgvBook.CurrentRow.Index != -1)
            {
                txtName.Text = dgvBook.CurrentRow.Cells[1].Value.ToString();
                txtAuthor.Text = dgvBook.CurrentRow.Cells[1].Value.ToString();
                txtDescription.Text = dgvBook.CurrentRow.Cells[1].Value.ToString();
                book_id = Convert.ToInt32(dgvBook.CurrentRow.Cells[0].Value.ToString());
                btnSave.Text = "Update";
                btnDelete.Enabled = true;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            using (MySqlConnection mysqlCon = new MySqlConnection(connectionString))
            {
                mysqlCon.Open();
                MySqlCommand mySqlCmd = new MySqlCommand("book_delete_by_id", mysqlCon);
                mySqlCmd.CommandType = CommandType.StoredProcedure;
                mySqlCmd.Parameters.AddWithValue("_book_id", book_id);
                mySqlCmd.ExecuteNonQuery();
                MessageBox.Show("Deleted Sucessfully");
                Clear();
                GridFill();
            }
        }
    }
}
