using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiJianShu.Model
{
    public class SubscriptionItem : ObservableObject
    {
        private bool isSubscribed;
        public bool IsSubscribed
        {
            get
            {
                return isSubscribed;
            }
            set
            {
                isSubscribed = value;
                RaisePropertyChanged();
            }
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string NewlyAddedAt { get; set; }
        public string Desc { get; set; }
        public string Image { get; set; }
        public int NotesCount { get; set; }
        public int SubscribersCount { get; set; }

    }
}
