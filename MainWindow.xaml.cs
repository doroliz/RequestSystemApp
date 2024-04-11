using System.Linq;
using System.Windows;

namespace TechnoServiceApp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        ///авторизация
        private void enterBtn_Click(object sender, RoutedEventArgs e)
        {
            string login = loginBox.Text.Trim().ToLower();
            string password = passBox.Password.Trim();
            string errors = "";
            user user = null;
            if (login.Length == 0)
            {
                errors += "Логин не введён. Попробуйте снова.\n";
                loginBox.Focus();
            }
            else if (password.Length == 0)
            {
                errors += "Пароль не введён. Попробуйте снова.\n";
                passBox.Focus();
            }
            else
            {
                using (TechnoserviceEntities entities = new TechnoserviceEntities())
                {
                    //поиск пользователя в базе с такими логином и паролем
                    user = entities.user.Where(u => u.login == login && u.password == password).FirstOrDefault();
                }
                if (user != null)
                {
                    MessageBox.Show("Вы успешно авторизованы. Приветствуем, " + user.fio + ".");
                    if (user.role_id == 1)//сотрудник
                    {
                        WorkerWindow workerWindow = new WorkerWindow(user.id);
                        workerWindow.Show();
                        this.Close();
                    }
                    else if (user.role_id == 2)//менеджер
                    {
                        ManagerWindow managerWindow = new ManagerWindow(user.id);
                        managerWindow.Show();
                        this.Close();
                    }
                    else if (user.role_id == 3) //клиент
                    {
                        ClientWindow clientWindow = new ClientWindow(user.id);
                        clientWindow.Show();
                        this.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Пользователm с таким логином или паролем не найден. " +
                        "Попробуйте снова или зарегистрируйтесь.", "Ошибка", 
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    loginBox.Clear();
                    passBox.Clear();
                    loginBox.Focus();
                }
            }
        }
        //>окно регистрации (клиента)
        private void regBtn_Click(object sender, RoutedEventArgs e)
        {
            RegisterWindow registerWindow = new RegisterWindow(0);
            registerWindow.Show();
            this.Close();
        }
        //быстрые проверки для администратора
        private void checkClientBtn_Click(object sender, RoutedEventArgs e)
        {
            //для клиента
            loginBox.Text = "client01@mail.ru";
            passBox.Password = "client0101";
        }
        private void checkWorkerBtn_Click(object sender, RoutedEventArgs e)
        {
            //для сотрудника
            loginBox.Text = "first@adm.in";
            passBox.Password = "admin01";
        }
        private void checkManagerBtn_Click(object sender, RoutedEventArgs e)
        {
            //для менеджера
            loginBox.Text = "first@manage.r";
            passBox.Password = "manager01";
        }
        private void okBtn_Click(object sender, RoutedEventArgs e)
        {
            if (adminBox.Text == "1251")
            {
                adminGrid.Visibility = Visibility.Visible;
            }
        }
    }
}
