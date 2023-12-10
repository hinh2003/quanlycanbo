using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace BaiTapLon
{
    public partial class capbac : Form
    {
        string conn = "Server=DESKTOP-M9OV124\\HINH;Database=QuanLyCanBo;Integrated Security=SSPI;";
        SqlConnection sqlConnection;
        public UpdateDataDelegate UpdateDataCallback;
        public FormMain formMain;


        public capbac(FormMain mainForm)
        {
            InitializeComponent();
            formMain = mainForm; // Gán giá trị từ tham số truyền vào cho biến formMain
            sqlConnection = new SqlConnection(conn);
        }
        public void hienthi()
        {
            try
            {
                if (sqlConnection.State == ConnectionState.Closed)
                {
                    sqlConnection.Open();
                }

                string sql = "SELECT capbac AS CapBac , capbac_id AS capbac_id  FROM capbac";

                SqlCommand command = new SqlCommand(sql, sqlConnection);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                if (dataTable.Rows.Count > 0)
                {
                    dataGridView1.DataSource = dataTable;
                    dataGridView1.Columns["capbac_id"].Visible = false;

                }

                if (sqlConnection.State == ConnectionState.Open)
                {
                    sqlConnection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void donvi_Load(object sender, EventArgs e)
        {
            hienthi();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (sqlConnection.State == ConnectionState.Closed)
                {
                    sqlConnection.Open();
                }            
                string capbac = textBox1.Text;
                if (!string.IsNullOrEmpty(capbac)) { 
                string sql = "INSERT INTO capbac(capbac) VALUES (@capbac)";
                using(SqlCommand command = new SqlCommand(sql,sqlConnection))
                {
                    command.Parameters.AddWithValue("@capbac", capbac); 
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Thêm dữ liệu thành công");
                        hienthi();
                    }
                    else
                    {
                        MessageBox.Show("Có lỗi xảy ra");
                    }
                }
            }
                else
                {
                    MessageBox.Show("Ky tu khong hop le");
                }
            }
            catch ( Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }
        int capbacid;
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        { 

        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow selectedRow = dataGridView1.Rows[e.RowIndex];

                DisplayCellContent(selectedRow, "CapBac", textBox1);
                if (dataGridView1.Columns.Contains("capbac_id"))
                {
                    object capbacidObj = selectedRow.Cells["capbac_id"].Value;
                    capbacid = capbacidObj != DBNull.Value ? Convert.ToInt32(capbacidObj) : 0;
                }
            }
        }

        private void DisplayCellContent(DataGridViewRow row, string columnName, Control control)
        {
            if (row.Cells[columnName].Value != DBNull.Value)
            {
                control.Text = row.Cells[columnName].Value.ToString();
            }
            else
            {
                control.Text = string.Empty;
            }
        }
       
        private void donvi_FormClosed(object sender, FormClosedEventArgs e)
        {
         formMain.UpdateDonViComboBox();
         formMain.UpdateDatagrid();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (sqlConnection.State == ConnectionState.Closed)
                {
                    sqlConnection.Open();
                }
                string capbacMoi = textBox1.Text;
                string sql = "UPDATE capbac SET capbac = @capbac WHERE capbac_id = @capbacid ";
                SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@capbac", capbacMoi);
                sqlCommand.Parameters.AddWithValue("@capbacid", capbacid);
                int rowsAffected = sqlCommand.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Cập nhập thành công");
                    textBox1.Text = "";
                    hienthi();

                }
                else
                {
                    MessageBox.Show("Không có gì được cập nhật.");
                }
            }
            catch (Exception ex) { 
                MessageBox.Show("Lỗi "+ex.Message);
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {

            try
            {
                if (sqlConnection.State == ConnectionState.Closed)
                {
                    sqlConnection.Open();
                }
                string sql = "DELETE FROM capbac WHERE capbac_id = @capbac_id";
                SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@capbac_id", capbacid);
                int rowsAffected = sqlCommand.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Xoa thành công!");

                    hienthi();


                }
                else
                {
                    MessageBox.Show("Không có gì được Xoa.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);

            }
            finally
            {
                if (sqlConnection.State == ConnectionState.Open)
                {
                    sqlConnection.Close();
                }
            }
        }

  
    }
}
