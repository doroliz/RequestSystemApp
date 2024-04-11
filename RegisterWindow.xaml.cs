using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace TechnoServiceApp
{
    /// <summary>
    /// Логика взаимодействия для RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        private user newUser = new user();

        public RegisterWindow(int manager)
        {
            InitializeComponent();
            DataContext = newUser;
            if (manager != 0)
            {
                backBtn.Visibility = Visibility.Hidden;
                registerBtn.Visibility = Visibility.Hidden;
                managerBackBtn.Visibility = Visibility.Visible;
                managerRegBtn.Visibility = Visibility.Visible;
                manName.Visibility = Visibility.Visible;
                manName.Content += " " + manager.ToString();
                roleLabel.Visibility = Visibility.Visible;
                roleBox.Visibility = Visibility.Visible;
                roleBox.ItemsSource = TechnoserviceEntities.GetEntities().role.ToList();
            }
        }

        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            mainWindow.loginBox.Focus();
            this.Close();
        }

        private void registerBtn_Click(object sender, RoutedEventArgs e)
        {
            string errors = "";
            if (loginBox.Text.Length < 7)
            {
                errors += "Длина логина слишком коротка. Введите минимум 7 символов. \n";
                loginBox.Focus();
            }
            if (passBox.Text.Length < 7)
            {
                errors += "Длина пароля слишком коротка. Введите минимум 7 символов. \n";
                passBox.Focus();
            }
            if (!loginBox.Text.Contains("@") || !loginBox.Text.Contains("."))
            {
                errors += "Логин должен содержать знаки собачки (@) и точку. Попробуйте снова.\n";
                loginBox.Focus();
            }


            if (errors.Length > 0)
            {
                MessageBox.Show(errors, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                //добавление пользователя
                newUser.role_id = 3; //клиент
                TechnoserviceEntities.GetEntities().user.Add(newUser);
                //сохранение пользователя в базе
                try
                {
                    TechnoserviceEntities.GetEntities().SaveChanges();
                    MessageBox.Show("Вы успешно зарегистрированы!");
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                    mainWindow.loginBox.Text = newUser.login;
                    mainWindow.passBox.Password = newUser.password;
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
 
        }

        private void phoneBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsDigit(e.Text, 0))
            {
                e.Handled = true;
            }
        }

        private void cardBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsDigit(e.Text, 0))
            {
                e.Handled = true;
            }
        }

        //для менеджера
        private void managerBackBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void managerRegBtn_Click(object sender, RoutedEventArgs e)
        {
            string errors = "";
            if (loginBox.Text.Length < 7)
            {
                errors += "Длина логина слишком коротка. Введите минимум 7 символов. \n";
                loginBox.Focus();
            }
            if (passBox.Text.Length < 7)
            {
                errors += "Длина пароля слишком коротка. Введите минимум 7 символов. \n";
                passBox.Focus();
            }
            if (!loginBox.Text.Contains("@") || !loginBox.Text.Contains("."))
            {
                errors += "Логин должен содержать знаки собачки (@) и точку. Попробуйте снова.\n";
                loginBox.Focus();
            }


            if (errors.Length > 0)
            {
                MessageBox.Show(errors, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                //добавление пользователя
                try
                {
                    TechnoserviceEntities.GetEntities().user.Add(newUser);
                    TechnoserviceEntities.GetEntities().SaveChanges();
                    MessageBox.Show("Пользователь добавлен!");
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }

        }
    }
}
