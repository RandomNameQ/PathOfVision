using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using System.IO;
using System.Windows.Controls;

namespace PathOfVision.CreateUI
{
    internal class UIHelper
    {
        public static T FindChild<T>(DependencyObject parent, string childName)
             where T : DependencyObject
        {
            if (parent == null) return null;

            T foundChild = null;

            if (parent is FrameworkElement frameworkElement && frameworkElement.Name == childName)
            {
                foundChild = parent as T;
                if (foundChild != null)
                {
                    return foundChild;
                }
            }

            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                foundChild = FindChild<T>(child, childName);
                if (foundChild != null)
                {
                    return foundChild;
                }
            }

            return foundChild;
        }

        public static void CopySettings(CheckBox source, CheckBox destination)
        {
            destination.FontSize = source.FontSize;
            destination.FontStretch = source.FontStretch;
            destination.Margin = source.Margin;
            destination.HorizontalContentAlignment = source.HorizontalContentAlignment;
            destination.VerticalContentAlignment= source.VerticalContentAlignment;
        }

        public static void CopySettings(Image source, Image destination)
        {
            destination.Source = source.Source;
            destination.Width = source.Width;
            destination.HorizontalAlignment= source.HorizontalAlignment;

        }
        public static void CopySettings(ListBoxItem source, ListBoxItem destination)
        {
            destination.VerticalContentAlignment=source.VerticalContentAlignment;
            destination.Width=source.Width;
            destination.HorizontalContentAlignment=source.HorizontalContentAlignment;
        }

    }
}
