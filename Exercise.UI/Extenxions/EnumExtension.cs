using System;
using System.Linq;

namespace Exercise.UI.Extenxions
{
    public static class EnumExtension
    {
        public static T[] GetValues<T>() where T : struct
        {
            var enums = Enum.GetValues(typeof(T)).Cast<T>().ToArray();
            return enums;
        }
    }
}