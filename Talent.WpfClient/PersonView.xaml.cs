using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
    /// Interaction logic for PersonView.xaml
    /// </summary>
    public partial class PersonView : UserControl
    {
        public PersonView()
        {
            InitializeComponent();
            CreditTypeDropDownColumn.ItemsSource = LookupCache.CreditTypes;
            ShowDropDownColumn.ItemsSource = LookupCache.Shows;
            HairColorComboBox.ItemsSource = LookupCache.HairColors;
            EyeColorComboBox.ItemsSource = LookupCache.EyeColors;
        }

        private void FileBrowseButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            if (dlg.ShowDialog() == true)
            {
                LoadAttachmentFromFile(dlg.FileName);
            }
        }

        private void ClipboardButton_Click(object sender, RoutedEventArgs e)
        {
            IDataObject data = Clipboard.GetDataObject();
            if (data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] selectedFiles = data.GetData(DataFormats.FileDrop) as string[];
                var fileName = selectedFiles[0];
                LoadAttachmentFromFile(fileName);
            }
        }

        #region Private Methods

        private void LoadAttachmentFromFile(string fileName)
        {
            var attach = new PersonAttachment
            {
                FileName = System.IO.Path.GetFileNameWithoutExtension(fileName),
                FileExtension = System.IO.Path.GetExtension(fileName).Replace(".", ""),
                FileBytes = File.ReadAllBytes(fileName)
            };
            ((Person)this.DataContext).Attachments.Add(attach);
        }

        #endregion

        private void File_Drop(object sender, DragEventArgs e)
        {
            var strs = (string[])e.Data.GetData(DataFormats.FileDrop);
            string fileName = strs[0];

            LoadAttachmentFromFile(fileName);

            e.Handled = true;
        }

        private void File_PreviewDragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.Copy;
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
        }

        private void File_PreviewDragOver(object sender, DragEventArgs e)
        {
            e.Handled = true;
        }

        private void FileSaveButton_Click(object sender, RoutedEventArgs e)
        {
            PersonAttachment att = PersonAttachmentListBox.SelectedItem as PersonAttachment;
            if (att == null) return;

            SaveFileDialog dlg = new SaveFileDialog();
            var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            dlg.DefaultExt = att.FileExtension;
            dlg.FileName = System.IO.Path.Combine(path, att.FileName) + "." + att.FileExtension;
            if (dlg.ShowDialog() == true)
            {
                File.WriteAllBytes(dlg.FileName, att.FileBytes);
            }
        }

        private void FileOpenButton_Click(object sender, RoutedEventArgs e)
        {
            PersonAttachment att = PersonAttachmentListBox.SelectedItem as PersonAttachment;
            if (att == null) return;
            try
            {
                SaveFileDialog dlg = new SaveFileDialog();
                var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                var fullPath = System.IO.Path.Combine(path, att.FileName) + "." + att.FileExtension;
                File.WriteAllBytes(fullPath, att.FileBytes);
                Process.Start(fullPath);
            }
            catch (IOException ex)
            {
                MessageBox.Show(ex.Message, "Error opening file", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Other exception type", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            PersonAttachment att = PersonAttachmentListBox.SelectedItem as PersonAttachment;
            if (att != null)
            {
                att.IsMarkedForDeletion = true;
            }
        }
    }
}
