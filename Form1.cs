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
using System.Xml.Linq;
using CefSharp;
using CefSharp.WinForms;

namespace FilmArsivi2
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
            GetFilms();
            chromiumWebBrowser2.Load("https://www.google.com");
            
        }
        public class Film
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Category { get; set; }
            public string Path { get; set; }

            public bool Statu { get; set; }

        }
        string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=FilmArsivi;";


        public void GetFilms()
        {
            string query = "SELECT * FROM Films";
            List<Film> films = new List<Film>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Film film = new Film()
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Category = reader.GetString(2),
                            Path = reader.GetString(3),
                            Statu = reader.GetBoolean(4)
                        };
                        films.Add(film);
                    }
                }

                reader.Close();

                dataGridView1.DataSource = films;
            }
            
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (tbxCategory.Text.Length>0&& tbxName.Text.Length>0&& tbxPath.Text.Length>0)
            {
                try
                {
                    Film newFilm = new Film()
                    {
                        Category = tbxCategory.Text,
                        Name = tbxName.Text,
                        Path = tbxPath.Text,
                        Statu = cbxStatu.Checked == true ? true : false
                    };
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        string query = "INSERT INTO Films (Name, Category, Path, Statu) VALUES (@Name, @Category, @Path, @Statu)";

                        SqlCommand command = new SqlCommand(query, connection);

                        command.Parameters.AddWithValue("@Name", newFilm.Name);
                        command.Parameters.AddWithValue("@Category", newFilm.Category);
                        command.Parameters.AddWithValue("@Path", newFilm.Path);
                        command.Parameters.AddWithValue("@Statu", newFilm.Statu);

                        command.ExecuteNonQuery();

                        connection.Close();
                    }
                    MessageBox.Show("Eklendi");
                    GetFilms();

                }
                catch (Exception err)
                {
                    MessageBox.Show(err.Message);
                }
            }
            else
            {
                MessageBox.Show("Lütfen tüm alanları doldurun");
            }

        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int selected = dataGridView1.SelectedCells[0].RowIndex;
            string link = dataGridView1.Rows[selected].Cells[3].Value.ToString();
            chromiumWebBrowser1.Load(link);

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        public class Renk
        {
            public int Id { get; set; }
            public string HtmlCode { get; set; }
        }

        private void btnColor_Click(object sender, EventArgs e)
        {
            List<Renk> colors = new List<Renk>();
            Renk color1 = new Renk() { Id = 1, HtmlCode = "#FFFFFF" };
            colors.Add(color1);
            Renk color2 = new Renk() { Id = 2, HtmlCode = "#FF0000" };
            colors.Add(color2);
            Renk color3 = new Renk() { Id = 3, HtmlCode = "#00FF00" };
            colors.Add(color3);
            Renk color4 = new Renk() { Id = 4, HtmlCode = "#0000FF" };
            colors.Add(color4);
            Renk color5 = new Renk() { Id = 5, HtmlCode = "#D3D3D3" };
            colors.Add(color5);
            Renk color6 = new Renk() { Id = 6, HtmlCode = "#9ACD32" };
            colors.Add(color6);
            Renk color7 = new Renk() { Id = 7, HtmlCode = "#DA70D6" };
            colors.Add(color7);
            Random random = new Random();
            int randomNumber = random.Next(1, 8);
            Renk renk = colors.Where(x => x.Id == randomNumber).First();
            BackColor = ColorTranslator.FromHtml(renk.HtmlCode);
        }
    }
}
