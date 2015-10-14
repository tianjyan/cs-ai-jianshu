using System;
using System.Collections;
using System.Text.RegularExpressions;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Media;

namespace AiJianShu.UserControls
{
    public sealed partial class ExtendRichTextBox : UserControl
    {
        public ExtendRichTextBox()
        {
            this.InitializeComponent();
        }

        public const string UserRegexStr = "\\<a\\s(href\\=\"|[^\\>]+?\\shref\\=\")(?<link>[^\"]+)\".*?\\>(?<text>.*?)(\\<\\/a\\>|$)";
        public const string ImgRegexStr = "\\<img\\s(src\\=\"|[^\\>]+?\\ssrc\\=\")(?<link>[^\"]+)\".*?\\>";

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(ExtendRichTextBox), 
            new PropertyMetadata(null, new PropertyChangedCallback(ExtendRichTextBox.OnTextProperyChanged)));

        public static readonly DependencyProperty NickNameProperty = DependencyProperty.Register("NickName", typeof(string), typeof(ExtendRichTextBox), null);

        public static readonly DependencyProperty NoteIdProperty = DependencyProperty.Register("NoteId", typeof(string), typeof(ExtendRichTextBox), null);

        public event EventHandler<HyperLinkEventArgs> OnClick;

        public string Text
        {
            get
            {
                return (string)base.GetValue(TextProperty);
            }
            set
            {
                base.SetValue(TextProperty, value);
            }
        }

        public string NickName
        {
            get
            {
                return (string)base.GetValue(NickNameProperty);
            }
            set
            {
                base.SetValue(NickNameProperty, value);
            }
        }

        public string NoteId
        {
            get
            {
                return (string)base.GetValue(NoteIdProperty);
            }
            set
            {
                base.SetValue(NoteIdProperty, value);
            }
        }

        //public TypedEventHandler<Hyperlink,HyperlinkClickEventArgs> OnClick
        //{
        //    get
        //    {
        //        return (TypedEventHandler<Hyperlink, HyperlinkClickEventArgs>)base.GetValue(OnClickProperty);
        //    }
        //    set
        //    {
        //        base.SetValue(OnClickProperty, value);
        //    }
        //}


        private static void OnTextProperyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            (obj as ExtendRichTextBox).OnTextChanged(e.NewValue?.ToString());
        }


        private void OnTextChanged(string content)
        {
            if (!string.IsNullOrEmpty(content))
            {
                content = content.Trim().Replace("<br>", "\r\n");
            }
            else
            {
                return;
            }

            MainTextBlock.Inlines.Clear();

            if (!string.IsNullOrEmpty(NickName))
            {
                MainTextBlock.Inlines.Add(new Run() { Text = NickName + ":", Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 0x66, 0xB3, 0xFF))});
            }

            ReplaceUser(content);
        }
        private void ReplaceUser(string content)
        {
            Regex regex = new Regex(UserRegexStr, RegexOptions.IgnoreCase | RegexOptions.Multiline);
            int index = 0;
            IEnumerator var = regex.Matches(content).GetEnumerator();
            while (var.MoveNext())
            {
                Match match = var.Current as Match;
                if(match.Index >= index)
                {
                    //将匹配到的一截字符串之前的字符进行图片的匹配
                    this.ReplaceImg(content.Substring(index, match.Index - index));
                    //设置匹配字符串的末尾的索引
                    index = match.Index + match.Length;
                    this.AppendLink(match.Groups["text"].Value, match.Groups["link"].Value);
                }
            }

            //如果索引没有到字符串结束的位置
            if(index < content.Length)
            {
                this.ReplaceImg(content.Substring(index));
            }
        }

        private void ReplaceImg(string content)
        {
            Regex regex = new Regex(ImgRegexStr);
            int index = 0;
            IEnumerator var = regex.Matches(content).GetEnumerator();
            while(var.MoveNext())
            {
                Match match = var.Current as Match;
                if(match.Index >=  index)
                {
                    //将匹配到的一截字符串之前的字符加到文本中
                    this.AppendText(content.Substring(index, match.Index - index));
                    //设置匹配字符串的末尾的索引
                    index = match.Index + match.Length;
                    this.AppendImg(match.Groups["link"].Value);
                }
            }
            //如果索引没有到字符串结束的位置
            if (index < content.Length)
            {
                this.AppendText(content.Substring(index));
            }
        }

        private void AppendText(string content)
        {
            MainTextBlock.Inlines.Add(new Run() { Text = content });
        }

        private void AppendImg(string text)
        {
            MainTextBlock.Inlines.Add(new Run() { Text = "[Emoji]" });
        }


        private void AppendLink(string text, string link)
        {
            Hyperlink hyperLink = new Hyperlink();
            //hyperLink.NavigateUri = new Uri(link);
            hyperLink.Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 0x66, 0xB3, 0xFF));;
            hyperLink.UnderlineStyle = UnderlineStyle.None;
            hyperLink.SetValue(NameProperty, link);
            //if(this.OnClick != null)
            //{
            //    hyperLink.Click += this.OnClick;
            //}
            hyperLink.Click += HyperLink_Click;
            hyperLink.Inlines.Add(new Run() { Text = text });
            MainTextBlock.Inlines.Add(hyperLink);
        }

        private void HyperLink_Click(Hyperlink sender, HyperlinkClickEventArgs args)
        {
            HyperLinkEventArgs arg;
            if (NoteId == null)
            {
                arg = new HyperLinkEventArgs(args.OriginalSource) { Tag = new Tuple<string, string>(item1: "user", item2: SplitUserId(sender.Name)) };
            }
            else
            {
                arg = new HyperLinkEventArgs(args.OriginalSource) { Tag = new Tuple<string, string, string>(item1: "user", item2: SplitUserId(sender.Name), item3: NoteId) };
            }
            if (OnClick != null)
            {
                OnClick(sender, arg);
            }
        }

        private string SplitUserId(string link)
        {
            return link.Substring(link.LastIndexOf('/') + 1);
        }
    }
}
