//using System;
//using System.Diagnostics;
//using System.Reflection;

//namespace Blauhaus.Analytics.Abstractions.Service
//{
//    public static class CallingClass
//    {
//        public static string NameOfCallingClass()
//        {
//            string fullName;
//            Type declaringType;
//            var skipFrames = 2;
//            do
//            {
//                var method = new StackFrame(skipFrames, false).GetMethod();
//                declaringType = method.DeclaringType;
//                if (declaringType == null)
//                {
//                    return method.Name;
//                }
//                skipFrames++;
//                fullName = declaringType.Name;
//            }
//            while (declaringType.Module.Name.Equals("mscorlib.dll", StringComparison.OrdinalIgnoreCase));

//            return fullName;
//        }
//    }
//}