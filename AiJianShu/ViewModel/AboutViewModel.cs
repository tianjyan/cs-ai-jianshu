using AiJianShu.Common;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Windows.Input;

namespace AiJianShu.ViewModel
{
    public class AboutViewModel : ViewModelBase
    {
        #region 字段
        private long cacheSize;
        #endregion

        #region
        public long CacheSize
        {
            get
            {
                return cacheSize;
            }
            set
            {
                cacheSize = value;
                RaisePropertyChanged();
            }

        }
        public ICommand CleanCache { get; set; }
        #endregion

        public AboutViewModel()
        {
            InitCMD();
        }

        public void RefreshSetting()
        {
            CacheSize = Untils.GetCacheSize();
        }


        public void InitCMD()
        {
            CleanCache = new RelayCommand(CleanupCache);
        }

        private void CleanupCache()
        {
            Untils.CleanUpCache();
            CacheSize = Untils.GetCacheSize();
        }
    }
}
