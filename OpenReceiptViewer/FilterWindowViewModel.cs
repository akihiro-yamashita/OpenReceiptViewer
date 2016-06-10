using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenReceiptViewer
{
    public class FilterWindowViewModel : NotifyPropertyChanged
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
    }
}
