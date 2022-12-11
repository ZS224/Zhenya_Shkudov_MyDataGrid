using Microsoft.EntityFrameworkCore;
using System.Windows;
namespace SQLiteApp
{
    public partial class MainWindow : Window
    {
        ApplicationContext db = new ApplicationContext();
        public MainWindow()
        {
            InitializeComponent();

            Loaded += MainWindow_Loaded;
        }
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            db.Database.EnsureCreated();
            db.Users.Load();
            DataContext = db.Users.Local.ToObservableCollection();
        }
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            UserWindow UserWindow = new UserWindow(new User());
            if (UserWindow.ShowDialog() == true)
            {
                User User = UserWindow.User;
                db.Users.Add(User);
                db.SaveChanges();
            }
        }
        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            User? user = usersList.SelectedItem as User;
            if (user is null) return;
            UserWindow UserWindow = new UserWindow(new User
            {
                Id = user.Id,
                Age = user.Age,
                Name = user.Name
            });
            if (UserWindow.ShowDialog() == true)
            {
                user = db.Users.Find(UserWindow.User.Id);
                if (user != null)
                {
                    user.Age = UserWindow.User.Age;
                    user.Name = UserWindow.User.Name;
                    db.SaveChanges();
                    usersList.Items.Refresh();
                }
            }
        }
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            User? user = usersList.SelectedItem as User;
            if (user is null) return;
            db.Users.Remove(user);
            db.SaveChanges();
        }
    }
}