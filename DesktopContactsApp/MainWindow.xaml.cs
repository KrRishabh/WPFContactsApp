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
using SQLite;
using DesktopContactsApp.Classes;


namespace DesktopContactsApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Contact> contactList;
        List<Contact> filteredList;
        public MainWindow()
        {
            InitializeComponent();
            
            contactList = new List<Contact>();
            ReadDatabase();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NewContactWindow newContactWindow = new NewContactWindow();
            newContactWindow.ShowDialog();
            ReadDatabase();
        }
        void ReadDatabase()
        {
            
            using (SQLiteConnection connection = new SQLiteConnection(App.databasePath))
            {
                connection.CreateTable<Contact>();
                contactList = (connection.Table<Contact>().ToList()).OrderBy(c => c.Name).ToList();
                
            }

            if (contactList != null)
            {
                
                contactsListView.ItemsSource = contactList;
            } 

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox enteredTextBox = sender as TextBox;
            filteredList = contactList.Where(c => c.Name.ToLower().Contains(enteredTextBox.Text.ToLower())).ToList();
            //filteredList = contactList;
            //contactsListView.ItemsSource = null;
            contactsListView.ItemsSource = filteredList;
            
        }

        private void contactsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Contact selectedContact = (Contact)contactsListView.SelectedItem;
            if (selectedContact != null)
            {
                ContactDetailWindow contactDetailWindow = new ContactDetailWindow(selectedContact);
                contactDetailWindow.ShowDialog();
                ReadDatabase();
            }
        }
    }
}
