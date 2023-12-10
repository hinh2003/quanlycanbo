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

namespace BaiTapLon
{
    public partial class FormMain : Form
    {
        string conn = "Server=DESKTOP-M9OV124\\HINH;Database=QuanLyCanBo;Integrated Security=SSPI;";
        SqlConnection sqlConnection;
        public delegate void UpdateDataDelegate();
        public UpdateDataDelegate UpdateDataCallback;


        public FormMain()
        {
            InitializeComponent();
            sqlConnection = new SqlConnection(conn);
            
            
        }

        private void dsadToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        public void hienthi()
        {
            try
            {
                if (sqlConnection.State == ConnectionState.Closed)
                {
                    sqlConnection.Open();
                }

                string sql = "SELECT  gv.giaovien_ten AS TenGiaoVien, cv.congviec AS CongViec, " +
                             "gv.giaovien_id AS giaovien_id," +
                             "gv.tuoi AS Tuoi,"+
                             " gv.giaovien_sdt AS SDT ,gv.Giaovien_email AS Email," +
                             "cb.capbac AS CapBac, " +
                             "lgv.luonggiaovien AS Luong, " +
                             "dv.donvi_ten AS DonVi " +
                             "FROM giaovien gv " +
                             "JOIN congviec cv ON gv.congviec_id = cv.congviec_id " +
                             "JOIN capbac cb ON gv.capbac_id = cb.capbac_id " +
                             "JOIN luonggiaovien lgv ON gv.luonggiaovien_id = lgv.luonggiaovien_id " +
                             "JOIN donvi dv ON gv.donvi_id = dv.donvi_id;";

                SqlCommand command = new SqlCommand(sql, sqlConnection);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                if (dataTable.Rows.Count > 0)
                {
                    dataGridView1.DataSource = dataTable;
                    dataGridView1.Columns["giaovien_id"].Visible = false;
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

        public void show()
        {
            sqlConnection.Open();
            string sqlCapBac = "SELECT capbac FROM capbac";
            SqlCommand commandCapBac = new SqlCommand(sqlCapBac, sqlConnection);
            SqlDataReader readerCapBac = commandCapBac.ExecuteReader();

            while (readerCapBac.Read())
            {
                comboBox1.Items.Add(readerCapBac["capbac"].ToString());
            }

            readerCapBac.Close();
            string sqlCongViec = "SELECT congviec FROM congviec";
            SqlCommand commandCongViec = new SqlCommand(sqlCongViec, sqlConnection);
            SqlDataReader readerCongViec = commandCongViec.ExecuteReader();

            while (readerCongViec.Read())
            {
                comboBox4.Items.Add(readerCongViec["congviec"].ToString());
            }

            readerCongViec.Close();


            string sqlLuongGV = "SELECT luonggiaovien FROM luonggiaovien";
            SqlCommand commandLuongGV = new SqlCommand(sqlLuongGV, sqlConnection);
            SqlDataReader readerLuongGV = commandLuongGV.ExecuteReader();

            while (readerLuongGV.Read())
            {
                comboBox3.Items.Add(readerLuongGV["luonggiaovien"].ToString());
            }

            readerLuongGV.Close();


            string sqlDonVi = "SELECT donvi_ten FROM donvi";
            SqlCommand commandDonVi = new SqlCommand(sqlDonVi, sqlConnection);
            SqlDataReader readerDonVi = commandDonVi.ExecuteReader();

            while (readerDonVi.Read())
            {
                comboBox2.Items.Add(readerDonVi["donvi_ten"].ToString());
            }

            readerDonVi.Close();
            sqlConnection.Close();
        }

    
        private void FormMain_Load(object sender, EventArgs e)
        {
            hienthi();
            show();



        }

        private void button1_Click(object sender, EventArgs e)
        {
            FormAdd formAdd = new FormAdd();
            this.Hide();
            formAdd.ShowDialog();
            this.Show();
            hienthi();
        }

        int giaovienId;

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow selectedRow = dataGridView1.Rows[e.RowIndex];

                DisplayCellContent(selectedRow, "TenGiaoVien", textBox1);
                DisplayCellContent(selectedRow, "CongViec", comboBox4);
                DisplayCellContent(selectedRow, "SDT", textBox3);
                DisplayCellContent(selectedRow, "Email", textBox4);
                DisplayCellContent(selectedRow, "CapBac", comboBox1);
                DisplayCellContent(selectedRow, "Luong", comboBox3);
                DisplayCellContent(selectedRow, "DonVi", comboBox2);
                DisplayCellContent(selectedRow, "Tuoi", textBox2);

                if (dataGridView1.Columns.Contains("giaovien_id"))
                {
                    object giaovienIdObj = selectedRow.Cells["giaovien_id"].Value;
                    giaovienId = giaovienIdObj != DBNull.Value ? Convert.ToInt32(giaovienIdObj) : 0;
                }
            }
        }

        public void DisplayCellContent(DataGridViewRow row, string columnName, Control control)
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


        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (sqlConnection.State == ConnectionState.Closed)
                {
                    sqlConnection.Open();
                }
                string name = textBox1.Text;
                string email = textBox4.Text;
                string phone = (textBox3.Text);
                string capbac = comboBox1.Text; 
                string luonggv = comboBox3.Text;
                string  DonVi = comboBox2.Text;
                string congviec =  comboBox4.Text;
                int tuoi;
                int phoneNumber;
                int luong;

                if (!int.TryParse(textBox3.Text, out phoneNumber) || !int.TryParse(textBox2.Text, out tuoi))
                {
                    MessageBox.Show("Vui lòng nhập số  hợp lệ.");
                    return;
                }
                 phoneNumber = int.Parse(phone);
                luong = int.Parse(luonggv);

                string sql1 = "UPDATE giaovien " +
                              "SET " +
                              "giaovien_ten = @name, " + 
                              "tuoi = @tuoi ," +
                              "Giaovien_email = @email, " +
                              "giaovien_sdt = @phoneNumber, " +
                              "donvi_id = (SELECT donvi_id FROM donvi WHERE donvi_ten = @DonVi), " +
                              "luonggiaovien_id = (SELECT luonggiaovien_id FROM luonggiaovien WHERE luonggiaovien = @luong), " +
                              "congviec_id = (SELECT congviec_id FROM congviec WHERE congviec = @congviec), " +
                              "capbac_id = (SELECT capbac_id FROM capbac WHERE capbac = @capbac)" +
                              "WHERE giaovien_id = @giaovienId";
                SqlCommand command = new SqlCommand(sql1, sqlConnection);
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@tuoi", tuoi);
                command.Parameters.AddWithValue("@email", email);
                command.Parameters.AddWithValue("@phoneNumber", phoneNumber);
                command.Parameters.AddWithValue("@DonVi", DonVi);
                command.Parameters.AddWithValue("@luong", luong);
                command.Parameters.AddWithValue("@congviec", congviec);
                command.Parameters.AddWithValue("@capbac", capbac);
                command.Parameters.AddWithValue("@giaovienId", giaovienId); // Sử dụng giá trị giaovien_id đã lấy từ DataGridView


                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Cập nhật thành công!");
                    textBox1.Text = "";
                    textBox3.Text = "";
                    textBox4.Text = "";
                    hienthi();
                   

                }
                else
                {
                    MessageBox.Show("Không có gì được cập nhật.");
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

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if(sqlConnection.State == ConnectionState.Closed)
                {
                    sqlConnection.Open();
                }
                string sql = "DELETE FROM giaovien WHERE giaovien_id = @giaovienId";
                SqlCommand sqlCommand = new SqlCommand (sql, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@giaovienId", giaovienId);
                int rowsAffected = sqlCommand.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Xoa thành công!");
                  
                    hienthi();


                }
                else
                {
                    MessageBox.Show("Không có gì được xoas.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);

            }
            finally {
                if (sqlConnection.State == ConnectionState.Open)
                {
                    sqlConnection.Close();
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                string giaovien = txtTim.Text;
                if (sqlConnection.State == ConnectionState.Closed)
                {
                    sqlConnection.Open();
                }

                string sql = "SELECT gv.giaovien_ten AS TenGiaoVien, cv.congviec AS CongViec, " +
               "gv.giaovien_id AS giaovien_id, gv.giaovien_sdt AS SDT, gv.Giaovien_email AS Email, " +
               "cb.capbac AS CapBac, lgv.luonggiaovien AS Luong, dv.donvi_ten AS DonVi " +
               "FROM giaovien gv " +
               "JOIN congviec cv ON gv.congviec_id = cv.congviec_id " +
               "JOIN capbac cb ON gv.capbac_id = cb.capbac_id " +
               "JOIN luonggiaovien lgv ON gv.luonggiaovien_id = lgv.luonggiaovien_id " +
               "JOIN donvi dv ON gv.donvi_id = dv.donvi_id " +
               "WHERE giaovien_ten LIKE @giaovien";

                SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@giaovien", "%" + giaovien + "%");

                SqlDataReader reader = sqlCommand.ExecuteReader();
                DataTable dataTable = new DataTable();
                dataTable.Load(reader);

                if (dataTable.Rows.Count > 0)
                {
                    dataGridView1.DataSource = dataTable;
                    dataGridView1.Columns["giaovien_id"].Visible = false;
                }
                else
                {
                    MessageBox.Show("Không tìm thấy tên.");
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

        private void danhSáchĐơnVịToolStripMenuItem_Click(object sender, EventArgs e)
        {
            capbac capbac = new capbac(this);
            this.Hide();
            capbac.ShowDialog();
            this.Show();
            
        }
        public void UpdateDonViComboBox()
        {
            comboBox2.Items.Clear(); // Xóa dữ liệu cũ trong combobox donvi
            comboBox1.Items.Clear(); // Xóa dữ liệu cũ trong combobox donvi
            comboBox3.Items.Clear(); // Xóa dữ liệu cũ trong combobox donvi
            comboBox4.Items.Clear(); // Xóa dữ liệu cũ trong combobox donvi
            show(); // Lấy dữ liệu mới từ cơ sở dữ liệu
        }
        public void UpdateDatagrid()
        {
            dataGridView1.DataSource = new DataTable();
            hienthi();
        }

        private void FormMain_FormClosed(object sender, FormClosedEventArgs e)
        {
         
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void danhSáchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            donvi donvi = new donvi(this);
            this.Hide();
            donvi.ShowDialog();
            this.Show();
        }

        private void bảngLươngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bangluong bangluong = new bangluong();
            this.Hide();
            bangluong.ShowDialog();
            this.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                if (sqlConnection.State == ConnectionState.Closed)
                {
                    sqlConnection.Open();
                }

                string sql = "SELECT  gv.giaovien_ten AS TenGiaoVien, cv.congviec AS CongViec, " +
                            "gv.giaovien_id AS giaovien_id," +
                            "gv.tuoi AS Tuoi," +
                            " gv.giaovien_sdt AS SDT ,gv.Giaovien_email AS Email," +
                            "cb.capbac AS CapBac, " +
                            "lgv.luonggiaovien AS Luong, " +
                            "dv.donvi_ten AS DonVi " +
                            "FROM giaovien gv " +
                            "JOIN congviec cv ON gv.congviec_id = cv.congviec_id " +
                            "JOIN capbac cb ON gv.capbac_id = cb.capbac_id " +
                            "JOIN luonggiaovien lgv ON gv.luonggiaovien_id = lgv.luonggiaovien_id " +
                            "JOIN donvi dv ON gv.donvi_id = dv.donvi_id " +
                            "WHERE tuoi > 60";

                SqlCommand command = new SqlCommand(sql, sqlConnection);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                if (dataTable.Rows.Count > 0)
                {
                    // Hiển thị dữ liệu trong DataGridView nếu có người tuổi trên 60
                    dataGridView1.DataSource = dataTable;
                }
                else
                {
                    MessageBox.Show("Không có người tuổi trên 60.");
                }

                if (sqlConnection.State == ConnectionState.Open)
                {
                    sqlConnection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
