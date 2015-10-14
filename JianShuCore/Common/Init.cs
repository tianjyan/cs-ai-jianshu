using JianShuCore.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JianShuCore
{
    public static class Init
    {
        internal static IStatusProvider StatusProvider;

        public static void InitStatusProvider(IStatusProvider provider)
        {
            StatusProvider = provider;
        }
    }
}
