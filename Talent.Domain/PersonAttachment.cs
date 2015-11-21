using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ucla.Common.BaseClasses;

namespace Talent.Domain
{
    public class PersonAttachment : DomainBase
    {
        #region Fields

        private int _personAttachmentId;
        private int _personId;
        private string _caption = String.Empty;
        private string _fileName = String.Empty;
        private string _fileExtension = String.Empty;
        private byte[] _fileBytes;

        #endregion

        #region Properties

        public int PersonAttachmentId
        {
            get { return _personAttachmentId; }
            set
            {
                if (_personAttachmentId == value) return;
                _personAttachmentId = value;
                OnPropertyChanged();
            }
        }

        public int PersonId
        {
            get { return _personId; }
            set
            {
                if (_personId == value) return;
                _personId = value;
                OnPropertyChanged();
            }
        }

        public string Caption
        {
            get { return _caption; }
            set
            {
                if (_caption == (value ?? String.Empty)) return;
                _caption = value ?? String.Empty;
            }
        }

        public string FileName
        {
            get { return _fileName; }
            set
            {
                if (_fileName == (value ?? String.Empty)) return;
                _fileName = value ?? String.Empty;
            }
        }

        public string FileExtension
        {
            get { return _fileExtension; }
            set
            {
                if (_fileExtension == (value ?? String.Empty)) return;
                _fileExtension = value ?? String.Empty;
            }
        }

        public byte[] FileBytes
        {
            get { return _fileBytes; }
            set
            {
                if (_fileBytes == value) return;
                _fileBytes = value;
            }
        }

        #endregion

        #region overrides

        public override string ToString()
        {
            return (FileName ?? "") + (FileExtension ?? "");
        }

        #endregion
    }
}

