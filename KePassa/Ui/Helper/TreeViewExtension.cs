using System.Windows.Controls;

namespace SecretStore.Ui.Helper;

public static class TreeViewExtension {
    public static void SelectTreeViewItem(this TreeView treeView, object itemValue) {
        var treeViewItem = FindTreeViewItem(treeView, itemValue);

        if (treeViewItem != null) {
            treeViewItem.IsSelected = true;
            treeViewItem.BringIntoView();
        }
    }

    private static TreeViewItem? FindTreeViewItem(this ItemsControl? parent, object itemValue) {
        if (parent == null) {
            return null;
        }

        for (var i = 0; i < parent.Items.Count; i++) {
            if (parent.ItemContainerGenerator.ContainerFromIndex(i) is TreeViewItem childItem) {
                if (childItem.DataContext == itemValue) {
                    return childItem;
                }

                var foundItem = FindTreeViewItem(childItem, itemValue);
                if (foundItem != null) {
                    return foundItem;
                }
            }
        }

        return null;
    }
}
