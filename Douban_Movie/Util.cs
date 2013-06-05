using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            starPath += " star.png";
            return starPath;
        }
    }
}
