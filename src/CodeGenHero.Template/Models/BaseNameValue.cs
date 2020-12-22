using System;
using System.ComponentModel;

namespace CodeGenHero.Template
{
    public abstract class BaseNameValue<T> : INotifyPropertyChanged
    {
        private string _name;
        private T _value;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    NotifyPropertyChanged("Name");
                }
            }
        }

        public T Value
        {
            get { return _value; }
            set
            {
                if (_value == null || !_value.Equals(value))
                {
                    _value = value;
                    NotifyPropertyChanged("Value");
                }
            }
        }

        public void NotifyPropertyChanged(String info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }
    }
}