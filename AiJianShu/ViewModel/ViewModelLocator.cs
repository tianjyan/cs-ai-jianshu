using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;

namespace AiJianShu.ViewModel
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            SimpleIoc.Default.Register<MainViewModel>();
        }
        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        public UserCenterViewModel UserCenter
        {
            get
            {
                return MainViewModel.userCenterViewModel;
            }
        }

        public SpecialTopicViewModel SpecialTopic
        {
            get
            {
                return MainViewModel.specialTopicViewModel;
            }
        }


        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}
