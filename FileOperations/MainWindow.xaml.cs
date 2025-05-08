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
using System.IO;
using System.IO.Abstractions;
using System.Diagnostics;
using Microsoft.VisualBasic.FileIO;
using System.Threading;

namespace FileOperations
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            GetAllProcess();
        }
       
        string Sourcepath = @"D:\MCES2\Applications";
        string Destination = @"D:\MCES2\Backup_MCES2-"+System.DateTime.Now.ToString("dd-MM-yyyy");

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            //StopApplication("MCES2*");
            lbl1.Content = "Doing the backup";
            await Task.Delay(100); // Allow UI to update

            bool success = false;

            await Task.Run(async () =>
            {
                try
                {
                    await Task.Run(() => { CopyDirectory(Sourcepath, Destination); });
                    await Task.Run(() => { DeleteFiles(Sourcepath); });
                   
                    success = true;
                }
                catch (Exception ex)
                {
                    Dispatcher.Invoke(() =>
                    {
                        MessageBox.Show(ex.Message, "Backup Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                    });
                }
            });

            if (success && Directory.Exists(Destination))
            {
               
                lbl1.Content = "Done Backup";


                
            }
            else
            {
                lbl1.Content = "Backup Failed";
            }
        }

        // Recursive copy function
        private void CopyDirectory(string sourceDir, string destDir)
        {
            if(!Directory.Exists(destDir))
            {
                Directory.CreateDirectory(destDir);
            }
            

            // Copy files
            foreach (string filePath in Directory.GetFiles(sourceDir))
            {
                string fileName = System.IO.Path.GetFileName(filePath);
                string destFile = System.IO.Path.Combine(destDir, fileName);
                File.Copy(filePath, destFile, true);
            }

            // Copy subdirectories
            foreach (string subDir in Directory.GetDirectories(sourceDir))
            {
                Directory.CreateDirectory(System.IO.Path.Combine(destDir, System.IO.Path.GetFileName(subDir)));
                foreach(string filesubdirec in Directory.GetFiles(subDir))
                {
                    string final=System.IO.Path.Combine(System.IO.Path.Combine(destDir, System.IO.Path.GetFileName(subDir)), System.IO.Path.GetFileName(filesubdirec));
                    File.Copy(filesubdirec, final, true);
                }
                
            }


        }
        private void DeleteFiles(string path)
        {
            foreach(string filepath in Directory.GetDirectories(path))
            {
                
                    try
                    {
                        foreach (string subfile in Directory.GetFiles(filepath))
                        {
                            File.Delete(subfile);
                        }
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine($"failed with Exception {ex.Message}");
                    }
                   
              
                
                Directory.Delete(filepath);
            }


            foreach(string file in Directory.GetFiles(path))
            {
                File.Delete(file);
            }
        }
        //pdf24
        public void GetAllProcess()
        {
            Process[] processes = Process.GetProcesses();
            if(processes.Length>0)
            {
                foreach(Process process in processes)
                {
                    try
                    {
                        Process_List.Items.Add(process.ProcessName);
                       
                        
                        
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show("Failed to Stop process", ex.Message);
                    }
                }
            }
        }

        private void StopApp(object sender, RoutedEventArgs e)
        {
            
          
                if (Process_List.SelectedValue != null)
                {
                    foreach (Process p in Process.GetProcesses())
                    {
                        if (p.ProcessName == (string)Process_List.SelectedValue)
                        {
                            p.Kill();
                            
                        }
                    }
                }
                else
                {
                    MessageBox.Show("No Process Selected");
                }
            
            
        }

        private void Refresh(object sender, RoutedEventArgs e)
        {
            Process_List.Items.Clear();
               Process[] refProcess = Process.GetProcesses();  
            if(refProcess.Length>0)
            {
                foreach (var item in refProcess)
                {
                    Process_List.Items.Add(item.ProcessName);
                }
            }

            
        }
    }
}
