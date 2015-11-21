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
    public partial class CreditTypesView : UserControl
    {
        IRepository<CreditType> _creditTypeRepository;
        ObservableCollection<CreditType> _creditTypes = new 
            ObservableCollection<CreditType>();

        public CreditTypesView()
        {
            InitializeComponent();
            _creditTypeRepository = new CreditTypeRepository();
            this.DataContext = _creditTypes;

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
                || !((CreditType)ResultsListBox.SelectedItem).IsDirty;
        }

        private void Find(object sender, ExecutedRoutedEventArgs e)
        {
            Search();
        }

        private void CanNew(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ResultsListBox.SelectedItem == null
                || !((CreditType)ResultsListBox.SelectedItem).IsDirty;
        }

        private void New(object sender, ExecutedRoutedEventArgs e)
        {
            var newItem = new CreditType();
            _creditTypes.Add(newItem);
            ResultsListBox.SelectedItem = newItem;
        }

        private void CanDelete(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ResultsListBox.SelectedItem != null;
        }

        private void Delete(object sender, ExecutedRoutedEventArgs e)
        {
            var item = (CreditType)ResultsListBox.SelectedItem;
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
                    _creditTypeRepository.Persist(item);
                    _creditTypes.Remove(item);
                    ResultsListBox.SelectedItem = null;
                    Search();
                }
            }
            catch(ApplicationException ex)
            {
                MessageBox.Show(ex.Message, "Delete Failed", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CanSave(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ResultsListBox.SelectedItem != null
                && ((CreditType)ResultsListBox.SelectedItem).IsGraphDirty
                && ((CreditType)ResultsListBox.SelectedItem).Error == null;
        }

        private void Save(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                var item = (CreditType)ResultsListBox.SelectedItem;
                if (item == null) return;
                _creditTypeRepository.Persist(item);
                Search();
            }
            catch(ApplicationException ex)
            {
                MessageBox.Show(ex.Message, "Save Failed", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CanCancel(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ResultsListBox.SelectedItem != null
                && ((CreditType)ResultsListBox.SelectedItem).IsGraphDirty;
        }

        private void Cancel(object sender, ExecutedRoutedEventArgs e)
        {
            Search();
        }

        private void Search()
        {
            var previouslySelectedItem = (CreditType)ResultsListBox.SelectedItem;
            _creditTypes.Clear();
            foreach (var item in _creditTypeRepository.Fetch())
            {
                _creditTypes.Add(item);
            }
            CreditType selectedCreditType = null;
            if (previouslySelectedItem != null)
            {
                selectedCreditType = _creditTypes
                .Where(o => o.CreditTypeId == previouslySelectedItem.CreditTypeId)
                .FirstOrDefault();
            }
            if (selectedCreditType != null)
            {
                ResultsListBox.SelectedItem = selectedCreditType;
            }
            else
            {
                ResultsListBox.SelectedItem = null;
            }
        }

    }
}

