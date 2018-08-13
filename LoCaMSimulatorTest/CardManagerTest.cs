using System;
using System.Collections.Generic;
using LoCaMEngine;
using LoCaMEngine.Entities;
using LoCaMSimulator;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LoCaMSimulatorTest
{
    [TestClass]
    public class CardManagerTest
    {
        [TestMethod]
        public void InitCardManager()
        {
            CardManager manager = new CardManager();
            List<Card> draft = manager.GetDraft();
            Card card = manager.CreateCardFromDraft(0);
            CardManagerTest.AssertCards(draft[0], card);
        }

        public static void AssertCards(Card expected, Card actual)
        {
            Assert.AreEqual(expected.Abils, actual.Abils);
            Assert.AreEqual(expected.Attack, actual.Attack);
            Assert.AreEqual(expected.Cost, actual.Cost);
            Assert.AreEqual(expected.Defense, actual.Defense);
            Assert.AreEqual(expected.Draw, actual.Draw);
            Assert.AreEqual(expected.MyHealthChange, actual.MyHealthChange);
            Assert.AreEqual(expected.OppHealthChange, actual.OppHealthChange);
            Assert.AreEqual(expected.Number, actual.Number);
            Assert.AreEqual(expected.Type, actual.Type);
            Assert.IsTrue(actual.Id >= 0);
        }
    }
}
