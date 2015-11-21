using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using Talent.Domain;

namespace Talent.WpfClient.Converters
{
    public class AttachmentToImageSourceConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var attachment = value as PersonAttachment;
            if (attachment == null) return null;
            switch (attachment.FileExtension.ToUpper())
            {
                case "PNG":
                case "JPG":
                case "ICO":
                    {
                        BitmapImage bi = new BitmapImage();
                        bi.BeginInit();
                        bi.StreamSource = new MemoryStream(attachment.FileBytes);
                        bi.EndInit();
                        return bi;
                    }
                default:
                    return new BitmapImage(new Uri("pack://application:,,,/Resources/Document.png"));
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is Visibility)
            {
                return value = Visibility.Visible;
            }
            return null;
        }

        #endregion
    }
}
