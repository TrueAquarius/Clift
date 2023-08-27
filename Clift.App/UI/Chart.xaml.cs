using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using AirfoilView.Model.Draw;
using AirfoilView.Model;
using AirfoilView.Model.Airfoil;

namespace AirfoilView.UI
{
    /// <summary>
    /// Interaktionslogik für Chart.xaml
    /// </summary>
    public partial class Chart : UserControl
    {
        public String Title { set { TitleText.Text = value; } }

       
        public String Xkey { get; set; }
        public double Xmin { get; set; }
        public double Xmax { get; set; }
        public double Xstep { get; set; }
        public double XstepLine { get; set; }

        public string Ykey { get; set; }
        public double Ymin { get; set; }
        public double Ymax { get; set; }
        public double Ystep { get; set; }
        public double YstepLine { get; set; }

        private List<Rectangle> HorizontalLines = new List<Rectangle>();
        private List<Rectangle> VerticalLines = new List<Rectangle>();
        
        private List<Polars> _polars = new();
        private List<List<ChartPoint>> _chartPointList = new();

        public void Add(Polars polars)
        {
            double xScale = (Xmax - Xmin) / (Face.ActualWidth);
            double yScale = (Ymax - Ymin) / (Face.ActualHeight);

            _polars.Add(polars);
            List<ChartPoint> _chartPoints = new();

            foreach (Polar point in polars.polars)
            {
                double x = point.GetPropertyValue(Xkey);
                double y = point.GetPropertyValue(Ykey);

                ChartPoint r = new ChartPoint(point);
                r.Square.Fill = new SolidColorBrush(Global.Instance.getColor(_polars.Count-1));
                r.Width = 4;
                r.Height = 4;
                double left = (x - Xmin) / xScale;
                double top = (Ymax - y) / yScale;
                r.SetValue(Canvas.LeftProperty, left);
                r.SetValue(Canvas.TopProperty, top);

                r.SetValue(Panel.ZIndexProperty, 100 + _polars.Count);
                
                _chartPoints.Add(r);
                ChartArea.Children.Add(r);
            }

            _chartPointList.Add(_chartPoints);

            this.Dispatcher.Invoke(() => {
                RepositionFace();
            });
        }

        public Chart()
        {
            InitializeComponent();
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);

            RepositionFace();
        }



        public void RepositionFace()
        {
            double xScale = (Xmax - Xmin) / (Face.ActualWidth);
            double yScale = (Ymax - Ymin) / (Face.ActualHeight);

            int index = 0;
            foreach(List<ChartPoint> l in _chartPointList)
            {
                foreach (ChartPoint cp in l)
                {
                    if (Global.Instance.Visibility[index] == Visibility.Visible)
                    {
                        if (cp.Polar.GetPropertyValue(Xkey) >= Xmin && cp.Polar.GetPropertyValue(Xkey) <= Xmax &&
                            cp.Polar.GetPropertyValue(Ykey) <= Ymax && cp.Polar.GetPropertyValue(Ykey) >= Ymin)
                        {
                            double left = (cp.Polar.GetPropertyValue(Xkey) - Xmin) / xScale;
                            double top = (Ymax - cp.Polar.GetPropertyValue(Ykey)) / yScale;
                            cp.SetValue(Canvas.LeftProperty, left);
                            cp.SetValue(Canvas.TopProperty, top);
                            cp.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            cp.Visibility = Visibility.Hidden;
                        }
                    }
                    else
                    {
                        cp.Visibility = Visibility.Hidden;
                    }
                }
                ++index;
            }

            DrawGrid();
        }

        public void DrawGrid()
        {
            ChartGrid.Children.RemoveRange(0, ChartGrid.Children.Count);
            double xScale = (Xmax - Xmin) / (Face.ActualWidth);
            double yScale = (Ymax - Ymin) / (Face.ActualHeight);

            // Draw vertical lines
            double xDist = GridLineDist(Xmin, Xmax);
            for (double x = Next(Xmin, xDist); x <= Xmax; x += xDist)
            {
                ChartGrid.Children.Add(CreateLine(x, Ymin, x, Ymax));
                TextBlock t = new TextBlock();
                t.Text = x.ToString();
                t.FontSize = 13;
                t.Foreground = new SolidColorBrush(Colors.White);
                t.HorizontalAlignment = HorizontalAlignment.Center;
                //t.Background = new SolidColorBrush(Colors.RosyBrown);
                t.TextAlignment = TextAlignment.Center;

                double left = (x - Xmin) / xScale;
                double top = (Ymax - Ymin) / yScale;
                t.Width = 20;
                t.SetValue(Canvas.LeftProperty, left - t.Width/2);
                t.SetValue(Canvas.TopProperty, top+4);
                ChartGrid.Children.Add(t);
            }

            // Draw vertical lines
            double yDist = GridLineDist(Ymin, Ymax);
            for (double y = Next(Ymin, yDist); y <= Ymax; y += yDist)
            {
                ChartGrid.Children.Add(CreateLine(Xmin, y, Xmax, y));
                TextBlock t = new TextBlock();
                t.Text = y.ToString();
                t.FontSize = 13;
                t.Foreground = new SolidColorBrush(Colors.White);
                t.HorizontalAlignment = HorizontalAlignment.Right;
                //t.Background = new SolidColorBrush(Colors.RosyBrown);
                t.TextAlignment = TextAlignment.Right;

                double left = 0;
                double top = (Ymax - y) / yScale;
                t.Height = 20;
                t.Width = 50;
                t.SetValue(Canvas.LeftProperty, left - t.Width - 4);
                t.SetValue(Canvas.TopProperty, top - t.Height/2);
                ChartGrid.Children.Add(t);
            }
        }


        private double GridLineDist(double min, double max)
        {
            double d = (max - min); 
            double l = Math.Log10(d);
            double g = Math.Round(l);
            double o = Math.Pow(10, g);
            return o;
        }

        public double Next(double start, double step)
        {
            double nextMultiple = Math.Ceiling(start / step) * step;
            return nextMultiple;
        }

        public Line CreateLine(double x1, double y1, double x2, double y2)
        {
            double xScale = (Xmax - Xmin) / (Face.ActualWidth);
            double yScale = (Ymax - Ymin) / (Face.ActualHeight);
            Line l = new Line();

            l.X1 = (x1 - Xmin) / xScale;
            l.Y1 = (Ymax - y1) / yScale;
            l.X2 = (x2 - Xmin) / xScale;
            l.Y2 = (Ymax - y2) / yScale;

            l.StrokeThickness = 1;
            l.Stroke = new SolidColorBrush(Colors.DarkGray);
            return l;

        }


        public Line CreateLine(AirfoilView.Model.Draw.Point p1, AirfoilView.Model.Draw.Point p2)
        {
            return CreateLine(p1.X, p1.Y, p2.X, p2.Y);
        }

    }
}
