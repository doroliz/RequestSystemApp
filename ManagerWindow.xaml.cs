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
    /// Логика взаимодействия для ManagerWindow.xaml
    /// </summary>
    public partial class ManagerWindow : Window
    {
        int idManager;
        public ManagerWindow(int id)
        {
            InitializeComponent();
            idManager = id;
            DataContext = activeRequestTable.SelectedItem as request;
            activeRequestTable.ItemsSource = TechnoserviceEntities.GetEntities().request.ToList();
            usersList.ItemsSource = TechnoserviceEntities.GetEntities().user.ToList();

            //статистика
            //количество заявок
            int count = TechnoserviceEntities.GetEntities().request.Where(r => r.status_id == 2).Count();
            requestCount.Content = count.ToString();

            workerCombo.ItemsSource = TechnoserviceEntities.GetEntities().user.Where(u=>u.role_id==1).ToList();
        }

        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            mainWindow.loginBox.Focus();
            this.Close();
        }

        private void userRegisterBtn_Click(object sender, RoutedEventArgs e)
        {
            RegisterWindow registerWindow = new RegisterWindow(idManager);
            registerWindow.Show();
        }

        private void updateBtn_Click(object sender, RoutedEventArgs e)
        {
            activeRequestTable.ItemsSource = TechnoserviceEntities.GetEntities().request.ToList();
            usersList.ItemsSource = TechnoserviceEntities.GetEntities().user.ToList();
        }

        private void changeBtn_Click(object sender, RoutedEventArgs e)
        {
            if (daysBox.Text != null)
            {
                int days = Convert.ToInt32(daysBox.Text);
                request changeRequest = activeRequestTable.SelectedItem as request;
                changeRequest.days = days;

                try
                {
                    TechnoserviceEntities.GetEntities().SaveChanges();
                    MessageBox.Show("Изменение прошло успешно.");
                    //обновление таблицы
                    activeRequestTable.ItemsSource = TechnoserviceEntities.GetEntities().request.ToList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }
    }
}
