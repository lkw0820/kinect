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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace _1125test
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }
        
        MySqlConnection connection = new MySqlConnection("Server=localhost;Database=mydb;Uid=root;Pwd=inha1958;");



        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MySqlCommand IdCheckCommand = new MySqlCommand("select id from kinect " + " where id = '" + this.tbId.Text + "';", connection);//입력한 id가 데이터베이스에 있는 id인가
                MySqlDataReader idReader;
                connection.Open();
                idReader = IdCheckCommand.ExecuteReader();
                int cid = 0; //id개수 체크용
                while (idReader.Read())
                {
                    cid = cid + 1;
                }
                if (cid == 0) //이게 ID가 없는지 먼저 확인을 해주세요.
                {
                    MessageBox.Show("ID가 없습니다, 회원가입을 해주세요!");
                    connection.Close();
                }
                else
                {
                    connection.Close();
                }

                //테이블명 확인바람.
                MySqlCommand SelectCommand = new MySqlCommand("select * from kinect " + " where id = '" + this.tbId.Text + "'and pw = '" + this.tbPw.Password + "';", connection);
                MySqlDataReader myReader;
                connection.Open();
                myReader = SelectCommand.ExecuteReader();
                int count = 0;
                while (myReader.Read())
                {
                    count = count + 1;
                }

                if (count == 1)
                {
                    User user = new User();
                    user.Id = this.tbId.Text;
                    MessageBox.Show("로그인 성공!");
                    //이후 다음화면 이동 소스코드 삽입해야함
                    _1125test.Afterlogin afterloginWindow = new _1125test.Afterlogin(user);
                    Window.GetWindow(this).Close();
                    afterloginWindow.ShowDialog();
                }
                else if (count > 1)
                {
                    MessageBox.Show("유저의 아이디와 패스워드가 중복됩니다.");
                    connection.Close();
                }
                else if (count == 0 && cid == 1)
                {
                    MessageBox.Show("비밀번호가 틀립니다!");
                    connection.Close();
                }
                else
                {
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                connection.Close();
            }


        }

        private void btnSignUp_Click(object sender, RoutedEventArgs e)
        {
            if (tbId.Text.Length<=0 || tbPw.Password.Length <= 0)
            {
                MessageBox.Show("ID와 비밀번호를 입력하세요");
            }
            else
            {
                string insertQuery = "INSERT INTO kinect(id,pw) VALUES('" + tbId.Text + "','" + tbPw.Password + "')";


                connection.Open();
                MySqlCommand command = new MySqlCommand(insertQuery, connection);//insert문, sql 연결

                try
                {

                    if (command.ExecuteNonQuery() == 1)
                    {
                        MessageBox.Show("회원가입이 완료되었습니다.");
                    }
                    else
                    {
                        MessageBox.Show("회원가입에 실패하였습니다.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("회원가입에 실패하였습니다.");
                    MessageBox.Show(ex.Message);
                }
            }
            


            connection.Close();
        }

        private void btnRank_Click(object sender, RoutedEventArgs e)
        {
            rank rank = new rank();
            Window.GetWindow(this).Close();
            rank.ShowDialog();
        }


        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }

        private void tbId_GotFocus(object sender, RoutedEventArgs e)
        {
            tbId.Text = null;
        }

        private void tbPw_GotFocus(object sender, RoutedEventArgs e)
        {
            tbPw.Password = null;
        }
    }
}
