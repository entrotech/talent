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
using Talent.Domain;

namespace Talent.WpfClient
{
    /// <summary>
    /// Interaction logic for ShowView.xaml
    /// </summary>
    public partial class ShowView : UserControl
    {
        public ShowView()
        {
            InitializeComponent();
            MpaaRatingComboBox.ItemsSource = LookupCache.MpaaRatings;
            CreditTypeDropDownColumn.ItemsSource = LookupCache.CreditTypes;
            PersonDropDownColumn.ItemsSource = LookupCache.People;
        }

        private void GenresEditButton_Click(object sender, RoutedEventArgs e)
        {
            var show = DataContext as Show;
            if (show == null) return;
            var dlg = new GenrePicker();
            dlg.SelectedGenreIds = show.ShowGenres
                .Where(o => o.IsMarkedForDeletion == false)
                .Select(o => o.GenreId).ToList();
            if (dlg.ShowDialog() == true)
            {
                var selectedGenreIds = dlg.SelectedGenreIds;
                foreach (var sgid in selectedGenreIds)
                {
                    var showGenre = show.ShowGenres.Where(o => o.GenreId == sgid).FirstOrDefault();
                    if (showGenre == null)
                    {
                        show.ShowGenres.Add(new ShowGenre { GenreId = sgid });
                    }
                    else
                    {
                        showGenre.IsMarkedForDeletion = false;
                    }
                }
                var showGenresToDelete = show.ShowGenres.Where(o => !selectedGenreIds.Contains(o.GenreId));
                foreach (var sg in showGenresToDelete)
                {
                    sg.IsMarkedForDeletion = true;
                }
                // Force an update to the GenresTextBox binding
                // This is necessary because the TextBox does not listen for 
                // INotifyCollectionChanged events.
                BindingOperations.GetBindingExpression(GenresTextBox, TextBox.TextProperty)
                    .UpdateTarget();
            }
        }
    }
}
