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
using System.IO;
using Microsoft.Win32;
using System.Windows.Threading;
using System.Collections.ObjectModel;

namespace CD_ALARM
{
    public partial class MainWindow : Window
    {
        private readonly IFileService FileService = new JsonFileService();
        private readonly IDialogService DialogService = new DefaultDialogService();
        public MainWindow()
        {
            InitializeComponent();
            lstObjects.ItemsSource = autObject;
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
            MessageBox.Show("Выберете файл сохранения!");
            CmdLoad_Click(new object(), new RoutedEventArgs());
        }

 
        public ObservableCollection<AutomationObject> autObject = new ObservableCollection<AutomationObject>();
        private void CmdDeleteObject_Click(object sender, RoutedEventArgs e)
        {
            autObject.Remove((AutomationObject)lstObjects.SelectedItem);
        }
       
        private void CmdAddObject_Click(object sender, RoutedEventArgs e)
        {
            autObject.Add(new AutomationObject(ObjectName.Text, ServerE3.Text, LocalE3.Text));
        }

        private bool FileCompare(string file1, string file2)
        {
            int file1byte;
            int file2byte;
            FileStream fs1;
            FileStream fs2;

            if (file1 == file2)
            {
                return true;
            }

            fs1 = new FileStream(file1, FileMode.Open);
            fs2 = new FileStream(file2, FileMode.Open);

            if (fs1.Length != fs2.Length)
            {
                fs1.Close();
                fs2.Close();

                return false;
            }

            do
            {
                file1byte = fs1.ReadByte();
                file2byte = fs2.ReadByte();
            }
            while ((file1byte == file2byte) && (file1byte != -1));

            fs1.Close();
            fs2.Close();

            return (file1byte - file2byte) == 0;
        }

        private void Button_CompareClick(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (var comparedObject in autObject)
                {
                    Comparer(comparedObject, sender);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                Progress.Visibility = Visibility.Hidden;
                Header.Visibility = Visibility.Hidden;
            }
        }

        private void Button_ServerE3Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            };
            if (openFileDialog.ShowDialog() == true)
                ServerE3.Text = Path.GetFullPath(openFileDialog.FileName);

        }

        private void Button_localE3Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            };
            if (openFileDialog.ShowDialog() == true)
                LocalE3.Text = Path.GetFullPath(openFileDialog.FileName);
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void TextBox2_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            lblTime.Content = DateTime.Now.ToLongTimeString();
            if (DateTime.Now.Hour == 10 & DateTime.Now.Minute==42 & DateTime.Now.Second==01)
                try
                {
                    foreach (var comparedObject in autObject)
                    {
                        Comparer(comparedObject, new Button());
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    Progress.Visibility = Visibility.Hidden;
                    Header.Visibility = Visibility.Hidden;
                }    
        }

        private void ObjectName_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private async void Comparer(AutomationObject automObject, object sender)
        {
            try
            {
                Progress.Visibility = Visibility.Visible;
                Header.Visibility = Visibility.Visible;
                if (await Task.Run(() => FileCompare(automObject.LocalE3, automObject.ServerE3)))
                {
                    if (((Button)sender).Name == "CompareButton")
                        MessageBox.Show($"Объект {automObject.Name}. Изменений КД пока что не было.");
                }
                else
                {
                    MessageBox.Show($"Объект {automObject.Name}. Несопадение версий КД", "Внимание", MessageBoxButton.OK,
                        MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.ServiceNotification);
                }
            }
            catch (Exception ex)
            {
                Progress.Visibility = Visibility.Hidden;
                Header.Visibility = Visibility.Hidden;
                MessageBox.Show($"Объект: {automObject.Name}. Ошибка: {ex.Message}");
            }
        }

        
        private void CmdSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (DialogService.SaveFileDialog())
                {
                    FileService.Save(DialogService.FilePath, autObject.ToList());
                    DialogService.ShowMessage("Файл сохранен");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                DialogService.ShowMessage(ex.Message);
            }
        }

        private void CmdLoad_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (DialogService.OpenFileDialog())
                {
                    var workers = FileService.Open(DialogService.FilePath);
                    autObject.Clear();
                    foreach (var p in workers)
                        autObject.Add(p);
                    DialogService.ShowMessage("Файл загружен");
                }
            }
            catch (Exception ex)
            {
                DialogService.ShowMessage(ex.Message);
            }
        }

        private void CmdAddStartup_Click(object sender, RoutedEventArgs e)
        {
            Autoload.AddStartup();
        }

        private void CmdRemoveStartup_Click(object sender, RoutedEventArgs e)
        {
            Autoload.RemoveStartup();
        }
    }
}
