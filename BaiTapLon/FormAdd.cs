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
    public delegate void UpdateDataDelegate();
    public partial class FormAdd : Form
    {
        public UpdateDataDelegate UpdateDataCallback;

        public FormAdd()
        {
            InitializeComponent();
            this.FormClosed += new FormClosedEventHandler(FormAdd_FormClosed);


        }

        private void FormAdd_Load(object sender, EventArgs e)
        {
            string conn = "Server=DESKTOP-M9OV124\\HINH;Database=QuanLyCanBo;Integrated Security=SSPI;";
            SqlConnection sqlConnection = new SqlConnection(conn);
            sqlConnection.Open();
            string sqlCapBac = "SELECT capbac FROM capbac";
            SqlCommand commandCapBac = new SqlCommand(sqlCapBac, sqlConnection);
            SqlDataReader readerCapBac = commandCapBac.ExecuteReader();

            while (readerCapBac.Read())
            {
                cmcapbac.Items.Add(readerCapBac["capbac"].ToString());
            }

            readerCapBac.Close();
            string sqlCongViec = "SELECT congviec FROM congviec";
            SqlCommand commandCongViec = new SqlCommand(sqlCongViec, sqlConnection);
            SqlDataReader readerCongViec = commandCongViec.ExecuteReader();

            while (readerCongViec.Read())
            {
                cmcongviec.Items.Add(readerCongViec["congviec"].ToString());
            }

            readerCongViec.Close();


            string sqlLuongGV = "SELECT luonggiaovien FROM luonggiaovien";
            SqlCommand commandLuongGV = new SqlCommand(sqlLuongGV, sqlConnection);
            SqlDataReader readerLuongGV = commandLuongGV.ExecuteReader();

            while (readerLuongGV.Read())
            {
                cmluong.Items.Add(readerLuongGV["luonggiaovien"].ToString());
            }

            readerLuongGV.Close();


            string sqlDonVi = "SELECT donvi_ten FROM donvi";
            SqlCommand commandDonVi = new SqlCommand(sqlDonVi, sqlConnection);
            SqlDataReader readerDonVi = commandDonVi.ExecuteReader();

            while (readerDonVi.Read())
            {
                cmdonvi.Items.Add(readerDonVi["donvi_ten"].ToString());
            }

            readerDonVi.Close();
        }
        private void label5_Click(object sender, EventArgs e)
        {

        }
        private void button1_Click(object sender, EventArgs e)
        {

            string name = textBox1.Text;
            string email = textBox2.Text;
            string sdt = textBox3.Text;
            string donVi = cmdonvi.SelectedItem?.ToString();
            string capBac = cmcapbac.SelectedItem?.ToString();
            string luong = cmluong.SelectedItem?.ToString();
            string congViec = cmcongviec.SelectedItem?.ToString();

            if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(sdt) && !string.IsNullOrEmpty(email) &&
                !string.IsNullOrEmpty(capBac) && !string.IsNullOrEmpty(donVi) &&
                !string.IsNullOrEmpty(luong) && !string.IsNullOrEmpty(congViec))
            {
                try
                {
                    int phoneNumber = int.Parse(sdt);
                    string conn = "Server=DESKTOP-M9OV124\\HINH;Database=QuanLyCanBo;Integrated Security=SSPI;";

                    using (SqlConnection connection = new SqlConnection(conn))
                    {
                        connection.Open();
                        string sql = "INSERT INTO giaovien (giaovien_ten, giaovien_sdt, Giaovien_email, capbac_id, donvi_id, luonggiaovien_id, congviec_id) " +
                                     "VALUES (@ten, @sdt, @email, " +
                                     "(SELECT capbac_id FROM capbac WHERE capbac = @capBac), " +
                                     "(SELECT donvi_id FROM donvi WHERE donvi_ten = @donVi), " +
                                     "(SELECT luonggiaovien_id FROM luonggiaovien WHERE luonggiaovien = @luong), " +
                                     "(SELECT congviec_id FROM congviec WHERE congviec = @congViec))";

                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("@ten", name);
                            command.Parameters.AddWithValue("@sdt", phoneNumber);
                            command.Parameters.AddWithValue("@email", email);
                            command.Parameters.AddWithValue("@capBac", capBac);
                            command.Parameters.AddWithValue("@donVi", donVi);
                            command.Parameters.AddWithValue("@luong", luong);
                            command.Parameters.AddWithValue("@congViec", congViec);

                            int rowsAffected = command.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Thêm dữ liệu thành công");
                            }
                            else
                            {
                                MessageBox.Show("Có lỗi xảy ra");
                            }
                            UpdateDataCallback?.Invoke();

                        }
                    }
                }
                catch (FormatException)
                {
                    MessageBox.Show("Số điện thoại phải là số nguyên");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin");
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            // Hiển thị lại form ban đầu
            foreach (Form frm in Application.OpenForms)
            {
                if (frm is FormMain)
                {
                    frm.Show();
                    break;
                }
            }

        }

        private void FormAdd_FormClosed(object sender, FormClosedEventArgs e)
        {
           
        }
    }
}