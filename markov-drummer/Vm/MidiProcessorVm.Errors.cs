using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace markov_drummer.Vm
{
    public partial class MidiProcessorVm : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        private readonly Dictionary<string,string> _errorsDictionary = new Dictionary<string, string>();

        private void ClearErrors()
        {
            _errorsDictionary.Clear();
            OnErrorsChanged(new DataErrorsChangedEventArgs(String.Empty));
        }

        private void SetErrorFor(string propertyName, string value)
        {
            _errorsDictionary[propertyName] = value;
            OnErrorsChanged(new DataErrorsChangedEventArgs(String.Empty));
        }

        public IEnumerable GetErrors(string propertyName)
        {
            yield return _errorsDictionary[propertyName];
        }

        public bool HasErrors => _errorsDictionary.Any();
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        protected virtual void OnErrorsChanged(DataErrorsChangedEventArgs e)
        {
            ErrorsChanged?.Invoke(this, e);
        }
    }
}