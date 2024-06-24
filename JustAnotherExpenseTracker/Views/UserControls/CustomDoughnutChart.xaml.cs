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

namespace JustAnotherExpenseTracker.Views.UserControls
{
    /// <summary>
    /// Interaction logic for CustomDoughnutChart.xaml
    /// </summary>
    public partial class CustomDoughnutChart : UserControl
    {
        public CustomDoughnutChart()
        {
            InitializeComponent();
        }

        private int innerRadius;
        private int outerRadius;

        private List<LinearGradientBrush> fillBrushes;

        public static readonly DependencyProperty ValuesProperty = DependencyProperty.Register("values", typeof(List<double>), typeof(CustomDoughnutChart), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

        public List<double> values
        {
            get { return (List<double>)GetValue(ValuesProperty); }
            set { SetValue(ValuesProperty, value); }
        }

        public static readonly DependencyProperty GradientStopsProperty1 = DependencyProperty.Register("gradientStops1", typeof(GradientStopCollection), typeof(CustomDoughnutChart));

        public GradientStopCollection gradientStops1
        {
            get { return (GradientStopCollection)GetValue(GradientStopsProperty1); }
            set { SetValue(GradientStopsProperty1, value); }
        }

        public static readonly DependencyProperty GradientStopsProperty2 = DependencyProperty.Register("gradientStops2", typeof(GradientStopCollection), typeof(CustomDoughnutChart));

        public GradientStopCollection gradientStops2
        {
            get { return (GradientStopCollection)GetValue(GradientStopsProperty2); }
            set { SetValue(GradientStopsProperty2, value); }
        }

        public static readonly DependencyProperty GradientStopsProperty3 = DependencyProperty.Register("gradientStops3", typeof(GradientStopCollection), typeof(CustomDoughnutChart));

        public GradientStopCollection gradientStops3
        {
            get { return (GradientStopCollection)GetValue(GradientStopsProperty3); }
            set { SetValue(GradientStopsProperty3, value); }
        }

        public static readonly DependencyProperty GradientStopsProperty4 = DependencyProperty.Register("gradientStops4", typeof(GradientStopCollection), typeof(CustomDoughnutChart));

        public GradientStopCollection gradientStops4
        {
            get { return (GradientStopCollection)GetValue(GradientStopsProperty4); }
            set { SetValue(GradientStopsProperty4, value); }
        }


        private void createBrushes()
        {
            fillBrushes = new List<LinearGradientBrush>();
            fillBrushes.Add(new LinearGradientBrush(gradientStops1));
            fillBrushes.Add(new LinearGradientBrush(gradientStops2));
            fillBrushes.Add(new LinearGradientBrush(gradientStops3));
            fillBrushes.Add(new LinearGradientBrush(gradientStops4));
        }

        protected override void OnRender(DrawingContext dc)
        {
            innerRadius = 60;
            outerRadius = 100;

            base.OnRender(dc);

            if (values == null || !values.Any())
                return;

            double totalValue = values.Sum();
            double startAngle = 0;
            createBrushes();

            for(int i = 0; i < values.Count; ++i)
            {
                double sweepAngle = (values[i] / totalValue) * 360;
                if(sweepAngle == 360)
                {
                    DrawSegment(dc, 0, 180, innerRadius, outerRadius, fillBrushes[0]);
                    DrawSegment(dc, 180, 180, innerRadius, outerRadius, fillBrushes[0]);
                    return;
                }
                else
                {
                    DrawSegment(dc, startAngle, sweepAngle, innerRadius, outerRadius, fillBrushes[i]);
                    startAngle += sweepAngle;
                    outerRadius -= 5;
                }
                
            }
        }

        private void DrawSegment(DrawingContext dc, double startAngle, double sweepAngle, double innerRadius, double outerRadius, Brush fill)
        {
            Point center = new Point(ActualWidth / 2, ActualHeight / 2);
            double startAngleRad = startAngle * (Math.PI / 180);
            double sweepAngleRad = sweepAngle * (Math.PI / 180);

            Point innerStart = new Point(center.X + innerRadius * Math.Cos(startAngleRad), center.Y + innerRadius * Math.Sin(startAngleRad));
            Point outerStart = new Point(center.X + outerRadius * Math.Cos(startAngleRad), center.Y + outerRadius * Math.Sin(startAngleRad));

            StreamGeometry geometry = new StreamGeometry();
            using (StreamGeometryContext ctx = geometry.Open())
            {
                ctx.BeginFigure(innerStart, true, true);
                ctx.ArcTo(new Point(center.X + innerRadius * Math.Cos(startAngleRad + sweepAngleRad), center.Y + innerRadius * Math.Sin(startAngleRad + sweepAngleRad)),
                          new Size(innerRadius, innerRadius), 0, sweepAngle > 180, SweepDirection.Clockwise, true, true);
                ctx.LineTo(new Point(center.X + outerRadius * Math.Cos(startAngleRad + sweepAngleRad), center.Y + outerRadius * Math.Sin(startAngleRad + sweepAngleRad)), true, false);
                ctx.ArcTo(outerStart, new Size(outerRadius, outerRadius), 0, sweepAngle > 180, SweepDirection.Counterclockwise, true, true);
            }

            geometry.Freeze();
            dc.DrawGeometry(fill, null, geometry);
        }



    }
}
