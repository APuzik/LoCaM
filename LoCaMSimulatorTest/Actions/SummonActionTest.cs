using System;
using LoCaMEngine;
using LoCaMEngine.Actions;
using LoCaMEngine.Entities;
using LoCaMSimulator;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LoCaMSimulatorTest
{
    [TestClass]
    public class SummonActionTest
    {
        [TestInitialize]
        public void TestInit()
        {
            manager = new CardManager();
            player1 = new Player();
            player1.Data.Health = DEFAULT_MY_HEALTH;

            player2 = new Player();
            player2.Data.Health = DEFAULT_OPP_HEALTH;
        }

        [TestMethod]
        public void Summon_Creature()
        {
            int cardSummon = 64;
            SetupHand(cardSummon);

            int id = 0;
            RunSummonCreatureTest(id);
        }

        [TestMethod]
        public void Summon_Creature_Charge()
        {
            int cardSummon = 82;
            SetupHand(cardSummon);

            int id = 0;
            RunSummonCreatureTest(id);
        }

        [TestMethod]
        public void Summon_Creature_Affect_Players_Health()
        {
            int cardSummon = 12;
            SetupHand(cardSummon);

            int id = 0;
            RunSummonCreatureTest(id);
        }

        [TestMethod]
        public void Summon_Creature_Affect_Player_Draw()
        {
            int cardSummon = 28;
            SetupHand(cardSummon);

            int id = 0;
            RunSummonCreatureTest(id);
        }

        [TestMethod]
        public void Summon_Creature_InvalidId()
        {
            int id = 0;
            RunInvalidSummonCreatureTest(id);
        }

        [TestMethod]
        public void Summon_Creature_NotEnoughMana()
        {
            int cardSummon = 64;
            SetupHand(cardSummon);

            int id = 0;
            RunInvalidSummonCreatureTest(id);
        }

        [TestMethod]
        public void Summon_Creature_MaxCreatures_On_Table()
        {
            int cardSummon = 64;
            SetupHand(cardSummon);
            for(int i = 0; i < MAX_ON_TABLE; i++)
            {
                AddToTable(player1, i);
            }

            player1.Mana = player1.Hand[0].Cost;

            int id = 0;
            RunInvalidSummonCreatureTest(id);
        }

        private void SetupHand(int cardForSummon)
        {
            Card card = manager.CreateCardByNumber(cardForSummon);
            player1.Hand[card.Id] = card;
        }

        private void AddToTable(Player player, int cardNumber)
        {
            Card card = manager.CreateCardByNumber(cardNumber);
            var creature = new CombatCreature(card);
            player.Table.Add(card.Id, creature);
        }

        private void RunSummonCreatureTest(int id)
        {
            Card card = player1.Hand[id];
            player1.Mana = card.Cost;

            int expectedMyHealth = player1.Data.Health + card.MyHealthChange;
            int expectedOppHealth = player2.Data.Health + card.OppHealthChange;
            int expectedNextDraw = player1.NextDrawSize + card.Draw;
            int expectedPlayerTable = Math.Min(player1.Table.Count + 1, MAX_ON_TABLE);
            int expectedPlayerHand = player1.Hand.Count - 1;

            SummonAction action = new SummonAction(id);
            bool result = action.Execute(player1, player2);

            Assert.IsTrue(result);
            Assert.AreEqual(expectedMyHealth, player1.Data.Health);
            Assert.AreEqual(expectedOppHealth, player2.Data.Health);
            Assert.AreEqual(expectedNextDraw, player1.NextDrawSize);
            Assert.AreEqual(expectedPlayerTable, player1.Table.Count);
            Assert.AreEqual(expectedPlayerHand, player1.Hand.Count);
            Assert.AreEqual(card.IsCharge, player1.Table[id].CanAttack);
        }

        private void RunInvalidSummonCreatureTest(int id)
        {
            int expectedMyHealth = player1.Data.Health;
            int expectedOppHealth = player2.Data.Health;
            int expectedNextDraw = player1.NextDrawSize;
            int expectedPlayerTable = player1.Table.Count;
            int expectedPlayerHand = player1.Hand.Count;

            SummonAction action = new SummonAction(id);
            bool result = action.Execute(player1, player2);

            Assert.IsTrue(result);
            Assert.AreEqual(expectedMyHealth, player1.Data.Health);
            Assert.AreEqual(expectedOppHealth, player2.Data.Health);
            Assert.AreEqual(expectedNextDraw, player1.NextDrawSize);
            Assert.AreEqual(expectedPlayerHand, player1.Hand.Count);
            Assert.AreEqual(expectedPlayerTable, player1.Table.Count);
        }

        CardManager manager;
        Player player1;
        Player player2;
        const int DEFAULT_MY_HEALTH = 29;
        const int DEFAULT_OPP_HEALTH = 30;
        const int MAX_ON_TABLE = 6;
    }
}
