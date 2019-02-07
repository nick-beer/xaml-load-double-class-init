using System.Windows;
using System.Windows.Controls;

namespace ControlsBase
{
    public abstract class Numeric<TData> : Control
    {
        public double NumericValue
        {
            get { return (double)GetValue(NumericValueProperty); }
            set { SetValue(NumericValueProperty, value); }
        }

        public static readonly DependencyProperty NumericValueProperty =
            DependencyProperty.Register("NumericValue", typeof(double), typeof(Numeric<TData>), new PropertyMetadata(0d));
    }
}