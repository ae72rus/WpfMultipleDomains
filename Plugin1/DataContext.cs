using System;
using System.Windows.Input;
using CommonAPI;

namespace Plugin1
{
    public class DataContext : DataContextBase
    {
        private ICommand _buttonCommand;
        private string _text = $"Domain: {AppDomain.CurrentDomain.FriendlyName}";

        public string Text
        {
            get => _text;
            set
            {
                _text = value;
                OnPropertyChanged();
            }
        }

        public ICommand ButtonCommand => _buttonCommand ?? (_buttonCommand = new RelayCommand(() => { Text += " clicked"; }));
    }
}