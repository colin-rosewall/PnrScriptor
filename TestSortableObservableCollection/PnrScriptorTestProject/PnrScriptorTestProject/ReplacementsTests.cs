using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestSortableObservableCollection.Models;
using TestSortableObservableCollection.Helpers;
using System.Collections.ObjectModel;

namespace PnrScriptorTestProject
{
    [TestClass]
    public class ReplacementsTests
    {
        [TestMethod]
        public void TestAmaMinimumAvailWithSpaces()
        {
            ObservableCollection<Flight> flightReplacements = new ObservableCollection<Flight>();
            flightReplacements.Add(new Flight() { Origin = "ABC", Destination = "XYZ", TravelDate = "31JAN" });
            int availabilityCounter = 0;
            string lineOfInput = "AN 1JUN SYD SIN";

            bool replacementsMade = ReplacementsHelper.ReplaceAmaAvail(ref lineOfInput, ref availabilityCounter, flightReplacements);

            Assert.AreEqual("AN31JANABCXYZ",lineOfInput);
        }

        [TestMethod]
        public void TestAmaMinimumAvailWithNoSpaces()
        {
            ObservableCollection<Flight> flightReplacements = new ObservableCollection<Flight>();
            flightReplacements.Add(new Flight() { Origin = "ABC", Destination = "XYZ", TravelDate = "31JAN" });
            int availabilityCounter = 0;
            string lineOfInput = "AN1JUNSYDSIN";

            bool replacementsMade = ReplacementsHelper.ReplaceAmaAvail(ref lineOfInput, ref availabilityCounter, flightReplacements);

            Assert.AreEqual("AN31JANABCXYZ", lineOfInput);
        }

        [TestMethod]
        public void TestAmaDayWith2DigitsAvailWithSpaces()
        {
            ObservableCollection<Flight> flightReplacements = new ObservableCollection<Flight>();
            flightReplacements.Add(new Flight() { Origin = "ABC", Destination = "XYZ", TravelDate = "31JAN" });
            int availabilityCounter = 0;
            string lineOfInput = "AN 03JUN SYD SIN";

            bool replacementsMade = ReplacementsHelper.ReplaceAmaAvail(ref lineOfInput, ref availabilityCounter, flightReplacements);

            Assert.AreEqual("AN31JANABCXYZ", lineOfInput);
        }

        [TestMethod]
        public void TestAmaDayWith1DigitAvailWithSpaces()
        {
            ObservableCollection<Flight> flightReplacements = new ObservableCollection<Flight>();
            flightReplacements.Add(new Flight() { Origin = "ABC", Destination = "XYZ", TravelDate = "31JAN" });
            int availabilityCounter = 0;
            string lineOfInput = "AN 4 JUN SYD SIN";

            bool replacementsMade = ReplacementsHelper.ReplaceAmaAvail(ref lineOfInput, ref availabilityCounter, flightReplacements);

            Assert.AreEqual("AN31JANABCXYZ", lineOfInput);
        }

        [TestMethod]
        public void TestAmaWithAirlineCodeWithSpaces()
        {
            ObservableCollection<Flight> flightReplacements = new ObservableCollection<Flight>();
            flightReplacements.Add(new Flight() { Origin = "ABC", Destination = "XYZ", TravelDate = "31JAN", AirlineCode = "EK"});
            int availabilityCounter = 0;
            string lineOfInput = "AN 03JUN SYD SIN /A SQ";

            bool replacementsMade = ReplacementsHelper.ReplaceAmaAvail(ref lineOfInput, ref availabilityCounter, flightReplacements);

            Assert.AreEqual("AN31JANABCXYZ/AEK", lineOfInput);
        }

        [TestMethod]
        public void TestAmaWithAirlineCodeWithNoSpaces()
        {
            ObservableCollection<Flight> flightReplacements = new ObservableCollection<Flight>();
            flightReplacements.Add(new Flight() { Origin = "ABC", Destination = "XYZ", TravelDate = "31JAN", AirlineCode = "VA" });
            int availabilityCounter = 0;
            string lineOfInput = "AN07DECBNESYD/AQF";

            bool replacementsMade = ReplacementsHelper.ReplaceAmaAvail(ref lineOfInput, ref availabilityCounter, flightReplacements);

            Assert.AreEqual("AN31JANABCXYZ/AVA", lineOfInput);
        }

        [TestMethod]
        public void TestAmaInvalidAirlineQualifier()
        {
            ObservableCollection<Flight> flightReplacements = new ObservableCollection<Flight>();
            flightReplacements.Add(new Flight() { Origin = "ABC", Destination = "XYZ", TravelDate = "31JAN", AirlineCode = "QF" });
            int availabilityCounter = 0;
            string lineOfInput = "AN 03JUN SYD SIN /SQ";

            bool replacementsMade = ReplacementsHelper.ReplaceAmaAvail(ref lineOfInput, ref availabilityCounter, flightReplacements);

            Assert.IsFalse(replacementsMade);
        }

        [TestMethod]
        public void TestAmaWithBookingClassAndAirlineCodeWithNoSpaces()
        {
            ObservableCollection<Flight> flightReplacements = new ObservableCollection<Flight>();
            flightReplacements.Add(new Flight() { Origin = "ABC", Destination = "XYZ", TravelDate = "31JAN", AirlineCode = "VA", BookingClass = "Y"});
            int availabilityCounter = 0;
            string lineOfInput = "AN23OCTJNBDXB/AEK/CU";

            bool replacementsMade = ReplacementsHelper.ReplaceAmaAvail(ref lineOfInput, ref availabilityCounter, flightReplacements);

            Assert.AreEqual("AN31JANABCXYZ/AVA/CY", lineOfInput);
        }
// #####################################################################

        [TestMethod]
        public void TestSabreMinimumAvailWithSpaces()
        {
            ObservableCollection<Flight> flightReplacements = new ObservableCollection<Flight>();
            flightReplacements.Add(new Flight() { Origin = "ABC", Destination = "XYZ", TravelDate = "31JAN" });
            int availabilityCounter = 0;
            string lineOfInput = "1 1JUN SYD SIN";

            bool replacementsMade = ReplacementsHelper.ReplaceSabreAvail(ref lineOfInput, ref availabilityCounter, flightReplacements);

            Assert.AreEqual("131JANABCXYZ", lineOfInput);
        }

        [TestMethod]
        public void TestSabreMinimumAvailWithNoSpaces()
        {
            ObservableCollection<Flight> flightReplacements = new ObservableCollection<Flight>();
            flightReplacements.Add(new Flight() { Origin = "ABC", Destination = "XYZ", TravelDate = "31JAN" });
            int availabilityCounter = 0;
            string lineOfInput = "11JUNSYDSIN";

            bool replacementsMade = ReplacementsHelper.ReplaceSabreAvail(ref lineOfInput, ref availabilityCounter, flightReplacements);

            Assert.AreEqual("131JANABCXYZ", lineOfInput);
        }

        [TestMethod]
        public void TestSabreDayWith2DigitsAvailWithSpaces()
        {
            ObservableCollection<Flight> flightReplacements = new ObservableCollection<Flight>();
            flightReplacements.Add(new Flight() { Origin = "ABC", Destination = "XYZ", TravelDate = "31JAN" });
            int availabilityCounter = 0;
            string lineOfInput = "1 03JUN SYD SIN";

            bool replacementsMade = ReplacementsHelper.ReplaceSabreAvail(ref lineOfInput, ref availabilityCounter, flightReplacements);

            Assert.AreEqual("131JANABCXYZ", lineOfInput);
        }

        [TestMethod]
        public void TestSabreDayWith1DigitAvailWithSpaces()
        {
            ObservableCollection<Flight> flightReplacements = new ObservableCollection<Flight>();
            flightReplacements.Add(new Flight() { Origin = "ABC", Destination = "XYZ", TravelDate = "31JAN" });
            int availabilityCounter = 0;
            string lineOfInput = "1 4 JUN SYD SIN";

            bool replacementsMade = ReplacementsHelper.ReplaceSabreAvail(ref lineOfInput, ref availabilityCounter, flightReplacements);

            Assert.AreEqual("131JANABCXYZ", lineOfInput);
        }

        [TestMethod]
        public void TestSabreWithAirlineCodeWithSpaces()
        {
            ObservableCollection<Flight> flightReplacements = new ObservableCollection<Flight>();
            flightReplacements.Add(new Flight() { Origin = "ABC", Destination = "XYZ", TravelDate = "31JAN", AirlineCode = "EK" });
            int availabilityCounter = 0;
            string lineOfInput = "1 03JUN SYD SIN ¥ SQ";

            bool replacementsMade = ReplacementsHelper.ReplaceSabreAvail(ref lineOfInput, ref availabilityCounter, flightReplacements);

            Assert.AreEqual("131JANABCXYZ¥EK", lineOfInput);
        }

        [TestMethod]
        public void TestSabreWithAirlineCodeWithNoSpaces()
        {
            ObservableCollection<Flight> flightReplacements = new ObservableCollection<Flight>();
            flightReplacements.Add(new Flight() { Origin = "ABC", Destination = "XYZ", TravelDate = "31JAN", AirlineCode = "VA" });
            int availabilityCounter = 0;
            string lineOfInput = "107DECBNESYD¥QF";

            bool replacementsMade = ReplacementsHelper.ReplaceSabreAvail(ref lineOfInput, ref availabilityCounter, flightReplacements);

            Assert.AreEqual("131JANABCXYZ¥VA", lineOfInput);
        }

        [TestMethod]
        public void TestSabreInvalidAirlineQualifier()
        {
            ObservableCollection<Flight> flightReplacements = new ObservableCollection<Flight>();
            flightReplacements.Add(new Flight() { Origin = "ABC", Destination = "XYZ", TravelDate = "31JAN", AirlineCode = "QF" });
            int availabilityCounter = 0;
            string lineOfInput = "1 03JUN SYD SIN SQ";

            bool replacementsMade = ReplacementsHelper.ReplaceSabreAvail(ref lineOfInput, ref availabilityCounter, flightReplacements);

            Assert.IsFalse(replacementsMade);
        }

        [TestMethod]
        public void TestSabreWithBookingClassAndAirlineCodeWithNoSpaces()
        {
            ObservableCollection<Flight> flightReplacements = new ObservableCollection<Flight>();
            flightReplacements.Add(new Flight() { Origin = "ABC", Destination = "XYZ", TravelDate = "31JAN", AirlineCode = "VA", BookingClass = "Y" });
            int availabilityCounter = 0;
            string lineOfInput = "123OCTJNBDXB¥EK-U";

            bool replacementsMade = ReplacementsHelper.ReplaceSabreAvail(ref lineOfInput, ref availabilityCounter, flightReplacements);

            Assert.AreEqual("131JANABCXYZ¥VA-Y", lineOfInput);
        }

        // #####################################################################

        [TestMethod]
        public void TestGalMinimumAvailWithSpaces()
        {
            ObservableCollection<Flight> flightReplacements = new ObservableCollection<Flight>();
            flightReplacements.Add(new Flight() { Origin = "ABC", Destination = "XYZ", TravelDate = "31JAN" });
            int availabilityCounter = 0;
            string lineOfInput = "A 1JUN SYD SIN";

            bool replacementsMade = ReplacementsHelper.ReplaceGalAvail(ref lineOfInput, ref availabilityCounter, flightReplacements);

            Assert.AreEqual("A31JANABCXYZ", lineOfInput);
        }

        [TestMethod]
        public void TestGalMinimumAvailWithNoSpaces()
        {
            ObservableCollection<Flight> flightReplacements = new ObservableCollection<Flight>();
            flightReplacements.Add(new Flight() { Origin = "ABC", Destination = "XYZ", TravelDate = "31JAN" });
            int availabilityCounter = 0;
            string lineOfInput = "A1JUNSYDSIN";

            bool replacementsMade = ReplacementsHelper.ReplaceGalAvail(ref lineOfInput, ref availabilityCounter, flightReplacements);

            Assert.AreEqual("A31JANABCXYZ", lineOfInput);
        }

        [TestMethod]
        public void TestGalDayWith2DigitsAvailWithSpaces()
        {
            ObservableCollection<Flight> flightReplacements = new ObservableCollection<Flight>();
            flightReplacements.Add(new Flight() { Origin = "ABC", Destination = "XYZ", TravelDate = "31JAN" });
            int availabilityCounter = 0;
            string lineOfInput = "A 03JUN SYD SIN";

            bool replacementsMade = ReplacementsHelper.ReplaceGalAvail(ref lineOfInput, ref availabilityCounter, flightReplacements);

            Assert.AreEqual("A31JANABCXYZ", lineOfInput);
        }

        [TestMethod]
        public void TestGalDayWith1DigitAvailWithSpaces()
        {
            ObservableCollection<Flight> flightReplacements = new ObservableCollection<Flight>();
            flightReplacements.Add(new Flight() { Origin = "ABC", Destination = "XYZ", TravelDate = "31JAN" });
            int availabilityCounter = 0;
            string lineOfInput = "A 4 JUN SYD SIN";

            bool replacementsMade = ReplacementsHelper.ReplaceGalAvail(ref lineOfInput, ref availabilityCounter, flightReplacements);

            Assert.AreEqual("A31JANABCXYZ", lineOfInput);
        }

        [TestMethod]
        public void TestGalWithAirlineCodeWithSpaces()
        {
            ObservableCollection<Flight> flightReplacements = new ObservableCollection<Flight>();
            flightReplacements.Add(new Flight() { Origin = "ABC", Destination = "XYZ", TravelDate = "31JAN", AirlineCode = "EK" });
            int availabilityCounter = 0;
            string lineOfInput = "A 03JUN SYD SIN / SQ";

            bool replacementsMade = ReplacementsHelper.ReplaceGalAvail(ref lineOfInput, ref availabilityCounter, flightReplacements);

            Assert.AreEqual("A31JANABCXYZ/EK", lineOfInput);
        }

        [TestMethod]
        public void TestGalWithAirlineCodeWithNoSpaces()
        {
            ObservableCollection<Flight> flightReplacements = new ObservableCollection<Flight>();
            flightReplacements.Add(new Flight() { Origin = "ABC", Destination = "XYZ", TravelDate = "31JAN", AirlineCode = "VA" });
            int availabilityCounter = 0;
            string lineOfInput = "A07DECBNESYD/QF";

            bool replacementsMade = ReplacementsHelper.ReplaceGalAvail(ref lineOfInput, ref availabilityCounter, flightReplacements);

            Assert.AreEqual("A31JANABCXYZ/VA", lineOfInput);
        }

        [TestMethod]
        public void TestGalInvalidAirlineQualifier()
        {
            ObservableCollection<Flight> flightReplacements = new ObservableCollection<Flight>();
            flightReplacements.Add(new Flight() { Origin = "ABC", Destination = "XYZ", TravelDate = "31JAN", AirlineCode = "QF" });
            int availabilityCounter = 0;
            string lineOfInput = "A 03JUN SYD SIN SQ";

            bool replacementsMade = ReplacementsHelper.ReplaceGalAvail(ref lineOfInput, ref availabilityCounter, flightReplacements);

            Assert.IsFalse(replacementsMade);
        }

        [TestMethod]
        public void TestGalWithBookingClassAndAirlineCodeWithNoSpaces()
        {
            ObservableCollection<Flight> flightReplacements = new ObservableCollection<Flight>();
            flightReplacements.Add(new Flight() { Origin = "ABC", Destination = "XYZ", TravelDate = "31JAN", AirlineCode = "VA", BookingClass = "Y" });
            int availabilityCounter = 0;
            string lineOfInput = "A23OCTJNBDXB/EK@U#";

            bool replacementsMade = ReplacementsHelper.ReplaceGalAvail(ref lineOfInput, ref availabilityCounter, flightReplacements);

            Assert.AreEqual("A31JANABCXYZ/VA@Y#", lineOfInput);
        }
    }
}
