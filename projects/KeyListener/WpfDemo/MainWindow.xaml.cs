using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using com.jarvisniu.utils;

namespace WpfDemo
{
    public partial class MainWindow : Window
    {
        KeyListener keyListener = new KeyListener();

        public MainWindow()
        {
            Console.OutputEncoding = System.Text.Encoding.Unicode;
            InitializeComponent();
            keyListener.onPress("Ctrl+Q", onPress);   // combined key & multiple combination
            FillProcessList();
        }

        private void onPress()
        {
            this.Dispatcher.Invoke(delegate
            {
                label1.Content = "keys pressed.";
                foreach (string processName in processListBox.SelectedItems)
                {
                    // 获取所有正在运行指定名称的进程
                    Process[] processes = Process.GetProcessesByName(processName.Trim());

                    if (processes.Length > 0)
                    {
                        foreach (Process process in processes)
                        {
                            try
                            {
                                // 强制关闭进程
                                process.Kill();
                                Console.WriteLine($"已成功关闭进程：{process.ProcessName}");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"关闭进程 '{process.ProcessName}' 时出现错误：{ex.Message}");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine($"没有找到名称为 '{processName}' 的进程。");
                    }
                }
                this.Close();
            });
        }

        private void FillProcessList()
        {
            try
            {
                // 获取所有正在运行的进程
                Process[] processes = Process.GetProcesses();

                // 将每个桌面应用程序的进程名添加到列表框中
                foreach (Process process in processes)
                {
                    if (!string.IsNullOrEmpty(process.MainWindowTitle) && process.MainWindowHandle != IntPtr.Zero)
                    {
                        processListBox.Items.Add(process.ProcessName);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            processListBox.Items.Clear();
            FillProcessList();
        }
        // end of class
    }
}
