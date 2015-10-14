using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiJianShu.Model
{
    public class FollowUser : ObservableObject
    {
        private bool isFollowing;
        public bool IsFollowing
        {
            get
            {
                return isFollowing;
            }
            set
            {
                isFollowing = value;
                RaisePropertyChanged();
            }
        }
        
        public bool IsFollower { get; set; }
        public string Id { get; set; }
        public string NickName { get; set; }
        public string Avatar { get; set; }
        
        public int TotalLikesReceived { get; set; }
        public int TotalWordage { get; set; }
    }
}
