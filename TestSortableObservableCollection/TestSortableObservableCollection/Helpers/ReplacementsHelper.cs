using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pidgin;
using static Pidgin.Parser;
using String = System.String;

namespace TestSortableObservableCollection.Helpers
{
    public static class ReplacementsHelper
    {
        public static bool ReplaceAmaAvail(ref string lineCopy, ref int availabilityCounter,
            IEnumerable<Models.Flight> flights)
        {
            bool result = false;

            var transactionCodeParser = from gap1 in Parser.SkipWhitespaces
                from firstTransactionCode in Parser.CIChar('a')
                from secondTransactionCode in OneOf(Try(Parser.CIChar('n')), Try(Parser.CIChar('d')),
                    Try(Parser.CIChar('a')))
                select new string(new char[] {firstTransactionCode, secondTransactionCode}).ToUpper();

            var optionalDayDigitParser = from secondDigit in Parser.Digit.Optional()
                select (secondDigit.HasValue ? new string(new char[] {secondDigit.Value}) : string.Empty);

            var optionalAirlineQualifierParser = from gap1 in Parser.SkipWhitespaces
                from firstQualifier in Parser.Char('/')
                from secondQualifier in Parser.Char('A')
                from gap2 in SkipWhitespaces
                from airline in Parser.Letter.Repeat(2)
                select new
                {
                    airlineQualifier = String.Concat(firstQualifier, secondQualifier),
                    airlineCode = new string(airline.ToArray())
                };


            var dayPartParser = from gap1 in Parser.SkipWhitespaces
                          from firstDigit in Parser.Digit
                          from secondOptionalDigit in optionalDayDigitParser
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

            var availabilityParser = from transactionCode in transactionCodeParser
                        from dayPart in dayPartParser
                        from monthPart in monthPartParser
                        from origin in cityPartParser
                        from destination in cityPartParser
                        from airlinePart in optionalAirlineQualifierParser.Optional()
                        select new { transactionCode, dayPart, monthPart, origin, destination, airlinePart };

            var parserResult = availabilityParser.Parse(lineCopy);
            if (parserResult.Success)
            {
                var flt = flights.ElementAtOrDefault(availabilityCounter);
                if (flt != null)
                {
                    var tc = parserResult.Value.transactionCode;
                    var dt = string.IsNullOrEmpty(flt.TravelDate) ? parserResult.Value.dayPart + parserResult.Value.monthPart : flt.TravelDate;
                    var or = string.IsNullOrEmpty(flt.Origin) ? parserResult.Value.origin : flt.Origin;
                    var ds = string.IsNullOrEmpty(flt.Destination) ? parserResult.Value.destination : flt.Destination;
                    string air = string.Empty;

                    if (parserResult.Value.airlinePart.HasValue)
                    {
                        air = string.IsNullOrEmpty(flt.AirlineCode)
                            ? String.Concat(parserResult.Value.airlinePart.Value.airlineQualifier, parserResult.Value.airlinePart.Value.airlineCode)
                            : String.Concat(parserResult.Value.airlinePart.Value.airlineQualifier, flt.AirlineCode);
                    }
                   
                    lineCopy = string.Concat(tc,  dt,  or,  ds, air);
                }
                availabilityCounter++;
                result = true;
            }

            return result;
        }

    }
}
