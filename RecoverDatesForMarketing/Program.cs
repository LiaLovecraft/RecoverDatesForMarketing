using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;

namespace RecoverDatesForMarketing
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine(@"Please enter the name of the file you wish to parse. If the file is not in the current directory, please specify the full path as well as the filename");
                Console.WriteLine("Usage: RecoverDatesForMarketing <file_to_parse>");
                return;
            }

            var fname = args[0];
            if (!File.Exists(fname))
            {
                Console.WriteLine($"Specified file '{fname}' not found");
                return;
            }

            //caveat: if this needed to handle huge files that would tax system memory, I'd look 
            //into other approaches and process the file in manageable chunks instead of all at once
            var txt = File.ReadAllText(fname);

            //strip non-printing control chars including crlf; alternative since this is an all-numeric file
            //would be to just remove everything that isn't a digit using [^\d] - would be more thorough ...
            var pattern = @"[\x00-\x1F\x7F]";
            var rgx = new Regex(pattern);
            txt = rgx.Replace(txt, "");

            //grab "date-like" matches as a first step; doesn't account for leap years, "June 31st," etc., or future dates
            pattern = @"((?:1[0-2]|0[1-9])(?:3[01]|[12][0-9]|0[1-9])\d{4})";
            rgx = new Regex(pattern);
            var possibleDateMatches = rgx.Matches(txt);

            List<DateTime> dates = new List<DateTime>();
            DateTime tmpDate;
            var today = DateTime.Now.Date;
            if (possibleDateMatches.Count > 0)
            {
                foreach (Match m in possibleDateMatches)
                {
                    //use TryParseExact to filter out leap year violations etc., and then filter if not before today
                    if (DateTime.TryParseExact(m.Value, "MMddyyyy",
                        CultureInfo.InvariantCulture, DateTimeStyles.None, out tmpDate) && tmpDate < today)
                    {
                        dates.Add(tmpDate);
                    }
                }
            }

            if (dates.Count == 0)
            {
                Console.WriteLine("No dates earlier than today found");
            }
            else
            {
                Console.WriteLine("Found the following dates:");
                foreach (DateTime date in dates)
                {
                    Console.WriteLine(date.ToString("MMddyyyy"));
                }
            }

        }
    }
}
