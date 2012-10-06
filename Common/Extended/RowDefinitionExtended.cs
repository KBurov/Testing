using System.Windows;
using System.Windows.Controls;

namespace Wanderer.Library.Wpf.Controls.Extended
{
    public class RowDefinitionExtended : RowDefinition
    {
        public static readonly DependencyProperty VisibleProperty;

        public bool Visible { get { return (bool) GetValue(VisibleProperty); } set { SetValue(VisibleProperty, value); } }

        static RowDefinitionExtended()
        {
            VisibleProperty = DependencyProperty.Register("Visible",
                                                          typeof(bool),
                                                          typeof(RowDefinitionExtended),
                                                          new PropertyMetadata(true, OnVisibleChanged));

            RowDefinition.HeightProperty.OverrideMetadata(typeof(RowDefinitionExtended),
                                                          new FrameworkPropertyMetadata(new GridLength(1, GridUnitType.Star), null,
                                                                                        CoerceWidth));
        }

        public static void SetVisible(DependencyObject obj, bool nVisible)
        {
            obj.SetValue(VisibleProperty, nVisible);
        }

        public static bool GetVisible(DependencyObject obj)
        {
            return (bool) obj.GetValue(VisibleProperty);
        }

        private static void OnVisibleChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            obj.CoerceValue(RowDefinition.HeightProperty);
        }

        private static object CoerceWidth(DependencyObject obj, object nValue)
        {
            return (((RowDefinitionExtended) obj).Visible) ? nValue : new GridLength(0);
        }
    }
}