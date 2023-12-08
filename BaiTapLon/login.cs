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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace BaiTapLon
{
    public partial class login : Form
    {
        public login()
        {
            InitializeComponent();

        }

        private void login_Load(object sender, EventArgs e)
        {
        }

        private void btnOut_Click(object sender, EventArgs e)
        {
           Application.Exit();
        }

        private void login_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(MessageBox.Show("Ban muon  thoat ?","Thong Bao",MessageBoxButtons.OKCancel) != System.Windows.Forms.DialogResult.OK)
            {
                e.Cancel = true;
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string conn = "Server=DESKTOP-M9OV124\\HINH;Database=QuanLyCanBo;Integrated Security=SSPI;";
            try
            {
                SqlConnection sqlConnection = new SqlConnection(conn);
                sqlConnection.Open();
                string tk = txtUseName.Text;
                string mk = txtPassWord.Text;
                string sql = "SELECT * FROM taiKhoan WHERE tenDangNhap = @tenDangNhap AND matKhau = @matKhau";
                SqlCommand command = new SqlCommand(sql, sqlConnection);
                command.Parameters.AddWithValue("@tenDangNhap", tk);
                command.Parameters.AddWithValue("@matKhau", mk);

                SqlDataReader reader = command.ExecuteReader();
                if(reader.Read())
                {
                    reader.Close();
                    FormMain form = new FormMain();
                    this.Hide();
                    form.ShowDialog();
                    this.Show();
                }
                else
                {
                    reader.Close();
                    MessageBox.Show("Dang Nhap lai!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối: " + ex.Message);
            }






        }
    }
}
