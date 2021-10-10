using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Data.OleDb;
using System.Windows.Forms;
using System.IO;

namespace StudentInformation
{
    public partial class Form1 : Form
    {
        OleDbConnection con = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\Student\Database3.accdb");
        OleDbDataAdapter da;
        OleDbCommand cmd;
        DataSet ds;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog o = new OpenFileDialog();
            o.Filter = "Image Files(*.jpg;*.jpeg:.*.gif;)|*.jpg;*.jeg;.*.gif";
            if (o.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = new Bitmap(o.FileName);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            con.Open();
            OleDbCommand cmd = new OleDbCommand("Insert into info values(@srno,@firstname,@lastname,@age,@email,@mobileno)", con);
            cmd.Parameters.AddWithValue("@srno",int.Parse(textBox1.Text));
            cmd.Parameters.AddWithValue("@firstnameo",textBox2.Text);
            cmd.Parameters.AddWithValue("@lastname",textBox3.Text);
            cmd.Parameters.AddWithValue("@age",int.Parse(textBox4.Text));
            cmd.Parameters.AddWithValue("@email",textBox5.Text);
            cmd.Parameters.AddWithValue("@mobileno",int.Parse(textBox6.Text));
                       
            cmd.ExecuteNonQuery();
            MessageBox.Show("Record Submitted", "Congrats");
            con.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            con.Open();
            string query = "update info set firstname=@firstname where srno=@srno";
            cmd = new OleDbCommand(query, con);
            cmd.Parameters.AddWithValue("srno",int.Parse(textBox1.Text));
            cmd.Parameters.AddWithValue("firstname",textBox2.Text);
            cmd.CommandText = query;
            cmd.ExecuteNonQuery();

            OleDbDataAdapter da = new OleDbDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            con.Close();
           

        }

        private void button4_Click(object sender, EventArgs e)
        {
            con.Open();
            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = con;
            string query = "delete from info where srno="+textBox1.Text+"";
            cmd.CommandText = query;
            cmd.ExecuteNonQuery();
            MessageBox.Show("Data delete Successful");
            con.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            con.Open();
            cmd = new OleDbCommand();
            cmd.Connection = con;
            string query = "select*from info";
            cmd.CommandText = query;

            OleDbDataAdapter da = new OleDbDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;

            con.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
