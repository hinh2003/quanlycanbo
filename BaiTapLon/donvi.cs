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
    public partial class donvi : Form
    {
        string conn = "Server=DESKTOP-M9OV124\\HINH;Database=QuanLyCanBo;Integrated Security=SSPI;";
        SqlConnection sqlConnection;
        public UpdateDataDelegate UpdateDataCallback;
        public FormMain formMain;
        public donvi(FormMain mainForm)
        {
            InitializeComponent();
            formMain = mainForm;
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

                string sql = "SELECT donvi_ten AS DonVi, donvi_id AS donvi_id, mldv.mucluong AS MucLuong " +
                             "FROM donvi " +
                             "JOIN mucluongdonvi mldv ON donvi.mucluong_id = mldv.mucluong_id";


                SqlCommand command = new SqlCommand(sql, sqlConnection);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                if (dataTable.Rows.Count > 0)
                {
                    dataGridView1.DataSource = dataTable;
                    dataGridView1.Columns["donvi_id"].Visible = false;

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
        private void show()
        {
            sqlConnection.Open();
            string sqlmucluong = "SELECT mucluong FROM mucluongdonvi";
            SqlCommand commandCapBac = new SqlCommand(sqlmucluong, sqlConnection);
            SqlDataReader readerCapBac = commandCapBac.ExecuteReader();

            while (readerCapBac.Read())
            {
                comboBox1.Items.Add(readerCapBac["mucluong"].ToString());
            }

            readerCapBac.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (sqlConnection.State == ConnectionState.Closed)
                {
                    sqlConnection.Open();
                }
                string donvi = textBox1.Text;
                int mucluong;
                if (!int.TryParse(comboBox1.Text, out mucluong))
                {
                    MessageBox.Show("Vui lòng nhập số cho mức lương.");
                    return;
                }

                if (!string.IsNullOrEmpty(donvi))
                {
                    string sql = "INSERT INTO donvi(donvi_ten, mucluong_id) " +
                                 "VALUES (@donvi, (SELECT mucluong_id FROM mucluongdonvi WHERE mucluong = @mucluong))";

                    using (SqlCommand command = new SqlCommand(sql, sqlConnection))
                    {
                        command.Parameters.AddWithValue("@donvi", donvi);
                        command.Parameters.AddWithValue("@mucluong", mucluong);
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
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void donvi_Load(object sender, EventArgs e)
        {
            hienthi();
            show();
        }

        private void donvi_FormClosed(object sender, FormClosedEventArgs e)
        {
            formMain.UpdateDonViComboBox();
        }
        int donvi_id;

        private void button2_Click(object sender, EventArgs e)
        {

            try
            {
                if (sqlConnection.State == ConnectionState.Closed)
                {
                    sqlConnection.Open();
                }
                string sql = "DELETE FROM donvi WHERE donvi_id = @donvi_id";
                SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@donvi_id", donvi_id);
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

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow selectedRow = dataGridView1.Rows[e.RowIndex];

                formMain.DisplayCellContent(selectedRow, "DonVi", textBox1);
                formMain.DisplayCellContent(selectedRow, "MucLuong", comboBox1);
               

                if (dataGridView1.Columns.Contains("donvi_id"))
                {
                    object donviIdObj = selectedRow.Cells["donvi_id"].Value;
                    donvi_id = donviIdObj != DBNull.Value ? Convert.ToInt32(donviIdObj) : 0;
                }
            }
        }
    }
}
