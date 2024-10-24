using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace SecretStore;

[ValueConversion(typeof(TreeViewItem), typeof(Thickness))]
public sealed class TreeViewItemAlimentConverter : IValueConverter {
    public double Length { get; set; }

    public object Convert(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture) {
        return value is not TreeViewItem item
            ? new Thickness(0)
            : new Thickness(Length * item.GetDepth(), 0, 0, 0);
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture) {
        throw new NotImplementedException();
    }
}
