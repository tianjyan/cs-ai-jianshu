using AiJianShu.Common;
using AiJianShu.Model;
using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace AiJianShu.Converter
{
    public class CompactPaneLengthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string device = value.ToString();
            double length = 0;
            switch (device)
            {
                case "Desktop":
                    length = (double)App.Current.Resources["CompactPaneLength"];
                    break;
                case "Mobile":
                    break;
                default:
                    break;
            }
            return length;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class ScrollVisbilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string device = value.ToString();
            ScrollBarVisibility visibility = ScrollBarVisibility.Auto;
            switch (device)
            {
                case "Desktop":             
                    break;
                case "Mobile":
                    visibility = ScrollBarVisibility.Hidden;
                    break;
                default:
                    break;
            }
            return visibility;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class StringVisbilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (string.IsNullOrEmpty(value?.ToString()))
            {
                return Visibility.Collapsed;
            }
            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class DateTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            long ticks;
            if (long.TryParse(value?.ToString(),out ticks))
            {
                DateTime dt = DateTime.Parse("1970-01-01 08:00");
                return dt.AddSeconds(ticks).ToString("yyyy/MM/dd-HH:mm");
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class MarginConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string device = value.ToString();
            Thickness margin;
            switch (device)
            {
                case "Desktop":
                    margin = new Thickness(3,0,0,0);
                    break;
                case "Mobile":
                    margin = new Thickness(38,0,0,0);
                    break;
                default:
                    break;
            }
            return margin;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class FollowVisbilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            UserContext u = value as UserContext;
            if(u?.IsLogin == true)
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class UriConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null || value.ToString().EndsWith("default_avatar.png"))
            {
               return  GlobalValue.DefaultAvatar;
            }
            return new Uri(value.ToString(), UriKind.Absolute);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class ActivityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            ActivityType type = (ActivityType)value;
            string result = "";
            switch(type)
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

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class BoolVisiblityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool? result = value as Nullable<bool>;
            if(result == true)
            {
                return Visibility.Visible;
            }
            return Visibility.Collapsed;

        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class FollowConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool result = (bool)value;
            if (result == true)
            {
                return App.Current.Resources["UnFollow"];
            }
            return App.Current.Resources["Follow"];

        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class NotesCountConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            int result = (int)value;
            return result.ToString() + App.Current.Resources["ArticleCount"];
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class SubscribersCountConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            int result = (int)value;
            return result.ToString() + App.Current.Resources["FollowCount"];
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class CoeditorsToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            List<string> result = (List<string>)value;
            string allNickName = "";
            foreach(var r in result)
            {
                allNickName += (r + " ");
            }
            return allNickName + App.Current.Resources["Coeditors"];
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class CacheSizeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string result = "0";
            long temp;
            if(long.TryParse(value?.ToString(),out temp))
            {
                if (temp > 1024 * 1024)
                {
                    result = string.Format("{0:F3}MB", temp / 1024 / 1024.0);
                }
                else
                {
                    result = temp / 1024 + "KB";
                }
            }
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
