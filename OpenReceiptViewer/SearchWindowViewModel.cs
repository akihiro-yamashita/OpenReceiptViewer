using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenReceiptViewer
{
    /// <summary></summary>
    public class SearchWindowViewModel : NotifyPropertyChanged
    {
        /// <summary></summary>
        public string Input
        {
            get
            {
                return this._input;
            }
            set
            {
                this._input = value;
                OnPropertyChanged("Input");
            }
        }
        private string _input;

        ///// <summary></summary>
        //public RelayCommand OkCommand
        //{
        //    get
        //    {
        //        return _okCommand = _okCommand ??
        //        new RelayCommand(() =>
        //        {
        //        });
        //    }
        //}
        //private RelayCommand _okCommand;
    }
}
