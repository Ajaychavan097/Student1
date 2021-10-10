using System;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Data.OleDb;
using System.Windows.Forms;
using System.IO;
using Microsoft.VisualBasic;

namespace access_img
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        OleDbConnection con;
        OleDbCommand cmd;
        OleDbDataAdapter adapter;
        DataSet ds;
        int rno = 0;
        MemoryStream ms;
        byte[] photo_aray;

        private void Form1_Load(object sender, EventArgs e)
        {
            con = new OleDbConnection(@" provider=microsoft.ace.oledb.12.0; data source=e:\prash\stud.accdb; persist securityiInfo=false");//stud.accdb->access07 filename
            //con = new OleDbConnection(@" provider=microsoft.jet.oledb.4.0; data source=e:\prash\stud.mdb");//stud.mdb->access03 filename
            loaddata();
            showdata();
        }
        void loaddata()
        {
            adapter = new OleDbDataAdapter("select * from student", con);
            ds = new DataSet();//student-> table name in stud.accdb/stud.mdb file
            adapter.Fill(ds, "student");
            ds.Tables[0].Constraints.Add("pk_sno", ds.Tables[0].Columns[0], true);//creating primary key for Tables[0] in dataset
        }
        void showdata()
        {
            textBox1.Text = ds.Tables[0].Rows[rno][0].ToString();
            textBox2.Text = ds.Tables[0].Rows[rno][1].ToString();
            textBox3.Text = ds.Tables[0].Rows[rno][2].ToString();
            pictureBox1.Image = null;
            if (ds.Tables[0].Rows[rno][3] != System.DBNull.Value)
            {
                photo_aray = (byte[])ds.Tables[0].Rows[rno][3];
                MemoryStream ms = new MemoryStream(photo_aray);
                pictureBox1.Image = Image.FromStream(ms);
            }
        }
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "jpeg|*.jpg|bmp|*.bmp|all files|*.*";
            DialogResult res = openFileDialog1.ShowDialog();
            if (res == DialogResult.OK)
            {
                pictureBox1.Image = Image.FromFile(openFileDialog1.FileName);
            }
        }
        private void btnInsert_Click(object sender, EventArgs e)
        {
            cmd = new OleDbCommand("insert into student(sno,sname,course,photo) values(" + textBox1.Text + ",'" + textBox2.Text + "','" + textBox3.Text + "',@photo)", con);
            conv_photo();
            con.Open();
            int n = cmd.ExecuteNonQuery();
            con.Close();
            if (n > 0)
            {
                MessageBox.Show("record inserted");
                loaddata();
                rno++;
            }
            else
                MessageBox.Show("insertion failed");
        }
        void conv_photo()
        {
            //converting photo to binary data
            if (pictureBox1.Image != null)
            {
                //using MemoryStream:
                ms = new MemoryStream();
                pictureBox1.Image.Save(ms, ImageFormat.Jpeg);
                byte[] photo_aray = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(photo_aray, 0, photo_aray.Length);
                cmd.Parameters.AddWithValue("@photo", photo_aray);
            }
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                int n = Convert.ToInt32(Interaction.InputBox("Enter sno:", "Search", "20", 100, 100));
                DataRow drow;
                drow = ds.Tables[0].Rows.Find(n);
                if (drow != null)
                {
                    rno = ds.Tables[0].Rows.IndexOf(drow);
                    textBox1.Text = drow[0].ToString();
                    textBox2.Text = drow[1].ToString();
                    textBox3.Text = drow[2].ToString();
                    pictureBox1.Image = null;
                    if (drow[3] != System.DBNull.Value)
                    {
                        photo_aray = (byte[])drow[3];
                        MemoryStream ms = new MemoryStream(photo_aray);
                        pictureBox1.Image = Image.FromStream(ms);
                    }
                }
                else
                    MessageBox.Show("Record Not Found");
            }
            catch
            {
                MessageBox.Show("Invalid Input");
            }
        }
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            cmd = new OleDbCommand("update student set sname='" + textBox2.Text + "', course='" + textBox3.Text + "', photo=@photo where sno=" + textBox1.Text, con);
            conv_photo();
            con.Open();
            int n = cmd.ExecuteNonQuery();
            con.Close();
            if (n > 0)
            {
                MessageBox.Show("Record Updated");
                loaddata();
            }
            else
                MessageBox.Show("Updation Failed");
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            cmd = new OleDbCommand("delete from student where sno=" + textBox1.Text, con);
            con.Open();
            int n = cmd.ExecuteNonQuery();
            con.Close();
            if (n > 0)
            {
                MessageBox.Show("Record Deleted");
                loaddata();
                rno = 0;
                showdata();
            }
            else
                MessageBox.Show("Deletion failed");
        }
        private void btnFirst_Click(object sender, EventArgs e)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                rno = 0;
                showdata();
                MessageBox.Show("First Record");
            }
            else
                MessageBox.Show("no records");
        }
        private void btnPrevious_Click(object sender, EventArgs e)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (rno > 0)
                {
                    rno--;
                    showdata();
                }
                else
                    MessageBox.Show("First Record");
            }
            else
                MessageBox.Show("no records");
        }
        private void btnNext_Click(object sender, EventArgs e)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (rno < ds.Tables[0].Rows.Count - 1)
                {
                    rno++;
                    showdata();
                }
                else
                    MessageBox.Show("Last Record");
            }
            else
                MessageBox.Show("no records");
        }
        private void btnLast_Click(object sender, EventArgs e)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                rno = ds.Tables[0].Rows.Count - 1;
                showdata();
                MessageBox.Show("Last Record");
            }
            else
                MessageBox.Show("no records");
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            textBox1.Text = textBox2.Text = textBox3.Text = "";
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}