using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace GoReflex
{
    public class Person
    {
        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        private double averTime;

        public double AverTime
        {
            get { return averTime; }
            set { averTime = value; }
        }

        public Person(string name, double averTime)
        {
            this.name = name;
            this.averTime = averTime;
        }

        public override string ToString()
        {
            return string.Format("{0, -12:F3}  {1, 5:F3}", name, averTime);
        }
    }
}
