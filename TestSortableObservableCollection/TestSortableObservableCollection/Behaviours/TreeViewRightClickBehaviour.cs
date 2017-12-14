using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSortableObservableCollection.Behaviours
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Media3D;
    public static class TreeViewRightClickBehaviour
    {
        public static readonly DependencyProperty SelectItemOnRightClickProperty = DependencyProperty.RegisterAttached(
           "SelectItemOnRightClick",
           typeof(bool),
           typeof(TreeViewRightClickBehaviour),
           new UIPropertyMetadata(false, OnSelectItemOnRightClickChanged));

        public static bool GetSelectItemOnRightClick(DependencyObject d)
        {
            return (bool)d.GetValue(SelectItemOnRightClickProperty);
        }

        public static void SetSelectItemOnRightClick(DependencyObject d, bool value)
        {
            d.SetValue(SelectItemOnRightClickProperty, value);
        }
        private static void OnSelectItemOnRightClickChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            bool selectItemOnRightClick = (bool)e.NewValue;

            TreeView treeView = d as TreeView;
            if (treeView != null)
            {
                if (selectItemOnRightClick)
                    treeView.PreviewMouseRightButtonDown += OnPreviewMouseRightButtonDown;
                else
                    treeView.PreviewMouseRightButtonDown -= OnPreviewMouseRightButtonDown;
            }
        }

        private static void OnPreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            var dp = FindTreeViewItem(e.OriginalSource as DependencyObject);

            TreeViewItem treeViewItem = dp as TreeViewItem;

            if (treeViewItem != null)
            {
                //treeViewItem.Selected = true;
                treeViewItem.Focus();
                e.Handled = true;
            }
        }

        public static DependencyObject FindTreeViewItem(DependencyObject source)
        {
            DependencyObject result = source;

            while (result != null && (result is TreeViewItem == false) )
            {
                
                if (result is Visual || result is Visual3D)
                {
                    result = VisualTreeHelper.GetParent(result);
                }
                else
                {
                    // If we're in Logical Land then we must walk 
                    // up the logical tree until we find a 
                    // Visual/Visual3D to get us back to Visual Land.
                    result = LogicalTreeHelper.GetParent(result);
                }
            }

            return result;
        }
    }

}
