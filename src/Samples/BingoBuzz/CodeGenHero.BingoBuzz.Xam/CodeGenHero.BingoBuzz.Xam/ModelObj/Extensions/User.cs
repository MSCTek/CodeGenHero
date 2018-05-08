using GalaSoft.MvvmLight;
using CodeGenHero.Xam.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace CodeGenHero.BingoBuzz.Xam.ModelObj.BB
{
    public partial class User : BaseAuditEdit
    {
        private bool _isSelected;

        public string CheckboxIcon
        {
            get { return IsSelected ? (string)App.Current.Resources["FAIcon.FACheckSquareO"] : (string)App.Current.Resources["FAIcon.FASquareO"]; }
        }

        public string FullName
        {
            get { return $"{FirstName} {LastName}"; }
        }

        public string UserIdDisplay
        {
            get { return $"{UserId.ToString()}"; }
        }

        public bool IsSelected
        {
            get { return _isSelected; }
            set { Set(ref _isSelected, value); RaisePropertyChanged(nameof(CheckboxIcon)); }
        }

        public RelayCommand TapCheckboxCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    //whatever it is now, reverse it
                    IsSelected = IsSelected ? false : true;
                });
            }
        }
    }
}