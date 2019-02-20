using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
using System.Xml;
using Currency;

namespace WpfExam1
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Window();
        }

        private void Window()
        {
            string data;
            WebClient client = new WebClient();
            data = client.DownloadString("https://nationalbank.kz/rss/rates_all.xml");

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(data);
            XmlNode newNode = doc.DocumentElement;

            string jsonString = JsonConvert.SerializeXmlNode(newNode);

            var currency = Welcome.FromJson(jsonString);

            currencyGrid.ItemsSource = currency.Rss.Channel.Item;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Window();
        }
    }
}
