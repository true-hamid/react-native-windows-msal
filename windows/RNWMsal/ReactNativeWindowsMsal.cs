using Microsoft.ReactNative.Managed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RNWMsal
{
    [ReactModule]
    class ReactNativeWindowsMsal
    {
        [ReactConstant]
        public double E = Math.E;

        [ReactConstant("Pi")]
        public double PI = Math.PI;

        [ReactMethod("add")]
        public double Add(double a, double b)
        {
            double result = a + b;
            return result;
        }
    }
}
