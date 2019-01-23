using BusinessLayer;
using BusinessLayer.Enums;
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

namespace BookWpf
{
    public partial class MainWindow : Window
    {
        public event EventHandler TryingToInsertOne;

        public MainWindow()
        {
            InitializeComponent();

            TryingToInsertOne += MainWindow_TryingToInsertOne;
        }

        private async void GetDataButton_Click(object sender, RoutedEventArgs e)
        {
            MyBookListView.ItemsSource = await Book.GetBooks();
            GetDataButton.IsEnabled = false;
        }

        private void Textbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            OnTryingToInsertOne(sender);
        }
        private void OnTryingToInsertOne(object sender)
        {
            TryingToInsertOne?.Invoke(sender, null);
        }
        private void MainWindow_TryingToInsertOne(object sender, EventArgs e)
        {
            AddButton.IsEnabled = true;
            AddOneMoreButton.IsEnabled = true;
        }

        private async void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Book book = new Book()
            {
                ReleaseDate = (ReleaseDateTextbox.Text != "") ? Convert.ToDateTime(ReleaseDateTextbox.Text) : default(DateTime?),
                Author = AuthorTextbox.Text,
                Name = NameTextbox.Text,
                Genre = GenreTextbox.Text,
                Price = (PriceTextbox.Text != "") ? Convert.ToSingle(PriceTextbox.Text) : default(float?),
                PageCount = (PageCountTextbox.Text != "") ? Convert.ToInt16(PageCountTextbox.Text) : default(short?),
                State = (StateTextbox.Text != "") ? (BookState?)Convert.ToByte(StateTextbox.Text) : default(BookState?)
            };

            await book.AddBook();
            MyBookListView.ItemsSource = await Book.GetBooks();
        }
        private void AddOneMoreButton_Click(object sender, RoutedEventArgs e)
        {            
            ReleaseDateTextbox.Text = "";
            AuthorTextbox.Text = "";
            NameTextbox.Text = "";
            GenreTextbox.Text = "";
            PriceTextbox.Text = "";
            PageCountTextbox.Text = "";
            StateTextbox.Text = "";
        }

        private void MyBookListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DeleteButton.IsEnabled = true;
            UpdateButton.IsEnabled = true;

            if (MyBookListView.SelectedItems.Count > 1)
            {
                DeleteManyButton.IsEnabled = true;
            }
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            Book book = MyBookListView.SelectedItem as Book;

            await book.DeleteBook();
            MyBookListView.ItemsSource = await Book.GetBooks();
        }
        private async void DeleteManyButton_Click(object sender, RoutedEventArgs e)
        {
            List<Book> books = MyBookListView.SelectedItems as List<Book>;

            foreach (Book book in books)
            {
                await book.DeleteBook();
            }            
            MyBookListView.ItemsSource = await Book.GetBooks();
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            Book book = MyBookListView.SelectedItem as Book;

            ReleaseDateTextbox.Text = book.ReleaseDate?.ToString();
            AuthorTextbox.Text = book.Author;
            NameTextbox.Text = book.Name;
            GenreTextbox.Text = book.Genre;
            PriceTextbox.Text = book.Price?.ToString();
            PageCountTextbox.Text = book.PageCount?.ToString();
            StateTextbox.Text = book.State?.ToString();

            UpdateButton_Copy.IsEnabled = true;
        }
        private void UpdateButton_Copy_Click(object sender, RoutedEventArgs e)
        {
            AddButton_Click(sender, e);
            UpdateButton_Copy.IsEnabled = false;
            AddOneMoreButton_Click(sender, e);
        }
    }
}
