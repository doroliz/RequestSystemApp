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
    /// Логика взаимодействия для WorkerWindow.xaml
    /// </summary>
    public partial class WorkerWindow : Window
    {
        int idWorker;
        public WorkerWindow(int id)
        {
            InitializeComponent();
            idWorker = id;
            DataContext = activeRequestTable.SelectedItem as request;
            requestTable.ItemsSource = TechnoserviceEntities.GetEntities().request.Where(r =>
            r.status_id == 0).ToList(); //0 - не зарегистрированы, т.е. их никто не выполняет
            activeRequestTable.ItemsSource = TechnoserviceEntities.GetEntities().request.Where(r => 
            r.status_id == 1 && r.worker_id == idWorker).ToList(); //исполняемые данным работником заявки

            problemCombo.ItemsSource = TechnoserviceEntities.GetEntities().problem.ToList();
            technicCombo.ItemsSource = TechnoserviceEntities.GetEntities().technic.ToList();
            statusCombo.ItemsSource = TechnoserviceEntities.GetEntities().status.ToList();
        }

        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            mainWindow.loginBox.Focus();
            this.Close();
        }

        private void filterBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int requestID = 0;
                if (searchBox.Text == "")
                {
                    requestTable.ItemsSource = TechnoserviceEntities.GetEntities().request.Where(r => r.status_id == 0).ToList();
                }
                else
                {
                    requestID = Convert.ToInt32(searchBox.Text);
                    requestTable.ItemsSource = TechnoserviceEntities.GetEntities().request.Where(r => r.status_id == 0 && r.id == requestID).ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }

            //try
            //{
            //    int problem = problemCombo.SelectedIndex;
            //    int status = statusCombo.SelectedIndex;
            //    int technic = technicCombo.SelectedIndex;
            //    requestTable.ItemsSource = TechnoserviceEntities.GetEntities().request.Where(r =>
            //    r.status_id == 0 && r.problem_id == problem && r.status_id == status && r.technic_id == technic).ToList();

            //    //activeRequestTable.ItemsSource = TechnoserviceEntities.GetEntities().request.Where(r =>r.status_id == 1 && r.worker_id == idWorker).ToList();
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message.ToString());
            //}
        }

        private void registerBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                requestTable.ItemsSource = TechnoserviceEntities.GetEntities().request.Where(r => r.status_id == 0).ToList();
                //добавление заявки
                request newRequest = requestTable.SelectedItem as request;
                newRequest.status_id = 1; //начать выполнение
                newRequest.worker_id = idWorker; //текущий сотрудник
                if (newRequest.days == null) newRequest.days = 1; //работа выполняется минимум 1 день
                TechnoserviceEntities.GetEntities().SaveChanges();
                MessageBox.Show("Заявка успешно зарегистрирована под номером №" +
                    newRequest.id + ".");
                //обновление таблиц
                requestTable.ItemsSource = TechnoserviceEntities.GetEntities().request.Where(r => r.status_id == 0).ToList();
                activeRequestTable.ItemsSource = TechnoserviceEntities.GetEntities().request.Where(r => r.status_id == 1 && r.worker_id == idWorker).ToList();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void addCommentBtn_Click(object sender, RoutedEventArgs e)
        {
            //сохранение заявки
            try
            {
                string newComment = commentBox.Text;
                request newRequest = activeRequestTable.SelectedItem as request;
                newRequest.comment = newComment;
                TechnoserviceEntities.GetEntities().SaveChanges();
                MessageBox.Show("Комментарий изменён.");
                //обновление таблицы
                activeRequestTable.ItemsSource = TechnoserviceEntities.GetEntities().request.Where(r => r.status_id == 1 && r.worker_id == idWorker).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void endBtn_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы уверены, что хотите завершить выбранную заявку?", "Уведомление", 
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    request newRequest = activeRequestTable.SelectedItem as request;
                    newRequest.status_id = 2; //выполнено
                    TechnoserviceEntities.GetEntities().SaveChanges();
                    MessageBox.Show("Заявка успешно завершена!");
                    //обновление таблицы активных заявок
                    activeRequestTable.ItemsSource = TechnoserviceEntities.GetEntities().request.Where(r => r.status_id == 1 && r.worker_id == idWorker).ToList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void updateBtn_Click(object sender, RoutedEventArgs e)
        {
            searchBox.Text = "";
            requestTable.ItemsSource = TechnoserviceEntities.GetEntities().request.Where(r => r.status_id == 0).ToList();
        }
    }
}
