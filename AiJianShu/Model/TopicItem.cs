using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiJianShu.Model
{
    public class TopicItem : ObservableObject
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
        public string Slug { get; set; }
        public string NewlyAddedAt { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public int NotesCount { get; set; }
        public int SubscribersCount { get; set; }
        public string Owner { get; set; }
        public List<string> Coeditors { get; set; }
    }
}
