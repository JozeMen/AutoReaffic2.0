using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTraffic
{
    internal class Car
    {
        /*        private int speed;
                private int size;
        */
        /*public int Speed
        {
            get { return speed; }
        }

        public int Size
        {
            get { return size; }
        }*/

        private string[] CarsIcons = new string[] 
        {
            "CarRed.ico",
            "Gruz.ico",
            "BusYellow.ico",
            "Bike.ico"
        };

        private string[] ReverseCarsIcons = new string[]
        {
            "BikeLeft.ico",
            "Police.ico",
            "Tractor.ico",
            "Mixer.ico"
        };

        public int speed;
        public int size;
        public int start_x;
        public int start_y;
        public int cur_x;
        public int cur_y;
        public int goal_x;

        public bool isGenerate;

        public Icon IconCar;

        public Car()
        {
            this.speed = new Random().Next(30, 60);
            goal_x = 2_000;
        }

        public Car(int size_num, bool isRevers)
        {
            this.size = size_num;
            this.speed = new Random().Next(0, 30);
            goal_x = 2000;


            if (isRevers)
            {
                var index = new Random().Next(0, ReverseCarsIcons.Length);
                IconCar = new Icon(ReverseCarsIcons[index]);
            }
            else
            {
                var index = new Random().Next(0, CarsIcons.Length);
                IconCar = new Icon(CarsIcons[index]);
            }
        }

        public Car(int speed_num, int size_num)
        {
            this.speed = speed_num;
            this.size = size_num;
            goal_x = 2000;
        }

        public void SetIcon(bool isRevers)
        {
            if (isRevers)
            {
                var index = new Random().Next(0, ReverseCarsIcons.Length);
                IconCar = new Icon(ReverseCarsIcons[index]);
            }
            else
            {
                var index = new Random().Next(0, CarsIcons.Length);
                IconCar = new Icon(CarsIcons[index]);
            }
        }
    }
}
