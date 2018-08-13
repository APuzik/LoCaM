using LoCaMEngine;
using LoCaMEngine.Actions;
using LoCaMEngine.Entities;
using LoCaMSimulator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace LoCaMSimulatorTest
{
    [TestClass]
    public class PickActionTest
    {
        [TestInitialize]
        public void TestInit()
        {
            manager = new CardManager();
            player1 = new Player();
            player2 = new Player();
        }

        [TestMethod]
        public void Pick0()
        {
            int expectedPick = 0;
            RunPickTest(expectedPick);
        }

        [TestMethod]
        public void Pick1()
        {
            int expectedPick = 1;
            RunPickTest(expectedPick);
        }

        [TestMethod]
        public void Pick2()
        {
            int expectedPick = 2;
            RunPickTest(expectedPick);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Pick_Invalid()
        {
            int expectedPick = -1;
            RunPickTest(expectedPick);
        }

        private void RunPickTest(int expectedPick)
        {
            List<Card> draft = manager.GetDraft();
            PickAction action = new PickAction(expectedPick, manager);

            bool result = action.Execute(player1, player2);
            Assert.IsTrue(result);
            Assert.AreEqual(player1.Deck.Count, 1);
            Assert.AreEqual(player2.Deck.Count, 0);
            CardManagerTest.AssertCards(draft[expectedPick], player1.Deck[0]);
        }


        CardManager manager;
        Player player1;
        Player player2;
        const int DEFAULT_MY_HEALTH = 29;
        const int DEFAULT_OPP_HEALTH = 30;
    }
}
