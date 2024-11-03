using System.Globalization;
using System.Windows.Data;

namespace SecretStore.Ui.Helper;

public class ValueToTypeConverter : IValueConverter {
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) {
        return value?.GetType() ?? Binding.DoNothing;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) {
        throw new Exception("Cannot convert back to value type");
    }
}
