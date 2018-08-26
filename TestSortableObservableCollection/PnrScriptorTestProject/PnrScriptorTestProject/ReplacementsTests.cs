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
        public void TestMethod1()
        {
            ObservableCollection<Flight> flightReplacements = new ObservableCollection<Flight>();
            flightReplacements.Add(new Flight() { Origin = "ABC", Destination = "XYZ", TravelDate = "31JAN" });
            int availabilityCounter = 0;
            string lineOfInput = "";

            bool replacementsMade = ReplacementsHelper.ReplaceAmaAvail(ref lineOfInput, ref availabilityCounter, flightReplacements);
            Assert.IsFalse(replacementsMade);
        }
    }
}
