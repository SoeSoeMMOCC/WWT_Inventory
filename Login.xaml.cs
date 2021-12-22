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
using WWT_Inventory.Controller;
using WWT_Inventory.Model;
using WWT_Inventory.Service;

namespace WWT_Inventory
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public bool status = true;
        public string role = "";
        private  UserInterface userInterface;
        UserController userController = new UserController();
        string error = "";
        public Login()
        {
            InitializeComponent();
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void txt_name_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key==Key.Enter)
            {
                txt_password.Focus();
                txt_password.SelectAll();
            }
                
        }

        private void txt_password_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btn_Login.Focus();
            }
                
        }

        private void btn_Login_Click(object sender, RoutedEventArgs e)
        {

            //string UserName = txt_name.Text.ToString().Trim();
            //string Password = txt_password.Password.ToString().Trim();
            /*For the Exact User Login Information -- Remove Trim() */
            string UserName = txt_name.Text.ToString();
            string Password = txt_password.Password.ToString();
            User user = userController.GetLoginUser(UserName, Password, out error);
            if (user != null)
            {
                MessageBox.Show("Welcome, to WWT Inventory System", "Welcome", MessageBoxButton.OK, MessageBoxImage.Information);
                status = true;
                role = user.role;
                this.Close();
            }
            else
            {
                MessageBox.Show("Invalid Login!", "Try Again", MessageBoxButton.OK, MessageBoxImage.Error); ;
                txt_name.Focus();
                txt_name.SelectAll();
                status = false;
            }
            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txt_name.Focus();
        }

        private void btn_Login_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                btn_Login_Click(sender, e);
        }
    }
}
