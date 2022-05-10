using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;

namespace _1125test
{
    /// <summary>
    /// rank.xaml에 대한 상호 작용 논리
    /// </summary>
    ///  

    public partial class rank : Window
    {
        //MySqlConnection connection = new MySqlConnection("Server=localhost;Database=kinect;Uid=root;Pwd=inha1958;");
        MySqlConnection connection = new MySqlConnection("Server=localhost;Database=mydb;Uid=root;Pwd=inha1958;");
        public rank()
        {
            InitializeComponent();
            //string SQL = "SELECT ranking, id, score FROM User";
            string SQL = "SELECT rank() over(order by score desc) as ranking, id, score FROM kinect ORDER BY ranking limit 10";
            txtRank.Text = "\n순위\t\t아이디\t\t점수";
            try
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand(SQL, connection);
                MySqlDataReader rdr = command.ExecuteReader();
                while (rdr.Read())
                {
                    txtRank.Text += "\n" + rdr[0] + "\t\t" + rdr[1] + "\t\t" + rdr[2];
                }
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }



        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            string SQL = "SELECT ranking ,id, score FROM(SELECT rank() over(order by score desc) as ranking, id, score FROM kinect) as a WHERE id = '" + txtSearch.Text + "'"; ;
            txtRank.Text = "\n순위\t\t아이디\t\t점수";
            try
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand(SQL, connection);
                MySqlDataReader rdr = command.ExecuteReader();
                if (rdr.Read())
                {
                    txtRank.Text += "\n" + rdr[0] + "\t\t" + rdr[1] + "\t\t" + rdr[2];
                }
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btn_back_Click(object sender, RoutedEventArgs e)
        {
            _1125test.MainWindow mainWindow = new _1125test.MainWindow();
            Window.GetWindow(this).Close();
            mainWindow.ShowDialog();
        }

        private void txtSearch_GotFocus(object sender, RoutedEventArgs e)
        {
            txtSearch.Text = null;
        }
    }
}
