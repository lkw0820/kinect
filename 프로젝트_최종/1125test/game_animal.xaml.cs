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
using System.IO;
using System.Threading;
using Microsoft.Kinect;
using Microsoft.Speech.Recognition;
using Microsoft.Speech.AudioFormat;
using System.Timers;
using System.Speech.Synthesis;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;


namespace _1125test
{
    /// <summary>
    /// game_animal.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class game_animal : Window
    {
        User user = new User();
        MySqlConnection connection = new MySqlConnection("Server=localhost;Database=mydb;Uid=root;Pwd=inha1958;");
        SpeechSynthesizer ss = new SpeechSynthesizer();
        ThreadStart ts;
        Thread th;

        SpeechRecognitionEngine sre;
        Random rand = new Random();
        System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();

        List<string> animals = new List<string> { "Alligator", "Bear", "Cat", "Deer", "Elephant", "Fox", "Goose", "Horse", "Iguana", "JellyFish",
                                                   "Koala", "Lion", "Mouse","Narwhal","Owl","Penguin","Quail","Rabbit","Sheep","Tiger","Unicorn",
                                                    "Vulture","Whale","X-ray Fish","Yak","Zebra" };
        int i;
        int count = 1;
        public game_animal(User user)
        {
            InitializeComponent();
            this.user = user;
            txtUser.Text = user.Id;

            RecognizerInfo ri = null;

            foreach (RecognizerInfo reinfo in SpeechRecognitionEngine.InstalledRecognizers())
            {
                if (reinfo.Id == "SR_MS_en-US_Kinect_10.0" ||
                reinfo.Id == "SR_MS_en-US_Kinect_11.0")
                {
                    ri = reinfo;
                    break;
                }
            }

            if (ri == null)
            {
                MessageBox.Show("실패");
                return;
            }

            sre = new SpeechRecognitionEngine(ri.Id);

            var choices = new Choices();
            choices.Add("Alligator", "Bear", "Cat", "Deer", "Elephant", "Fox", "Goose", "Horse", "Iguana", "JellyFish", "Koala", "Lion", "Mouse");
            choices.Add("Narwhal", "Owl", "Penguin", "Quail", "Rabbit", "Sheep", "Tiger", "Unicorn", "Vulture", "Whale", "X-ray Fish", "Yak", "Zebra");


            var gb = new GrammarBuilder();
            gb.Culture = ri.Culture;
            gb.Append(choices);
            var g = new Grammar(gb);

            sre.LoadGrammar(g);

            sre.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(sre_SpeechRecognized);


            ts = new ThreadStart(UserFunc);
            th = new Thread(ts);
            th.Start();


            i = rand.Next(0, animals.Count);
            txtQuiz.Text = animals[i];
            if (txtQuiz.Text.Equals("Alligator"))
            {
                img0.Visibility = Visibility.Visible;
                textBlock1.Text = "뜻 : 악어";
            }
            if (txtQuiz.Text.Equals("Bear"))
            {
                img1.Visibility = Visibility.Visible;
                textBlock1.Text = "뜻 : 곰";
            }
            if (txtQuiz.Text.Equals("Cat"))
            {
                img2.Visibility = Visibility.Visible;
                textBlock1.Text = "뜻 : 고양이";
            }
            if (txtQuiz.Text.Equals("Deer"))
            {
                img3.Visibility = Visibility.Visible;
                textBlock1.Text = "뜻 : 사슴";
            }
            if (txtQuiz.Text.Equals("Elephant"))
            {
                img4.Visibility = Visibility.Visible;
                textBlock1.Text = "뜻 : 코끼리";
            }
            if (txtQuiz.Text.Equals("Fox"))
            {
                img5.Visibility = Visibility.Visible;
                textBlock1.Text = "뜻 : 여우";
            }
            if (txtQuiz.Text.Equals("Goose"))
            {
                img6.Visibility = Visibility.Visible;
                textBlock1.Text = "뜻 : 거위";
            }
            if (txtQuiz.Text.Equals("Horse"))
            {
                img7.Visibility = Visibility.Visible;
                textBlock1.Text = "뜻 : 말";
            }
            if (txtQuiz.Text.Equals("Iguana"))
            {
                img8.Visibility = Visibility.Visible;
                textBlock1.Text = "뜻 : 이구아나";
            }
            if (txtQuiz.Text.Equals("JellyFish"))
            {
                img9.Visibility = Visibility.Visible;
                textBlock1.Text = "뜻 : 해파리";
            }
            if (txtQuiz.Text.Equals("Koala"))
            {
                img10.Visibility = Visibility.Visible;
                textBlock1.Text = "뜻 : 코알라";
            }
            if (txtQuiz.Text.Equals("Lion"))
            {
                img11.Visibility = Visibility.Visible;
                textBlock1.Text = "뜻 : 사자";
            }
            if (txtQuiz.Text.Equals("Mouse"))
            {
                img12.Visibility = Visibility.Visible;
                textBlock1.Text = "뜻 : 쥐";
            }
            if (txtQuiz.Text.Equals("Narwhal"))
            {
                img13.Visibility = Visibility.Visible;
                textBlock1.Text = "뜻 : 일각고래";
            }
            if (txtQuiz.Text.Equals("Owl"))
            {
                img14.Visibility = Visibility.Visible;
                textBlock1.Text = "뜻 : 올빼미";
            }
            if (txtQuiz.Text.Equals("Penguin"))
            {
                img15.Visibility = Visibility.Visible;
                textBlock1.Text = "뜻 : 펭귄";
            }
            if (txtQuiz.Text.Equals("Quail"))
            {
                img16.Visibility = Visibility.Visible;
                textBlock1.Text = "뜻 : 메추리";
            }
            if (txtQuiz.Text.Equals("Rabbit"))
            {
                img17.Visibility = Visibility.Visible;
                textBlock1.Text = "뜻 : 토끼";
            }
            if (txtQuiz.Text.Equals("Sheep"))
            {
                img18.Visibility = Visibility.Visible;
                textBlock1.Text = "뜻 : 양";
            }
            if (txtQuiz.Text.Equals("Tiger"))
            {
                img19.Visibility = Visibility.Visible;
                textBlock1.Text = "뜻 : 호랑이";
            }
            if (txtQuiz.Text.Equals("Unicorn"))
            {
                img20.Visibility = Visibility.Visible;
                textBlock1.Text = "뜻 : 유니콘";
            }
            if (txtQuiz.Text.Equals("Vulture"))
            {
                img21.Visibility = Visibility.Visible;
                textBlock1.Text = "뜻 : 대머리독수리";
            }
            if (txtQuiz.Text.Equals("Whale"))
            {
                img22.Visibility = Visibility.Visible;
                textBlock1.Text = "뜻 : 고래";
            }
            if (txtQuiz.Text.Equals("X-ray Fish"))
            {
                img23.Visibility = Visibility.Visible;
                textBlock1.Text = "뜻 : 프리스텔라";
            }
            if (txtQuiz.Text.Equals("Yak"))
            {
                img24.Visibility = Visibility.Visible;
                textBlock1.Text = "뜻 : 야크";
            }
            if (txtQuiz.Text.Equals("Zebra"))
            {
                img25.Visibility = Visibility.Visible;
                textBlock1.Text = "뜻 : 얼룩말";
            }


            timer.Tick += timer_Tick;
            timer.Interval = TimeSpan.FromMilliseconds(1000);
            timer.Start();

        }

      
        async void sre_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {

            if (e.Result.Confidence >= 0.7)
            {
                textBlock2.Text = e.Result.Text;
            }

            txtStatus.Visibility = Visibility.Visible;
            txtStatus.Text = "내가 생각한 답 : " + e.Result.Text + "( 정확도 : " + (e.Result.Confidence * 100).ToString("0") + "% )";

            txtScore.Text = (int.Parse(txtScore.Text) - 300).ToString();



            if (txtQuiz.Text.Equals(textBlock2.Text))
            {
                count = count + 1;
                animals.Remove(txtQuiz.Text);
                txtScore.Text = (int.Parse(txtScore.Text) + 3000 + (int.Parse(txtTimer.Text) * 50) + (int.Parse((e.Result.Confidence*100).ToString("0")))*10).ToString();
                txtResult.Text = "정답 !!\n" + txtQuiz.Text;
                txtResult.Visibility = Visibility.Visible;
                timer.Stop();
                await Task.Delay(100);
                ss.Speak(txtQuiz.Text);
                await Task.Delay(800);
                timer.Start();
                if (count > 8) return;
                txtStatus.Text = null;
                txtTimer.Text = "30";
                txtResult.Visibility = Visibility.Hidden;
                if (txtQuiz.Text.Equals("Alligator"))
                {
                    img0.Visibility = Visibility.Hidden;
                }
                if (txtQuiz.Text.Equals("Bear"))
                {
                    img1.Visibility = Visibility.Hidden;
                }
                if (txtQuiz.Text.Equals("Cat"))
                {
                    img2.Visibility = Visibility.Hidden;
                }
                if (txtQuiz.Text.Equals("Deer"))
                {
                    img3.Visibility = Visibility.Hidden;
                }
                if (txtQuiz.Text.Equals("Elephant"))
                {
                    img4.Visibility = Visibility.Hidden;
                }
                if (txtQuiz.Text.Equals("Fox"))
                {
                    img5.Visibility = Visibility.Hidden;
                }
                if (txtQuiz.Text.Equals("Goose"))
                {
                    img6.Visibility = Visibility.Hidden;
                }
                if (txtQuiz.Text.Equals("Horse"))
                {
                    img7.Visibility = Visibility.Hidden;
                }
                if (txtQuiz.Text.Equals("Iguana"))
                {
                    img8.Visibility = Visibility.Hidden;
                }
                if (txtQuiz.Text.Equals("JellyFish"))
                {
                    img9.Visibility = Visibility.Hidden;
                }
                if (txtQuiz.Text.Equals("Koala"))
                {
                    img10.Visibility = Visibility.Hidden;
                }
                if (txtQuiz.Text.Equals("Lion"))
                {
                    img11.Visibility = Visibility.Hidden;
                }
                if (txtQuiz.Text.Equals("Mouse"))
                {
                    img12.Visibility = Visibility.Hidden;
                }
                if (txtQuiz.Text.Equals("Narwhal"))
                {
                    img13.Visibility = Visibility.Hidden;
                }
                if (txtQuiz.Text.Equals("Owl"))
                {
                    img14.Visibility = Visibility.Hidden;
                }
                if (txtQuiz.Text.Equals("Penguin"))
                {
                    img15.Visibility = Visibility.Hidden;
                }
                if (txtQuiz.Text.Equals("Quail"))
                {
                    img16.Visibility = Visibility.Hidden;
                }
                if (txtQuiz.Text.Equals("Rabbit"))
                {
                    img17.Visibility = Visibility.Hidden;
                }
                if (txtQuiz.Text.Equals("Sheep"))
                {
                    img18.Visibility = Visibility.Hidden;
                }
                if (txtQuiz.Text.Equals("Tiger"))
                {
                    img19.Visibility = Visibility.Hidden;
                }
                if (txtQuiz.Text.Equals("Unicorn"))
                {
                    img20.Visibility = Visibility.Hidden;
                }
                if (txtQuiz.Text.Equals("Vulture"))
                {
                    img21.Visibility = Visibility.Hidden;
                }
                if (txtQuiz.Text.Equals("Whale"))
                {
                    img22.Visibility = Visibility.Hidden;
                }
                if (txtQuiz.Text.Equals("X-ray Fish"))
                {
                    img23.Visibility = Visibility.Hidden;
                }
                if (txtQuiz.Text.Equals("Yak"))
                {
                    img24.Visibility = Visibility.Hidden;
                }
                if (txtQuiz.Text.Equals("Zebra"))
                {
                    img25.Visibility = Visibility.Hidden;
                }
                i = rand.Next(0, animals.Count);
                txtQuiz.Text = animals[i];
                if (txtQuiz.Text.Equals("Alligator"))
                {
                    img0.Visibility = Visibility.Visible;
                    textBlock1.Text = "뜻 : 악어";
                }
                if (txtQuiz.Text.Equals("Bear"))
                {
                    img1.Visibility = Visibility.Visible;
                    textBlock1.Text = "뜻 : 곰";
                }
                if (txtQuiz.Text.Equals("Cat"))
                {
                    img2.Visibility = Visibility.Visible;
                    textBlock1.Text = "뜻 : 고양이";
                }
                if (txtQuiz.Text.Equals("Deer"))
                {
                    img3.Visibility = Visibility.Visible;
                    textBlock1.Text = "뜻 : 사슴";
                }
                if (txtQuiz.Text.Equals("Elephant"))
                {
                    img4.Visibility = Visibility.Visible;
                    textBlock1.Text = "뜻 : 코끼리";
                }
                if (txtQuiz.Text.Equals("Fox"))
                {
                    img5.Visibility = Visibility.Visible;
                    textBlock1.Text = "뜻 : 여우";
                }
                if (txtQuiz.Text.Equals("Goose"))
                {
                    img6.Visibility = Visibility.Visible;
                    textBlock1.Text = "뜻 : 거위";
                }
                if (txtQuiz.Text.Equals("Horse"))
                {
                    img7.Visibility = Visibility.Visible;
                    textBlock1.Text = "뜻 : 말";
                }
                if (txtQuiz.Text.Equals("Iguana"))
                {
                    img8.Visibility = Visibility.Visible;
                    textBlock1.Text = "뜻 : 이구아나";
                }
                if (txtQuiz.Text.Equals("JellyFish"))
                {
                    img9.Visibility = Visibility.Visible;
                    textBlock1.Text = "뜻 : 해파리";
                }
                if (txtQuiz.Text.Equals("Koala"))
                {
                    img10.Visibility = Visibility.Visible;
                    textBlock1.Text = "뜻 : 코알라";
                }
                if (txtQuiz.Text.Equals("Lion"))
                {
                    img11.Visibility = Visibility.Visible;
                    textBlock1.Text = "뜻 : 사자";
                }
                if (txtQuiz.Text.Equals("Mouse"))
                {
                    img12.Visibility = Visibility.Visible;
                    textBlock1.Text = "뜻 : 쥐";
                }
                if (txtQuiz.Text.Equals("Narwhal"))
                {
                    img13.Visibility = Visibility.Visible;
                    textBlock1.Text = "뜻 : 일각고래";
                }
                if (txtQuiz.Text.Equals("Owl"))
                {
                    img14.Visibility = Visibility.Visible;
                    textBlock1.Text = "뜻 : 올빼미";
                }
                if (txtQuiz.Text.Equals("Penguin"))
                {
                    img15.Visibility = Visibility.Visible;
                    textBlock1.Text = "뜻 : 펭귄";
                }
                if (txtQuiz.Text.Equals("Quail"))
                {
                    img16.Visibility = Visibility.Visible;
                    textBlock1.Text = "뜻 : 메추리";
                }
                if (txtQuiz.Text.Equals("Rabbit"))
                {
                    img17.Visibility = Visibility.Visible;
                    textBlock1.Text = "뜻 : 토끼";
                }
                if (txtQuiz.Text.Equals("Sheep"))
                {
                    img18.Visibility = Visibility.Visible;
                    textBlock1.Text = "뜻 : 양";
                }
                if (txtQuiz.Text.Equals("Tiger"))
                {
                    img19.Visibility = Visibility.Visible;
                    textBlock1.Text = "뜻 : 호랑이";
                }
                if (txtQuiz.Text.Equals("Unicorn"))
                {
                    img20.Visibility = Visibility.Visible;
                    textBlock1.Text = "뜻 : 유니콘";
                }
                if (txtQuiz.Text.Equals("Vulture"))
                {
                    img21.Visibility = Visibility.Visible;
                    textBlock1.Text = "뜻 : 대머리독수리";
                }
                if (txtQuiz.Text.Equals("Whale"))
                {
                    img22.Visibility = Visibility.Visible;
                    textBlock1.Text = "뜻 : 고래";
                }
                if (txtQuiz.Text.Equals("X-ray Fish"))
                {
                    img23.Visibility = Visibility.Visible;
                    textBlock1.Text = "뜻 : 프리스텔라";
                }
                if (txtQuiz.Text.Equals("Yak"))
                {
                    img24.Visibility = Visibility.Visible;
                    textBlock1.Text = "뜻 : 야크";
                }
                if (txtQuiz.Text.Equals("Zebra"))
                {
                    img25.Visibility = Visibility.Visible;
                    textBlock1.Text = "뜻 : 얼룩말";
                }

            }

        }

        async void timer_Tick(object sender, EventArgs e)
        {
            if (count > 8)
            {

                txtResult.Text = "Game Over";
                txtResult.Visibility = Visibility.Visible;
                user.Score = int.Parse(this.txtScore.Text);
                bool isMax = false;
                string SQL = "SELECT score FROM kinect where id = " + "'" + txtUser.Text + "'";
                try
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand(SQL, connection);
                    MySqlDataReader rdr = command.ExecuteReader();
                    while (rdr.Read())
                    {
                        var a = rdr[0];
                        if (int.Parse(a.ToString()) < int.Parse(txtScore.Text))
                        {
                            isMax = true;
                        }
                    }
                    connection.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                if (isMax == true)
                {
                    string SQL1 = "update kinect set score = " + txtScore.Text.ToString() + " where id = "+"'"+txtUser.Text+"'";
                    try
                    {
                        connection.Open();
                        MySqlCommand command = new MySqlCommand(SQL1, connection);
                        command.ExecuteNonQuery();

                        connection.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                await Task.Delay(300);
                th.Abort();
                timer.Stop();
                timer.IsEnabled = false;
                
                _1125test.result resultWindow = new _1125test.result(user);
                Window.GetWindow(this).Close();
                resultWindow.ShowDialog();
            }

            txtTimer.Text = (int.Parse(txtTimer.Text) - 1).ToString();
            if (txtTimer.Text.Equals("0"))
            {
                count = count + 1;
                animals.Remove(txtQuiz.Text);
                txtScore.Text = (int.Parse(txtScore.Text) - 1500).ToString();
                txtResult.Text = "실패 ㅠㅠ\n" + "(정답 : " + txtQuiz.Text + " )";
                txtResult.Visibility = Visibility.Visible;
                timer.Stop();
                await Task.Delay(100);
                ss.Speak(txtQuiz.Text);
                await Task.Delay(900);
                timer.Start();
                if (count > 8) return;
                txtResult.Visibility = Visibility.Hidden;
                if (txtQuiz.Text.Equals("Alligator"))
                {
                    img0.Visibility = Visibility.Hidden;
                }
                if (txtQuiz.Text.Equals("Bear"))
                {
                    img1.Visibility = Visibility.Hidden;
                }
                if (txtQuiz.Text.Equals("Cat"))
                {
                    img2.Visibility = Visibility.Hidden;
                }
                if (txtQuiz.Text.Equals("Deer"))
                {
                    img3.Visibility = Visibility.Hidden;
                }
                if (txtQuiz.Text.Equals("Elephant"))
                {
                    img4.Visibility = Visibility.Hidden;
                }
                if (txtQuiz.Text.Equals("Fox"))
                {
                    img5.Visibility = Visibility.Hidden;
                }
                if (txtQuiz.Text.Equals("Goose"))
                {
                    img6.Visibility = Visibility.Hidden;
                }
                if (txtQuiz.Text.Equals("Horse"))
                {
                    img7.Visibility = Visibility.Hidden;
                }
                if (txtQuiz.Text.Equals("Iguana"))
                {
                    img8.Visibility = Visibility.Hidden;
                }
                if (txtQuiz.Text.Equals("JellyFish"))
                {
                    img9.Visibility = Visibility.Hidden;
                }
                if (txtQuiz.Text.Equals("Koala"))
                {
                    img10.Visibility = Visibility.Hidden;
                }
                if (txtQuiz.Text.Equals("Lion"))
                {
                    img11.Visibility = Visibility.Hidden;
                }
                if (txtQuiz.Text.Equals("Mouse"))
                {
                    img12.Visibility = Visibility.Hidden;
                }
                if (txtQuiz.Text.Equals("Narwhal"))
                {
                    img13.Visibility = Visibility.Hidden;
                }
                if (txtQuiz.Text.Equals("Owl"))
                {
                    img14.Visibility = Visibility.Hidden;
                }
                if (txtQuiz.Text.Equals("Penguin"))
                {
                    img15.Visibility = Visibility.Hidden;
                }
                if (txtQuiz.Text.Equals("Quail"))
                {
                    img16.Visibility = Visibility.Hidden;
                }
                if (txtQuiz.Text.Equals("Rabbit"))
                {
                    img17.Visibility = Visibility.Hidden;
                }
                if (txtQuiz.Text.Equals("Sheep"))
                {
                    img18.Visibility = Visibility.Hidden;
                }
                if (txtQuiz.Text.Equals("Tiger"))
                {
                    img19.Visibility = Visibility.Hidden;
                }
                if (txtQuiz.Text.Equals("Unicorn"))
                {
                    img20.Visibility = Visibility.Hidden;
                }
                if (txtQuiz.Text.Equals("Vulture"))
                {
                    img21.Visibility = Visibility.Hidden;
                }
                if (txtQuiz.Text.Equals("Whale"))
                {
                    img22.Visibility = Visibility.Hidden;
                }
                if (txtQuiz.Text.Equals("X-ray Fish"))
                {
                    img23.Visibility = Visibility.Hidden;
                }
                if (txtQuiz.Text.Equals("Yak"))
                {
                    img24.Visibility = Visibility.Hidden;
                }
                if (txtQuiz.Text.Equals("Zebra"))
                {
                    img25.Visibility = Visibility.Hidden;
                }

                txtTimer.Text = "30";
                i = rand.Next(0, animals.Count);
                txtQuiz.Text = animals[i];
                if (txtQuiz.Text.Equals("Alligator"))
                {
                    img0.Visibility = Visibility.Visible;
                    textBlock1.Text = "뜻 : 악어";
                }
                if (txtQuiz.Text.Equals("Bear"))
                {
                    img1.Visibility = Visibility.Visible;
                    textBlock1.Text = "뜻 : 곰";
                }
                if (txtQuiz.Text.Equals("Cat"))
                {
                    img2.Visibility = Visibility.Visible;
                    textBlock1.Text = "뜻 : 고양이";
                }
                if (txtQuiz.Text.Equals("Deer"))
                {
                    img3.Visibility = Visibility.Visible;
                    textBlock1.Text = "뜻 : 사슴";
                }
                if (txtQuiz.Text.Equals("Elephant"))
                {
                    img4.Visibility = Visibility.Visible;
                    textBlock1.Text = "뜻 : 코끼리";
                }
                if (txtQuiz.Text.Equals("Fox"))
                {
                    img5.Visibility = Visibility.Visible;
                    textBlock1.Text = "뜻 : 여우";
                }
                if (txtQuiz.Text.Equals("Goose"))
                {
                    img6.Visibility = Visibility.Visible;
                    textBlock1.Text = "뜻 : 거위";
                }
                if (txtQuiz.Text.Equals("Horse"))
                {
                    img7.Visibility = Visibility.Visible;
                    textBlock1.Text = "뜻 : 말";
                }
                if (txtQuiz.Text.Equals("Iguana"))
                {
                    img8.Visibility = Visibility.Visible;
                    textBlock1.Text = "뜻 : 이구아나";
                }
                if (txtQuiz.Text.Equals("JellyFish"))
                {
                    img9.Visibility = Visibility.Visible;
                    textBlock1.Text = "뜻 : 해파리";
                }
                if (txtQuiz.Text.Equals("Koala"))
                {
                    img10.Visibility = Visibility.Visible;
                    textBlock1.Text = "뜻 : 코알라";
                }
                if (txtQuiz.Text.Equals("Lion"))
                {
                    img11.Visibility = Visibility.Visible;
                    textBlock1.Text = "뜻 : 사자";
                }
                if (txtQuiz.Text.Equals("Mouse"))
                {
                    img12.Visibility = Visibility.Visible;
                    textBlock1.Text = "뜻 : 쥐";
                }
                if (txtQuiz.Text.Equals("Narwhal"))
                {
                    img13.Visibility = Visibility.Visible;
                    textBlock1.Text = "뜻 : 일각고래";
                }
                if (txtQuiz.Text.Equals("Owl"))
                {
                    img14.Visibility = Visibility.Visible;
                    textBlock1.Text = "뜻 : 올빼미";
                }
                if (txtQuiz.Text.Equals("Penguin"))
                {
                    img15.Visibility = Visibility.Visible;
                    textBlock1.Text = "뜻 : 펭귄";
                }
                if (txtQuiz.Text.Equals("Quail"))
                {
                    img16.Visibility = Visibility.Visible;
                    textBlock1.Text = "뜻 : 메추리";
                }
                if (txtQuiz.Text.Equals("Rabbit"))
                {
                    img17.Visibility = Visibility.Visible;
                    textBlock1.Text = "뜻 : 토끼";
                }
                if (txtQuiz.Text.Equals("Sheep"))
                {
                    img18.Visibility = Visibility.Visible;
                    textBlock1.Text = "뜻 : 양";
                }
                if (txtQuiz.Text.Equals("Tiger"))
                {
                    img19.Visibility = Visibility.Visible;
                    textBlock1.Text = "뜻 : 호랑이";
                }
                if (txtQuiz.Text.Equals("Unicorn"))
                {
                    img20.Visibility = Visibility.Visible;
                    textBlock1.Text = "뜻 : 유니콘";
                }
                if (txtQuiz.Text.Equals("Vulture"))
                {
                    img21.Visibility = Visibility.Visible;
                    textBlock1.Text = "뜻 : 대머리독수리";
                }
                if (txtQuiz.Text.Equals("Whale"))
                {
                    img22.Visibility = Visibility.Visible;
                    textBlock1.Text = "뜻 : 고래";
                }
                if (txtQuiz.Text.Equals("X-ray Fish"))
                {
                    img23.Visibility = Visibility.Visible;
                    textBlock1.Text = "뜻 : 프리스텔라";
                }
                if (txtQuiz.Text.Equals("Yak"))
                {
                    img24.Visibility = Visibility.Visible;
                    textBlock1.Text = "뜻 : 야크";
                }
                if (txtQuiz.Text.Equals("Zebra"))
                {
                    img25.Visibility = Visibility.Visible;
                    textBlock1.Text = "뜻 : 얼룩말";
                }
              
            }


        }



        private void UserFunc()
        {
            KinectSensor ks = KinectSensor.KinectSensors[0];
            ks.Start();

            KinectAudioSource source = ks.AudioSource;

            Stream audioStream = source.Start();

            sre.SetInputToAudioStream(audioStream,
                new SpeechAudioFormatInfo(EncodingFormat.Pcm,
                                            16000,
                                            16,
                                            1,
                                            32000,
                                            2,
                                            null));
            sre.RecognizeAsync(RecognizeMode.Multiple);
        }

       
    }
}
