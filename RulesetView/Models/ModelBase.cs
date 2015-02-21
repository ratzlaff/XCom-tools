using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using RulesetView.Annotations;

namespace RulesetView.Models
{
    public class ModelBase : INotifyPropertyChanged
    {
        private readonly Dictionary<string, object> _values = new Dictionary<string, object>();

        protected T GetValue<T>([CallerMemberName] string propertyName = "")
        {
            object value;
            _values.TryGetValue(propertyName, out value);
            if (value is T) return (T) value;
            return default (T);
        }

        protected void SetValue<T>(T value, [CallerMemberName] string propertyName = "")
        {
            _values[propertyName] = value;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}