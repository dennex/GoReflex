using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Navigation;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Coding4Fun.Phone.Controls;
using System.Windows.Media.Imaging;


namespace GoReflex
{
    public partial class MainPage : PhoneApplicationPage
    {
        public int state;
        DateTime startTime;
        DateTime endTime;
        Random r;
        int numTimes;
        double averTime;
        double totTime;
        InputPrompt input;
        int []scoresCount;
        int currentPoints;
        int prevPoints;
        int[] levelThresh;
        string[] levelLabel;


        System.Windows.Threading.DispatcherTimer dt;

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            (Application.Current as App).score = new List<Person>();
            scoresCount = new int[9];
            for (int i = 0; i < 9; i++)
            {
                scoresCount[i] = 0;
            }
            levelThresh = new int[9];
            levelThresh[0] = 0;
            levelThresh[1] = 20;
            levelThresh[2] = 40;
            levelThresh[3] = 80;
            levelThresh[4] = 160;
            levelThresh[5] = 320;
            levelThresh[6] = 640;
            levelThresh[7] = 1280;
            levelThresh[8] = 2000;
            levelLabel = new string[9];
            levelLabel[0] = "Sloth";
            levelLabel[1] = "Turtle";
            levelLabel[2] = "Rabbit";
            levelLabel[3] = "Bear";
            levelLabel[4] = "Human";
            levelLabel[5] = "Tiger";
            levelLabel[6] = "Shrew";
            levelLabel[7] = "Spiderman";
            levelLabel[8] = "Jedi";
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("Shitttt! Hello world!");
            if (state == 0) // button is red, so we start the timer
            {
                GetReady();
            }
            else if (state == 2)
            {
                processClick();
            }
            else if (state == 1)
            {
                // have to reset the timer
                dt.Stop();
                ResetState();
                button1.Content = "Cheater!\nReset.\nClick Again.";
                
                textBlockAver.Text = "Tic!";
                textBlockReflex.Text = "Tac!";
            }
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            dt = new System.Windows.Threading.DispatcherTimer();
            state = 0;
            button1.Background = new SolidColorBrush(Colors.Red);
            button1.Content = "Click ME!";
            r = new Random();
            
            BitmapImage bi3 = new BitmapImage(new Uri(@"greenbutton.jpg", UriKind.Relative));
            
            
            imageButton.Source = bi3;
            averTime = 0;
        }

        private void TimerEventProcessor(Object myObject, EventArgs myEventArgs)
        {
            if (state == 1)
            {
                dt.Stop(); // not really necessary because processing is fast but we certainly wouldnt want 2 of these processing events going on at the same time
                state = 2;
                button1.Background = new SolidColorBrush(Colors.Green); // Red
                button1.Content = "GO!!!";
                //tone.Play();
                
                startTime = DateTime.Now;
            }
        }

        private void GetReady()
        {
            state = 1;
            button1.Background = new SolidColorBrush(Colors.Yellow); // yellow
            // start the timer
            dt.Tick += new EventHandler(TimerEventProcessor);
            dt.Interval = new TimeSpan(10000000 * ((r.Next() % 3) + 1));
            dt.Start();
            button1.Content = "Get Ready...";
        }

        private void processClick()
        {
            endTime = DateTime.Now;
            TimeSpan totalTime = (endTime - startTime);
            totTime = Convert.ToDouble(totalTime.Milliseconds) / 1000.0 + Convert.ToDouble(totalTime.Seconds);
            textBlockReflex.Text = string.Format("{0:F3}", totTime);
            numTimes++;

            if (numTimes < 3)
            {
                double totalTimes = averTime * (numTimes - 1) + totTime;
                averTime = totalTimes / numTimes;
                // display the times
                textBlockAver.Text = string.Format("{0:F3}", averTime);

                GetReady();
            }
            else
            {
                double totalTimes = averTime * (numTimes - 1) + totTime;

                averTime = totalTimes / numTimes;
                textBlockAver.Text = string.Format("{0:F3}", averTime);

                // show the grade
                ShowGrade(averTime);

                ResetState();
            }

        }

        private void ResetState()
        {
            // reset the accumulators
            numTimes = 0;

            state = 0;
            button1.Background = new SolidColorBrush(Colors.Red); // Red
            button1.Content = "Click Again";
        }

        private void ShowGrade(double averTime)
        {
            prevPoints = currentPoints;
            string message;
            if (averTime > 0.5)
            {// you suck
                message = "Definitely can't drive\nright now!";
                // no points for this crap
                scoresCount[0]++;
            }
            else if (averTime > 0.4)
            {// not too bad
                message = "Not too bad, but\nstill shouldnt drive";
                currentPoints += 2;
                scoresCount[1]++;
            }
            else if (averTime > 0.35)
            {// average slow human
                message = "Average slow human\nFocus.";
                currentPoints += 5;
                scoresCount[2]++;
            }
            else if (averTime > 0.32)
            {// average human
                message = "Average human";
                currentPoints += 10;
                scoresCount[3]++;
            }
            else if (averTime > 0.3)
            {// average fast human
                message = "Average fast human";
                currentPoints += 20;
                scoresCount[4]++;
            }
            else if (averTime > 0.28)
            {// damn fast human
                message = "super fast human";
                currentPoints += 50;
                scoresCount[5]++;
            }
            else if (averTime > 0.25)
            {// outstanding human
                message = "Not human";
                currentPoints += 100;
                scoresCount[6]++;
            }
            else if (averTime > 0.15)
            {// caffeine addict
                message = "Fortune teller";
                currentPoints += 250;
                scoresCount[7]++;
            }
            else
            {// holy shit
                message = "Spider-Human";
                currentPoints += 500;
                scoresCount[8]++;
            }

            // get the guy's name for hall of fame
            input = new InputPrompt();
            input.Value = "<Enter name>";

            // if the guy levelled up, add it to the string
            for (int i = 0;i<9;i++)
            {
                if (prevPoints < levelThresh[i] && currentPoints >= levelThresh[i])
                {
                    message += string.Format("\nyou are now a {0}!", levelLabel[i]);
                }
            }
            input.Message = message;
            input.FontSize = 36;
            input.Completed += new EventHandler<PopUpEventArgs<string, PopUpResult>>(input_Completed);
            input.Show();
        }

        void input_Completed(object sender, PopUpEventArgs<string, PopUpResult> e)
        {
            if (String.Compare(input.Value, "<Enter name>") != 0)
            {
                // add the name to the list
                (Application.Current as App).score.Add(new Person(input.Value, averTime));
                (Application.Current as App).score.Sort(delegate(Person p1, Person p2) { return p1.AverTime.CompareTo(p2.AverTime); });
                // maybe should binary search and insert
            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/scores.xaml", UriKind.Relative));
        }

        private void buttonReset_Click(object sender, RoutedEventArgs e)
        {
            (Application.Current as App).score.Clear();
        }
    }
}