using System;

using MSC.BingoBuzz.Xam.Models;

namespace MSC.BingoBuzz.Xam.ViewModels
{
    public class ItemDetailViewModel : BaseViewModel
    {
        public Item Item { get; set; }
        public ItemDetailViewModel(Item item = null)
        {
            Title = item?.Text;
            Item = item;
        }
    }
}
