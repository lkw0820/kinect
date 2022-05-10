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
    /// Afterlogin.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Afterlogin : Window
    {
        User user = new User();
        /*public Afterlogin()
        {
            InitializeComponent();
            
        }*/
        public Afterlogin(User user)
        {
            InitializeComponent();
            this.user = user;
            lblId.Content = user.Id +"님 환영합니다.";

        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }


        private void btnAni_Click(object sender, RoutedEventArgs e)
        {
            _1125test.game_animal animalwindow = new _1125test.game_animal(user);
            Window.GetWindow(this).Close();
            animalwindow.ShowDialog();
        }

        private void btnFru_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnFood_Click(object sender, RoutedEventArgs e)
        {
            _1125test.game_food foodWindow = new _1125test.game_food(user);
            Window.GetWindow(this).Close();
            foodWindow.ShowDialog();
        }

        private void btnEx_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }
    }
}
