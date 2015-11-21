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
    public partial class ShowsView : UserControl
    {
        IRepository<Show> _showRepository;
        ObservableCollection<Show> _shows = new
            ObservableCollection<Show>();

        public ShowsView()
        {
            InitializeComponent();

            var mpaaRatingList = LookupCache.MpaaRatings.ToList();
            mpaaRatingList.Insert(0, new MpaaRating { MpaaRatingId = 0, Code = "(Any)" });
            MpaaRatingCriterion.ItemsSource = mpaaRatingList;
            MpaaRatingCriterion.SelectedIndex = 0;

            _showRepository = new ShowRepository();
            this.DataContext = _shows;

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
                || !((Show)ResultsListBox.SelectedItem).IsDirty;
        }

        private void Find(object sender, ExecutedRoutedEventArgs e)
        {
            Search();
        }

        private void CanNew(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ResultsListBox.SelectedItem == null
                || !((Show)ResultsListBox.SelectedItem).IsDirty;
        }

        private void New(object sender, ExecutedRoutedEventArgs e)
        {
            var newItem = new Show();
            _shows.Add(newItem);
            ResultsListBox.SelectedItem = newItem;
        }

        private void CanDelete(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ResultsListBox.SelectedItem != null;
        }

        private void Delete(object sender, ExecutedRoutedEventArgs e)
        {
            var item = (Show)ResultsListBox.SelectedItem;
            if (item == null) return;
            try
            {
                var msg = String.Format("Are you sure you want to delete {0}?",
                    item.Title);
                if (MessageBox.Show(msg, "Confirm Delete?",
                    MessageBoxButton.YesNo, MessageBoxImage.Warning)
                    == MessageBoxResult.Yes)
                {
                    item.IsMarkedForDeletion = true;
                    _showRepository.Persist(item);
                    _shows.Remove(item);
                    ResultsListBox.SelectedItem = null;
                    Search();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Delete Failed",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CanSave(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ResultsListBox.SelectedItem != null
                && ((Show)ResultsListBox.SelectedItem).IsGraphDirty
                && ((Show)ResultsListBox.SelectedItem).Error == null;
        }

        private void Save(object sender, ExecutedRoutedEventArgs e)
        {
            var item = (Show)ResultsListBox.SelectedItem;
            if (item == null) return;
            try
            {
                _showRepository.Persist(item);
                Search();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Save Failed",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CanCancel(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ResultsListBox.SelectedItem != null
                && ((Show)ResultsListBox.SelectedItem).IsGraphDirty;
        }

        private void Cancel(object sender, ExecutedRoutedEventArgs e)
        {
            Search();
        }

        private void Search()
        {
            var previouslySelectedItem = (Show)ResultsListBox.SelectedItem;
            _shows.Clear();
            ShowCriteria crit = new ShowCriteria
            {
                Title = this.TitleCriterion.Text,
                MpaaRatingId = (int)MpaaRatingCriterion.SelectedValue
            };
            foreach (var item in _showRepository.Fetch(crit))
            {
                _shows.Add(item);
            }
            Show selectedShow = null;
            if (previouslySelectedItem != null)
            {
                selectedShow = _shows
                .Where(o => o.ShowId == previouslySelectedItem.ShowId)
                .FirstOrDefault();
            }
            if (selectedShow != null)
            {
                ResultsListBox.SelectedItem = selectedShow;
            }
            else
            {
                ResultsListBox.SelectedItem = null;
            }
        }

    }
}

