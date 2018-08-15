using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace MedForms
{
    public partial class Form2 : Form
    {
        SqlConnectionStringBuilder scsb;
        public string strReceiveOrder_id = "";
        public string strReceiveCus_name = "";
        public string strReceiveTotal_price = "";
        
        List<string> keyOrder = new List<string>();
        List<string> keyPro = new List<string>();

        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            scsb = new SqlConnectionStringBuilder();
            scsb.DataSource = @".";
            scsb.InitialCatalog = "com.Dumplin";
            scsb.IntegratedSecurity = true;
            lblOrder_idF2.Text = strReceiveOrder_id;
            lblNameF2.Text = strReceiveCus_name;
            tbTotalPriceF2.Text = strReceiveTotal_price;
            
            display4();
            count();
        }

        public void display4()
        {
            string strMsg = "";
            string strMessage = "";
            try
            {
                SqlConnection con = new SqlConnection(scsb.ToString());
                con.Open();
                string strSQL = "select * from Order_detail where Order_id = @SearchID;";
                SqlCommand cmd = new SqlCommand(strSQL, con);
                cmd.Parameters.AddWithValue("@SearchID",lblOrder_idF2.Text);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read() == true)
                {
                    strMsg = string.Format("{0},{1}包\n", reader["Pro_name"], reader["Qty"]);
                    keyOrder.Add(reader["Order_detail_id"].ToString());
                    lboxF2.Items.Add(strMsg);
                }
                reader.Close();
                con.Close();

                SqlConnection con2 = new SqlConnection(scsb.ToString());
                con2.Open();
                string strSQL2 = "select * from Product ";
                SqlCommand cmd2 = new SqlCommand(strSQL2, con2);
                SqlDataReader reader2 = cmd2.ExecuteReader();
                while (reader2.Read() == true)
                {
                    strMessage = string.Format("{0},{1}元\n", reader2["Pro_name"], reader2["Pro_price"]);
                    if ((bool)reader2["On_sale"] == true)
                    {
                        keyPro.Add(reader2["id"].ToString());
                        lboxPro_nameF2.Items.Add(strMessage);
                    }
                }
                reader2.Close();
                con2.Close();
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
                MessageBox.Show(error.ToString());
            }
        }

        public void count() {
            try
            {
                tbTotalPriceF2.Text = "";
                string total = "";
                SqlConnection con = new SqlConnection(scsb.ToString());
                con.Open();
                string strSQL = "select sum(Qty*Single_price) as Total " +
                    "from Order_detail where Order_id = @SearchOrder_id;";
                SqlCommand cmd = new SqlCommand(strSQL, con);
                cmd.Parameters.AddWithValue("@SearchOrder_id",lblOrder_idF2.Text);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read()==true)
                {
                    total = string.Format("{0}", reader["Total"]);
                    tbTotalPriceF2.Text = "$"+total;
                }
                reader.Close();
                con.Close();

                SqlConnection con2 = new SqlConnection(scsb.ToString());
                con2.Open();
                string strSQL2 = "update Order_list set price = @NewPrice where Order_id = @SearchOrder_id;";
                SqlCommand cmd2 = new SqlCommand(strSQL2, con2);
                cmd2.Parameters.AddWithValue("@NewPrice", total);
                cmd2.Parameters.AddWithValue("@SearchOrder_id", lblOrder_idF2.Text);
                cmd2.ExecuteReader();
                
                con2.Close();
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
                MessageBox.Show(error.ToString());
                cleanF2();
            }
        }

        private void btAddF2_Click(object sender, EventArgs e)
        {
            try
            {
                if ((tbPro_NameF2.Text != "") && (tbQtyF2.Text != "") && (tbSinglePriceF2.Text != ""))
                {
                    SqlConnection con = new SqlConnection(scsb.ToString());
                    con.Open();
                    string strSQL = "INSERT INTO Order_detail VALUES (@NewOrder_id, @NewCus_Name," +
                                                    "@NewPro_name,@NewQty,@NewSingle_price);";
                    SqlCommand cmd = new SqlCommand(strSQL, con);
                    cmd.Parameters.AddWithValue("@NewOrder_id", lblOrder_idF2.Text);
                    cmd.Parameters.AddWithValue("@NewCus_Name", lblNameF2.Text);
                    cmd.Parameters.AddWithValue("@NewPro_name", tbPro_NameF2.Text);
                    cmd.Parameters.AddWithValue("@NewQty", tbQtyF2.Text);
                    cmd.Parameters.AddWithValue("@NewSingle_price", tbSinglePriceF2.Text);

                    int rows = cmd.ExecuteNonQuery();
                    con.Close();
                    cleanF2();
                    count();
                    MessageBox.Show("Add Successful," + rows.ToString() + "data Changed");
                }
                else
                {
                    MessageBox.Show("Quality or singleprice or product name can't be empty");
                    cleanF2();
                }
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
                MessageBox.Show(error.ToString());
                cleanF2();
            }
        }

        private void btChangeF2_Click(object sender, EventArgs e)
        {
            try
            {
                if ((tbPro_NameF2.Text != "")&&(tbQtyF2.Text != "")&&(tbSinglePriceF2.Text != ""))
                {
                    int num;
                    num = Convert.ToInt32(lboxF2.SelectedIndex.ToString());
                    SqlConnection con = new SqlConnection(scsb.ToString());
                    con.Open();
                    string strSQL = "update Order_detail set Order_id = @NewOrder_id, Cus_name = @NewCus_Name," +
                                                    "Pro_name = @NewPro_name,Qty = @NewQty, " +
                                                    "Single_price = @NewSingle_price " +
                                                    "where Order_detail_id = @SearchOrder_detail_id;";
                    SqlCommand cmd = new SqlCommand(strSQL,con);
                    cmd.Parameters.AddWithValue("@SearchOrder_detail_id", keyOrder.ElementAt(num));
                    cmd.Parameters.AddWithValue("@NewOrder_id",lblOrder_idF2.Text);
                    cmd.Parameters.AddWithValue("@NewCus_Name", lblNameF2.Text);
                    cmd.Parameters.AddWithValue("@NewPro_name", tbPro_NameF2.Text);
                    cmd.Parameters.AddWithValue("@NewQty", tbQtyF2.Text);
                    cmd.Parameters.AddWithValue("@NewSingle_price", tbSinglePriceF2.Text);

                    int rows = cmd.ExecuteNonQuery();
                    con.Close();
                    cleanF2();
                    count();
                    MessageBox.Show("Change Successful," + rows.ToString() + "data Changed");
                }
                else
                {
                    MessageBox.Show("Nothing");
                    cleanF2();
                }
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
                MessageBox.Show(error.ToString());
                cleanF2();
            }
        }

        private void btDeleteF2_Click(object sender, EventArgs e)
        {
            try
            {
                if ((tbPro_NameF2.Text != "") && (tbQtyF2.Text != "") && (tbSinglePriceF2.Text != ""))
                {
                    int num;
                    num = Convert.ToInt32(lboxF2.SelectedIndex.ToString());
                    SqlConnection con = new SqlConnection(scsb.ToString());
                    con.Open();
                    string strSQL = "delete from Order_detail where Order_detail_id = @SearchOrder_detail_id;";
                    SqlCommand cmd = new SqlCommand(strSQL, con);
                    cmd.Parameters.AddWithValue("@SearchOrder_detail_id", keyOrder.ElementAt(num));
                    
                    int rows = cmd.ExecuteNonQuery();
                    con.Close();
                    cleanF2();
                    count();
                    MessageBox.Show("Add Successful," + rows.ToString() + "data Changed");
                }
                else
                {
                    MessageBox.Show("Nothing");
                    cleanF2();
                }
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
                MessageBox.Show(error.ToString());
                cleanF2();
            }
        }

        private void btClearF2_Click(object sender, EventArgs e)
        {
            cleanF2();
        }

        private void btSearchF2_Click(object sender, EventArgs e)
        {
            try
            {
                lboxF2.Items.Clear();
                SqlConnection con = new SqlConnection(scsb.ToString());
                con.Open();
                string strSQL = "select * from Order_detail where Order_id = @SearchOrder_id;";
                SqlCommand cmd = new SqlCommand(strSQL, con);
                cmd.Parameters.AddWithValue("@SearchOrder_id", lblOrder_idF2.Text);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read() == true)
                {
                    lboxF2.Items.Add(reader["Pro_name"]);
                }
                reader.Close();
                con.Close();
                count();
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
                MessageBox.Show(error.ToString());
                cleanF2();
            }

        }

        private void lboxF2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int num;
                num = Convert.ToInt32(lboxF2.SelectedIndex.ToString());
                string strSearchPro_name = lboxF2.SelectedItem.ToString();
                if (strSearchPro_name != "")
                {
                    SqlConnection con = new SqlConnection(scsb.ToString());
                    con.Open();
                    string strSQL = "select * from Order_detail where Order_detail_id= @SearchOrder_detail_id;";
                    SqlCommand cmd = new SqlCommand(strSQL,con);
                    cmd.Parameters.AddWithValue("@SearchOrder_detail_id", keyOrder.ElementAt(num));
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read() == true)
                    {
                        tbPro_NameF2.Text = string.Format("{0}",reader["Pro_name"]);
                        tbQtyF2.Text = string.Format("{0}", reader["Qty"]);
                        tbSinglePriceF2.Text = string.Format("{0}", reader["Single_price"]);
                    }
                    else
                    {
                        MessageBox.Show("Nothing");
                        cleanF2();
                    }
                    reader.Close();
                    con.Close();
                }
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
                MessageBox.Show(error.ToString());
                cleanF2();
            }
        }

        private void lboxPro_nameF2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int num;
                num = Convert.ToInt32(lboxPro_nameF2.SelectedIndex.ToString());
                string strSearchPro_name = lboxPro_nameF2.SelectedItem.ToString();
                if (strSearchPro_name != "")
                {
                    SqlConnection con = new SqlConnection(scsb.ToString());
                    con.Open();
                    string strSQL = "select * from Product where id = @Searchid;";
                    SqlCommand cmd = new SqlCommand(strSQL, con);
                    cmd.Parameters.AddWithValue("@Searchid", keyPro.ElementAt(num));
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read() == true)
                    {
                        tbPro_NameF2.Text = string.Format("{0}", reader["Pro_name"]);
                        tbSinglePriceF2.Text = string.Format("{0}",reader["Pro_price"]);
                    }
                    else
                    {
                        MessageBox.Show("Nothing");
                        cleanF2();
                    }
                    reader.Close();
                    con.Close();
                }
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
                MessageBox.Show(error.ToString());
                cleanF2();
            }
        }

        private void btCloseF2_Click(object sender, EventArgs e)
        {
            cleanF2();
            Close();
        }

        private void cleanF2()
        {
            //lblOrder_idF2.Text = "";
            //lblNameF2.Text = "";
            tbPro_NameF2.Text = "";
            tbQtyF2.Text = "";
            tbSinglePriceF2.Text = "";
            tbTotalPriceF2.Text = "";
            lboxF2.Items.Clear();
            lboxPro_nameF2.Items.Clear();
            tbTotalPriceF2.Text = "";
            keyOrder.Clear();
            keyPro.Clear();
            display4();
            count();
        }

    }
}
