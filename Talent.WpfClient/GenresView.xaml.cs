using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
//using Talent.DataAccess.Fake;
using Talent.DataAccess.Ado;
using Talent.Domain;
using Ucla.Common.Interfaces;

namespace Talent.WpfClient
{
    public partial class GenresView : UserControl
    {
        IRepository<Genre> _genreRepository;
        ObservableCollection<Genre> _genres = new 
            ObservableCollection<Genre>();

        public GenresView()
        {
            InitializeComponent();
            _genreRepository = new GenreRepository();
            this.DataContext = _genres;

            CommandBindings.Add(new CommandBinding(ApplicationCommands.Find, 
                Find, CanFind));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.New, 
                New, CanNew));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Delete, 
                Delete, CanDelete));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Save, 
                Save, CanSave));
            CommandBindings.Add(new CommandBinding(CustomCommands.Cancel, 
                Cancel, CanCancel));
        }

        private void CanFind(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ResultsListBox.SelectedItem == null
                || !((Genre)ResultsListBox.SelectedItem).IsDirty;
        }

        private void Find(object sender, ExecutedRoutedEventArgs e)
        {
            Search();
        }

        private void CanNew(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ResultsListBox.SelectedItem == null
                || !((Genre)ResultsListBox.SelectedItem).IsDirty;
        }

        private void New(object sender, ExecutedRoutedEventArgs e)
        {
            var newItem = new Genre();
            _genres.Add(newItem);
            ResultsListBox.SelectedItem = newItem;
        }

        private void CanDelete(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ResultsListBox.SelectedItem != null;
        }

        private void Delete(object sender, ExecutedRoutedEventArgs e)
        {
            var item = (Genre)ResultsListBox.SelectedItem;
            if (item == null) return;
            try
            {
                var msg = String.Format("Are you sure you want to delete the {0} genre?",
                    item.Name);
                if (MessageBox.Show(msg, "Confirm Delete?",
                    MessageBoxButton.YesNo, MessageBoxImage.Warning)
                    == MessageBoxResult.Yes)
                {
                    item.IsMarkedForDeletion = true;
                    _genreRepository.Persist(item);
                    _genres.Remove(item);
                    ResultsListBox.SelectedItem = null;
                    Search();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Delete Failed", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CanSave(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ResultsListBox.SelectedItem != null
                && ((Genre)ResultsListBox.SelectedItem).IsGraphDirty
                && ((Genre)ResultsListBox.SelectedItem).Error == null;
        }

        private void Save(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                var item = (Genre)ResultsListBox.SelectedItem;
                if (item == null) return;
                _genreRepository.Persist(item);
                Search();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Save Failed", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CanCancel(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ResultsListBox.SelectedItem != null
                && ((Genre)ResultsListBox.SelectedItem).IsGraphDirty;
        }

        private void Cancel(object sender, ExecutedRoutedEventArgs e)
        {
            Search();
        }

        private void Search()
        {
            var previouslySelectedItem = (Genre)ResultsListBox.SelectedItem;
            _genres.Clear();
            foreach (var item in _genreRepository.Fetch())
            {
                _genres.Add(item);
            }
            Genre selectedGenre = null;
            if (previouslySelectedItem != null)
            {
                selectedGenre = _genres
                .Where(o => o.GenreId == previouslySelectedItem.GenreId)
                .FirstOrDefault();
            }
            if (selectedGenre != null)
            {
                ResultsListBox.SelectedItem = selectedGenre;
            }
            else
            {
                ResultsListBox.SelectedItem = null;
            }
        }

    }
}

