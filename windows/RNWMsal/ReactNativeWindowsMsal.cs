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

        [ReactConstant]
        public string STR = "TEST NATIVE MODULE";

        [ReactMethod]
        public double subtract(double a, double b)
        {
            double result = a - b;
            return result;
        }

        [ReactMethod("add")]
        public void Add(double a, double b)
        {
            double result = a + b;
        }
    }
}
