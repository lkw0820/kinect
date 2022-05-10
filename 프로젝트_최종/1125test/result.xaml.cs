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

namespace _1125test
{
    /// <summary>
    /// result.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class result : Window
    {
        User user = new User();
        public result(User user)
        {
            InitializeComponent();
            this.user = user;
            lblResult.Content = user.Score.ToString() + " 점";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _1125test.MainWindow mainWindow = new _1125test.MainWindow();
            Window.GetWindow(this).Close();
            mainWindow.ShowDialog();
        }

        private void btnRank_Click(object sender, RoutedEventArgs e)
        {
            _1125test.rank rankWindow = new _1125test.rank();
            Window.GetWindow(this).Close();
            rankWindow.ShowDialog();
        }

        private void btnReplay_Click(object sender, RoutedEventArgs e)
        {
            _1125test.Afterlogin afterloginWindow = new _1125test.Afterlogin(user);
            Window.GetWindow(this).Close();
            afterloginWindow.ShowDialog();
        }
    }
}
