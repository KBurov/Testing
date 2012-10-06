using System.Windows;
using System.Windows.Controls;

namespace Wanderer.Library.Wpf.Controls.Extended
{
    public class ColumnDefinitionExtended : ColumnDefinition
    {
        public static readonly DependencyProperty VisibleProperty;

        public bool Visible { get { return (bool) GetValue(VisibleProperty); } set { SetValue(VisibleProperty, value); } }

        static ColumnDefinitionExtended()
        {
            VisibleProperty = DependencyProperty.Register("Visible",
                                                          typeof(bool),
                                                          typeof(ColumnDefinitionExtended),
                                                          new PropertyMetadata(true, OnVisibleChanged));

            ColumnDefinition.WidthProperty.OverrideMetadata(typeof(ColumnDefinitionExtended),
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
            obj.CoerceValue(ColumnDefinition.WidthProperty);
        }

        private static object CoerceWidth(DependencyObject obj, object nValue)
        {
            return (((ColumnDefinitionExtended) obj).Visible) ? nValue : new GridLength(0);
        }
    }
}