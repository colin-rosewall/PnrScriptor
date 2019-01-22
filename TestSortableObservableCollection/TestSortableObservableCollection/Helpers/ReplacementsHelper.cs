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
        public static bool ReplaceGalBuySeats(ref string linecopy, int availabilityCounter,  IEnumerable<Models.Flight> flights)
        // beaware that availibilityCounter will already have been increased so we need to decrease it by 1 to refer to the correct row in flights 
        {
            bool result = false;

            var optionalDigitParser = from secondDigit in Parser.Digit.Optional()
                                      select (secondDigit.HasValue ? new string(new char[] { secondDigit.Value }) : string.Empty);

            var optionalSeatClassParser = from gap1 in Parser.SkipWhitespaces
                                          from seatClass in Parser.Letter.Optional()
                                          select (seatClass.HasValue ? new string(new char[] { seatClass.Value }) : string.Empty);

            var optionalLineReferenceParser = from gap1 in Parser.SkipWhitespaces
                                              from firstOptionalDigit in optionalDigitParser
                                              from secondOptionalDigit in optionalDigitParser
                                              select new { lineReference = String.Concat(firstOptionalDigit, secondOptionalDigit) };

            var sellCodeParser = from gap1 in Parser.SkipWhitespaces
                                 from sellCode in OneOf(Try(Parser.CIChar('n')), Try(Parser.Digit))
                                 select sellCode;

            var seatPartParser = from gap1 in Parser.SkipWhitespaces
                                 from firstDigit in Parser.Digit
                                 from secondOptionalDigit in optionalDigitParser
                                 select new string(new char[] { firstDigit }) + secondOptionalDigit;

            var seatClassParser = from gap1 in Parser.SkipWhitespaces
                                  from seatClass in Parser.Letter
                                  select new string(new char[] { seatClass });

            var lineReferenceParser = from gap1 in Parser.SkipWhitespaces
                                      from firstDigit in Parser.Digit
                                      from secondOptionalDigit in optionalDigitParser
                                      select new string(new char[] { firstDigit }) + secondOptionalDigit;

            var shortSellParser = from sellCode in sellCodeParser
                                  from seatPart in seatPartParser
                                  from seatClass in seatClassParser
                                  from lineRef in lineReferenceParser
                                  from secondSeatClass in optionalSeatClassParser
                                  from secondLineRef in optionalLineReferenceParser
                                  select new { firstBit = String.Concat(sellCode, seatPart), seatClass, lineRef, secondSeatClass, secondLineRef };

            var parserResult = shortSellParser.Parse(linecopy);
            if (parserResult.Success)
            {
                availabilityCounter -= 1;
                var flt = flights.ElementAtOrDefault(availabilityCounter);
                if (flt != null)
                {
                    var firstBit = parserResult.Value.firstBit;
                    var bc = string.IsNullOrEmpty(flt.BookingClass) ? parserResult.Value.seatClass : flt.BookingClass;
                    var lr = parserResult.Value.lineRef;
                    var lastBit = string.Empty;

                    if ((parserResult.Value.secondSeatClass.Length > 0) && (parserResult.Value.secondLineRef.lineReference.Length > 0 ))
                    {
                        if (string.IsNullOrEmpty(flt.BookingClass))
                        {
                            lastBit = String.Concat(parserResult.Value.secondSeatClass, parserResult.Value.secondLineRef.lineReference);
                        }
                        else
                        {
                            lastBit = String.Concat(flt.BookingClass, parserResult.Value.secondLineRef.lineReference);
                        }
                    }

                    linecopy = String.Concat(firstBit, bc, lr, lastBit);
                }
                result = true;
            }

            return result;
        }

        public static bool ReplaceAmaBuySeats(ref string linecopy, int availabilityCounter, IEnumerable<Models.Flight> flights)
        // beaware that availibilityCounter will already have been increased so we need to decrease it by 1 to refer to the correct row in flights 
        {
            bool result = false;

            var optionalDigitParser = from secondDigit in Parser.Digit.Optional()
                                      select (secondDigit.HasValue ? new string(new char[] { secondDigit.Value }) : string.Empty);

            var optionalSeatClassParser = from gap1 in Parser.SkipWhitespaces
                                          from seatClass in Parser.Letter.Optional()
                                          select (seatClass.HasValue ? new string(new char[] { seatClass.Value }) : string.Empty);

            var optionalLineReferenceParser = from gap1 in Parser.SkipWhitespaces
                                              from firstOptionalDigit in optionalDigitParser
                                              from secondOptionalDigit in optionalDigitParser
                                              select new { lineReference = String.Concat(firstOptionalDigit, secondOptionalDigit) };

            var sellCodeParser = from gap1 in Parser.SkipWhitespaces
                                 from sellCode in Parser.CIChar('s').Repeat(2)
                                 select new string(sellCode.ToArray()).ToUpper();

            var seatPartParser = from gap1 in Parser.SkipWhitespaces
                                 from firstDigit in Parser.Digit
                                 from secondOptionalDigit in optionalDigitParser
                                 select new string(new char[] { firstDigit }) + secondOptionalDigit;

            var seatClassParser = from gap1 in Parser.SkipWhitespaces
                                  from seatClass in Parser.Letter
                                  select new string(new char[] { seatClass });

            var lineReferenceParser = from gap1 in Parser.SkipWhitespaces
                                      from firstDigit in Parser.Digit
                                      from secondOptionalDigit in optionalDigitParser
                                      select new string(new char[] { firstDigit }) + secondOptionalDigit;

            var shortSellParser = from sellCode in sellCodeParser
                                  from seatPart in seatPartParser
                                  from seatClass in seatClassParser
                                  from lineRef in lineReferenceParser
                                  from secondSeatClass in optionalSeatClassParser
                                  from secondLineRef in optionalLineReferenceParser
                                  select new { firstBit = String.Concat(sellCode, seatPart), seatClass, lineRef, secondSeatClass, secondLineRef };

            var parserResult = shortSellParser.Parse(linecopy);
            if (parserResult.Success)
            {
                availabilityCounter -= 1;
                var flt = flights.ElementAtOrDefault(availabilityCounter);
                if (flt != null)
                {
                    var firstBit = parserResult.Value.firstBit;
                    var bc = string.IsNullOrEmpty(flt.BookingClass) ? parserResult.Value.seatClass : flt.BookingClass;
                    var lr = parserResult.Value.lineRef;
                    var lastBit = string.Empty;

                    if ((parserResult.Value.secondSeatClass.Length > 0) && (parserResult.Value.secondLineRef.lineReference.Length > 0))
                    {
                        if (string.IsNullOrEmpty(flt.BookingClass))
                        {
                            lastBit = String.Concat(parserResult.Value.secondSeatClass, parserResult.Value.secondLineRef.lineReference);
                        }
                        else
                        {
                            lastBit = String.Concat(flt.BookingClass, parserResult.Value.secondLineRef.lineReference);
                        }
                    }

                    linecopy = String.Concat(firstBit, bc, lr, lastBit);
                }
                result = true;
            }

            return result;
        }

        public static bool ReplaceSabreBuySeats(ref string linecopy, int availabilityCounter, IEnumerable<Models.Flight> flights)
        // beaware that availibilityCounter will already have been increased so we need to decrease it by 1 to refer to the correct row in flights 
        {
            bool result = false;

            var optionalDigitParser = from secondDigit in Parser.Digit.Optional()
                                      select (secondDigit.HasValue ? new string(new char[] { secondDigit.Value }) : string.Empty);

            var optionalSeatClassParser = from gap1 in Parser.SkipWhitespaces
                                          from seatClass in Parser.Letter.Optional()
                                          select (seatClass.HasValue ? new string(new char[] { seatClass.Value }) : string.Empty);

            var optionalLineReferenceParser = from gap1 in Parser.SkipWhitespaces
                                              from firstOptionalDigit in optionalDigitParser
                                              from secondOptionalDigit in optionalDigitParser
                                              select new { lineReference = String.Concat(firstOptionalDigit, secondOptionalDigit) };

            var sellCodeParser = from gap1 in Parser.SkipWhitespaces
                                 from sellCode in OneOf(Try(Parser.CIChar('n')), Try(Parser.Digit))
                                 select sellCode;

            var seatPartParser = from gap1 in Parser.SkipWhitespaces
                                 from firstDigit in Parser.Digit
                                 from secondOptionalDigit in optionalDigitParser
                                 select new string(new char[] { firstDigit }) + secondOptionalDigit;

            var seatClassParser = from gap1 in Parser.SkipWhitespaces
                                  from seatClass in Parser.Letter
                                  select new string(new char[] { seatClass });

            var lineReferenceParser = from gap1 in Parser.SkipWhitespaces
                                      from firstDigit in Parser.Digit
                                      from secondOptionalDigit in optionalDigitParser
                                      select new string(new char[] { firstDigit }) + secondOptionalDigit;

            var shortSellParser = from sellCode in sellCodeParser
                                  from seatPart in seatPartParser
                                  from seatClass in seatClassParser
                                  from lineRef in lineReferenceParser
                                  from secondSeatClass in optionalSeatClassParser
                                  from secondLineRef in optionalLineReferenceParser
                                  select new { firstBit = String.Concat(sellCode, seatPart), seatClass, lineRef, secondSeatClass, secondLineRef };

            var parserResult = shortSellParser.Parse(linecopy);
            if (parserResult.Success)
            {
                availabilityCounter -= 1;
                var flt = flights.ElementAtOrDefault(availabilityCounter);
                if (flt != null)
                {
                    var firstBit = parserResult.Value.firstBit;
                    var bc = string.IsNullOrEmpty(flt.BookingClass) ? parserResult.Value.seatClass : flt.BookingClass;
                    var lr = parserResult.Value.lineRef;
                    var lastBit = string.Empty;

                    if ((parserResult.Value.secondSeatClass.Length > 0) && (parserResult.Value.secondLineRef.lineReference.Length > 0))
                    {
                        if (string.IsNullOrEmpty(flt.BookingClass))
                        {
                            lastBit = String.Concat(parserResult.Value.secondSeatClass, parserResult.Value.secondLineRef.lineReference);
                        }
                        else
                        {
                            lastBit = String.Concat(flt.BookingClass, parserResult.Value.secondLineRef.lineReference);
                        }
                    }

                    linecopy = String.Concat(firstBit, bc, lr, lastBit);
                }
                result = true;
            }

            return result;
        }

        public static bool ReplaceGalAvail(ref string lineCopy, ref int availabilityCounter, IEnumerable<Models.Flight> flights)
        {
            bool result = false;

            var transactionCodeParser = from gap1 in Parser.SkipWhitespaces
                                        from firstTransactionCode in Parser.CIChar('A')
                                        select new string(new char[] { firstTransactionCode }).ToUpper();

            var optionalSecondTransactionCodeParser = from gap1 in Parser.SkipWhitespaces
                                                      from secondTransactionCode in OneOf(Try(Parser.CIChar('d')), Try(Parser.CIChar('j')), Try(Parser.CIChar('a')), Try(Parser.CIChar('f')), Try(Parser.CIChar('p')), Try(Parser.CIChar('q')), Try(Parser.CIChar('u'))).Optional()
                                                      select (secondTransactionCode.HasValue ? new string(new char[] { secondTransactionCode.Value }).ToUpper() : string.Empty);

            var optionalDayDigitParser = from secondDigit in Parser.Digit.Optional()
                                         select (secondDigit.HasValue ? new string(new char[] { secondDigit.Value }) : string.Empty);

            var optionalAirlineQualifierParser = from gap1 in Parser.SkipWhitespaces
                                                 from firstQualifier in Parser.Char('/')
                                                 from gap2 in SkipWhitespaces
                                                 from airline in Parser.Letter.Repeat(2)
                                                 select new
                                                 {
                                                     airlineQualifier = String.Concat(firstQualifier).ToUpper(),
                                                     airlineCode = new string(airline.ToArray()).ToUpper()
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
                                     from secondTransactionCode in optionalSecondTransactionCodeParser
                                     from dayPart in dayPartParser
                                     from monthPart in monthPartParser
                                     from origin in cityPartParser
                                     from destination in cityPartParser
                                     from airlinePart in optionalAirlineQualifierParser.Optional()
                                     select new { transactionCode = String.Concat(transactionCode, secondTransactionCode), dayPart, monthPart, origin, destination, airlinePart };

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

                    lineCopy = string.Concat(tc, dt, or, ds, air);
                }
                availabilityCounter++;
                result = true;
            }

            return result;
        }

        public static bool ReplaceSabreAvail(ref string lineCopy, ref int availabilityCounter,
            IEnumerable<Models.Flight> flights)
        {
            bool result = false;

            var transactionCodeParser = from gap1 in Parser.SkipWhitespaces
                from firstTransactionCode in Parser.Char('1')
                select new string(new char[] { firstTransactionCode }).ToUpper();

            var optionalDayDigitParser = from secondDigit in Parser.Digit.Optional()
                select (secondDigit.HasValue ? new string(new char[] { secondDigit.Value }) : string.Empty);

            var optionalAirlineQualifierParser = from gap1 in Parser.SkipWhitespaces
                                                 from firstQualifier in Parser.Char('¥')
                                                 from gap2 in SkipWhitespaces
                                                 from airline in Parser.Letter.Repeat(2)
                                                 select new
                                                 {
                                                     airlineQualifier = String.Concat(firstQualifier).ToUpper(),
                                                     airlineCode = new string(airline.ToArray()).ToUpper()
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

                    lineCopy = string.Concat(tc, dt, or, ds, air);
                }
                availabilityCounter++;
                result = true;
            }

            return result;
        }

        public static bool ReplaceAmaAvail(ref string lineCopy, ref int availabilityCounter,
            IEnumerable<Models.Flight> flights)
        {
            bool result = false;

            var transactionCodeParser = from gap1 in Parser.SkipWhitespaces
                from firstTransactionCode in Parser.CIChar('a')
                from secondTransactionCode in OneOf(Try(Parser.CIChar('n')), Try(Parser.CIChar('d')), Try(Parser.CIChar('a')))
                select new string(new char[] {firstTransactionCode, secondTransactionCode}).ToUpper();

            var optionalDayDigitParser = from secondDigit in Parser.Digit.Optional()
                select (secondDigit.HasValue ? new string(new char[] {secondDigit.Value}) : string.Empty);

            var optionalAirlineQualifierParser = from gap1 in Parser.SkipWhitespaces
                from firstQualifier in Parser.Char('/')
                from secondQualifier in Parser.CIChar('A')
                from gap2 in SkipWhitespaces
                from airline in Parser.Letter.Repeat(2)
                select new
                {
                    airlineQualifier = String.Concat(firstQualifier, secondQualifier).ToUpper(),
                    airlineCode = new string(airline.ToArray()).ToUpper()
                };

            var optionalBookingClassQualifierParser = from gap1 in Parser.SkipWhitespaces
                                                      from firstQualifier in Parser.Char('/')
                                                      from secondQualifier in Parser.CIChar('C')
                                                      from gap2 in Parser.SkipWhitespaces
                                                      from bookClass in Parser.Letter
                                                      select new
                                                      {
                                                          bookClassQualifier = String.Concat(firstQualifier, secondQualifier).ToUpper(),
                                                          bookingClass = new string(new char[] { bookClass }).ToUpper()
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
                        from bookingClassPart in optionalBookingClassQualifierParser.Optional()
                        select new { transactionCode, dayPart, monthPart, origin, destination, airlinePart, bookingClassPart };

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

                    string bookClass = string.Empty;
                    if (parserResult.Value.bookingClassPart.HasValue)
                    {
                        bookClass = string.IsNullOrEmpty(flt.BookingClass)
                            ? String.Concat(parserResult.Value.bookingClassPart.Value.bookClassQualifier, parserResult.Value.bookingClassPart.Value.bookingClass)
                            : String.Concat(parserResult.Value.bookingClassPart.Value.bookClassQualifier, flt.BookingClass);
                    }
                   
                    lineCopy = string.Concat(tc,  dt,  or,  ds, air, bookClass);
                }
                availabilityCounter++;
                result = true;
            }

            return result;
        }

        public static void ReplaceMaskDates(ref string lineCopy)
        {
            int foundPos = -1;
            string[] maskDateKeywords = { "MASK/DATE**", "RETAIL/DATE**" };
            string todaysDate = DateTime.Today.ToString("ddMMMyy").ToUpper();

            if (!string.IsNullOrEmpty(lineCopy))
            {
                foreach (string term in maskDateKeywords)
                {
                    foundPos = lineCopy.IndexOf(term, 0, StringComparison.InvariantCultureIgnoreCase);
                    if (foundPos >= 0)
                    {
                        int startPos = foundPos + term.Length;
                        if (lineCopy.Length > startPos)
                        {
                            if (lineCopy[startPos] == ';')
                                startPos += 1;
                            if (lineCopy.Length >= (startPos + todaysDate.Length))
                            {
                                // replace whatever is in there with todays date
                                lineCopy = lineCopy.Substring(0, startPos) + todaysDate +
                                           lineCopy.Substring(startPos + todaysDate.Length);
                            }
                            else
                            {
                                // insert todays date
                                lineCopy = lineCopy.Insert(startPos, todaysDate + ' ');
                            }
                        }
                    }
                }
            }
        }
    }
}
