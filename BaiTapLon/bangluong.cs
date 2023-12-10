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
    public partial class bangluong : Form
    {
        string conn = "Server=DESKTOP-M9OV124\\HINH;Database=QuanLyCanBo;Integrated Security=SSPI;";
        SqlConnection sqlConnection;
        public bangluong()
        {
            InitializeComponent();
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
                string donvi = comboBox1.Text;

                string sql ="SELECT gv.giaovien_ten AS TenGiaoVien, dv.donvi_ten AS DonVi, lg.luonggiaovien AS Luong " +
                            "FROM giaovien gv " +
                            "JOIN donvi dv ON gv.donvi_id = dv.donvi_id " +
                            "JOIN luonggiaovien lg ON gv.luonggiaovien_id = lg.luonggiaovien_id " + 
                            "WHERE dv.donvi_ten = @donvi ";




                SqlCommand command = new SqlCommand(sql, sqlConnection);
                command.Parameters.AddWithValue("@donvi", donvi);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                if (dataTable.Rows.Count > 0)
                {
                    dataGridView1.DataSource = dataTable;
                    
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

        private void bangluong_Load(object sender, EventArgs e)
        {
           
            
            show();
        }
        private void show()
        {
            sqlConnection.Open();
            string sqlmucluong = "SELECT donvi_ten FROM donvi";
            SqlCommand commandCapBac = new SqlCommand(sqlmucluong, sqlConnection);
            SqlDataReader readerCapBac = commandCapBac.ExecuteReader();

            while (readerCapBac.Read())
            {
                comboBox1.Items.Add(readerCapBac["donvi_ten"].ToString());
            }

            readerCapBac.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {


            hienthi();
            
           
        }
    }
}
