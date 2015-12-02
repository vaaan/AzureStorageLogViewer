using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace StorageLogViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                CheckFileExists = true,
                Multiselect = false,
                Filter = "Log File|*.txt"
            };
            openFileDialog.ShowDialog();
            var lines = File.ReadAllLines(openFileDialog.FileName);
            var logEntries = GetLogEntries(lines);
            xdg.ItemsSource = null;
            xdg.ItemsSource = logEntries;
        }

        private IList<LogEntry> GetLogEntries(string[] lines)
        {
            var result = new List<LogEntry>();
            foreach (var line in lines)
            {
                var logEntry = new LogEntry();
                result.Add(logEntry);
                var currentIndex = 0;
                // skip first version item
                GetNextItemValue(ref currentIndex, line);
                logEntry.RequestStartTime = DateTime.Parse(GetNextItemValue(ref currentIndex, line));
                logEntry.OperationType = GetNextItemValue(ref currentIndex, line);
                logEntry.RequestStatus = GetNextItemValue(ref currentIndex, line);
                logEntry.HttpStatusCode = GetNextItemValue(ref currentIndex, line);
                logEntry.EndToEndLatencyInMs = int.Parse(GetNextItemValue(ref currentIndex, line));
                logEntry.ServerLatencyInMs = int.Parse(GetNextItemValue(ref currentIndex, line));
                logEntry.AuthenticationType = GetNextItemValue(ref currentIndex, line);
                logEntry.RequesterAccountName = GetNextItemValue(ref currentIndex, line);
                logEntry.OwnerAccountName = GetNextItemValue(ref currentIndex, line);
                logEntry.ServiceType = GetNextItemValue(ref currentIndex, line);
                logEntry.RequestUrl = GetNextItemValue(ref currentIndex, line);
                logEntry.RequestedObjectKey = GetNextItemValue(ref currentIndex, line);
                logEntry.RequestIdHeader = GetNextItemValue(ref currentIndex, line);
                logEntry.OperationCount = int.Parse(GetNextItemValue(ref currentIndex, line));
                logEntry.RequesterIpAddress = GetNextItemValue(ref currentIndex, line);
                logEntry.RequestVersionHeader = GetNextItemValue(ref currentIndex, line);
                logEntry.RequestHeaderSize = long.Parse(GetNextItemValue(ref currentIndex, line));
                logEntry.RequestPacketSize = long.Parse(GetNextItemValue(ref currentIndex, line));
                logEntry.ResponseHeaderSize = long.Parse(GetNextItemValue(ref currentIndex, line));
                logEntry.RespoinsePacketSize = long.Parse(GetNextItemValue(ref currentIndex, line));
                logEntry.RequestContentLength = long.Parse(GetNextItemValue(ref currentIndex, line));
                logEntry.RequestMd5 = GetNextItemValue(ref currentIndex, line);
                logEntry.ServerMd5 = GetNextItemValue(ref currentIndex, line);
                logEntry.EtagIdentifier = GetNextItemValue(ref currentIndex, line);
                var lastModifiedTime = GetNextItemValue(ref currentIndex, line);
                if (String.IsNullOrEmpty(lastModifiedTime))
                    logEntry.LastModifiedTime = null;
                else 
                    logEntry.LastModifiedTime = DateTime.Parse(lastModifiedTime);
                logEntry.ConditionsUsed = GetNextItemValue(ref currentIndex, line);
                logEntry.UserAgentHeader = GetNextItemValue(ref currentIndex, line);
                logEntry.ReferrerHeader = GetNextItemValue(ref currentIndex, line);
                logEntry.ClientRequestId = GetNextItemValue(ref currentIndex, line);
            }
            return result;
        }

        private string GetNextItemValue(ref int currentIndex, string line)
        {
            if (line.Length <= currentIndex) return "";
            var sbResult = new StringBuilder();
            var startWithQuotation = line[currentIndex] == '\"';
            if (startWithQuotation)
            {
                sbResult.Append(line[currentIndex]);
                currentIndex++;
            }
            while (true)
            {
                if (line[currentIndex] == ';')
                {
                    if (startWithQuotation)
                    {
                        sbResult.Append(line[currentIndex]);
                        currentIndex++;
                        continue;
                    }
                    else
                    {
                        currentIndex++;
                        break;
                    }
                }
                if (line[currentIndex] == '\"' && startWithQuotation)
                {
                    sbResult.Append(line[currentIndex]);
                    currentIndex++;
                    currentIndex++;
                    break;
                }
                sbResult.Append(line[currentIndex]);
                currentIndex++;
            }
            return HttpUtility.HtmlDecode(sbResult.ToString());
        }
    }

    public class LogEntry
    {
        public DateTime RequestStartTime { get; set; }
        public string OperationType { get; set; }
        public string RequestStatus { get; set; }
        public string HttpStatusCode { get; set; }
        public int EndToEndLatencyInMs { get; set; }
        public int ServerLatencyInMs { get; set; }
        public string AuthenticationType { get; set; }
        public string RequesterAccountName { get; set; }
        public string OwnerAccountName { get; set; }
        public string ServiceType { get; set; }
        public string RequestUrl { get; set; }
        public string RequestedObjectKey { get; set; }
        public string RequestIdHeader { get; set; }
        public int OperationCount { get; set; }
        public string RequesterIpAddress { get; set; }
        public string RequestVersionHeader { get; set; }
        public long RequestHeaderSize { get; set; }
        public long RequestPacketSize { get; set; }
        public long ResponseHeaderSize { get; set; }
        public long RespoinsePacketSize { get; set; }
        public long RequestContentLength { get; set; }
        public string RequestMd5 { get; set; }
        public string ServerMd5 { get; set; }
        public string EtagIdentifier { get; set; }
        public DateTime? LastModifiedTime { get; set; }
        public string ConditionsUsed { get; set; }
        public string UserAgentHeader { get; set; }
        public string ReferrerHeader { get; set; }
        public string ClientRequestId { get; set; }
    }
}
