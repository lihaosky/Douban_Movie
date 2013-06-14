using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace PanoramaApp2.JsonParser
{
    class JsonParsers
    {
        public static string getValue(JObject obj, string attr)
        {
            string value = "";
            if (obj != null)
            {
                try
                {
                    value = (string)obj[attr];
                }
                catch (Exception)
                {
                }
            }
            if (value == null)
            {
                return "";
            }
            return value;
        }

        public static string getDouble(JObject obj, string first, string second)
        {
            string value = "";
            try
            {
                value = (string)obj[first][second];
            }
            catch (Exception)
            {
            }
            if (value == null)
            {
                return "";
            }
            return value;
        }


        public static string getArray(JObject obj, string attr)
        {
            if (obj == null)
            {
                return "";
            }
            string value = "";
            try
            {
                object[] genres = obj[attr].ToArray();
                if (genres == null || genres.Length <= 0)
                {
                    return "";
                }
                for (int i = 0; i < genres.Length - 1; i++)
                {
                    value += genres[i].ToString();
                    value += " / ";
                }
                value += genres[genres.Length - 1].ToString();
            }
            catch (Exception)
            {
            }
            return value;
        }

        public static string getValue(JToken obj, string attr)
        {
            string value = "";
            if (obj != null)
            {
                try
                {
                    value = (string)obj[attr];
                }
                catch (Exception)
                {
                }
            }
            if (value == null)
            {
                return "";
            }
            return value;
        }

        public static string getDouble(JToken obj, string first, string second)
        {
            string value = "";
            try
            {
                value = (string)obj[first][second];
            }
            catch (Exception)
            {
            }
            if (value == null)
            {
                return "";
            }
            return value;
        }

        public static string getTriple(JToken obj, string first, string second, string third)
        {
            string value = "";
            try
            {
                value = (string)obj[first][second][third];
            }
            catch (Exception)
            {
            }
            if (value == null)
            {
                return "";
            }
            return value;
        }
    }
}
