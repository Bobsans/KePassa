using System.Windows;
using System.Windows.Media;

namespace SecretStore.Ui.Helper;

public static class DependencyObjectExtension {
    public static T? FindAncestor<T>(this DependencyObject? current) where T : DependencyObject {
        while (current != null) {
            if (current is T ancestor) {
                return ancestor;
            }

            current = VisualTreeHelper.GetParent(current);
        }

        return null;
    }
}
