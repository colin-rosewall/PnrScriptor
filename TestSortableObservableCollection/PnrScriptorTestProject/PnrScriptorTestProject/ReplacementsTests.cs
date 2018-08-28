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
            flightReplacements.Add(new Flight() { Origin = "ABC", Destination = "XYZ", TravelDate = "31JAN", AirlineCode = "QF" });
            int availabilityCounter = 0;
            string lineOfInput = "AN 03JUN SYD SIN /ASQ";

            bool replacementsMade = ReplacementsHelper.ReplaceAmaAvail(ref lineOfInput, ref availabilityCounter, flightReplacements);

            Assert.AreEqual("AN31JANABCXYZ/AQF", lineOfInput);
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
    }
}
