using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace PanoramaApp2
{
    class Util
    {
        /// <summary>
        /// Get star image path from rating
        /// </summary>
        /// <param name="rate"></param>
        /// <returns></returns>
        public static string getStarPath(string rate)
        {
            double rating = 0;
            try
            {
                rating = double.Parse(rate);
            }
            catch (System.FormatException)
            {
            }
            double stars = rating / 2.0;
            int baseStar = (int)stars;
            int roundStar = (int)Math.Round(stars);
            bool half = roundStar > baseStar ? true : false;
            string starPath = App.imagePath + baseStar;
            if (half)
            {
                starPath += 5;
            }
            else
            {
                starPath += 0;
            }
            starPath += " star.png";
            return starPath;
        }

        public static string formatShortReview(string review)
        {
            string newReview = review.Trim().Replace(Environment.NewLine, "");
            newReview = replaceSpecialChar(newReview);
            return newReview;
        }

        public static string replaceSpecialChar(string text)
        {
            text = text.Replace("&amp;", "&");
            text = text.Replace("&nbsp;", " ");
            text = text.Replace("&ndash;", "-");
            text = text.Replace("&quot;", "\"");
            text = text.Replace("&ldquo;", "\"");
            text = text.Replace("&rdquo;", "\"");
            text = text.Replace("&lsquo;", "'");
            text = text.Replace("&rsquo;", "'");
            text = text.Replace("&laquo;", "<");
            text = text.Replace("&raquo;", ">");
            return text;
        }
    }
}
