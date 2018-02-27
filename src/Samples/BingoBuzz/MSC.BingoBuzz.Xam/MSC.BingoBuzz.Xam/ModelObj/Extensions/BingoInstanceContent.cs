using CodeGenHero.EAMVCXamPOCO.Xam.ModelObj;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Text;

namespace MSC.BingoBuzz.Xam.ModelObj.BB
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