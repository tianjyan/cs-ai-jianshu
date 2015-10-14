using AiJianShu.Model;
using System;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Media;

namespace AiJianShu.UserControls
{
    public sealed partial class ExtendActivityTextBox : UserControl
    {
        public ExtendActivityTextBox()
        {
            this.InitializeComponent();
        }

        public static readonly DependencyProperty ItemProperty = DependencyProperty.Register("Item", typeof(ActivityItem), typeof(ExtendActivityTextBox), 
            new PropertyMetadata(null, new PropertyChangedCallback(ExtendActivityTextBox.OnItemProperyChanged)));


        public ActivityItem Item
        {
            get
            {
                return (ActivityItem)base.GetValue(ItemProperty);
            }
            set
            {
                base.SetValue(ItemProperty, value);
            }
        }

        private static void OnItemProperyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            (obj as ExtendActivityTextBox).OnItemChanged(e.NewValue as ActivityItem);
        }

        private void OnItemChanged(ActivityItem item)
        {
            if(item == null)
            {
                return;
            }
            MainTextBlock.Inlines.Clear();

            Hyperlink userHyperlink = new Hyperlink();
            userHyperlink.Inlines.Add(new Run { Text = item.NickName });
            userHyperlink.Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 0x66, 0xB3, 0xFF));
            userHyperlink.UnderlineStyle = UnderlineStyle.None;
            userHyperlink.Click += HyperlinkClick;
            userHyperlink.SetValue(NameProperty, "user");
            MainTextBlock.Inlines.Add(userHyperlink);

            MainTextBlock.Inlines.Add(new Run() { Text = ConverterType(item.Type) });

            if (item.TargetLink != null)
            {
                if (item.TargetLink.Item1 == "notebook" || item.TargetLink.Item1 == "collection")
                {
                    MainTextBlock.Inlines.Add(new Run() { Text = item.Target, FontWeight = new FontWeight() { Weight = 700 } });
                }
                else
                {
                    Hyperlink targetHyperlink = new Hyperlink();
                    targetHyperlink.Inlines.Add(new Run { Text = item.Target });
                    targetHyperlink.UnderlineStyle = UnderlineStyle.None;
                    targetHyperlink.Click += HyperlinkClick;
                    targetHyperlink.Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 0x66, 0xB3, 0xFF));
                    targetHyperlink.SetValue(NameProperty, "target");
                    MainTextBlock.Inlines.Add(targetHyperlink);
                }
            }
        }

        private void HyperlinkClick(Hyperlink sender, HyperlinkClickEventArgs args)
        {
            HyperLinkEventArgs arg = new HyperLinkEventArgs(args.OriginalSource) { Tag = sender.Name == "user"? Item.UserLink : Item.TargetLink };
            OnClick(sender, arg);
        }

        public event EventHandler<HyperLinkEventArgs> OnClick;

        private Tuple<string,string> SplitLink(string link)
        {
            string[] values = link.Split('-');
            if (values.Length == 2)
            {
                return new Tuple<string, string>(item1: values[0], item2: values[1]); 
            }
            throw new ArgumentException("Invail argument");
        }

        private string ConverterType(ActivityType type)
        {
            string result = "";
            switch (type)
            {
                case ActivityType.None:
                    throw new ArgumentException("The Activity Type should not be none");
                case ActivityType.FollowUser:
                    result = App.Current.Resources["FollowUser"].ToString();
                    break;
                case ActivityType.LikeNote:
                    result = App.Current.Resources["LikeNote"].ToString();
                    break;
                case ActivityType.LikeNoteBook:
                    result = App.Current.Resources["LikeNoteBook"].ToString();
                    break;
                case ActivityType.LikeCollection:
                    result = App.Current.Resources["LikeCollection"].ToString();
                    break;
                case ActivityType.Comment:
                    result = App.Current.Resources["Comment"].ToString();
                    break;
                case ActivityType.Created:
                    result = App.Current.Resources["Created"].ToString();
                    break;
                case ActivityType.Note:
                    result = App.Current.Resources["Note"].ToString();
                    break;
                default:
                    throw new ArgumentException("This type is invaild");
            }
            return " "+result+" ";
        }
    }
}
