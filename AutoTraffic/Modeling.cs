using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Animation;
using static System.Windows.Forms.LinkLabel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;
using Timer = System.Windows.Forms.Timer;
/*using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
*/
namespace AutoTraffic
{
    public partial class Modeling : Form
    {
        private int CountWays;
        private int CountLines;
        private string roadType;
        private bool trigger = false;

        private bool _isStopped = false;

        //private Car[] cars;
        //private Car[] _reverseCars;


        int wid, y, y1, speed_x = 3, speed_x1 = 4;
        int x1 = -10;
        int x = 300;
        int count = 0;
        int[] speed = { 30, 60, 90 };
        Timer timer = new Timer();

        private bool _isDeterminate = false;
        private bool _isRandom = false;

        private string _law;

        private int _determinateInterval;
        private float _intensity;
        private float _startInterval;
        private float _endInterval;
        private float _randomDispersion;
        private float _mathExpectation;

        private double _lengthBetweenCars = 0;

        private List<List<Car>> _reverseCars;
        private List<List<Car>> _cars;

        public string setRoaType
        {
            set { roadType = value; }
        }
        public int setCountLines
        {
            set { CountLines = value; }
        }

        public int setCountWays
        {
            set { CountWays = value; }
        }
        public Modeling()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
        }

        private void buttonSet_Click(object sender, EventArgs e)
        {
            Settings sett_form = new Settings();
            sett_form.FormClosed += new FormClosedEventHandler(SettingFormClosed);
            sett_form.ShowDialog();
        }

        public void SettingFormClosed(object sender, FormClosedEventArgs e)
        {
            var form = (Settings)sender;

            _isDeterminate = form.IsDeterminate;
            _isRandom = form.IsRandom;

            _intensity = form.Intensity;
            _startInterval = form.StartInterval;
            _endInterval = form.EndInterval;
            _randomDispersion = form.RandomDispersion;
            _mathExpectation = form.MathExpectation;
            _determinateInterval = form.DeterminateInterval;
        }

        private void buttonPause_Click(object sender, EventArgs e)
        {
            _isStopped = true;

            timer.Tick -= new EventHandler(timer1_FirestTick);
            timer.Tick -= new EventHandler(timer1_SecondTick);
            trigger = false;
            timer.Stop();
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            timer.Tick -= new EventHandler(timer1_FirestTick);
            timer.Tick -= new EventHandler(timer1_SecondTick);
            trigger = false;
            timer.Stop();
            x = 0;
            x1 = -wid;
            y = 0;
            y1 = 0; 
            speed_x = 3; 
            speed_x1 = 4;
            this.Refresh();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            wid = ((pictureBox1.Height / (CountLines * CountWays)) - 20);
            if (!this.DesignMode)
            {
                g = e.Graphics;
                Pen pn = new Pen(Color.Blue, 2);

                if (trigger)
                {
                    for (int i = 0; i < CountLines * CountWays; i++)
                    {
                        for (int j = 0; j < _cars.Count; j++)
                        {
                            foreach (var item in _cars[j])
                            {
                                e.Graphics.FillEllipse(Brushes.Aqua, item.cur_x, item.cur_y, wid, wid);
                            }
                            //e.Graphics.FillEllipse(Brushes.Green, cars[j].cur_x, cars[j].cur_y, wid, wid);
                        }
                        for (int j = 0; j < _reverseCars.Count; j++)
                        {
                            foreach (var item in _reverseCars[j])
                            {
                                e.Graphics.FillEllipse(Brushes.Aqua, item.cur_x, item.cur_y, wid, wid);
                            }
                        }
                        //e.Graphics.FillEllipse(Brushes.Red, x, y, wid, wid);
                        if (x >= pictureBox1.Width / 8 || count > 0)
                        {
                            //e.Graphics.FillEllipse(Brushes.Blue, x1, y1, wid, wid);
                        }

                        base.OnPaint(e);
                    }
                }
            }
        }

        Graphics g;



        private void timer1_FirestTick(object sender, EventArgs e)
        {
            int decrease = 0;
            if (!trigger)
            {

            }
            else
            {
                if (x >= pictureBox1.Width / 8 || count > 0)
                {
                    if (((Math.Abs(x - x1) < wid + 20) && (Math.Abs(y1 - y) < wid + 20)))
                    {
                        x1 += speed_x - 1;
                        if ((Math.Abs(y1 - y) < wid + 20 && CountLines > 1))
                        {
                            x1 += speed_x - 1;
                            y1++;
                        }
                        else if (CountLines == 1)
                        {
                            x1 += --speed_x1;
                            decrease++;
                        }

                    }
                    else
                    {
                        speed_x1 = speed_x + 5;
                        x1 += speed_x1;
                    }
                }
                if (x > pictureBox1.Width + wid)
                {
                    count = 1;
                    x = 0;
                }
                if (x1 > pictureBox1.Width + wid)
                {
                    x1 = 0;
                }
            }

            int max = 1000;
            /*x += speed_x; //неа, тут меняем ск-ть
            for (int i = 0; i < _cars.Count; i++)
            {
                //cars[i].cur_x += 5;
            }*/

            for (int j = 0; j < _cars.Count; j++)
            {
                for (var i = 0; i < _cars[j].Count; i++)
                {
                    if (_cars[j][i].cur_x > wid + 10 && !_cars[j][i].isGenerate)
                    {
                        _cars[j][i].isGenerate = true;

                        var index = new Random().Next(0, _cars.Count);

                        GenerateCars(_cars[j][i].cur_x, _cars[index][0].cur_y, index, true);
                    }

                    _cars[j][i].cur_x += _cars[j][i].speed;

                    if (_cars[j][i].cur_x > 200 * wid)
                    {
                        _cars[j].Remove(_cars[j][i]);
                    }
                }
            }

            this.Refresh();
        }
        private void timer1_SecondTick(object sender, EventArgs e)
        {
            int max = 1000;
            //x += speed_x; //неа, тут меняем ск-ть
            /*for (int i = 0; i < _reverseCars.Length; i++)
            {
                if (_reverseCars[i].cur_x < -wid - 100)
                {
                    _reverseCars[i].cur_x = _reverseCars[i].start_x;
                }
                else
                {
                    _reverseCars[i].cur_x -= _reverseCars[i].speed;
                }
            }*/
            for (int j = 0; j < _reverseCars.Count; j++)
            {
                for (var i = 0; i < _reverseCars[j].Count; i++)
                {
                    if (_reverseCars[j][i].cur_x < 2 * wid + 500 + (j * 100) && !_reverseCars[j][i].isGenerate)
                    {
                        _reverseCars[j][i].isGenerate = true;

                        var index = new Random().Next(0, _reverseCars.Count);

                        GenerateCars(_reverseCars[j][i].cur_x, _reverseCars[index][0].cur_y, index, false);
                    }

                    _reverseCars[j][i].cur_x -= _reverseCars[j][i].speed;

                    if (_reverseCars[j][i].cur_x < -200*wid)
                    {
                        _reverseCars[j].Remove(_reverseCars[j][i]);
                    }
                }
            }

            this.Refresh();
        }

        /// <summary>
        /// Создать новую машину.
        /// </summary>
        /// <param name="x">Позиция Х.</param>
        /// <param name="y">Позиция Y.</param>
        /// <param name="index">Индекс полосы для генерации машины.</param>
        /// <param name="isReverse">Генерировать ли авто в противоположную сторону.</param>
        private void GenerateCars(int x, int y, int index, bool isReverse)
        {
            var positionX = 0f;

            if (_isDeterminate)
            {
                positionX = (float)(5 / 0.03) * _determinateInterval;
            }
            else if (_isRandom)
            {
                switch (_law)
                {
                    case "нормальное":
                        var t = new Random().NextDouble();
                        positionX = (float)(1 / Math.Sqrt(2 * Math.PI * Math.Pow(_randomDispersion, 2)) * Math.Exp(-Math.Pow((t - _mathExpectation), 2) / 2 * _randomDispersion));
                        break;
                    case "равномерное":
                        t = new Random().NextDouble();
                        positionX = (float)((t - _startInterval) / (_endInterval - _startInterval));
                        break;
                    case "показательное":
                        t = new Random().NextDouble();
                        positionX = (float)(1 - Math.Exp(-_intensity * t));
                        break;
                }
            }
            else
            {
                positionX = 100f;
            }

            var car = new Car(wid);
            car.start_y = y;
            car.cur_y = y;
            car.speed = new Random().Next(5, 7);

            if (isReverse)
            {
                car.start_x = (int)(x - positionX - 100);
                car.cur_x = (int)(x - positionX - 100);

                _cars[index].Add(car);
            }
            else
            {
                car.start_x = (int)(x + positionX + 100);
                car.cur_x = (int)(x + positionX + 100);

                _reverseCars[index].Add(car);
            }
        }

        public void Lighto4(int x, int y)
        {
            g.FillRectangle(Brushes.Aquamarine, new Rectangle(x, y, wid / 2, (2* wid / 3)));
            g.FillEllipse(Brushes.DarkGreen, x + (wid / 4), y + (2 * wid / 3) / 4, wid / 4, wid / 4);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);

            if (!_isStopped)
            {
                _reverseCars = new List<List<Car>>();
                _cars = new List<List<Car>>();

                for (int i = 0; i < CountLines; i++)
                {
                    var coordYReverse = ((i + CountLines) * pictureBox1.Height / (CountLines * CountWays)) + 5;
                    var coordY = (i * pictureBox1.Height / (CountLines * CountWays)) + 5;

                    _reverseCars.Add(new List<Car>());

                    var reverseCar = new Car(wid);
                    reverseCar.start_x = wid + 600 + (i * 100);
                    reverseCar.cur_x = wid + 600 + (i * 100);
                    reverseCar.start_y = coordYReverse;
                    reverseCar.cur_y = coordYReverse;
                    reverseCar.speed = new Random().Next(5, 7);
                    reverseCar.isGenerate = false;

                    _reverseCars[i].Add(reverseCar);

                    _cars.Add(new List<Car>());

                    var car = new Car(wid);
                    car.start_x = -wid;
                    car.cur_x = -wid;
                    car.start_y = coordY;
                    car.cur_y = coordY;
                    car.speed = new Random().Next(5, 7);
                    car.isGenerate = false;

                    _cars[i].Add(car);
                }
            }

            _isStopped = false;

            using (g = Graphics.FromImage(pictureBox1.Image))
            {
                g.Clear(Color.Black);
                int lines = CountLines;
               
                switch (roadType)
                {
                    case "город":
                        Pen p = new Pen(Color.White, 4);
                        p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                        for (int i = 1; i < lines * CountWays; i++)
                        {
                            if (CountWays > 1 && i == lines)
                            {
                                Pen p1 = new Pen(Color.White, 4);

                                var firstCoord = (i * pictureBox1.Height / (lines * CountWays)) - 5;
                                var secondCoord = (i * pictureBox1.Height / (lines * CountWays)) + 5;

                                g.DrawLine(p1, 0, firstCoord, pictureBox1.Width - 1, firstCoord);
                                g.DrawLine(p1, 0, secondCoord, pictureBox1.Width - 1, secondCoord);
                                continue;
                            }
                            else
                            {
                                for (int j = 1; j < 10; j++)
                                {
                                    var firstCoord = (i * pictureBox1.Height / (lines * CountWays)) - 5;
                                    g.DrawLine(p, 0, firstCoord, (pictureBox1.Width / 9) * j, firstCoord);
                                }
                            }
                        }
                        break;
                    case "загород":
                        p = new Pen(Color.White, 4);
                        p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                        for (int i = 2; i < (lines * CountWays) + 1; i++)
                        {
                            if (CountWays > 1 && i == lines)
                            {
                                Pen p1 = new Pen(Color.White, 4);
                                g.DrawLine(p1, 0, (i * pictureBox1.Height / (lines * CountWays)) - 5, pictureBox1.Width, (i * pictureBox1.Height / (lines * CountWays)) - 5);
                                g.DrawLine(p1, 0, (i * pictureBox1.Height / (lines * CountWays)) + 5, pictureBox1.Width, (i * pictureBox1.Height / (lines * CountWays)) + 5);
                                continue;
                            }
                            g.FillRectangle(Brushes.Green, new Rectangle(0, 0, pictureBox1.Width, wid));
                            g.FillRectangle(Brushes.Green, new Rectangle(0, ((lines * CountWays) - 1) * pictureBox1.Height / (lines * CountWays), pictureBox1.Width, wid));

                            if (i + 2 > (lines * CountWays))
                            {
                                break;
                            }

                            g.DrawLine(p, 0, i * pictureBox1.Height / (lines * CountWays), pictureBox1.Width, i * pictureBox1.Height / (lines * CountWays));
                        }
                        break;
                    case "тоннель":
                        g.FillRectangle(Brushes.Gray, new Rectangle((wid * 2), 0, pictureBox1.Width - (wid * 4), wid/4));
                        g.FillRectangle(Brushes.LightGray, new Rectangle((wid * 2), wid/4, pictureBox1.Width - (wid * 4), pictureBox1.Height - (wid / 2)));
                        g.FillRectangle(Brushes.Gray, new Rectangle((wid * 2), pictureBox1.Height - (wid / 4), pictureBox1.Width - (wid * 4), wid / 4));
                        if (CountWays > 1)
                        {
                            Lighto4(0, 0);
                        }
                        break;
                    default:
                        break;
                }

                //timer = new Timer();
                if (!DesignMode && !trigger)

                {
                    timer.Interval = 30; // каждые 30 миллисекунд
                    timer.Tick += new EventHandler(timer1_FirestTick);
                    timer.Tick += new EventHandler(timer1_SecondTick);
                    //timer.Tick += new EventHandler();
                    timer.Start();
                }
                trigger = true;
            }
        }
    }
}
