using System;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;

namespace JianShuCore.Common
{
    internal static class Untils
    {
        #region Staitc Function
        //Reference:
        //https://social.msdn.microsoft.com/Forums/en-US/2c42c952-4ea6-4050-9979-00bcfaff10f4/windows-8-c-how-to-keep-listbox-items?forum=winappswithcsharp
        internal static string Serialize(object obj)
        {
            if(obj == null)
            {
                throw new ArgumentNullException();
            }

            
            XmlSerializer serializer = new XmlSerializer(obj.GetType());
            using (StringWriter writer = new StringWriter())
            {
                serializer.Serialize(writer, obj);
                return SerializeBase64(writer.ToString());
            }
        }

        internal static T Deserialize<T>(string source)
        {
            if (source == null)
            {
                throw new ArgumentNullException();
            }

            string xml = DeserializeBase64(source);

            //Type of object will raise InvalidOperationException.
            //So if T is object, give the XmlSerializer with the argument of type of string.
            System.Type type = typeof(T);
            if (type == typeof(object))
            {
                type = typeof(string);
            }
            XmlSerializer serializer = new XmlSerializer(type);
            using (StringReader reader = new StringReader(xml))
            {
                return (T)serializer.Deserialize(reader);
            }
        }

        internal static string SerializeBase64(string str)
        {
            byte[] bytes = System.Text.Encoding.Unicode.GetBytes(str);
            return Convert.ToBase64String(bytes);
        }

        internal static string DeserializeBase64(string str)
        {
            byte[] bytes = Convert.FromBase64String(str);
            return System.Text.Encoding.Unicode.GetString(bytes);
        }

        //Get the description of the enum
        //Reference:http://blogs.msdn.com/b/abhinaba/archive/2005/10/20/483000.aspx
        internal static string GetDescription(Enum en)
        {
            Type type = en.GetType();
            MemberInfo[] memInfo = type.GetMember(en.ToString());
            if (memInfo != null && memInfo.Length > 0)
            {
                DescriptionAttribute des = memInfo[0].GetCustomAttribute<DescriptionAttribute>(false);
                return des.Description;
            }
            return en.ToString();
        }

        internal static void ChangeStatus(StatusType status)
        {
            if(Init.StatusProvider != null)
            {
                Init.StatusProvider.ChangeStatus(status);
            }
        }
        #endregion
    }
}
