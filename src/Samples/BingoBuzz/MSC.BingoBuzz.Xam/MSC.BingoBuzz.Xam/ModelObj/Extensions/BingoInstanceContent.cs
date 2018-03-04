using CodeGenHero.Xam.MvvmLight;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeGenHero.BingoBuzz.Xam.ModelObj.BB
{
    public partial class BingoInstanceContent : BaseAuditEdit
    {
        private bool _isSelected;

        public bool IsSelected
        {
            get { return _isSelected; }
            set { Set(ref _isSelected, value); }
        }
    }
}