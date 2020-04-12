using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;

namespace 超星尔雅刷课wpf.Service
{
    class MainServiceManager
    {
        private HttpClient httpClient;

        public MainServiceManager()
        {
            httpClient = new HttpClient();
        }

        /// <summary>
        /// 获取在线脚本（获取失败使用本地脚本）
        /// </summary>
        public async Task<string> GetScript()
        {
            try
            {
                //在线获取最新脚本
                string scriptUri = "http://www.lllxy.net/cxsk/getscript.ashx";
                HttpResponseMessage response = await httpClient.GetAsync(scriptUri);
                string script = await response.Content.ReadAsStringAsync();
                return script;
            }
            catch (Exception)
            {
                MessageBox.Show("请检查网络并重启程序", "获取脚本失败", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        /// <summary>
        /// 获取当前版本号
        /// </summary>
        public string GetVersion() => Application.ResourceAssembly.GetName().Version.ToString();

    }
}
