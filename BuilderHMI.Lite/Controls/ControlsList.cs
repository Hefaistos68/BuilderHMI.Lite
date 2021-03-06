﻿using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace BuilderHMI.Lite
{
    // List controls manage a fixed-list of options: Listbox, Dropdown List, Checkboxes and Radio Buttons.

    public class HmiListBox : ListBox, IHmiListControl
    {
        public HmiListBox()
        {
            Focusable = false;
            Elements = "OPTION 1|OPTION 2";
            SetResourceReference(StyleProperty, "ListBoxStyle");
        }

        public static readonly DependencyProperty ElementsProperty =
            DependencyProperty.Register("Elements", typeof(string), typeof(HmiListBox), new FrameworkPropertyMetadata("", OnElementsChanged));

        public string Elements
        {
            get { return (string)GetValue(ElementsProperty); }
            set { SetValue(ElementsProperty, value); }
        }

        private static void OnElementsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as HmiListBox).CreateItems((string)e.NewValue);
        }

        private void CreateItems(string elements)
        {
            Items.Clear();
            string[] labels = elements.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string label in labels)
            {
                if (label.Length > 0)
                {
                    var lbi = new ListBoxItem();
                    lbi.SetResourceReference(StyleProperty, "ListBoxItemStyle");
                    lbi.Content = label;
                    Items.Add(lbi);
                }
            }
        }

        public FrameworkElement fe { get { return this; } }
        public MainWindow OwnerPage { get; set; }
        public string NamePrefix { get { return "listbox"; } }
        public Size InitialSize { get { return new Size(100, 100); } }
        public ECtrlFlags Flags { get { return ECtrlFlags.Resize; } }

        private static HmiItemsProperties properties = new HmiItemsProperties("List Box Properties");
        public UserControl PropertyPage { get { properties.TheControl = this; return properties; } }

        public string ToXaml(int indentLevel, bool vs = false)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < indentLevel; i++) sb.Append("    ");

            if (vs)
            {
                sb.AppendFormat("<ListBox Style=\"{{DynamicResource ListBoxStyle}}\" Name=\"{0}\"", Name);
                OwnerPage.AppendLocationXaml(this, sb);
                sb.AppendLine(">");
                string[] labels = Elements.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string label in labels)
                {
                    for (int i = 0; i < indentLevel + 1; i++) sb.Append("    ");
                    sb.AppendFormat("<ListBoxItem Style=\"{{DynamicResource ListBoxItemStyle}}\" Content=\"{0}\" />\r\n", label);
                }
                for (int i = 0; i < indentLevel; i++) sb.Append("    ");
                sb.Append("</ListBox>");
            }
            else
            {
                sb.AppendFormat("<HmiListBox Name=\"{0}\"", Name);
                if (Elements.Length > 0) sb.AppendFormat(" Elements=\"{0}\"", Elements);
                OwnerPage.AppendLocationXaml(this, sb);
                sb.Append(" />");
            }

            return sb.ToString();
        }
    }

    public class HmiDropdownList : ComboBox, IHmiListControl
    {
        public HmiDropdownList()
        {
            Focusable = false;
            Elements = "OPTION 1|OPTION 2";
            SetResourceReference(StyleProperty, "ComboBoxStyle");
        }

        public static readonly DependencyProperty ElementsProperty =
            DependencyProperty.Register("Elements", typeof(string), typeof(HmiDropdownList), new FrameworkPropertyMetadata("", OnElementsChanged));

        public string Elements
        {
            get { return (string)GetValue(ElementsProperty); }
            set { SetValue(ElementsProperty, value); }
        }

        private static void OnElementsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as HmiDropdownList).CreateItems((string)e.NewValue);
        }

        private void CreateItems(string elements)
        {
            Items.Clear();
            string[] labels = elements.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string label in labels)
            {
                if (label.Length > 0)
                {
                    var cbi = new ComboBoxItem();
                    cbi.SetResourceReference(StyleProperty, "ComboBoxItemStyle");
                    cbi.Content = label;
                    Items.Add(cbi);
                }
            }
        }

        public FrameworkElement fe { get { return this; } }
        public MainWindow OwnerPage { get; set; }
        public string NamePrefix { get { return "dropdown"; } }
        public Size InitialSize { get { return new Size(100, double.NaN); } }
        public ECtrlFlags Flags { get { return ECtrlFlags.ResizeWidth; } }

        private static HmiItemsProperties properties = new HmiItemsProperties("Dropdown List Properties");
        public UserControl PropertyPage { get { properties.TheControl = this; return properties; } }

        public string ToXaml(int indentLevel, bool vs = false)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < indentLevel; i++) sb.Append("    ");

            if (vs)
            {
                sb.AppendFormat("<ComboBox Style=\"{{DynamicResource ComboBoxStyle}}\" Name=\"{0}\"", Name);
                OwnerPage.AppendLocationXaml(this, sb);
                sb.AppendLine(">");
                string[] labels = Elements.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string label in labels)
                {
                    for (int i = 0; i < indentLevel + 1; i++) sb.Append("    ");
                    sb.AppendFormat("<ComboBoxItem Style=\"{{DynamicResource ComboBoxItemStyle}}\" Content=\"{0}\" />\r\n", label);
                }
                for (int i = 0; i < indentLevel; i++) sb.Append("    ");
                sb.Append("</ComboBox>");
            }
            else
            {
                sb.AppendFormat("<HmiDropdownList Name=\"{0}\"", Name);
                if (Elements.Length > 0) sb.AppendFormat(" Elements=\"{0}\"", Elements);
                OwnerPage.AppendLocationXaml(this, sb);
                sb.Append(" />");
            }

            return sb.ToString();
        }
    }

    public class HmiCheckBoxes : StackPanel, IHmiListControl
    {
        public HmiCheckBoxes()
        {
            Background = Brushes.Transparent;
            Elements = "OPTION 1|OPTION 2";
            Value = 0;
        }

        public int Value { get; private set; }

        public static readonly DependencyProperty ElementsProperty =
            DependencyProperty.Register("Elements", typeof(string), typeof(HmiCheckBoxes), new FrameworkPropertyMetadata("", OnElementsChanged));

        public string Elements
        {
            get { return (string)GetValue(ElementsProperty); }
            set { SetValue(ElementsProperty, value); }
        }

        private static void OnElementsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as HmiCheckBoxes).CreateCheckBoxes((string)e.NewValue);
        }

        private void CreateCheckBoxes(string items)
        {
            Children.Clear();
            string[] labels = items.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string label in labels)
            {
                if (label.Length > 0)
                {
                    var cb = new CheckBox();
                    cb.SetResourceReference(StyleProperty, "CheckBoxStyle");
                    cb.Content = label;
                    if (Children.Add(cb) >= 30)
                        break;
                }
            }
        }

        public FrameworkElement fe { get { return this; } }
        public MainWindow OwnerPage { get; set; }
        public string NamePrefix { get { return "check"; } }
        public Size InitialSize { get { return new Size(double.NaN, double.NaN); } }
        public ECtrlFlags Flags { get { return ECtrlFlags.None; } }

        private static HmiItemsProperties properties = new HmiItemsProperties("Checkbox Group Properties");
        public UserControl PropertyPage { get { properties.TheControl = this; return properties; } }

        public string ToXaml(int indentLevel, bool vs = false)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < indentLevel; i++) sb.Append("    ");

            if (vs)
            {
                sb.AppendFormat("<StackPanel Name=\"{0}\"", Name);
                OwnerPage.AppendLocationXaml(this, sb);
                sb.AppendLine(">");
                string[] labels = Elements.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string label in labels)
                {
                    for (int i = 0; i < indentLevel + 1; i++) sb.Append("    ");
                    sb.AppendFormat("<CheckBox Style=\"{{DynamicResource CheckBoxStyle}}\" Content=\"{0}\" />\r\n", label);
                }
                for (int i = 0; i < indentLevel; i++) sb.Append("    ");
                sb.Append("</StackPanel>");
            }
            else
            {
                sb.AppendFormat("<HmiCheckBoxes Name=\"{0}\"", Name);
                if (Elements.Length > 0) sb.AppendFormat(" Elements=\"{0}\"", Elements);
                OwnerPage.AppendLocationXaml(this, sb);
                sb.Append(" />");
            }

            return sb.ToString();
        }
    }

    public class HmiRadioButtons : StackPanel, IHmiListControl
    {
        public HmiRadioButtons()
        {
            Background = Brushes.Transparent;
            Elements = "OPTION 1|OPTION 2";
            Value = -1;
        }

        public int Value { get; private set; }

        public static readonly DependencyProperty ElementsProperty =
            DependencyProperty.Register("Elements", typeof(string), typeof(HmiRadioButtons), new FrameworkPropertyMetadata("", OnElementsChanged));

        public string Elements
        {
            get { return (string)GetValue(ElementsProperty); }
            set { SetValue(ElementsProperty, value); }
        }

        private static void OnElementsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as HmiRadioButtons).CreateRadioButtons((string)e.NewValue);
        }

        private void CreateRadioButtons(string items)
        {
            Children.Clear();
            string[] labels = items.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string label in labels)
            {
                if (label.Length > 0)
                {
                    var rb = new RadioButton();
                    rb.SetResourceReference(StyleProperty, "RadioButtonStyle");
                    rb.Content = label;
                    Children.Add(rb);
                }
            }
        }

        public FrameworkElement fe { get { return this; } }
        public MainWindow OwnerPage { get; set; }
        public string NamePrefix { get { return "radio"; } }
        public Size InitialSize { get { return new Size(double.NaN, double.NaN); } }
        public ECtrlFlags Flags { get { return ECtrlFlags.None; } }

        private static HmiItemsProperties properties = new HmiItemsProperties("Radio Button Group Properties");
        public UserControl PropertyPage { get { properties.TheControl = this; return properties; } }

        public string ToXaml(int indentLevel, bool vs = false)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < indentLevel; i++) sb.Append("    ");

            if (vs)
            {
                sb.AppendFormat("<StackPanel Name=\"{0}\"", Name);
                OwnerPage.AppendLocationXaml(this, sb);
                sb.AppendLine(">");
                string[] labels = Elements.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string label in labels)
                {
                    for (int i = 0; i < indentLevel + 1; i++) sb.Append("    ");
                    sb.AppendFormat("<RadioButton Style=\"{{DynamicResource RadioButtonStyle}}\" Content=\"{0}\" />\r\n", label);
                }
                for (int i = 0; i < indentLevel; i++) sb.Append("    ");
                sb.Append("</StackPanel>");
            }
            else
            {
                sb.AppendFormat("<HmiRadioButtons Name=\"{0}\"", Name);
                if (Elements.Length > 0) sb.AppendFormat(" Elements=\"{0}\"", Elements);
                OwnerPage.AppendLocationXaml(this, sb);
                sb.Append(" />");
            }

            return sb.ToString();
        }
    }

    public class HmiTreeView : TreeView, IHmiListControl
    {
        public HmiTreeView()
        {
            Focusable = false;
            Elements = "Item 1|Item 2|Item 3||Item 4|Item 5";
            SetResourceReference(StyleProperty, "TreeViewStyle");
        }

        public static readonly DependencyProperty ElementsProperty =
            DependencyProperty.Register("Elements", typeof(string), typeof(HmiTreeView), new FrameworkPropertyMetadata("", OnElementsChanged));

        public string Elements
        {
            get { return (string)GetValue(ElementsProperty); }
            set { SetValue(ElementsProperty, value); }
        }

        private static void OnElementsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as HmiTreeView).CreateItems((string)e.NewValue);
        }

        private void CreateItems(string elements)
        {
            Items.Clear();
            ItemCollection items = Items;
            string[] labels = elements.Split('|');  // "Item 1|Item 2|Item 3||Item 4|Item 5"
            foreach (string label in labels)
            {
                if (label.Length > 0)
                {
                    var tvi = new TreeViewItem();
                    tvi.SetResourceReference(StyleProperty, "TreeViewItemStyle");
                    tvi.Header = label;
                    tvi.IsExpanded = true;
                    items.Add(tvi);
                    if (items == Items)
                        items = tvi.Items;
                }
                else
                {
                    items = Items;
                }
            }
        }

        public FrameworkElement fe { get { return this; } }
        public MainWindow OwnerPage { get; set; }
        public string NamePrefix { get { return "tree"; } }
        public Size InitialSize { get { return new Size(100, 100); } }
        public ECtrlFlags Flags { get { return ECtrlFlags.Resize; } }

        private static HmiItemsProperties properties = new HmiItemsProperties("Tree View Properties", "Double pipe \"||\" before top-level items.");
        public UserControl PropertyPage { get { properties.TheControl = this; return properties; } }

        public string ToXaml(int indentLevel, bool vs = false)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < indentLevel; i++) sb.Append("    ");

            if (vs)
            {
                sb.AppendFormat("<TreeView Style=\"{{DynamicResource TreeViewStyle}}\" Name=\"{0}\"", Name);
                OwnerPage.AppendLocationXaml(this, sb);
                sb.AppendLine(">");
                bool subitem = false;
                string[] labels = Elements.Split('|');
                foreach (string label in labels)
                {
                    if (label.Length > 0)
                    {
                        if (subitem)
                        {
                            for (int i = 0; i < indentLevel + 2; i++) sb.Append("    ");
                            sb.AppendFormat("<TreeViewItem Style=\"{{DynamicResource TreeViewItemStyle}}\" Header=\"{0}\" />\r\n", label);
                        }
                        else
                        {
                            for (int i = 0; i < indentLevel + 1; i++) sb.Append("    ");
                            sb.AppendFormat("<TreeViewItem Style=\"{{DynamicResource TreeViewItemStyle}}\" Header=\"{0}\">\r\n", label);
                            subitem = true;
                        }
                    }
                    else if (subitem)
                    {
                        for (int i = 0; i < indentLevel + 1; i++) sb.Append("    ");
                        sb.AppendLine("</TreeViewItem>");
                        subitem = false;
                    }
                }

                if (subitem)
                {
                    for (int i = 0; i < indentLevel + 1; i++) sb.Append("    ");
                    sb.AppendLine("</TreeViewItem>");
                }
                for (int i = 0; i < indentLevel; i++) sb.Append("    ");
                sb.Append("</TreeView>");
            }
            else
            {
                sb.AppendFormat("<HmiTreeView Name=\"{0}\"", Name);
                if (Elements.Length > 0) sb.AppendFormat(" Elements=\"{0}\"", Elements);
                OwnerPage.AppendLocationXaml(this, sb);
                sb.Append(" />");
            }

            return sb.ToString();
        }
    }
}
