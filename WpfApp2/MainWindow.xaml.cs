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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Threading;

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public double tikAngle, secondsAngle, startHour;
        public Stopwatch stopWatch;

        public MainWindow()
        {
            InitializeComponent();

            startHour = System.DateTime.Now.Hour;

            this.MouseDown += delegate { DragMove(); };

            

            SetColor();
            SetClock();

            Thread thread1 = new Thread(UpdateClock);
            thread1.Start();
            Thread thread2 = new Thread(TestColor);
            //thread2.Start();

        }
        
        public void SetColor()
        {
            //average days in a month
            int daysNow = (int)(DateTime.Now.Month * 30.436875) + DateTime.Now.Day;

            int[] colorArr = GetRgb(daysNow);
            Console.WriteLine(colorArr[0] + "\n" + colorArr[1] + "\n" + colorArr[2]);

            Color color = Color.FromArgb(255, 
                (byte)colorArr[0], 
                (byte)colorArr[1],
                (byte)colorArr[2]);


            Circle.Fill = new SolidColorBrush(color);
        }


        public int[] GetRgb(int days)
        {
            int[] rgb = { 0, 0, 0 };

            int colorCode = (int)(days * 4.191780821917808);

            int phase = colorCode / 255;
            int remainder = colorCode % 255;

            switch(phase)
            {
                case 0:
                    rgb[0] = 255;
                    rgb[1] = remainder;
                    break;
                case 1:
                    rgb[0] = 255 - remainder;
                    rgb[1] = 255;
                    break;
                case 2:
                    rgb[1] = 255;
                    rgb[2] = remainder;
                    break;
                case 3:
                    rgb[1] = 255 - remainder;
                    rgb[2] = 255;
                    break;
                case 4:
                    rgb[0] = remainder;
                    rgb[2] = 255;
                    break;
                case 5:
                    rgb[0] = 255;
                    rgb[2] = 255 - remainder;
                    break;
            }

            for (int i = 0; i < 3; i++)
            {
                if (rgb[i] < 0)
                    rgb[i] = 0;
                if (rgb[i] > 255)
                    rgb[i] = 255;
            }
            
            return rgb;
        }


        public void SetClock()
        {
            tikAngle = (DateTime.Now.Hour % 12) * (360 / 12);
            secondsAngle = (DateTime.Now.Second % 60) * (360 / 60);

            rect.RenderTransform = new RotateTransform(tikAngle + 90);
            rect2.RenderTransform = new RotateTransform(secondsAngle + 90);
        }

        //thread 
        public void UpdateClock()
        {
            while(true)
            {
                //seconds tick
                Thread.Sleep(1000);
                secondsAngle += (360 / 60);
                this.Dispatcher.Invoke(() =>
                {
                    rect2.RenderTransform = new RotateTransform(secondsAngle + 90);
                });

                //hour tick
                if (System.DateTime.Now.Hour != startHour)
                {
                    tikAngle += 30;
                    this.Dispatcher.Invoke(() =>
                    {
                        rect.RenderTransform = new RotateTransform(tikAngle + 90);
                    });
                    startHour = System.DateTime.Now.Hour;
                }
            }
        }

        //thread
        public void TestColor()
        {
            for(int day =0; day < 366; day++)
            {
                Thread.Sleep(100);
                int[] colorArr = GetRgb(day);
                Console.WriteLine("\n\n" + colorArr[0] + "\n" + colorArr[1] + "\n" + colorArr[2]);

                Color color = Color.FromArgb(255,
                    (byte)colorArr[0],
                    (byte)colorArr[1],
                    (byte)colorArr[2]);


                this.Dispatcher.Invoke(() =>
                {
                    Circle.Fill = new SolidColorBrush(color);

                });
            }
        }
    }
    
}
