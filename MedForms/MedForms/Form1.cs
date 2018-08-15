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
    public partial class Form1 : Form
    {
        SqlConnectionStringBuilder scsb;

        List<string> key1 = new List<string>();
        List<string> key2 = new List<string>();
        List<string> key3 = new List<string>();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            tabControl1.Appearance = TabAppearance.FlatButtons; //Hide tabControl Tag
            tabControl1.SizeMode = TabSizeMode.Fixed; //Hide tabControl Tag
            tabControl1.ItemSize = new Size(0, 1); //Hide tabControl Tag
            tabControl2.Appearance = TabAppearance.FlatButtons; //Hide tabControl Tag
            tabControl2.SizeMode = TabSizeMode.Fixed; //Hide tabControl Tag
            tabControl2.ItemSize = new Size(0, 1); //Hide tabControl Tag
            scsb = new SqlConnectionStringBuilder();
            scsb.DataSource = @".";
            scsb.InitialCatalog = "com.Dumplin";
            scsb.IntegratedSecurity = true;
            timerClock.Enabled = true;
            display1();
            tbVIPNum1.Enabled = false;
            tbAD3.Enabled = false;
            tbMonth4.Enabled = false;
            tbYear4.Enabled = false;
            tbYear5.Enabled = false;
            tbMonth5.Enabled = false;
            
        }
        
        private void timerClock_Tick(object sender, EventArgs e)
        {
            lblTime1.Text = string.Format("{0}", DateTime.Now);
        }

        public void display1() {
            try
            {
                string strMessage1 = "";
                lboxName1.Items.Clear();
                key1.Clear();
                SqlConnection con = new SqlConnection(scsb.ToString());
                con.Open();
                string strSQL = "select * from Customer;";
                SqlCommand cmd = new SqlCommand(strSQL,con);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read() == true)
                {
                    if (reader["Cus_id"].ToString()!="0")
                    {
                        strMessage1 = string.Format("{0},電話:{1}", reader["Cus_name"], reader["Cus_phone"]);
                        lboxName1.Items.Add(strMessage1);
                        key1.Add(reader["Cus_id"].ToString());
                    }
                }
                reader.Close();
                con.Close();
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
                MessageBox.Show(error.ToString());
            }
        }

        public void display2()
        {
            string strMessage = "";
            try
            {
                SqlConnection con = new SqlConnection(scsb.ToString());
                con.Open();
                string strSQL = "select * from Product;";
                SqlCommand cmd = new SqlCommand(strSQL, con);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read() == true)
                {
                    strMessage = string.Format("{0},{1}元\n", reader["Pro_name"],reader["Pro_price"]);
                    key2.Add(reader["id"].ToString());
                    if ((bool)reader["On_sale"] == true)
                    {
                        strMessage += ",供貨中\n";
                        lboxProduct2.Items.Add(strMessage);
                    }
                    else
                    {
                        strMessage += ",未供貨中\n";
                        lboxProduct2.Items.Add(strMessage);
                    }

                }
                reader.Close();
                con.Close();
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
                MessageBox.Show(error.ToString());
            }
        }

        public void display3()
        {
            key3.Clear();
            string strMessage = "";
            try
            {
                SqlConnection con = new SqlConnection(scsb.ToString());
                con.Open();
                string strSQL = "select * from Order_list where payment = @payment;";
                SqlCommand cmd = new SqlCommand(strSQL, con);
                cmd.Parameters.AddWithValue("@payment", false);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read() == true)
                {
                    strMessage = string.Format("訂單編號:{0},客戶姓名:{1}", reader["Order_id"], reader["Cus_name"]+"\n");
                    lbox3.Items.Add(strMessage);
                    key3.Add(reader["Order_id"].ToString());
                }

                reader.Close();
                con.Close();
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
                MessageBox.Show(error.ToString());
            }
        }

        public void display3andhalf()
        {
            try
            {
                string strMsg3 = "";
                lboxCus_name3.Items.Clear();
                key1.Clear();
                SqlConnection con = new SqlConnection(scsb.ToString());
                con.Open();
                string strSQL = "select * from Customer;";
                SqlCommand cmd = new SqlCommand(strSQL, con);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read() == true)
                {
                    if (reader["Cus_id"].ToString() != "0")
                    {
                        strMsg3 = string.Format("{0},電話:{1}", reader["Cus_name"], reader["Cus_phone"]);
                        lboxCus_name3.Items.Add(strMsg3);
                        key1.Add(reader["Cus_id"].ToString());
                    }
                }
                reader.Close();
                con.Close();
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
                MessageBox.Show(error.ToString());
            }
        }

        private void btAdd2_Click(object sender, EventArgs e)
        {
            try
            {
                if (tbPro_Name2.Text != "")
                {
                    SqlConnection con = new SqlConnection(scsb.ToString());
                    con.Open();
                    string strSQL = "INSERT INTO Product VALUES" +
                        "(@NewPro_name, @NewPro_price, @NewOn_sale);";
                    SqlCommand cmd = new SqlCommand(strSQL,con);
                    cmd.Parameters.AddWithValue("@NewPro_name",tbPro_Name2.Text);
                    cmd.Parameters.AddWithValue("@NewPro_price", tbPro_Price2.Text);
                    cmd.Parameters.AddWithValue("@NewOn_sale",(bool)ckOn_sale2.Checked);
                    //cmd.Parameters.AddWithValue("@NewId", tbNum2.Text);
                    int rows = cmd.ExecuteNonQuery();
                    con.Close();

                    cancel2();
                    MessageBox.Show("Add finished," + rows.ToString() + "data Changed");
                }
                else
                {
                    MessageBox.Show("Please give name's value");
                }
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
                MessageBox.Show(error.ToString());
            }
        }

        private void btSimpleSearch2_Click(object sender, EventArgs e)
        {
            try
            {
                if (tbPro_Name2.Text != "")
                {
                    SqlConnection con = new SqlConnection(scsb.ToString());
                    con.Open();
                    String strSQL = "select * from Product where Pro_name like @SearchName;";
                    SqlCommand cmd = new SqlCommand(strSQL, con);
                    cmd.Parameters.AddWithValue("@SearchName", "%" + tbPro_Name2.Text + "%");
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read() == true)
                    {
                        tbPro_Name2.Text = string.Format("{0}", reader["Pro_name"]);
                        tbPro_Price2.Text = string.Format("{0}", reader["Pro_price"]);
                        ckOn_sale2.Checked = (bool)reader["On_sale"];
                        lblID2.Text = string.Format("{0}", reader["id"]);
                    }
                    else
                    {
                        MessageBox.Show("Nothing");
                        cancel2();
                    }
                    reader.Close();
                    con.Close();
                }
                else
                {
                    MessageBox.Show("Please type in name");
                }
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
                MessageBox.Show(error.ToString());
            }
        }

        private void btChange2_Click(object sender, EventArgs e)
        {
            int intID = 0;
            Int32.TryParse(lblID2.Text,out intID);
            
            if (intID > 0)
            {
                SqlConnection con = new SqlConnection(scsb.ToString());
                con.Open();
                string strSQL = "update Product set Pro_name = @NewPro_name,Pro_price = @NewPro_price," +
                            "On_sale=@NewOn_sale where id = @Searchid";
                SqlCommand cmd = new SqlCommand(strSQL,con);
                cmd.Parameters.AddWithValue("@Searchid", lblID2.Text);
                cmd.Parameters.AddWithValue("@NewPro_name", tbPro_Name2.Text);
                cmd.Parameters.AddWithValue("@NewPro_price", tbPro_Price2.Text);
                cmd.Parameters.AddWithValue("@NewOn_sale", (bool)ckOn_sale2.Checked);

                int rows = cmd.ExecuteNonQuery();
                con.Close();
                cancel2();
                MessageBox.Show("Change Successful," + rows.ToString() + "data Changed");
            }
            else
            {
                MessageBox.Show("Nothing");
                cancel1();
            }
        }

        private void btSearch2_Click(object sender, EventArgs e)
        {
            try
            {
                lboxProduct2.Items.Clear();
                SqlConnection con = new SqlConnection(scsb.ToString());
                con.Open();
                string strSQL = "select * from Product where Pro_name like @SearchPro_name;";
                SqlCommand cmd = new SqlCommand(strSQL, con);
                cmd.Parameters.AddWithValue("@SearchPro_name", "%" + tbPro_Name2.Text + "%");
                SqlDataReader reader = cmd.ExecuteReader();
                    
                while (reader.Read() == true)
                {
                    lboxProduct2.Items.Add(reader["Pro_name"]);
                }
                reader.Close();
                con.Close();
                
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
                MessageBox.Show(error.ToString());
            }
        }

        private void btDelete2_Click(object sender, EventArgs e)
        {
            int intID = 0;
            Int32.TryParse(lblID2.Text, out intID);
            if (intID > 0 )
            {
                SqlConnection con = new SqlConnection(scsb.ToString());
                con.Open();
                string strSQL = "delete from Product where id = @SearchID";
                SqlCommand cmd = new SqlCommand(strSQL,con);
                cmd.Parameters.AddWithValue("@SearchID", intID);

                int rows = cmd.ExecuteNonQuery();
                con.Close();
                cancel2();
                MessageBox.Show("Delete finished," + rows.ToString() + "data delete");
            }
            else
            {
                MessageBox.Show("Useless ID");
            }
        }

        private void btData2_Click(object sender, EventArgs e)
        {
            try
            {
                SqlConnection con = new SqlConnection(scsb.ToString());
                con.Open();
                string strSQL = "select * from Product";
                SqlCommand cmd = new SqlCommand(strSQL, con);
                SqlDataReader reader = cmd.ExecuteReader();

                string strMsg = "";
                int i = 0;

                while (reader.Read() == true)
                {
                    i += 1;
                    strMsg += string.Format("{0}.{1}, {2}元",
                        reader["id"],reader["Pro_name"], reader["Pro_price"]);
                    if ((bool)reader["On_sale"]==true)
                    {
                        strMsg += ",供貨中\n";
                    }
                    else
                    {
                        strMsg += ",未供貨中\n";
                    }
                }

                strMsg += "資料筆數:" + i.ToString();

                reader.Close();
                con.Close();

                MessageBox.Show(strMsg);
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
                MessageBox.Show(error.ToString());
            }
        }

        private void lboxProduct2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int num;
                num = Convert.ToInt32(lboxProduct2.SelectedIndex.ToString());
                string strSearchPro_name = lboxProduct2.SelectedItem.ToString();
                if (strSearchPro_name != "")
                {
                    SqlConnection con = new SqlConnection(scsb.ToString());
                    con.Open();
                    string strSQL = "select * from Product where id = @Searchid;";
                    SqlCommand cmd = new SqlCommand(strSQL, con);
                    cmd.Parameters.AddWithValue("@Searchid", key2.ElementAt(num));
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read() == true)
                    {
                        tbPro_Name2.Text = string.Format("{0}", reader["Pro_name"]);
                        tbPro_Price2.Text = string.Format("{0}", reader["Pro_price"]);
                        ckOn_sale2.Checked = (bool)reader["On_sale"];
                        lblID2.Text = string.Format("{0}", reader["id"]);
                    }
                    else
                    {
                        MessageBox.Show("Nothing");
                        cancel2();
                    }
                    reader.Close();
                    con.Close();
                }
                else
                {
                    MessageBox.Show("Nothing");
                }
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
                MessageBox.Show(error.ToString());
            }
            
        }

        private void btClean2_Click(object sender, EventArgs e)
        {
            cancel2();
        }

        public void cancel2() {
            lblID2.Text = "";
            tbPro_Name2.Text = "";
            tbPro_Price2.Text = "";
            ckOn_sale2.Checked = false;
            lboxProduct2.Items.Clear();
            key2.Clear();
            display2();
        }

        private void btAdd1_Click(object sender, EventArgs e)
        {
            try
            {
                if (tbName1.Text != "")
                {
                    SqlConnection con = new SqlConnection(scsb.ToString());
                    con.Open();
                    string strSQL = "insert into Customer values" +
                    "(@NewCus_Name, @NewCus_phone," +
                    "@NewCus_address, @Newvip_card, @Newvip_number);";
                    SqlCommand cmd = new SqlCommand(strSQL , con);
                    //cmd.Parameters.AddWithValue("@NewCus_id",tbNum1.Text);
                    cmd.Parameters.AddWithValue("@NewCus_Name", tbName1.Text);
                    cmd.Parameters.AddWithValue("@NewCus_phone", tbPhone1.Text);
                    cmd.Parameters.AddWithValue("@NewCus_address", tbAD1.Text);
                    cmd.Parameters.AddWithValue("@Newvip_card", (bool)ckVIP1.Checked);
                    cmd.Parameters.AddWithValue("@Newvip_number", tbVIPNum1.Text);
                    int rows = cmd.ExecuteNonQuery();
                    
                    con.Close();
                    cancel1();
                    MessageBox.Show("Add finished," + rows.ToString() + "data Changed");
                }
                else
                {
                    MessageBox.Show("Nothing");
                    cancel1();
                }
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
                MessageBox.Show(error.ToString());
            }
        }

        private void btChange1_Click(object sender, EventArgs e)
        {
            int intID = 0;
            Int32.TryParse(lblID1.Text, out intID);

            if (intID >0)
            {
                SqlConnection con = new SqlConnection(scsb.ToString());
                con.Open();
                string strSQL = "update Customer set Cus_name = @NewCus_Name," +
                                                    "Cus_phone = @NewCus_phone,Cus_address = @NewCus_address, " +
                                                    "vip_card = @Newvip_card,vip_number = @Newvip_number " +
                                                    "where Cus_id = @Searchid;";
                SqlCommand cmd = new SqlCommand(strSQL,con);
                cmd.Parameters.AddWithValue("@Searchid", lblID1.Text);
                cmd.Parameters.AddWithValue("@NewCus_Name", tbName1.Text);
                cmd.Parameters.AddWithValue("@NewCus_phone", tbPhone1.Text);
                cmd.Parameters.AddWithValue("@NewCus_address", tbAD1.Text);
                cmd.Parameters.AddWithValue("@Newvip_card", (bool)ckVIP1.Checked);
                cmd.Parameters.AddWithValue("@Newvip_number", tbVIPNum1.Text);

                int rows = cmd.ExecuteNonQuery();
                con.Close();
                cancel1();
                MessageBox.Show("Change Successful," + rows.ToString() + "data Changed");
            }
            else
            {
                MessageBox.Show("Nothing");
                cancel1();
            }
        }

        private void btSimpleSearch1_Click(object sender, EventArgs e)
        {
            try
            {
                if ((tbName1.Text != "") && (tbPhone1.Text ==""))
                {
                    SqlConnection con = new SqlConnection(scsb.ToString());
                    con.Open();
                    string strSQL = "select * from Customer where Cus_name like @SearchName;";
                    SqlCommand cmd = new SqlCommand(strSQL, con);
                    cmd.Parameters.AddWithValue("@SearchName", "%" + tbName1.Text + "%");
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read()==true)
                    {
                        lblID1.Text = string.Format("{0}", reader["Cus_id"]);
                        tbName1.Text = string.Format("{0}", reader["Cus_name"]);
                        tbPhone1.Text = string.Format("{0}", reader["Cus_phone"]);
                        tbAD1.Text = string.Format("{0}", reader["Cus_address"]);
                        ckVIP1.Checked = (bool)reader["vip_card"];
                        tbVIPNum1.Text = string.Format("{0}", reader["vip_number"]);
                    }
                    else
                    {
                        MessageBox.Show("Nothing");
                        cancel1();
                    }
                    reader.Close();
                    con.Close();
                }
                else if ((tbName1.Text == "") && (tbPhone1.Text != ""))
                {
                    SqlConnection con = new SqlConnection(scsb.ToString());
                    con.Open();
                    string strSQL = "select * from Customer where Cus_phone = @SearchCus_phone;";
                    SqlCommand cmd = new SqlCommand(strSQL, con);
                    cmd.Parameters.AddWithValue("@SearchCus_phone", tbPhone1.Text);
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read() == true)
                    {
                        lblID1.Text = string.Format("{0}", reader["Cus_id"]);
                        tbName1.Text = string.Format("{0}", reader["Cus_name"]);
                        tbAD1.Text = string.Format("{0}", reader["Cus_address"]);
                        ckVIP1.Checked = (bool)reader["vip_card"];
                        tbVIPNum1.Text = string.Format("{0}", reader["vip_number"]);
                    }
                    else
                    {
                        MessageBox.Show("Nothing");
                        cancel1();
                    }
                    reader.Close();
                    con.Close();
                }
                else
                {
                    MessageBox.Show("Nothing");
                    cancel1();
                }
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
                MessageBox.Show(error.ToString());
            }
        }

        private void btDelete1_Click(object sender, EventArgs e)
        {
            int intID = 0;
            Int32.TryParse(lblID1.Text,out intID);
            if (intID > 0)
            {
                SqlConnection con = new SqlConnection(scsb.ToString());
                con.Open();
                string strSQL = "delete from Customer where Cus_id = @SearchID";
                SqlCommand cmd = new SqlCommand(strSQL,con);
                cmd.Parameters.AddWithValue("@SearchID", lblID1.Text);

                int rows = cmd.ExecuteNonQuery();
                con.Close();
                cancel1();
                MessageBox.Show("Delete finished," + rows.ToString() + "data delete");
            }
            else
            {
                MessageBox.Show("Useless ID");
                cancel1();
            }
        }

        private void btData1_Click(object sender, EventArgs e)
        {                                       //useless
            try
            {
                SqlConnection con = new SqlConnection(scsb.ToString());
                con.Open();
                string strSQL = "select * from Customer;";
                SqlCommand cmd = new SqlCommand(strSQL,con);
                SqlDataReader reader = cmd.ExecuteReader();

                string msg = "";
                int i = 0;

                while (reader.Read()==true)
                {
                    i += 1;
                    msg += string.Format("{0},{1},",reader["Cus_id"],reader["Cus_name"]);
                    if ((bool)reader["vip_card"] == true)
                    {
                        msg += "會員編號"+reader["vip_number"] + "\n";
                    }
                    else
                    {
                        msg += "\n";
                    }
                }
                msg += "資料筆數:" + i.ToString();
                reader.Close();
                con.Close();
                MessageBox.Show(msg);
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
                MessageBox.Show(error.ToString());
            }
        }

        private void btSearch1_Click(object sender, EventArgs e)
        {                   //useless
            try
            {
                lboxName1.Items.Clear();
                SqlConnection con = new SqlConnection(scsb.ToString());
                con.Open();
                string strSQL = "select * from Customer where Cus_name like @SearchName;";
                SqlCommand cmd = new SqlCommand(strSQL,con);
                cmd.Parameters.AddWithValue("@SearchName", "%"+tbName1.Text+"%");
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read() == true)
                {
                    lboxName1.Items.Add(reader["Cus_name"]);
                }
                reader.Close();
                con.Close();
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
                MessageBox.Show(error.ToString());
            }
        }

        private void lboxName1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int num;
                num = Convert.ToInt32(lboxName1.SelectedIndex.ToString());
                string strSearchCus_name = lboxName1.SelectedItem.ToString();
                if (strSearchCus_name != "")
                {
                    SqlConnection con = new SqlConnection(scsb.ToString());
                    con.Open();
                    string strSQL = "select * from Customer where Cus_id = @Searchid;";
                    SqlCommand cmd = new SqlCommand(strSQL, con);
                    cmd.Parameters.AddWithValue("@Searchid", key1.ElementAt(num));
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read() == true)
                    {
                        lblID1.Text = string.Format("{0}", reader["Cus_id"]);
                        tbName1.Text = string.Format("{0}", reader["Cus_name"]);
                        tbPhone1.Text = string.Format("{0}", reader["Cus_phone"]);
                        tbAD1.Text = string.Format("{0}", reader["Cus_address"]);
                        ckVIP1.Checked = (bool)reader["vip_card"];
                        tbVIPNum1.Text = string.Format("{0}", reader["vip_number"]);
                    }
                    else
                    {
                        MessageBox.Show("Nothing");
                        cancel1();
                    }
                    reader.Close();
                    con.Close();
                }
                else
                {
                    MessageBox.Show("Nothing");
                    cancel1();
                }
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
                MessageBox.Show(error.ToString());
            }
        }

        private void btClean1_Click(object sender, EventArgs e)
        {
            cancel1();
        }

        public void cancel1()
        {
            lblID1.Text = "";
            tbName1.Text = "";
            tbPhone1.Text = "";
            tbAD1.Text = "";
            ckVIP1.Checked = false;
            tbVIPNum1.Text = "";
            lboxName1.Items.Clear();
            key1.Clear();
            display1();
        }

        private void ckVIP1_CheckedChanged(object sender, EventArgs e)
        {
            if (ckVIP1.Checked == true)
            {
                tbVIPNum1.Enabled = true;
            }
            else
            {
                tbVIPNum1.Enabled = false;
            }
        }

        public void addNewCustomer() {
            try
            {
                bool state = false;
                SqlConnection con = new SqlConnection(scsb.ToString());
                con.Open();
                string strSQL = "insert into Customer values" +
                        "(@NewCus_Name, @NewCus_phone, @NewCus_address,@Newvip_card,@Newvip_number);";
                SqlCommand cmd = new SqlCommand(strSQL, con);
                cmd.Parameters.AddWithValue("@NewCus_Name", tbTaker3.Text);
                cmd.Parameters.AddWithValue("@NewCus_phone", tbPhone3.Text);
                cmd.Parameters.AddWithValue("@NewCus_address", tbAD3.Text);
                cmd.Parameters.AddWithValue("@Newvip_card", state);
                cmd.Parameters.AddWithValue("@Newvip_number", 0);
                int rows = cmd.ExecuteNonQuery();      //先產生客人資料

                if (rows != 0)
                {
                    string id = "";
                    strSQL = "select Cus_id from Customer " +
                        "where Cus_name= @SearchName and Cus_phone = @SearchPhone and Cus_address = @SearchAD;";
                    cmd = new SqlCommand(strSQL, con);
                    cmd.Parameters.AddWithValue("@SearchName", tbTaker3.Text);
                    cmd.Parameters.AddWithValue("@SearchPhone", tbPhone3.Text);
                    cmd.Parameters.AddWithValue("@SearchAD", tbAD3.Text);
                    SqlDataReader reader = cmd.ExecuteReader(); //從客人資料表取 名字 電話 地址
                    if (reader.Read() == true)
                    {
                        id = reader["Cus_id"].ToString();

                        reader.Close();

                        strSQL = "INSERT INTO Order_list VALUES" +
                            "(@NewCus_id, @NewCus_name, @NewOrder_date, @Newship," +
                            "@Newship_way, @NewPrice, @Newtaker, @Newpayment, @Newtaker_phone);";
                        cmd = new SqlCommand(strSQL, con);
                        cmd.Parameters.AddWithValue("@NewCus_id", id);
                        cmd.Parameters.AddWithValue("@NewCus_name", tbTaker3.Text);
                        cmd.Parameters.AddWithValue("@NewOrder_date", (DateTime)dtpDate3.Value);
                        cmd.Parameters.AddWithValue("@Newship", tbShip3.Text);
                        cmd.Parameters.AddWithValue("@Newship_way", tbAD3.Text);
                        cmd.Parameters.AddWithValue("@NewPrice", lblTotal3.Text);
                        cmd.Parameters.AddWithValue("@Newtaker", tbTaker3.Text);
                        cmd.Parameters.AddWithValue("@Newpayment", cbPay3.Checked);
                        cmd.Parameters.AddWithValue("@Newtaker_phone", tbPhone3.Text);
                        rows = cmd.ExecuteNonQuery();    //建立訂購單

                        if (rows != 0)
                        {
                            MessageBox.Show("Add finished," + rows.ToString() + "data Changed");
                            clean3();
                            count3();
                        }
                        else
                        {
                            MessageBox.Show("Add fail," + rows.ToString() + "check again 724");
                        }
                        
                        con.Close();
                    }
                    else
                    {
                        MessageBox.Show("Add fail," + rows.ToString() + "check again 731");
                    }
                }
                else
                {
                    MessageBox.Show("Add fail," + rows.ToString() + "check again 736");
                }
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
                MessageBox.Show(error.ToString());
                clean3();
            }
        }

        private void btAdd3_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult;
            
            if ((ckSend3.Checked == true)||(ckTake3.Checked == true))
            {
                if ((tbTaker3.Text != "") && (tbShip3.Text != "") && (tbPhone3.Text != ""))
                {
                    if (ckNoneData.Checked == true)
                    {
                        dialogResult = MessageBox.Show("第一次購買，是否需要加入客人資料?", "ok", MessageBoxButtons.YesNo);
                        if (dialogResult == DialogResult.Yes)
                        {
                            addNewCustomer();   
                            openForm2();
                        }
                        else
                        {
                            add3();
                            openForm2();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Taker ,Address and Phone Number can't be empty");
                }
            }
            else
            {
                MessageBox.Show("請選擇要宅配或是自行取貨");
            }
        }

        public void add3() {
            try
            {
                if (tbName3.Text != "")
                {
                    SqlConnection con = new SqlConnection(scsb.ToString());
                    con.Open();
                    string strSQL = "INSERT INTO Order_list VALUES" +
                        "(@NewCus_id, @NewCus_name, @NewOrder_date, @Newship," +
                        "@Newship_way, @NewPrice, @Newtaker, @Newpayment, @Newtaker_phone);";
                    SqlCommand cmd = new SqlCommand(strSQL, con);
                    cmd.Parameters.AddWithValue("@NewCus_id", tbNum3.Text);
                    cmd.Parameters.AddWithValue("@NewCus_name", tbName3.Text);
                    cmd.Parameters.AddWithValue("@NewOrder_date", (DateTime)dtpDate3.Value);
                    cmd.Parameters.AddWithValue("@Newship", tbShip3.Text);
                    cmd.Parameters.AddWithValue("@Newship_way", tbAD3.Text);
                    cmd.Parameters.AddWithValue("@NewPrice", lblTotal3.Text);
                    cmd.Parameters.AddWithValue("@Newtaker", tbTaker3.Text);
                    cmd.Parameters.AddWithValue("@Newpayment", cbPay3.Checked);
                    cmd.Parameters.AddWithValue("@Newtaker_phone", tbPhone3.Text);
                    int rows = cmd.ExecuteNonQuery();
                    con.Close();

                    if (rows != 0)
                    {
                        MessageBox.Show("Add finished," + rows.ToString() + "data Changed");
                        clean3();
                        count3();
                    }
                    else
                    {
                        MessageBox.Show("Add fail," + rows.ToString() + "check again");
                        count3();
                    }
                }
                else
                {
                    MessageBox.Show("Nothing");
                    clean3();
                }
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
                MessageBox.Show(error.ToString());
                clean3();
            }
        }

        public void change3() {
            try
            {
                int intID = 0;
                Int32.TryParse(tbNum3.Text, out intID);
                if (intID > 0)
                {
                    SqlConnection con = new SqlConnection(scsb.ToString());
                    con.Open();
                    string strSQL = "update Order_list set Order_date = @NewOrder_date,ship = @Newship" +
                        " ,ship_way = @Newship_way,taker = @Newtaker , payment= @Newpayment , taker_phone = @Newtaker_phone " +
                        "where Order_id = @SearchID";
                    SqlCommand cmd = new SqlCommand(strSQL, con);
                    cmd.Parameters.AddWithValue("@SearchID", lblNum3.Text);
                    cmd.Parameters.AddWithValue("@NewOrder_date", (DateTime)dtpDate3.Value);
                    cmd.Parameters.AddWithValue("@Newship", tbShip3.Text);
                    cmd.Parameters.AddWithValue("@Newship_way", tbAD3.Text);
                    cmd.Parameters.AddWithValue("@Newtaker", tbTaker3.Text);
                    cmd.Parameters.AddWithValue("@Newpayment", cbPay3.Checked);
                    cmd.Parameters.AddWithValue("@Newtaker_phone", tbPhone3.Text);
                    int rows = cmd.ExecuteNonQuery();
                    con.Close();

                    if (rows != 0)
                    {
                        MessageBox.Show("Change Successful," + rows.ToString() + "data Changed");
                        clean3();
                        count3();
                    }
                    else
                    {
                        MessageBox.Show("Change fail," + rows.ToString() + "check again");
                        count3();
                    }
                }
                else
                {
                    MessageBox.Show("Nothing");
                    clean3();
                }
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
                MessageBox.Show(error.ToString());
                clean3();
            }
            
        }

        private void btChange3_Click(object sender, EventArgs e)
        {
            if (ckSend3.Checked == true)
            {
                if ((tbTaker3.Text != "") && (tbAD3.Text != "")&&(tbShip3.Text != ""))
                {
                    change3();
                }
                else
                {
                    MessageBox.Show("Taker or Address can't be empty");
                }
            }
            if (ckTake3.Checked == true)
            {
                if ((tbTaker3.Text != "")&&(tbShip3.Text != ""))
                {
                    change3();
                }
                else
                {
                    MessageBox.Show("Taker can't be empty");
                }
            }
            
        }

        private void btDelete3_Click(object sender, EventArgs e)
        {
            int intID = 0;
            Int32.TryParse(tbNum3.Text, out intID);
            if (intID >= 0)
            {
                SqlConnection con = new SqlConnection(scsb.ToString());
                con.Open();
                string strSQL = "delete from Order_list where Order_id = @SearchID";
                SqlCommand cmd = new SqlCommand(strSQL,con);
                cmd.Parameters.AddWithValue("@SearchID", lblNum3.Text);

                int rows = cmd.ExecuteNonQuery();
                con.Close();
                clean3();
                count3();
                if (rows != 0)
                {
                    MessageBox.Show("Delete finished," + rows.ToString() + "data delete");
                    clean3();
                    count3();
                }
                else
                {
                    MessageBox.Show("Delete fail," + rows.ToString() + "check again");
                    count3();
                }
                
            }
            else
            {
                MessageBox.Show("Nothing");
                clean3();
            }
        }

        private void btClean3_Click(object sender, EventArgs e)
        {
            clean3();
        }

        public void clean3()
        {
            ckNoneData.Checked = false;
            cbSelf3.Checked = false;
            cbPay3.Checked = false;
            ckSend3.Checked = false;
            ckTake3.Checked = false;
            lblNum3.Text = "";
            tbNum3.Text = "";
            tbName3.Text = "";
            dtpDate3.Value = DateTime.Now;
            tbShip3.Text = "";
            lbox3.Items.Clear();
            tbAD3.Text = "";
            lblTotal3.Text = "";
            tbTaker3.Text = "";
            tbPhone3.Text = "";
            key3.Clear();
            display3();
            display3andhalf();
        }

        private void btMSearch3_Click(object sender, EventArgs e)
        {                               //already pay
            lbox3.Items.Clear();
            key3.Clear();
            string strMessage = "";
            try
            {
                SqlConnection con = new SqlConnection(scsb.ToString());
                con.Open();
                string strSQL = "select * from Order_list where payment = @payment;";
                SqlCommand cmd = new SqlCommand(strSQL, con);
                cmd.Parameters.AddWithValue("@payment",true);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read() == true)
                {
                    strMessage = string.Format("訂單編號:{0},客戶姓名:{1}", reader["Order_id"], reader["Cus_name"] + "\n");
                    lbox3.Items.Add(strMessage);
                    key3.Add(reader["Order_id"].ToString());
                }

                reader.Close();
                con.Close();
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
                MessageBox.Show(error.ToString());
            }

        }

        private void btMSearchNoPay3_Click(object sender, EventArgs e)
        {             //pay not yet
            lbox3.Items.Clear();
            key3.Clear();
            display3();
        }

        public void count3()
        {
            try
            {
                lblTotal3.Text = "";
                SqlConnection con = new SqlConnection(scsb.ToString());
                con.Open();
                string strSQL = "select *from Order_list where Order_id = @SearchOrder_id;";
                SqlCommand cmd = new SqlCommand(strSQL, con);
                cmd.Parameters.AddWithValue("@SearchOrder_id", lblNum3.Text);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read() == true)
                {
                    lblTotal3.Text = string.Format("${0}", reader["price"]);
                }
                reader.Close();
                con.Close();
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
                MessageBox.Show(error.ToString());
                clean3();
            }
        }

        private void lbox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int num ;
                string strSearchCus_name = lbox3.SelectedItem.ToString();
                num = Convert.ToInt32(lbox3.SelectedIndex.ToString());

                if (strSearchCus_name != "")
                {
                    SqlConnection con = new SqlConnection(scsb.ToString());
                    con.Open();
                    string strSQL = "select * from Order_list " +
                        "where Order_id = @SearchOrder_id";
                    SqlCommand cmd = new SqlCommand(strSQL,con);
                    cmd.Parameters.AddWithValue("@SearchOrder_id", key3.ElementAt(num));
                    SqlDataReader reader = cmd.ExecuteReader();

                    //Console.WriteLine("Index= "+num+",id= "+ keyID.ElementAt(num));

                    if (reader.Read() == true)
                    {
                        lblNum3.Text = string.Format("{0}", reader["Order_id"]);
                        tbNum3.Text = string.Format("{0}", reader["Cus_id"]);
                        tbName3.Text = string.Format("{0}", reader["Cus_name"]);
                        dtpDate3.Value = (DateTime)reader["Order_date"];
                        tbShip3.Text = string.Format("{0}", reader["ship"]);
                        tbAD3.Text = string.Format("{0}", reader["ship_way"]);
                        tbTaker3.Text = string.Format("{0}", reader["taker"]);
                        cbPay3.Checked = (bool)reader["payment"];
                        tbPhone3.Text = string.Format("{0}", reader["taker_phone"]);
                    }
                    else
                    {
                        MessageBox.Show("Nothing");
                        clean3();
                    }
                    reader.Close();
                    con.Close();
                    count3();
                }
                else
                {
                    MessageBox.Show("Nothing");
                    clean3();
                }
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
                MessageBox.Show(error.ToString());
                clean3();
            }
        }

        private void ckNoneData_CheckedChanged(object sender, EventArgs e)
        {
            if (ckNoneData.Checked == true)
            {
                tbName3.Text = "other";
                tbNum3.Text = "0";
            }
            else
            {
                tbName3.Text = "";
                tbNum3.Text = "";
            }
        }

        private void lboxCus_name3_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int num;
                num = Convert.ToInt32(lboxCus_name3.SelectedIndex.ToString());
                string strSearchCus_name = lboxCus_name3.SelectedItem.ToString();
                if (strSearchCus_name != "")
                {
                    SqlConnection con = new SqlConnection(scsb.ToString());
                    con.Open();
                    string strSQL = "select * from Customer where Cus_id = @Searchid;";
                    SqlCommand cmd = new SqlCommand(strSQL, con);
                    cmd.Parameters.AddWithValue("@Searchid", key1.ElementAt(num));
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read() == true)
                    {
                        tbNum3.Text = string.Format("{0}", reader["Cus_id"]);
                        tbName3.Text = string.Format("{0}", reader["Cus_name"]);
                    }
                    else
                    {
                        MessageBox.Show("Nothing");
                    }
                    reader.Close();
                    con.Close();
                }
                else
                {
                    MessageBox.Show("Nothing");
                }
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
                MessageBox.Show(error.ToString());
            }
        }

        public void openForm2()
        {
            string strOrder_id = "";
            string strCus_name = "";

            int intID = 0;
            Int32.TryParse(lblNum3.Text, out intID);
            try
            {
                if (intID >= 0)
                {

                    SqlConnection con = new SqlConnection(scsb.ToString());
                    con.Open();
                    string strSQL = "select * from Order_list where Order_id like @SearchOrder_id;";
                    SqlCommand cmd = new SqlCommand(strSQL, con);
                    cmd.Parameters.AddWithValue("@SearchOrder_id", "%" + lblNum3.Text + "%");
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read() == true)
                    {
                        strOrder_id = string.Format("{0}", reader["Order_id"]);
                        strCus_name = string.Format("{0}", reader["Cus_name"]);
                    }
                    reader.Close();
                    con.Close();


                    Form2 myForm2 = new Form2();

                    myForm2.strReceiveOrder_id = strOrder_id;
                    myForm2.strReceiveCus_name = strCus_name;
                    myForm2.ShowDialog();
                    strOrder_id = "";
                    strCus_name = "";
                }
                else
                {
                    MessageBox.Show("Nothing");
                    clean3();
                }
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
                MessageBox.Show(error.ToString());
                clean3();
            }
        }

        private void btFormDetail_Click(object sender, EventArgs e)
        {
            openForm2();
        }

        private void ckSend3_CheckedChanged(object sender, EventArgs e)
        {
            tbShip3.Text = "";
            tbAD3.Text = "";
            if (ckSend3.Checked ==true)
            {
                tbShip3.Text = "宅配";
                tbAD3.Enabled = true;
                ckTake3.Checked = false;
            }
            else
            {
                tbShip3.Text = "自取";
                tbAD3.Text = "自取";
                tbAD3.Enabled = false;
                //ckTake3.Checked = true;
            }
        }

        private void ckTake3_CheckedChanged(object sender, EventArgs e)
        {
            tbShip3.Text = "";
            tbAD3.Text = "";
            tbAD3.Enabled = false;
            if (ckTake3.Checked == true)
            {
                tbShip3.Text = "自取";
                tbAD3.Text = "自取";
                ckSend3.Checked = false;
            }
            else
            {
                tbShip3.Text = "宅配";
                tbAD3.Enabled = true;
                //ckSend3.Checked = true;
            }
        }

        private void cbSelf3_CheckedChanged(object sender, EventArgs e)
        {
            if (cbSelf3.Checked == true)
            {
                tbTaker3.Text = tbName3.Text;
            }
            else
            {
                tbTaker3.Text = "";
            }
        }

        private void cbMonth4_CheckedChanged(object sender, EventArgs e)
        {
            if (cbMonth4.Checked == true)
            {
                tbMonth4.Enabled = true;
                cbYear4.Checked = false;
                tbYear4.Enabled = false;
                tbYear4.Text = "";
            }
            else
            {
                tbMonth4.Enabled = false;
                tbMonth4.Text = "";
            }
        }

        private void cbYear4_CheckedChanged(object sender, EventArgs e)
        {
            if (cbYear4.Checked == true)
            {
                tbMonth4.Enabled = false;
                cbMonth4.Checked = false;
                tbYear4.Enabled = true;
                tbMonth4.Text = "";
            }
            else
            {
                tbYear4.Enabled = false;
                tbYear4.Text = "";
            }
        }

        private void btStart4_Click(object sender, EventArgs e)
        {
            lblMonit4.Text = "";
            if (tbMonth4.Text != "")
            {
                countMonth4();
            }
            else
            {
                countYear4();
            }
        }
        public void countMonth4()
        {
            string countAna = "小姨婆餛飩店\n********************\n";
            countAna += tbMonth4.Text+"月營收統計:\n$";
            try
            {
                SqlConnection con = new SqlConnection(scsb.ToString());
                con.Open();
                string strSQL = "select sum(price) as Month_count from Order_list " +
                    "where month(Order_date)= @searchMonth;";
                SqlCommand cmd = new SqlCommand(strSQL,con);
                cmd.Parameters.AddWithValue("@searchMonth",tbMonth4.Text);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read()==true)
                {
                    countAna += string.Format("{0}元", reader["Month_count"]);
                }
                lblMonit4.Text = countAna;
                reader.Close();
                con.Close();
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
                clean4();
            }
        }

        public void countYear4()
        {
            string countAna = "小姨婆餛飩店\n********************\n";
            countAna += tbYear4.Text + "年營收統計:\n$";
            try
            {
                SqlConnection con = new SqlConnection(scsb.ToString());
                con.Open();
                string strSQL = "select sum(price) as Year_count from Order_list " +
                    "where year(Order_date)= @searchYear;";
                SqlCommand cmd = new SqlCommand(strSQL, con);
                cmd.Parameters.AddWithValue("@searchYear", tbYear4.Text);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read() == true)
                {
                    countAna += string.Format("{0}元", reader["Year_count"]);
                }
                lblMonit4.Text = countAna;
                reader.Close();
                con.Close();
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
                clean4();
            }
        }

        public void clean4() {
            lblMonit4.Text = "";
            tbYear4.Text = "";
            tbMonth4.Text = "";
            cbMonth4.Checked = false;
            cbYear4.Checked = false;
        }

        public void clean5(){
            lblMonit5.Text = "";
            tbYear5.Text = "";
            tbMonth5.Text = "";
            ckYear5.Checked = false;
            ckMonth5.Checked = false;
        }

        private void ckMonth5_CheckedChanged(object sender, EventArgs e)
        {
            if (ckMonth5.Checked == true)
            {
                tbMonth5.Enabled = true;
                ckYear5.Checked = false;
                tbYear5.Enabled = false;
                tbYear5.Text = "";
            }
            else
            {
                tbMonth5.Enabled = false;
                tbMonth5.Text = "";
            }
        }

        private void ckYear5_CheckedChanged(object sender, EventArgs e)
        {
            if (ckYear5.Checked == true)
            {
                tbMonth5.Enabled = false;
                ckMonth5.Checked = false;
                tbYear5.Enabled = true;
                tbMonth5.Text = "";
            }
            else
            {
                tbYear5.Enabled = false;
                tbYear5.Text = "";
            }
        }

        private void btStart5_Click(object sender, EventArgs e)
        {
            lblMonit5.Text = "";
            if (tbMonth5.Text != "")
            {
                countMonth5();
            }
            else
            {
                countYear5();
            }
        }
        public void countMonth5()
        {
            string countAna = "小姨婆餛飩店\n********************\n";
            countAna += tbMonth5.Text + "月銷售統計:\n";
            try
            {
                SqlConnection con = new SqlConnection(scsb.ToString());
                con.Open();
                string strSQL = "select od.Pro_name , count(*) as Qty " +
                    "from Order_detail as od inner join Order_list as ol on(od.Order_id = ol.Order_id) " +
                    "where month(Order_date) = @searchMonth " +
                    "group by Pro_name; ";
                SqlCommand cmd = new SqlCommand(strSQL, con);
                cmd.Parameters.AddWithValue("@searchMonth", tbMonth5.Text);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read() == true)
                {
                    countAna += string.Format("{0}:{1}包\n", reader["Pro_name"], reader["Qty"]);
                }
                lblMonit5.Text = countAna;
                reader.Close();
                con.Close();
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
                lblMonit5.Text = "";
                tbYear5.Text = "";
                tbMonth5.Text = "";
            }
        }
        public void countYear5()
        {
            string countAna = "小姨婆餛飩店\n********************\n";
            countAna += tbYear5.Text + "年銷售統計:\n";
            try
            {
                SqlConnection con = new SqlConnection(scsb.ToString());
                con.Open();
                string strSQL = "select od.Pro_name , count(*) as Qty " +
                    "from Order_detail as od inner join Order_list as ol on(od.Order_id = ol.Order_id) " +
                    "where year(Order_date) = @searchYear " +
                    "group by Pro_name; ";
                SqlCommand cmd = new SqlCommand(strSQL, con);
                cmd.Parameters.AddWithValue("@searchYear", tbYear5.Text);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read() == true)
                {
                    countAna += string.Format("{0}:{1}包\n", reader["Pro_name"],reader["Qty"]);
                }
                lblMonit5.Text = countAna;
                reader.Close();
                con.Close();
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
                lblMonit5.Text = "";
                tbYear5.Text = "";
                tbMonth5.Text = "";
            }
        }

        private void btViewProduct_Click(object sender, EventArgs e)
        {
            lboxProduct2.Items.Clear();
            tabControl1.SelectedIndex = 1;
            display2();
            cancel1();
            clean3();
            clean4();
            clean5();
        }

        private void btViewCustomer_Click(object sender, EventArgs e)
        {
            lboxName1.Items.Clear();
            tabControl1.SelectedIndex = 0;
            display1();
            cancel2();
            clean3();
            clean4();
            clean5();
        }

        private void btViewList_Click(object sender, EventArgs e)
        {
            lbox3.Items.Clear();

            tabControl1.SelectedIndex = 2;
            //display3();
            cancel1();
            cancel2();
            clean3();
            clean4();
            clean5();
        }

        private void btViewAn_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 3;
            cancel1();
            cancel2();
            clean3();
            clean4();
            clean5();
            
        }

        private void btSaleAn_Click(object sender, EventArgs e)
        {
            tabControl2.SelectedIndex = 1;
            clean4();
        }

        private void btIncomeAn_Click(object sender, EventArgs e)
        {
            tabControl2.SelectedIndex = 0;
            clean5();
        }

        
    }
}
