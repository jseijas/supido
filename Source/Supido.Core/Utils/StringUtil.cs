using System;
using System.IO;
using System.Text;
using System.Linq;

namespace Supido.Core.Utils
{
    /// <summary>
    /// Static class for helper methods for use with strings.
    /// </summary>
    public static class StringUtil
    {
        /// <summary>
        /// If the texts starts with trimText, then remove trimText from text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="trimText">The trim text.</param>
        /// <returns></returns>
        public static string TrimStringStart(string text, string trimText)
        {
            return text.StartsWith(trimText) ? text.Remove(0, trimText.Length) : text;
        }

        /// <summary>
        /// Camelizes one string.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="fistCapital">if set to <c>true</c> [fist capital].</param>
        /// <returns></returns>
        public static string Camelize(string name, bool fistCapital = false)
        {
            if (string.IsNullOrEmpty(name))
            {
                return name;
            }
            StringBuilder builder = new StringBuilder();
            name = name.ToLower();
            bool capitalize = fistCapital;
            for (int i = 0; i < name.Length; i++)
            {
                char c = name[i];
                if (char.IsLetter(c))
                {
                    if (char.IsUpper(c))
                    {
                        builder.Append(c);
                    }
                    else
                    {
                        builder.Append(capitalize ? char.ToUpper(c) : c);
                    }
                    capitalize = false;
                }
                else
                {
                    if (char.IsDigit(c))
                    {
                        builder.Append(c);
                    }
                    capitalize = true;
                }
            }
            return builder.ToString();
        }

        /// <summary>
        /// Pascalizes one string.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public static string Pascalize(string name)
        {
            return StringUtil.Camelize(name, true);
        }

        /// <summary>
        /// Pops the substring of a string between two strings.
        /// Example:   
        /// - str = abcd[[efg]]hij 
        /// - initStr = [[ 
        /// - endStr = ]] 
        /// => returns efg
        /// => str = abcdhij
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="initStr">The init string.</param>
        /// <param name="endStr">The end string.</param>
        /// <returns></returns>
        public static string PopBetween(ref string str, string initStr, string endStr)
        {
            int start = str.IndexOf(initStr);
            int end = str.IndexOf(endStr);
            if ((start != -1) && (end != -1))
            {
                string copystr = str.Substring(start + initStr.Length);
                string result = copystr.Substring(0, end - 1);
                str = str.Remove(start, end + endStr.Length);
                return result;
            }
            return "";
        }

        /// <summary>
        /// Loads the file and return the content as string.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        public static string LoadFile(string fileName)
        {
            if (File.Exists(fileName))
            {
                return File.ReadAllText(fileName);
            }
            return string.Empty;
        }

        /// <summary>
        /// Writes content into one file, forcing the directory.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="content">The content.</param>
        public static void WriteFile(string fileName, string content)
        {
            FileInfo info = new FileInfo(fileName);
            Directory.CreateDirectory(info.Directory.FullName);
            File.WriteAllText(fileName, content);
        }


        /// <summary>
        /// Adds leading zeroes until given length.
        /// </summary>
        /// <param name="i">The i.</param>
        /// <param name="l">The l.</param>
        /// <returns></returns>
        public static string LeadZeros(int i, int l)
        {
            string result = i.ToString();
            while (result.Length < l)
            {
                result = "0" + result;
            }
            return result;
        }

        /// <summary>
        /// Splits string into lines.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <param name="removeEmpty">if set to <c>true</c> [remove empty].</param>
        /// <returns></returns>
        public static string[] SplitLines(string s, bool removeEmpty = true)
        {
            StringSplitOptions options = removeEmpty ? StringSplitOptions.RemoveEmptyEntries : StringSplitOptions.None;
            return s.Split(new[] { '\r', '\n' }, options);
        }

        /// <summary>
        /// Pop from a string until find separator.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <param name="separator">The separator.</param>
        /// <param name="trimmed">if set to <c>true</c> [trimmed].</param>
        /// <returns></returns>
        public static string PopUntil(ref string s, string separator, bool trimmed = true)
        {
            int i = s.IndexOf(separator);
            string result = string.Empty;
            if (i == -1)
            {
                result = s;
                s = string.Empty;
            }
            else
            {
                result = s.Substring(0, i);
                s = s.Remove(0, i + separator.Length);
            }
            if (trimmed)
            {
                result = result.Trim();
                s = s.Trim();
            }
            return result;
        }

        /// <summary>
        /// Removes first and last characters from string.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <param name="trimmed">if set to <c>true</c> [trimmed].</param>
        /// <returns></returns>
        public static string RemoveFirstAndLast(string s, bool trimmed = true)
        {
            string result = s.Remove(0, 1);
            result = result.Remove(result.Length - 1, 1);
            if (trimmed)
            {
                result = result.Trim();
            }
            return result;
        }

        /// <summary>
        /// Makes uppercase the first character of a string.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns></returns>
        public static string FirstUpper(string s)
        {
            return char.ToUpper(s[0]) + s.Substring(1);
        }

        /// <summary>
        /// Unquotes the specified string.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns></returns>
        public static string Unquote(string s)
        {
            int n = s.Length;
            return (n > 2 && s.First() == '"' && s[n - 1] == '"')
                ? s.Substring(1, n - 2)
                : s;
        }

    }
}
