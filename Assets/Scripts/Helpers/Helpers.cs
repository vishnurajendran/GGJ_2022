using System;
using System.Collections.Generic;
using System.Drawing;

namespace Flippards.Helpers
{
    public static class Helpers
    {
        private static Random rng = new Random();

        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                (list[k], list[n]) = (list[n], list[k]);
            }
        }

        public static string GetRichText(this string text, string col)
        {
            return $"<color={col}>{text}</color>";
        }
    }
}