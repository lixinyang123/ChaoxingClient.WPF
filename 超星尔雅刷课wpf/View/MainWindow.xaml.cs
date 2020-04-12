using System.ComponentModel;
using Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT;
using Panuon.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls.Primitives;
using 超星尔雅刷课wpf.Service;

namespace 超星尔雅刷课wpf.View
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : PUWindow
    {
        private string script;
        private bool isLoading = false;
        private SearchWindow searchWindow;
        private MainServiceManager manager;

        public MainWindow()
        {
            InitializeComponent();
            manager = new MainServiceManager();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            script = await manager.GetScript();
            OperationBar.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// 开始刷课按钮
        /// </summary>
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (btn_Start.IsChecked == true)
            {
                //执行刷课脚本
                await webview.InvokeScriptAsync("btn_start", new List<string>());
                startWarning.Visibility = Visibility.Visible;
            }
            else
            {
                webview.Refresh();
            }
        }

        private void Home_Click(object sender, RoutedEventArgs e)
        {
            webview.Navigate(new Uri("http://i.mooc.chaoxing.com/ "));
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (webview.CanGoBack)
                webview.GoBack();
        }

        private void Btn_search_Click(object sender, RoutedEventArgs e)
        {
            if(searchWindow == null)
            {
                searchWindow = new SearchWindow();
                searchWindow.Closing += new CancelEventHandler((object obj , CancelEventArgs args)=> {
                    searchWindow = null;
                });
                searchWindow.Show();
            }
        }


        private void Feedback_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("http://www.lllxy.net/feedback/");
        }

        private async void Reset_Click(object sender, RoutedEventArgs e)
        {
            string str = @"function clearCookie() {var keys = document.cookie.match(/[^ =;]+(?=\=)/g);if (keys) {for (var i = keys.length; i--;) {document.cookie = keys[i] + '=0;path=/;expires=' + new Date(0).toUTCString();document.cookie = keys[i] + '=0;path=/;domain=' + document.domain + ';expires=' + new Date(0).toUTCString();document.cookie = keys[i] + '=0;path=/;domain=kevis.com;expires=' + new Date(0).toUTCString();}}}clearCookie();";
            await webview.InvokeScriptAsync("eval", new[] { str });
            webview.Navigate(new Uri("http://i.mooc.chaoxing.com/ "));
        }

        /// <summary>
        /// 修改脚本参数按钮（静音/自动答题）
        /// </summary>
        private void ResetScript_Click(object sender, RoutedEventArgs e)
        {
            btn_Start.IsChecked = false;

            string startStr;
            ToggleButton button = sender as ToggleButton;
            if (button.Name == "auto_answer")
            {
                startStr = "auto_answer: ";
            }
            else
            {
                startStr = "muted: ";
            }

            string endStr = ",";
            int start = script.IndexOf(startStr) + startStr.Length;
            int end = script.IndexOf(endStr, start);

            script = script.Remove(start, end - start);

            if (button.IsChecked == true)
            {
                script = script.Insert(start, "true");
            }
            else
            {
                script = script.Insert(start, "false");
            }

            webview.Refresh();
        }

        private void Openbrowser_Click(object sender, RoutedEventArgs e) => Process.Start(webview.Source.ToString());

        private void Webview_NavigationStarting(object sender, WebViewControlNavigationStartingEventArgs e)
        {
            if(isLoading == false)
            {
                webview.Visibility = Visibility.Hidden;
                PUMessageBox.ShowAwait("努力加载中....", "超星尔雅刷课",
                    new RoutedEventHandler((object obj , RoutedEventArgs arg)=> {
                        if (webview.CanGoBack)
                            webview.GoBack();
                    }),
                    AnimationStyles.Gradual
                );
                isLoading = true;
            }
            startWarning.Visibility = Visibility.Collapsed;
        }

        private async void Webview_NavigationCompleted(object sender, WebViewControlNavigationCompletedEventArgs e)
        {
            await webview.InvokeScriptAsync("eval", new[] { script });

            //如果进入视频页面，解锁刷课按钮
            btn_Start.IsEnabled = false;
            if (e.Uri.ToString().Contains("study"))
            {
                btn_Start.IsEnabled = true;
            }
            else
            {
                btn_Start.IsChecked = false;
            }

            if (isLoading == true)
            {
                PUMessageBox.CloseAwait();
                webview.Visibility = Visibility.Visible;
                isLoading = false;
            }
        }

        private void Webview_NewWindowRequested(object sender, WebViewControlNewWindowRequestedEventArgs args)
        {
            args.Handled = true;
            string uri = args.Uri.ToString();
            webview.Navigate(new Uri(uri));
        }

        private void PUWindow_Closing(object sender, CancelEventArgs e)
        {
            webview.Visibility = Visibility.Hidden;
        }

        private void OfficialBtn_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://www.lllxy.net/");
        }

        private async void loginBtn_Click(object sender, RoutedEventArgs e)
        {
            await webview.InvokeScriptAsync("eval", new[] { "mysubmit('form')" });
        }
    }
}
