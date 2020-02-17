using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace markov_drummer.Vm
{
    public partial class MainVm : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        public IEnumerable GetErrors(string propertyName)
        {
            _errorsDictionary.TryGetValue(propertyName ?? String.Empty, out var errors);
            return errors;
        }

        private readonly Dictionary<string, List<string>> _errorsDictionary = new Dictionary<string, List<string>>();

        public bool HasErrors => _errorsDictionary.Any(t => t.Value.Any());


        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        private void SetErrorFor(string property, string error)
        {
            if (!_errorsDictionary.ContainsKey(property))
                _errorsDictionary[property] = new List<string> {error};
            else
                _errorsDictionary[property].Add(error);

            OnErrorsChanged(property);
        }

        private void ClearErrorFor(string property)
        {
            _errorsDictionary[property] = new List<string> { };
            OnErrorsChanged(property);
        }

        private void ClearAllErrors()
        {
            var names = _errorsDictionary.Keys.ToArray();

            _errorsDictionary.Clear();

            foreach (var name in names)
            {
                OnErrorsChanged(name);
            }
        }

        public IEnumerable<string> ErrorsPlain => _errorsDictionary.Values.SelectMany(s => s).Distinct();

        private void OnErrorsChanged(string name)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(name));
            OnPropertyChanged(nameof(ErrorsPlain));
        }


    }
}