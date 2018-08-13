using LoCaMEngine;
using LoCaMEngine.Actions;
using LoCaMEngine.Entities;
using LoCaMSimulator;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LoCaMSimulatorTest.Actions
{
    [TestClass]
    public class UseActionTest
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
        public void Use_GreenItem()
        {
            int cardForUse = 123;
            SetupHand(cardForUse);

            int sourceId = 0;
            int targetId = 1;

            AddToTable(player1, 0);

            RunUseActionCreatureTest(sourceId, targetId);
        }

        [TestMethod]
        public void Use_GreenItem_Abils()
        {
            int cardForUse = 138;
            SetupHand(cardForUse);

            int sourceId = 0;
            int targetId = 1;

            AddToTable(player1, 0);

            RunUseActionCreatureTest(sourceId, targetId);
        }

        [TestMethod]
        public void Use_GreenItem_Players_Effects()
        {
            int cardForUse = 129;
            SetupHand(cardForUse);

            int sourceId = 0;
            int targetId = 1;

            AddToTable(player1, 0);

            RunUseActionCreatureTest(sourceId, targetId);
        }

        [TestMethod]
        public void Use_RedItem()
        {
            int cardForUse = 144;
            SetupHand(cardForUse);

            int sourceId = 0;
            int targetId = 1;

            int cardSummon = 81;
            AddToTable(player2, cardSummon);

            RunUseActionCreatureTest(sourceId, targetId);
        }

        [TestMethod]
        public void Use_RedItem_Abils()
        {
            int cardForUse = 141;
            SetupHand(cardForUse);

            int sourceId = 0;
            int targetId = 1;

            int cardSummon = 115;
            AddToTable(player2, cardSummon);

            RunUseActionCreatureTest(sourceId, targetId);
        }

        [TestMethod]
        public void Use_RedItem_Players_Effects()
        {
            int cardForUse = 145;
            SetupHand(cardForUse);

            int sourceId = 0;
            int targetId = 1;

            int cardSummon = 81;
            AddToTable(player2, cardSummon);

            RunUseActionCreatureTest(sourceId, targetId);
        }

        [TestMethod]
        public void Use_BlueItem()
        {
            int cardForUse = 152;
            SetupHand(cardForUse);

            int sourceId = 0;
            int targetId = -1;

            RunUseActionPlayerTest(sourceId, targetId);
        }

        [TestMethod]
        public void Use_BlueItem_Players_Effects()
        {
            int cardForUse = 156;
            SetupHand(cardForUse);

            int sourceId = 0;
            int targetId = -1;

            RunUseActionPlayerTest(sourceId, targetId);
        }

        [TestMethod]
        public void Use_BlueItem_Target_Creature()
        {
            int cardForUse = 154;
            SetupHand(cardForUse);

            int sourceId = 0;
            int targetId = 1;

            int cardSummon = 81;
            AddToTable(player2, cardSummon);

            RunUseActionCreatureTest(sourceId, targetId);
        }

        [TestMethod]
        public void Use_Invalid_Source()
        {
            int invalidSourceId = 0;
            int targetId = 0;

            RunInvalidUseActionTest(invalidSourceId, targetId);
        }

        [TestMethod]
        public void Use_GreenItem_Invalid_Target()
        {
            int sourceId = 0;
            int invalidTargetId = 0;

            RunInvalidUseActionTest(sourceId, invalidTargetId);
        }


        [TestMethod]
        public void Use_RedItem_Invalid_Target()
        {
            int invalidSourceId = 0;
            int targetId = 0;

            UseAction action = new UseAction(invalidSourceId, targetId);
            bool result = action.Execute(player1, player2);

            Assert.IsTrue(result);
            Assert.AreEqual(DEFAULT_MY_HEALTH, player1.Data.Health);
            Assert.AreEqual(DEFAULT_OPP_HEALTH, player2.Data.Health);
        }

        [TestMethod]
        public void Use_BlueItem_Invalid_Target()
        {
            int invalidSourceId = 0;
            int targetId = 0;

            UseAction action = new UseAction(invalidSourceId, targetId);
            bool result = action.Execute(player1, player2);

            Assert.IsTrue(result);
            Assert.AreEqual(DEFAULT_MY_HEALTH, player1.Data.Health);
            Assert.AreEqual(DEFAULT_OPP_HEALTH, player2.Data.Health);
        }

        [TestMethod]
        public void Use_Invalid_BlueItem_Target_Creature()
        {
            int invalidSourceId = 0;
            int targetId = 0;

            UseAction action = new UseAction(invalidSourceId, targetId);
            bool result = action.Execute(player1, player2);

            Assert.IsTrue(result);
            Assert.AreEqual(DEFAULT_MY_HEALTH, player1.Data.Health);
            Assert.AreEqual(DEFAULT_OPP_HEALTH, player2.Data.Health);
        }

        private void RunUseActionCreatureTest(int sourceId, int targetId)
        {
            Card card = player1.Hand[sourceId];
            player1.Mana = card.Cost;

            int expectedMyHealth = player1.Data.Health + card.MyHealthChange;
            int expectedOppHealth = player2.Data.Health + card.OppHealthChange;

            int expectedNextDraw = player1.NextDrawSize + card.Draw;
            int expectedPlayerHand = player1.Hand.Count - 1;


            player1.Table.TryGetValue(targetId, out CombatCreature target);
            if (target == null)
                player2.Table.TryGetValue(targetId, out target);

            int expectedAttack = target.Attack + (targetId == -1 ? 0 : card.Attack);
            int expectedDefense = target.Defense + (targetId == -1 ? 0 : card.Defense);

            Card expectedCard = target.Card.Clone();
            if (card.Type == 2)
                expectedCard.RemoveAbils(card.Abils);
            else if (card.Type == 1)
                expectedCard.AddAbils(card.Abils);

            UseAction action = new UseAction(sourceId, targetId);
            bool result = action.Execute(player1, player2);

            Assert.IsTrue(result);
            Assert.AreEqual(expectedMyHealth, player1.Data.Health);
            Assert.AreEqual(expectedOppHealth, player2.Data.Health);
            Assert.AreEqual(expectedNextDraw, player1.NextDrawSize);
            Assert.AreEqual(expectedPlayerHand, player1.Hand.Count);
            Assert.AreEqual(expectedAttack, target.Attack);
            Assert.AreEqual(expectedDefense, target.Defense);
            Assert.AreEqual(expectedCard.Abils, target.Card.Abils);
        }

        private void RunUseActionPlayerTest(int sourceId, int targetId)
        {
            Card card = player1.Hand[sourceId];
            player1.Mana = card.Cost;

            int expectedMyHealth = player1.Data.Health + card.MyHealthChange;
            int expectedOppHealth = player2.Data.Health + card.OppHealthChange + card.Defense;

            int expectedNextDraw = player1.NextDrawSize + card.Draw;
            int expectedPlayerHand = player1.Hand.Count - 1;

            UseAction action = new UseAction(sourceId, targetId);
            bool result = action.Execute(player1, player2);

            Assert.IsTrue(result);
            Assert.AreEqual(expectedMyHealth, player1.Data.Health);
            Assert.AreEqual(expectedOppHealth, player2.Data.Health);
            Assert.AreEqual(expectedNextDraw, player1.NextDrawSize);
            Assert.AreEqual(expectedPlayerHand, player1.Hand.Count);
        }

        private void RunInvalidUseActionTest(int sourceId, int targetId)
        {
            int expectedMyHealth = player1.Data.Health;
            int expectedOppHealth = player2.Data.Health;
            int expectedNextDraw = player1.NextDrawSize;
            int expectedOppTable = player2.Table.Count;
            int expectedPlayerHand = player1.Hand.Count;


            UseAction action = new UseAction(sourceId, targetId);
            bool result = action.Execute(player1, player2);

            Assert.IsTrue(result);
            Assert.AreEqual(expectedMyHealth, player1.Data.Health);
            Assert.AreEqual(expectedOppHealth, player2.Data.Health);
            Assert.AreEqual(expectedNextDraw, player1.NextDrawSize);
            Assert.AreEqual(expectedPlayerHand, player1.Hand.Count);
            Assert.AreEqual(expectedOppTable, player2.Table.Count);
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

        CardManager manager;
        Player player1;
        Player player2;
        const int DEFAULT_MY_HEALTH = 29;
        const int DEFAULT_OPP_HEALTH = 30;
        const int DEFAULT_DRAW_SIZE = 1;
    }
}
