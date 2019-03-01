using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akari
{
    public class Utilities
    {
        public static void WaitForSeconds(float seconds)
        {
            System.Threading.Thread.Sleep((int)(seconds * 1000));
        }

        public static void TypeWrite(string text, float delay)
        {
            Console.WriteLine("");
            for (int i = 0; i < text.Length; i++)
            {
                Console.Write(text.ElementAt(i));
                WaitForSeconds(delay);
            }
        }

        public static void VerticalWrite(string[] textRows, float delay)
        {
            Console.WriteLine("");
            for (int i = 0; i < textRows.Length; i++)
            {
                Console.WriteLine(textRows[i]);
                WaitForSeconds(delay);
            }
        }
    }
}
