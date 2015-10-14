using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiJianShu.Model
{
    public class NoteItem : ObservableObject
    {
        private bool isLike;
        public bool IsLike
        {
            get
            {
                return isLike;
            }
            set
            {
                isLike = value;
                RaisePropertyChanged();
            }
        }

        public string Avatar { get; set; }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Desc { get; set; }
        public int PublishedAt { get; set; }
        public string NickName { get; set; }
    }
}
