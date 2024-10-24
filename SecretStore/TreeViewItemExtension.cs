using System.Windows.Controls;
using System.Windows.Media;

namespace SecretStore;

static class TreeViewItemExtension {
    public static TreeViewItem? GetParent(this TreeViewItem item) {
        var parent = VisualTreeHelper.GetParent(item);
        while (parent is not (TreeViewItem or TreeView)) {
            parent = VisualTreeHelper.GetParent(parent!);
        }

        return parent as TreeViewItem;
    }

    public static int GetDepth(this TreeViewItem item) {
        while (item.GetParent() is { } parent) {
            return GetDepth(parent) + 1;
        }

        return 0;
    }
}
