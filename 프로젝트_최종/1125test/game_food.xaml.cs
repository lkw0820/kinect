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
    /// game_food.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class game_food : Window
    {
        User user = new User();
        MySqlConnection connection = new MySqlConnection("Server=localhost;Database=mydb;Uid=root;Pwd=inha1958;");
        SpeechSynthesizer ss = new SpeechSynthesizer();
        ThreadStart ts;
        Thread th;

        SpeechRecognitionEngine sre;
        Random rand = new Random();
        System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();

        List<string> food = new List<string> { "apple", "banana", "cookie", "donut", "egg", "french-fries", "garlic", "hamburger", "ice-cream", "juice",
                                                   "kiwi", "lemonade", "meat","nacho","onion","pancake","quesadilla","rice","sandwich","tomato","udon",
                                                    "vinegar","wine","xylitol","yogurt","zucchini" };
        int i;
        int count = 1;
        public game_food(User user)
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
            choices.Add("apple", "banana", "cookie", "donut", "egg", "french-fries", "garlic", "hamburger", "ice-cream", "juice","kiwi", "lemonade", "meat");
            choices.Add("nacho", "onion", "pancake", "quesadilla", "rice", "sandwich", "tomato", "udon","vinegar", "wine", "xylitol", "yogurt", "zucchini");


            var gb = new GrammarBuilder();
            gb.Culture = ri.Culture;
            gb.Append(choices);
            var g = new Grammar(gb);

            sre.LoadGrammar(g);
            

            sre.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(sre_SpeechRecognized);


            ts = new ThreadStart(UserFunc);
            th = new Thread(ts);
            th.Start();


            i = rand.Next(0, food.Count);
            txtQuiz.Text = food[i];
            if (txtQuiz.Text.Equals("apple"))
            {
                img0.Visibility = Visibility.Visible;
                textBlock1.Text = "뜻 : 사과";
            }
            if (txtQuiz.Text.Equals("banana"))
            {
                img1.Visibility = Visibility.Visible;
                textBlock1.Text = "뜻 : 바나나";
            }
            if (txtQuiz.Text.Equals("cookie"))
            {
                img2.Visibility = Visibility.Visible;
                textBlock1.Text = "뜻 : 쿠키";
            }
            if (txtQuiz.Text.Equals("donut"))
            {
                img3.Visibility = Visibility.Visible;
                textBlock1.Text = "뜻 : 도넛";
            }
            if (txtQuiz.Text.Equals("egg"))
            {
                img4.Visibility = Visibility.Visible;
                textBlock1.Text = "뜻 : 달걀";
            }
            if (txtQuiz.Text.Equals("french-fries"))
            {
                img5.Visibility = Visibility.Visible;
                textBlock1.Text = "뜻 : 감자튀김";
            }
            if (txtQuiz.Text.Equals("garlic"))
            {
                img6.Visibility = Visibility.Visible;
                textBlock1.Text = "뜻 : 마늘";
            }
            if (txtQuiz.Text.Equals("hamburger"))
            {
                img7.Visibility = Visibility.Visible;
                textBlock1.Text = "뜻 : 햄버거";
            }
            if (txtQuiz.Text.Equals("ice-cream"))
            {
                img8.Visibility = Visibility.Visible;
                textBlock1.Text = "뜻 : 아이스크림";
            }
            if (txtQuiz.Text.Equals("juice"))
            {
                img9.Visibility = Visibility.Visible;
                textBlock1.Text = "뜻 : 주스";
            }
            if (txtQuiz.Text.Equals("kiwi"))
            {
                img10.Visibility = Visibility.Visible;
                textBlock1.Text = "뜻 : 키위";
            }
            if (txtQuiz.Text.Equals("lemonade"))
            {
                img11.Visibility = Visibility.Visible;
                textBlock1.Text = "뜻 : 레몬에이드";
            }
            if (txtQuiz.Text.Equals("meat"))
            {
                img12.Visibility = Visibility.Visible;
                textBlock1.Text = "뜻 : 고기";
            }
            if (txtQuiz.Text.Equals("nacho"))
            {
                img13.Visibility = Visibility.Visible;
                textBlock1.Text = "뜻 : 나초";
            }
            if (txtQuiz.Text.Equals("onion"))
            {
                img14.Visibility = Visibility.Visible;
                textBlock1.Text = "뜻 : 양파";
            }
            if (txtQuiz.Text.Equals("pancake"))
            {
                img15.Visibility = Visibility.Visible;
                textBlock1.Text = "뜻 : 팬케이크";
            }
            if (txtQuiz.Text.Equals("quesadilla"))
            {
                img16.Visibility = Visibility.Visible;
                textBlock1.Text = "뜻 : 퀘사디아 ( 멕시코 전통 요리 )";
            }
            if (txtQuiz.Text.Equals("rice"))
            {
                img17.Visibility = Visibility.Visible;
                textBlock1.Text = "뜻 : 밥";
            }
            if (txtQuiz.Text.Equals("sandwich"))
            {
                img18.Visibility = Visibility.Visible;
                textBlock1.Text = "뜻 : 샌드위치";
            }
            if (txtQuiz.Text.Equals("tomato"))
            {
                img19.Visibility = Visibility.Visible;
                textBlock1.Text = "뜻 : 토마토";
            }
            if (txtQuiz.Text.Equals("udon"))
            {
                img20.Visibility = Visibility.Visible;
                textBlock1.Text = "뜻 : 우동";
            }
            if (txtQuiz.Text.Equals("vinegar"))
            {
                img21.Visibility = Visibility.Visible;
                textBlock1.Text = "뜻 : 식초";
            }
            if (txtQuiz.Text.Equals("wine"))
            {
                img22.Visibility = Visibility.Visible;
                textBlock1.Text = "뜻 : 와인";
            }
            if (txtQuiz.Text.Equals("xylitol"))
            {
                img23.Visibility = Visibility.Visible;
                textBlock1.Text = "뜻 : 자일리톨";
            }
            if (txtQuiz.Text.Equals("yogurt"))
            {
                img24.Visibility = Visibility.Visible;
                textBlock1.Text = "뜻 : 요거트";
            }
            if (txtQuiz.Text.Equals("zucchini"))
            {
                img25.Visibility = Visibility.Visible;
                textBlock1.Text = "뜻 : 서양 호박";
            }


            timer.Tick += timer_Tick;
            timer.Interval = TimeSpan.FromMilliseconds(1000);
            timer.Start();
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
                food.Remove(txtQuiz.Text);
                txtScore.Text = (int.Parse(txtScore.Text) + 3000 + (int.Parse(txtTimer.Text) * 50) + (int.Parse((e.Result.Confidence * 100).ToString("0"))) * 10).ToString();
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
                if (txtQuiz.Text.Equals("apple"))
                {
                    img0.Visibility = Visibility.Hidden;

                }
                if (txtQuiz.Text.Equals("banana"))
                {
                    img1.Visibility = Visibility.Hidden;
                }
                if (txtQuiz.Text.Equals("cookie"))
                {
                    img2.Visibility = Visibility.Hidden;
                }
                if (txtQuiz.Text.Equals("donut"))
                {
                    img3.Visibility = Visibility.Hidden;
                }
                if (txtQuiz.Text.Equals("egg"))
                {
                    img4.Visibility = Visibility.Hidden;
                }
                if (txtQuiz.Text.Equals("french-fries"))
                {
                    img5.Visibility = Visibility.Hidden;
                }
                if (txtQuiz.Text.Equals("garlic"))
                {
                    img6.Visibility = Visibility.Hidden;
                }
                if (txtQuiz.Text.Equals("hamburger"))
                {
                    img7.Visibility = Visibility.Hidden;
                }
                if (txtQuiz.Text.Equals("ice-cream"))
                {
                    img8.Visibility = Visibility.Hidden;
                }
                if (txtQuiz.Text.Equals("juice"))
                {
                    img9.Visibility = Visibility.Hidden;
                }
                if (txtQuiz.Text.Equals("kiwi"))
                {
                    img10.Visibility = Visibility.Hidden;
                }
                if (txtQuiz.Text.Equals("lemonade"))
                {
                    img11.Visibility = Visibility.Hidden;
                }
                if (txtQuiz.Text.Equals("meat"))
                {
                    img12.Visibility = Visibility.Hidden;
                }
                if (txtQuiz.Text.Equals("nacho"))
                {
                    img13.Visibility = Visibility.Hidden;
                }
                if (txtQuiz.Text.Equals("onion"))
                {
                    img14.Visibility = Visibility.Hidden;
                }
                if (txtQuiz.Text.Equals("pancake"))
                {
                    img15.Visibility = Visibility.Hidden;
                }
                if (txtQuiz.Text.Equals("quesadilla"))
                {
                    img16.Visibility = Visibility.Hidden;
                }
                if (txtQuiz.Text.Equals("rice"))
                {
                    img17.Visibility = Visibility.Hidden;
                }
                if (txtQuiz.Text.Equals("sandwich"))
                {
                    img18.Visibility = Visibility.Hidden;
                }
                if (txtQuiz.Text.Equals("tomato"))
                {
                    img19.Visibility = Visibility.Hidden;
                }
                if (txtQuiz.Text.Equals("udon"))
                {
                    img20.Visibility = Visibility.Hidden;
                }
                if (txtQuiz.Text.Equals("vinegar"))
                {
                    img21.Visibility = Visibility.Hidden;
                }
                if (txtQuiz.Text.Equals("wine"))
                {
                    img22.Visibility = Visibility.Hidden;
                }
                if (txtQuiz.Text.Equals("xylitol"))
                {
                    img23.Visibility = Visibility.Hidden;
                }
                if (txtQuiz.Text.Equals("yogurt"))
                {
                    img24.Visibility = Visibility.Hidden;
                }
                if (txtQuiz.Text.Equals("zucchini"))
                {
                    img25.Visibility = Visibility.Hidden;
                }
                i = rand.Next(0, food.Count);
                txtQuiz.Text = food[i];
                if (txtQuiz.Text.Equals("apple"))
                {
                    img0.Visibility = Visibility.Visible;
                    textBlock1.Text = "뜻 : 사과";
                }
                if (txtQuiz.Text.Equals("banana"))
                {
                    img1.Visibility = Visibility.Visible;
                    textBlock1.Text = "뜻 : 바나나";
                }
                if (txtQuiz.Text.Equals("cookie"))
                {
                    img2.Visibility = Visibility.Visible;
                    textBlock1.Text = "뜻 : 쿠키";
                }
                if (txtQuiz.Text.Equals("donut"))
                {
                    img3.Visibility = Visibility.Visible;
                    textBlock1.Text = "뜻 : 도넛";
                }
                if (txtQuiz.Text.Equals("egg"))
                {
                    img4.Visibility = Visibility.Visible;
                    textBlock1.Text = "뜻 : 달걀";
                }
                if (txtQuiz.Text.Equals("french-fries"))
                {
                    img5.Visibility = Visibility.Visible;
                    textBlock1.Text = "뜻 : 감자튀김";
                }
                if (txtQuiz.Text.Equals("garlic"))
                {
                    img6.Visibility = Visibility.Visible;
                    textBlock1.Text = "뜻 : 마늘";
                }
                if (txtQuiz.Text.Equals("hamburger"))
                {
                    img7.Visibility = Visibility.Visible;
                    textBlock1.Text = "뜻 : 햄버거";
                }
                if (txtQuiz.Text.Equals("ice-cream"))
                {
                    img8.Visibility = Visibility.Visible;
                    textBlock1.Text = "뜻 : 아이스크림";
                }
                if (txtQuiz.Text.Equals("juice"))
                {
                    img9.Visibility = Visibility.Visible;
                    textBlock1.Text = "뜻 : 주스";
                }
                if (txtQuiz.Text.Equals("kiwi"))
                {
                    img10.Visibility = Visibility.Visible;
                    textBlock1.Text = "뜻 : 키위";
                }
                if (txtQuiz.Text.Equals("lemonade"))
                {
                    img11.Visibility = Visibility.Visible;
                    textBlock1.Text = "뜻 : 레몬에이드";
                }
                if (txtQuiz.Text.Equals("meat"))
                {
                    img12.Visibility = Visibility.Visible;
                    textBlock1.Text = "뜻 : 고기";
                }
                if (txtQuiz.Text.Equals("nacho"))
                {
                    img13.Visibility = Visibility.Visible;
                    textBlock1.Text = "뜻 : 나초";
                }
                if (txtQuiz.Text.Equals("onion"))
                {
                    img14.Visibility = Visibility.Visible;
                    textBlock1.Text = "뜻 : 양파";
                }
                if (txtQuiz.Text.Equals("pancake"))
                {
                    img15.Visibility = Visibility.Visible;
                    textBlock1.Text = "뜻 : 팬케이크";
                }
                if (txtQuiz.Text.Equals("quesadilla"))
                {
                    img16.Visibility = Visibility.Visible;
                    textBlock1.Text = "뜻 : 퀘사디아 ( 멕시코 전통 요리 )";
                }
                if (txtQuiz.Text.Equals("rice"))
                {
                    img17.Visibility = Visibility.Visible;
                    textBlock1.Text = "뜻 : 밥";
                }
                if (txtQuiz.Text.Equals("sandwich"))
                {
                    img18.Visibility = Visibility.Visible;
                    textBlock1.Text = "뜻 : 샌드위치";
                }
                if (txtQuiz.Text.Equals("tomato"))
                {
                    img19.Visibility = Visibility.Visible;
                    textBlock1.Text = "뜻 : 토마토";
                }
                if (txtQuiz.Text.Equals("udon"))
                {
                    img20.Visibility = Visibility.Visible;
                    textBlock1.Text = "뜻 : 우동";
                }
                if (txtQuiz.Text.Equals("vinegar"))
                {
                    img21.Visibility = Visibility.Visible;
                    textBlock1.Text = "뜻 : 식초";
                }
                if (txtQuiz.Text.Equals("wine"))
                {
                    img22.Visibility = Visibility.Visible;
                    textBlock1.Text = "뜻 : 와인";
                }
                if (txtQuiz.Text.Equals("xylitol"))
                {
                    img23.Visibility = Visibility.Visible;
                    textBlock1.Text = "뜻 : 자일리톨";
                }
                if (txtQuiz.Text.Equals("yogurt"))
                {
                    img24.Visibility = Visibility.Visible;
                    textBlock1.Text = "뜻 : 요거트";
                }
                if (txtQuiz.Text.Equals("zucchini"))
                {
                    img25.Visibility = Visibility.Visible;
                    textBlock1.Text = "뜻 : 서양 호박";
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
                    string SQL1 = "update kinect set score = " + txtScore.Text.ToString() + " where id = " + "'" + txtUser.Text + "'";
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
                await Task.Delay(100);
                timer.Stop();
                th.Abort();
                timer.IsEnabled = false;

                _1125test.result resultWindow = new _1125test.result(user);
                Window.GetWindow(this).Close();
                resultWindow.ShowDialog();
            }

            txtTimer.Text = (int.Parse(txtTimer.Text) - 1).ToString();
            if (txtTimer.Text.Equals("0"))
            {
                count = count + 1;
                food.Remove(txtQuiz.Text);
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
                if (txtQuiz.Text.Equals("apple"))
                {
                    img0.Visibility = Visibility.Hidden;
                    
                }
                if (txtQuiz.Text.Equals("banana"))
                {
                    img1.Visibility = Visibility.Hidden;
                }
                if (txtQuiz.Text.Equals("cookie"))
                {
                    img2.Visibility = Visibility.Hidden;
                }
                if (txtQuiz.Text.Equals("donut"))
                {
                    img3.Visibility = Visibility.Hidden;
                }
                if (txtQuiz.Text.Equals("egg"))
                {
                    img4.Visibility = Visibility.Hidden;
                }
                if (txtQuiz.Text.Equals("french-fries"))
                {
                    img5.Visibility = Visibility.Hidden;
                }
                if (txtQuiz.Text.Equals("garlic"))
                {
                    img6.Visibility = Visibility.Hidden;
                }
                if (txtQuiz.Text.Equals("hamburger"))
                {
                    img7.Visibility = Visibility.Hidden;
                }
                if (txtQuiz.Text.Equals("ice-cream"))
                {
                    img8.Visibility = Visibility.Hidden;
                }
                if (txtQuiz.Text.Equals("juice"))
                {
                    img9.Visibility = Visibility.Hidden;
                }
                if (txtQuiz.Text.Equals("kiwi"))
                {
                    img10.Visibility = Visibility.Hidden;
                }
                if (txtQuiz.Text.Equals("lemonade"))
                {
                    img11.Visibility = Visibility.Hidden;
                }
                if (txtQuiz.Text.Equals("meat"))
                {
                    img12.Visibility = Visibility.Hidden;
                }
                if (txtQuiz.Text.Equals("nacho"))
                {
                    img13.Visibility = Visibility.Hidden;
                }
                if (txtQuiz.Text.Equals("onion"))
                {
                    img14.Visibility = Visibility.Hidden;
                }
                if (txtQuiz.Text.Equals("pancake"))
                {
                    img15.Visibility = Visibility.Hidden;
                }
                if (txtQuiz.Text.Equals("quesadilla"))
                {
                    img16.Visibility = Visibility.Hidden;
                }
                if (txtQuiz.Text.Equals("rice"))
                {
                    img17.Visibility = Visibility.Hidden;
                }
                if (txtQuiz.Text.Equals("sandwich"))
                {
                    img18.Visibility = Visibility.Hidden;
                }
                if (txtQuiz.Text.Equals("tomato"))
                {
                    img19.Visibility = Visibility.Hidden;
                }
                if (txtQuiz.Text.Equals("udon"))
                {
                    img20.Visibility = Visibility.Hidden;
                }
                if (txtQuiz.Text.Equals("vinegar"))
                {
                    img21.Visibility = Visibility.Hidden;
                }
                if (txtQuiz.Text.Equals("wine"))
                {
                    img22.Visibility = Visibility.Hidden;
                }
                if (txtQuiz.Text.Equals("xylitol"))
                {
                    img23.Visibility = Visibility.Hidden;
                }
                if (txtQuiz.Text.Equals("yogurt"))
                {
                    img24.Visibility = Visibility.Hidden;
                }
                if (txtQuiz.Text.Equals("zucchini"))
                {
                    img25.Visibility = Visibility.Hidden;
                }

                txtTimer.Text = "30";
                i = rand.Next(0, food.Count);
                txtQuiz.Text = food[i];
                if (txtQuiz.Text.Equals("apple"))
                {
                    img0.Visibility = Visibility.Visible;
                    textBlock1.Text = "뜻 : 사과";
                }
                if (txtQuiz.Text.Equals("banana"))
                {
                    img1.Visibility = Visibility.Visible;
                    textBlock1.Text = "뜻 : 바나나";
                }
                if (txtQuiz.Text.Equals("cookie"))
                {
                    img2.Visibility = Visibility.Visible;
                    textBlock1.Text = "뜻 : 쿠키";
                }
                if (txtQuiz.Text.Equals("donut"))
                {
                    img3.Visibility = Visibility.Visible;
                    textBlock1.Text = "뜻 : 도넛";
                }
                if (txtQuiz.Text.Equals("egg"))
                {
                    img4.Visibility = Visibility.Visible;
                    textBlock1.Text = "뜻 : 달걀";
                }
                if (txtQuiz.Text.Equals("french-fries"))
                {
                    img5.Visibility = Visibility.Visible;
                    textBlock1.Text = "뜻 : 감자튀김";
                }
                if (txtQuiz.Text.Equals("garlic"))
                {
                    img6.Visibility = Visibility.Visible;
                    textBlock1.Text = "뜻 : 마늘";
                }
                if (txtQuiz.Text.Equals("hamburger"))
                {
                    img7.Visibility = Visibility.Visible;
                    textBlock1.Text = "뜻 : 햄버거";
                }
                if (txtQuiz.Text.Equals("ice-cream"))
                {
                    img8.Visibility = Visibility.Visible;
                    textBlock1.Text = "뜻 : 아이스크림";
                }
                if (txtQuiz.Text.Equals("juice"))
                {
                    img9.Visibility = Visibility.Visible;
                    textBlock1.Text = "뜻 : 주스";
                }
                if (txtQuiz.Text.Equals("kiwi"))
                {
                    img10.Visibility = Visibility.Visible;
                    textBlock1.Text = "뜻 : 키위";
                }
                if (txtQuiz.Text.Equals("lemonade"))
                {
                    img11.Visibility = Visibility.Visible;
                    textBlock1.Text = "뜻 : 레몬에이드";
                }
                if (txtQuiz.Text.Equals("meat"))
                {
                    img12.Visibility = Visibility.Visible;
                    textBlock1.Text = "뜻 : 고기";
                }
                if (txtQuiz.Text.Equals("nacho"))
                {
                    img13.Visibility = Visibility.Visible;
                    textBlock1.Text = "뜻 : 나초";
                }
                if (txtQuiz.Text.Equals("onion"))
                {
                    img14.Visibility = Visibility.Visible;
                    textBlock1.Text = "뜻 : 양파";
                }
                if (txtQuiz.Text.Equals("pancake"))
                {
                    img15.Visibility = Visibility.Visible;
                    textBlock1.Text = "뜻 : 팬케이크";
                }
                if (txtQuiz.Text.Equals("quesadilla"))
                {
                    img16.Visibility = Visibility.Visible;
                    textBlock1.Text = "뜻 : 퀘사디아 ( 멕시코 전통 요리 )";
                }
                if (txtQuiz.Text.Equals("rice"))
                {
                    img17.Visibility = Visibility.Visible;
                    textBlock1.Text = "뜻 : 밥";
                }
                if (txtQuiz.Text.Equals("sandwich"))
                {
                    img18.Visibility = Visibility.Visible;
                    textBlock1.Text = "뜻 : 샌드위치";
                }
                if (txtQuiz.Text.Equals("tomato"))
                {
                    img19.Visibility = Visibility.Visible;
                    textBlock1.Text = "뜻 : 토마토";
                }
                if (txtQuiz.Text.Equals("udon"))
                {
                    img20.Visibility = Visibility.Visible;
                    textBlock1.Text = "뜻 : 우동";
                }
                if (txtQuiz.Text.Equals("vinegar"))
                {
                    img21.Visibility = Visibility.Visible;
                    textBlock1.Text = "뜻 : 식초";
                }
                if (txtQuiz.Text.Equals("wine"))
                {
                    img22.Visibility = Visibility.Visible;
                    textBlock1.Text = "뜻 : 와인";
                }
                if (txtQuiz.Text.Equals("xylitol"))
                {
                    img23.Visibility = Visibility.Visible;
                    textBlock1.Text = "뜻 : 자일리톨";
                }
                if (txtQuiz.Text.Equals("yogurt"))
                {
                    img24.Visibility = Visibility.Visible;
                    textBlock1.Text = "뜻 : 요거트";
                }
                if (txtQuiz.Text.Equals("zucchini"))
                {
                    img25.Visibility = Visibility.Visible;
                    textBlock1.Text = "뜻 : 서양 호박";
                }

            }


        }
    }
}
