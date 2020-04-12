using Panuon.UI;
using System.Windows;

namespace 超星尔雅刷课wpf.View
{
    /// <summary>
    /// SearchWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SearchWindow : PUWindow
    {
        public SearchWindow()
        {
            InitializeComponent();
        }

        private void PUWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            answerWebView.Visibility = Visibility.Hidden;
        }
    }
}
