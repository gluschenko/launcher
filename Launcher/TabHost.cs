using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Markup;

namespace Launcher
{
    public class TabHost
    {
        public StackPanel Panel { get; private set; }
        public Frame Frame { get; private set; }
        public Dictionary<TabButton, Page> Tabs { get; private set; }

        public TabHost(StackPanel panel, Frame frame, TabButton tabPattern, Page[] pages)
        {
            Panel = panel;
            Frame = frame;
            Tabs = new Dictionary<TabButton, Page>();
            //
            panel.Children.Clear();
            foreach (var page in pages)
            {
                var tab = CloneControl(tabPattern);
                tab.Content = page.Title;
                tab.Click += OnClick;
                panel.Children.Add(tab);
                Tabs.Add(tab, page);

                void OnClick(object sender, RoutedEventArgs args)
                {
                    Navigate(page);
                }
            }

            if (pages.Length > 0) Navigate(pages[0]);
        }

        public void Navigate(Page page)
        {
            if (Tabs.ContainsValue(page))
            {
                var pair = Tabs.First(p => p.Value == page);
                var tab = pair.Key;

                Frame.Navigate(page);

                foreach (var _tab in Tabs.Keys)
                {
                    _tab.Select(tab == _tab);
                }

                foreach (var _page in Tabs.Values)
                {
                    try
                    {
                        if (page == _page)
                        {
                            ((ITabPage)_page).OnShown();
                        }
                        else
                        {
                            ((ITabPage)_page).OnHidden();
                        }
                    }
                    catch(Exception e)
                    {
                        MessageBox.Show(e.ToString(), e.GetType().Name, MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        public T CloneControl<T>(T obj) where T : FrameworkElement
        {
            return (T)XamlReader.Parse(XamlWriter.Save(obj));
        }
    }

    public class TabButton : Button
    {
        public Style StyleDefault { get; set; }
        public Style StyleSelected { get; set; }

        public void Select(bool state)
        {
            Style = state ? StyleSelected : StyleDefault;
        }
    }

    public interface ITabPage
    {
        void OnShown();
        void OnHidden();
    }
}
