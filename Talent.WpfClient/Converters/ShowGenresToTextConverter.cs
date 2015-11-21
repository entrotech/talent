using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using Talent.Domain;

namespace Talent.WpfClient.Converters
{
    public class ShowGenresToTextConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, 
            object parameter, System.Globalization.CultureInfo culture)
        {
            var coll = value as IEnumerable<ShowGenre>;

            if (coll != null)
            {
                return String.Join(", ", coll
                    .Where(o => !o.IsMarkedForDeletion)
                    .Select(o => LookupGenreById(o.GenreId)));
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, 
            object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException(
                "CollectionToTextConverter only supports OneWay binding.");
        }

        #endregion

        private static string LookupGenreById(int id)
        {
            return LookupCache.Genres.Where(o => o.GenreId == id)
                .Select(o => o.Name).FirstOrDefault();
        }
    }

}
