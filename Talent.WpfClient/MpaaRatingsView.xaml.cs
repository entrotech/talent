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
    public partial class MpaaRatingsView : UserControl
    {
        IRepository<MpaaRating> _mpaaRatingRepository;
        ObservableCollection<MpaaRating> _mpaaRatings = new 
            ObservableCollection<MpaaRating>();

        public MpaaRatingsView()
        {
            InitializeComponent();
            _mpaaRatingRepository = new MpaaRatingRepository();
            this.DataContext = _mpaaRatings;

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
                || !((MpaaRating)ResultsListBox.SelectedItem).IsDirty;
        }

        private void Find(object sender, ExecutedRoutedEventArgs e)
        {
            Search();
        }

        private void CanNew(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ResultsListBox.SelectedItem == null
                || !((MpaaRating)ResultsListBox.SelectedItem).IsDirty;
        }

        private void New(object sender, ExecutedRoutedEventArgs e)
        {
            var newItem = new MpaaRating();
            _mpaaRatings.Add(newItem);
            ResultsListBox.SelectedItem = newItem;
        }

        private void CanDelete(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ResultsListBox.SelectedItem != null;
        }

        private void Delete(object sender, ExecutedRoutedEventArgs e)
        {
            var item = (MpaaRating)ResultsListBox.SelectedItem;
            if (item == null) return;
            try
            {
                var msg = String.Format("Are you sure you want to delete the {0} rating?",
                    item.Name);
                if (MessageBox.Show(msg, "Confirm Delete?",
                    MessageBoxButton.YesNo, MessageBoxImage.Warning)
                    == MessageBoxResult.Yes)
                {
                    item.IsMarkedForDeletion = true;
                    _mpaaRatingRepository.Persist(item);
                    _mpaaRatings.Remove(item);
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
                && ((MpaaRating)ResultsListBox.SelectedItem).IsGraphDirty
                && ((MpaaRating)ResultsListBox.SelectedItem).Error == null;
        }

        private void Save(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                var item = (MpaaRating)ResultsListBox.SelectedItem;
                if (item == null) return;
                _mpaaRatingRepository.Persist(item);
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
                && ((MpaaRating)ResultsListBox.SelectedItem).IsGraphDirty;
        }

        private void Cancel(object sender, ExecutedRoutedEventArgs e)
        {
            Search();
        }

        private void Search()
        {
            var previouslySelectedItem = (MpaaRating)ResultsListBox.SelectedItem;
            _mpaaRatings.Clear();
            foreach (var item in _mpaaRatingRepository.Fetch())
            {
                _mpaaRatings.Add(item);
            }
            MpaaRating selectedMpaaRating = null;
            if (previouslySelectedItem != null)
            {
                selectedMpaaRating = _mpaaRatings
                .Where(o => o.MpaaRatingId == previouslySelectedItem.MpaaRatingId)
                .FirstOrDefault();
            }
            if (selectedMpaaRating != null)
            {
                ResultsListBox.SelectedItem = selectedMpaaRating;
            }
            else
            {
                ResultsListBox.SelectedItem = null;
            }
        }

    }
}

