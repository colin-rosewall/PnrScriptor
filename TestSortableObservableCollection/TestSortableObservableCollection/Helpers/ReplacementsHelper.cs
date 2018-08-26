using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pidgin;
using static Pidgin.Parser;

namespace TestSortableObservableCollection.Helpers
{
    static class ReplacementsHelper
    {
        public static bool ReplaceAmaAvail(ref string lineCopy, ref int availabilityCounter, IEnumerable<Models.Flight> flights)
        {
            bool result = false;

            var transactionCodeParser = from gap1 in Parser.SkipWhitespaces
                                  from firstTransactionCode in Parser.CIChar('a')
                                  from secondTransactionCode in OneOf(Try(Parser.CIChar('n')), Try(Parser.CIChar('d')), Try(Parser.CIChar('a')))
                                  select new string(new char[] { firstTransactionCode, secondTransactionCode }).ToUpper();

            var OptionalDayDigitParser = from secondDigit in Parser.Digit.Optional()
                                   select (secondDigit.HasValue ? new string(new char[] { secondDigit.Value }) : string.Empty);

            var dayPartParser = from gap1 in Parser.SkipWhitespaces
                          from firstDigit in Parser.Digit
                          from secondOptionalDigit in OptionalDayDigitParser
                          select new string(new char[] { firstDigit }) + secondOptionalDigit;

            var monthPartParser = from gap1 in Parser.SkipWhitespaces
                            from monthShortName in Parser.OneOf(
                                Try(Parser.CIString("JAN")), Try(Parser.CIString("FEB")), Try(Parser.CIString("MAR")), Try(Parser.CIString("APR")),
                                Try(Parser.CIString("MAY")), Try(Parser.CIString("JUN")), Try(Parser.CIString("JUL")), Try(Parser.CIString("AUG")),
                                Try(Parser.CIString("SEP")), Try(Parser.CIString("OCT")), Try(Parser.CIString("NOV")), Try(Parser.CIString("DEC")))
                            select monthShortName.ToUpper();

            var cityPartParser = from gap1 in Parser.SkipWhitespaces
                           from city in Parser.Letter.Repeat(3)
                           select new string(city.ToArray()).ToUpper();

            var AvailabilityParser = from transactionCode in transactionCodeParser
                        from dayPart in dayPartParser
                        from monthPart in monthPartParser
                        from origin in cityPartParser
                        from destination in cityPartParser
                        select new { transactionCode, dayPart, monthPart, origin, destination };

            var ParserResult = AvailabilityParser.Parse(lineCopy);
            if (ParserResult.Success)
            {
                var flt = flights.ElementAtOrDefault(availabilityCounter);
                if (flt != null)
                {
                    var tc = ParserResult.Value.transactionCode;
                    var dt = string.IsNullOrEmpty(flt.TravelDate) ? ParserResult.Value.dayPart + ParserResult.Value.monthPart : flt.TravelDate;
                    var or = string.IsNullOrEmpty(flt.Origin) ? ParserResult.Value.origin : flt.Origin;
                    var ds = string.IsNullOrEmpty(flt.Destination) ? ParserResult.Value.destination : flt.Destination;
                    lineCopy = string.Concat(tc,  dt,  or,  ds);
                }
                availabilityCounter++;
                result = true;
            }

            return result;
        }

    }
}
