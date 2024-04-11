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
using System.Windows.Shapes;

namespace TechnoServiceApp
{
    /// <summary>
    /// Логика взаимодействия для ClientWindow.xaml
    /// </summary>
    public partial class ClientWindow : Window
    {
        request newRequest = new request();
        int idClient;
        public ClientWindow(int id)
        {
            InitializeComponent();
            idClient = id;
            DataContext = newRequest;
            technicBox.ItemsSource = TechnoserviceEntities.GetEntities().technic.ToList();
            problemBox.ItemsSource = TechnoserviceEntities.GetEntities().problem.ToList();
            requestTable.ItemsSource = TechnoserviceEntities.GetEntities().request.Where(r => r.client_id == idClient).ToList();
            if (TechnoserviceEntities.GetEntities().request.Where(r => r.client_id == idClient).Count() > 0)
            {
                linkBox.Visibility = Visibility.Visible;
                linkLabel.Visibility = Visibility.Visible;
            }

        }

        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            mainWindow.loginBox.Focus();
            this.Close();
        }

        private void enterBtn_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы уверены, что хотите подать заявку?", "Уведомление", 
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                //добавление заявки
                newRequest.date = DateTime.Today;
                newRequest.client_id = idClient;
                TechnoserviceEntities.GetEntities().request.Add(newRequest);
                //сохранение заявки
                try
                {
                    TechnoserviceEntities.GetEntities().SaveChanges();
                    MessageBox.Show("Заявка успешно подана!");
                    //обновление таблицы заявок
                    requestTable.ItemsSource = TechnoserviceEntities.GetEntities().request.Where(r => r.client_id == idClient).ToList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void searchBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int requestID = 0;
                if (searchBox.Text == "")
                {
                    requestTable.ItemsSource = TechnoserviceEntities.GetEntities().request.Where(r => r.client_id == idClient).ToList();
                }
                else
                {
                    requestID = Convert.ToInt32(searchBox.Text);
                    requestTable.ItemsSource = TechnoserviceEntities.GetEntities().request.Where(r => r.client_id == idClient && r.id == requestID).ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void updateBtn_Click(object sender, RoutedEventArgs e)
        {
            searchBox.Text = "";
            requestTable.ItemsSource = TechnoserviceEntities.GetEntities().request.Where(r => r.client_id == idClient).ToList();
        }
    }
}
