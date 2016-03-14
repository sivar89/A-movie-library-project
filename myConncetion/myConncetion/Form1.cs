using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace myConncetion
{
    public partial class Form1 : Form
    {
        MySqlConnection MyConn3;
        MySqlCommand cmd;
        static string MyConnection3 = "datasource=localhost; database=movie;port=3306;username=root;password=666666";
        Color myRgbColor;

        public Form1()
        {
            InitializeComponent();
            richTextBox1.BackColor = Color.FromArgb(224, 224, 224);

            fill_listbox();
        }
        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.name.Text) ||
                    string.IsNullOrWhiteSpace(this.director.Text) ||
                    string.IsNullOrWhiteSpace(this.stars.Text) ||
                        string.IsNullOrWhiteSpace(this.genre.Text) ||
                    string.IsNullOrWhiteSpace(this.description.Text))
            {
                MessageBox.Show("Please dont leave any field empty");
            }
            else {
                try
                {
                    //This is my insert query in which i am taking input from the user through windows forms
                    string Query = "insert into movie.new_table(name,director,stars,genre,description) values('" + this.name.Text + "','" + this.director.Text + "','" + this.stars.Text + "','" + this.genre.Text + "','" + this.description.Text + "');";

                    //This is  MySqlConnection here i have created the object and pass my connection string.
                    MyConn3 = new MySqlConnection(MyConnection3);

                    //This is command class which will handle the query and connection object.
                    cmd = new MySqlCommand(Query, MyConn3);
                    MySqlDataReader MyReader2;
                    MyConn3.Open();
                    MyReader2 = cmd.ExecuteReader();     // Here our query will be executed and data saved into the database.

                    MessageBox.Show("Movie added");
                }
                catch (Exception ex)

                {
                    MessageBox.Show(ex.Message);
                }
                MyConn3.Close();
            }
            last_record();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            myRgbColor = new Color();
            myRgbColor = Color.FromArgb(255, 153, 51);

            MyConn3 = new MySqlConnection(MyConnection3);
            if (string.IsNullOrWhiteSpace(this.textBox6.Text))
            {
                MessageBox.Show("Please enter a name");
            }
            else {
                string test1 = "select count(*) from new_table where name='" + this.textBox6.Text + "'";
                cmd = new MySqlCommand(test1, MyConn3);
                try
                {
                    MyConn3.Open();
                    Int32 count = Convert.ToInt32(cmd.ExecuteScalar());

                    if (count == 0)
                    {
                        MessageBox.Show("Movie not found");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            MyConn3.Close();

            string query = ("select director, name, stars, genre, description from new_table where name='" + this.textBox6.Text + "'");
            getData(query);
        }

        public void fill_listbox()
        {
            MyConn3 = new MySqlConnection(MyConnection3);
            string test = ("select name from new_table");
            cmd = new MySqlCommand(test, MyConn3);

            MySqlDataReader MyReader2;
            try
            {
                MyConn3.Open();
                MyReader2 = cmd.ExecuteReader();
                while (MyReader2.Read())
                {
                    string eName = MyReader2.GetString("name");
                    listBox1.Items.Add(eName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            MyConn3.Close();
        }
        public void last_record()
        {
            MyConn3 = new MySqlConnection(MyConnection3);
            string test = ("SELECT name FROM new_table order by id desc limit 1");
            cmd = new MySqlCommand(test, MyConn3);

            try
            {
                MyConn3.Open();
                string meep = (string)cmd.ExecuteScalar();

                listBox1.Items.Add(meep);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            MyConn3.Close();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string query = "select director, name, stars, genre, description from new_table where name='" + listBox1.SelectedItem.ToString() + "'";
            getData(query);
        }
        public void style_info()
        {
            myRgbColor = new Color();
            myRgbColor = Color.FromArgb(255, 153, 51);
            string[] col = { "Name:", "Director:", "Stars:", "Genre:", "Description:" };

            for (int i = 0; i < col.Length; i++)
            {
                richTextBox1.Select(richTextBox1.Text.IndexOf(col[i]), col[i].Length);
                richTextBox1.SelectionFont = new Font("Lucida Handwriting", 12, FontStyle.Bold);
                richTextBox1.SelectionColor = myRgbColor;
            }
        }

        public void getData(string query)
        {
            cmd = new MySqlCommand(query, MyConn3);
            MySqlDataReader MyReader2;
            try
            {
                MyConn3.Open();
                MyReader2 = cmd.ExecuteReader();
                while (MyReader2.Read())
                {
                    for (int i = 0; i < MyReader2.FieldCount; i++)
                    {
                        richTextBox1.ReadOnly = true;
                        string name = " Name: ";
                        string director = " Director: ";
                        string stars = " Stars: ";
                        string genre = " Genre: ";
                        string description = " Description: ";

                        richTextBox1.Text = name + Environment.NewLine + Environment.NewLine + MyReader2["name"].ToString() + "    "
                            + Environment.NewLine + Environment.NewLine
                            + director
                            + Environment.NewLine + Environment.NewLine
                            + MyReader2["director"].ToString() + "    "

                             + Environment.NewLine + Environment.NewLine
                            + stars
                             + Environment.NewLine + Environment.NewLine
                            + MyReader2["stars"].ToString() + "    "

                             + Environment.NewLine + Environment.NewLine
                             + genre
                             + Environment.NewLine + Environment.NewLine
                             + MyReader2["genre"].ToString() + "    "

                             + Environment.NewLine + Environment.NewLine
                             + description
                             + Environment.NewLine + Environment.NewLine
                             + MyReader2["description"].ToString() + "    ";

                        style_info();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            MyConn3.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {           
            try
            {
                string query = "delete from new_table where name='" + listBox1.SelectedItem+ "'";
                MyConn3 = new MySqlConnection(MyConnection3);
                MySqlCommand MyCommand2 = new MySqlCommand(query, MyConn3);
                MySqlDataReader MyReader2;
                MyConn3.Open();
                MyReader2 = MyCommand2.ExecuteReader();
                MessageBox.Show("Movie Deleted");

                listBox1.Items.Clear();
                fill_listbox();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            MyConn3.Close();           
        }
       
    }

}