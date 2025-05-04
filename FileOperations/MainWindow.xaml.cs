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
        }
       
        string Sourcepath = @"D:\MCES2\Applications";
        string Destination = @"D:\MCES2\Backup_MCES2-"+System.DateTime.Now.ToString("dd-MM-yyyy");

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            lbl1.Content = "Doing the backup";
            await Task.Delay(100); // Allow UI to update

            bool success = false;

            await Task.Run(() =>
            {
                try
                {
                    CopyDirectory(Sourcepath, Destination);
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
                DeleteFiles(Sourcepath);
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
                foreach(string subfile in Directory.GetFiles(filepath))
                {
                    File.Delete(subfile);
                }
                Directory.Delete(filepath);
            }


            foreach(string file in Directory.GetFiles(path))
            {
                File.Delete(file);
            }
        }

        public void StopApplication(string AppName)
        {

        }


    }
}
