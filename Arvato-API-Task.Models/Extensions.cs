using System;
using System.Collections.Generic;
using System.Text;

namespace Arvato_API_Task.Models
{
    public static class Extensions
    {
        public static string ConcatWithDelimeter(this ICollection<string> arr, char delimeter)
        {
            StringBuilder sb = new StringBuilder();
            int index = 0;
            foreach (var word in arr)
            {
                if (string.IsNullOrEmpty(word))
                    continue;
                sb.Append(word);
                if(index < arr.Count-1) sb.Append(delimeter);

                index++;
            }
            return sb.ToString();
        }
    }
}
