namespace AiJianShu.UserControls
{
    public class HyperLinkEventArgs : Windows.UI.Xaml.RoutedEventArgs
    {
        public HyperLinkEventArgs()
        {

        }

        public HyperLinkEventArgs(object originalSource)
        {
            OriginalSource = originalSource;
        }

        public new object OriginalSource
        {
            get;
            private set;
        }

        public object Tag { get; set; }
    }
}
