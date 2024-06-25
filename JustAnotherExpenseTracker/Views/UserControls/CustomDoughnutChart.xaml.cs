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
        private LinearGradientBrush transparentBursh;

        public static readonly DependencyProperty ValuesProperty = DependencyProperty.Register("values", typeof(List<double>), typeof(CustomDoughnutChart), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

        public List<double> values
        {
            get { return (List<double>)GetValue(ValuesProperty); }
            set { SetValue(ValuesProperty, value); }
        }

        public static readonly DependencyProperty CategoryNamesProperty = DependencyProperty.Register("categoryNames", typeof(List<string>), typeof(CustomDoughnutChart));

        public List<string> categoryNames
        {
            get { return (List<string>)GetValue(CategoryNamesProperty); }
            set { SetValue(CategoryNamesProperty, value); }
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

        public static readonly DependencyProperty TransparentGradientProperty = DependencyProperty.Register("transparentGradient", typeof(GradientStopCollection), typeof(CustomDoughnutChart));

        public GradientStopCollection transparentGradient
        {
            get { return (GradientStopCollection)GetValue(TransparentGradientProperty); }
            set { SetValue(TransparentGradientProperty, value); }
        }


        public static readonly DependencyProperty HoveredSegmentIndexProperty = DependencyProperty.Register("HoveredSegmentIndex", typeof(int), typeof(CustomDoughnutChart), new FrameworkPropertyMetadata(-1, FrameworkPropertyMetadataOptions.AffectsRender));

        public int HoveredSegmentIndex
        {
            get { return (int)GetValue(HoveredSegmentIndexProperty); }
            set { SetValue(HoveredSegmentIndexProperty, value); }
        }

        public static readonly DependencyProperty DisplayValueProperty = DependencyProperty.Register("displayValue", typeof(string), typeof(CustomDoughnutChart));

        public string displayValue
        {
            get { return (string)GetValue(DisplayValueProperty); }
            set { SetValue(DisplayValueProperty, value); }
        }

        public static readonly DependencyProperty DisplayTextProperty = DependencyProperty.Register("displayText", typeof(string), typeof(CustomDoughnutChart));

        public string displayText
        {
            get { return (string)GetValue(DisplayTextProperty); }
            set { SetValue(DisplayTextProperty, value); }
        }

        public static readonly DependencyProperty DisplayTextColorProperty = DependencyProperty.Register("displayTextColor", typeof(Brush), typeof(CustomDoughnutChart));

        public Brush displayTextColor
        {
            get { return (Brush)GetValue(DisplayTextColorProperty); }
            set { SetValue(DisplayTextColorProperty, value); }
        }


        private void createBrushes()
        {
            fillBrushes = new List<LinearGradientBrush>();
            fillBrushes.Add(new LinearGradientBrush(gradientStops1));
            fillBrushes.Add(new LinearGradientBrush(gradientStops2));
            fillBrushes.Add(new LinearGradientBrush(gradientStops3));
            fillBrushes.Add(new LinearGradientBrush(gradientStops4));

            transparentBursh = new LinearGradientBrush(transparentGradient); // Later if some other color instead of transparent is to be used we can use this property
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
            ResetValuesDisplayedInDoughnut();

            for (int i = 0; i < values.Count; ++i)
            {
                double sweepAngle = (values[i] / totalValue) * 360;
                if(sweepAngle == 360)
                {
                    DrawSegment(dc, 0, 180, innerRadius, outerRadius, fillBrushes[0]);
                    DrawSegment(dc, 180, 180, innerRadius, outerRadius, fillBrushes[0]);

                    displayText = categoryNames[0];
                    displayTextColor = fillBrushes[0];

                    return;
                }
                else
                {
                    bool isHovered = (HoveredSegmentIndex == i);

                    double explodedOffset = isHovered ? 20 : 0;

                    if(isHovered)
                    {
                        DrawSegment(dc, startAngle, sweepAngle, innerRadius, outerRadius, transparentBursh);

                        // Setting the value, text and its color to be displayed inside the doughnut
                        displayValue = "$ " + values[i].ToString();
                        displayText = categoryNames[i];
                        displayTextColor = fillBrushes[i];
                    }

                    DrawSegment(dc, startAngle, sweepAngle, innerRadius + explodedOffset, outerRadius + explodedOffset, fillBrushes[i]);

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
            
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            // Determine which segment the mouse is over
            Point mousePos = e.GetPosition(this);
            double angle = CalculateAngleFromPoint(mousePos);

            int hoveredIndex = -1;
            double startAngle = 0;
            double totalValue = values.Sum();


            for (int i = 0; i < values.Count; i++)
            {
                double sweepAngle = (values[i] / totalValue) * 360;

                if (angle >= startAngle && angle < startAngle + sweepAngle)
                {
                    hoveredIndex = i;
                    break;
                }

                startAngle += sweepAngle;
            }

            HoveredSegmentIndex = hoveredIndex;
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            HoveredSegmentIndex = -1; // Reset hovered segment index
            ResetValuesDisplayedInDoughnut();
        }

        private void ResetValuesDisplayedInDoughnut()
        {
            displayValue = "$ " + values.Sum().ToString();
            displayText = "Total";
            displayTextColor = (Brush)FindResource("creditCardPageTextColor");
        }

        private double CalculateAngleFromPoint(Point point)
        {
            Point center = new Point(ActualWidth / 2, ActualHeight / 2);
            Vector vector = point - center;
            double angle = Vector.AngleBetween(new Vector(1, 0), vector);

            // Ensure angle is within 0 to 360 degrees
            if (angle < 0)
            {
                angle += 360;
            }

            return angle;
        }





    }
}
