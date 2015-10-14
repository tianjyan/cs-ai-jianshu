using System;

namespace JianShuCore.Common
{
    /// <summary>
    /// Attribute for get the fullname of type
    /// Reference:http://blogs.msdn.com/b/abhinaba/archive/2005/10/20/483000.aspx
    /// </summary>
    public class DescriptionAttribute : Attribute
    {
        public string Description { get; private set; }

        public DescriptionAttribute(string des)
        {
            Description = des;
        }
    }
}
