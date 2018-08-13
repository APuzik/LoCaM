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
    public class AttackActionTest
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
        public void Attack_Player()
        {
            List<int> player1Table = new List<int> { (int)CardDefs.Slimer };
            List<int> player2Table = new List<int>();

            SetupTable(player1Table, player2Table);

            int sourceId = 0;
            RunAttackPlayerTest(sourceId);
        }        

        [TestMethod]
        public void Attack_Player_Drain()
        {
            List<int> player1Table = new List<int> { 37 };
            List<int> player2Table = new List<int>();

            SetupTable(player1Table, player2Table);

            int sourceId = 0;
            RunAttackPlayerTest(sourceId);
        }

        [TestMethod]
        public void Attack_Player_Invalid_Source()
        {
            int sourceId = 0;
            RunInvalidAttackPlayerTest(sourceId);
        }

        [TestMethod]
        public void Attack_Player_Cant_Attack()
        {
            List<int> player1Table = new List<int> { 37 };
            List<int> player2Table = new List<int>();

            SetupTable(player1Table, player2Table);

            int sourceId = 0;
            RunInvalidAttackPlayerTest(sourceId);
        }

        [TestMethod]
        public void Attack_Player_Has_Guards()
        {
            List<int> player1Table = new List<int> { 37 };
            List<int> player2Table = new List<int> { 54 };

            SetupTable(player1Table, player2Table);

            int sourceId = 0;
            RunInvalidAttackPlayerTest(sourceId);
        }        

        [TestMethod]
        public void Attack_Creature()
        {
            List<int> player1Table = new List<int> { 0 };
            List<int> player2Table = new List<int> { 0 };

            SetupTable(player1Table, player2Table);

            int sourceId = 0;
            int targetId = 1;
            RunAttackCreatureTest(sourceId, targetId);
        }

        [TestMethod]
        public void Attack_Creature_Trample()
        {
            List<int> player1Table = new List<int> { 57 };
            List<int> player2Table = new List<int> { 56 };

            SetupTable(player1Table, player2Table);

            int sourceId = 0;
            int targetId = 1;
            RunAttackCreatureTest(sourceId, targetId);
        }

        [TestMethod]
        public void Attack_Creature_Lethal()
        {
            int cardAttacker = 51;
            int cardDefender = 61;

            List<int> player1Table = new List<int> { cardAttacker };
            List<int> player2Table = new List<int> { cardDefender };

            SetupTable(player1Table, player2Table);

            int sourceId = 0;
            int targetId = 1;
            RunAttackCreatureTest(sourceId, targetId);

        }

        [TestMethod]
        public void Attack_Creature_Drain()
        {
            int cardAttacker = 46;
            int cardDefender = 54;

            List<int> player1Table = new List<int> { cardAttacker };
            List<int> player2Table = new List<int> { cardDefender };

            SetupTable(player1Table, player2Table);

            int sourceId = 0;
            int targetId = 1;
            RunAttackCreatureTest(sourceId, targetId);
        }

        [TestMethod]
        public void Attack_Creature_Trample_Exceed()
        {
            int cardAttacker = 57;
            int cardDefender = 53;

            List<int> player1Table = new List<int> { cardAttacker };
            List<int> player2Table = new List<int> { cardDefender };

            SetupTable(player1Table, player2Table);

            int sourceId = 0;
            int targetId = 1;
            RunAttackCreatureTest(sourceId, targetId);
        }

        [TestMethod]
        public void Attack_Creature_Trample_Ward()
        {
            int cardAttacker = 57;
            int cardDefender = 64;

            List<int> player1Table = new List<int> { cardAttacker };
            List<int> player2Table = new List<int> { cardDefender };

            SetupTable(player1Table, player2Table);

            int sourceId = 0;
            int targetId = 1;
            RunAttackCreatureTest(sourceId, targetId);
        }

        [TestMethod]
        public void Attack_Creature_Lethal_Ward()
        {
            int cardAttacker = 51;
            int cardDefender = 66;

            List<int> player1Table = new List<int> { cardAttacker };
            List<int> player2Table = new List<int> { cardDefender };

            SetupTable(player1Table, player2Table);

            int sourceId = 0;
            int targetId = 1;
            RunAttackCreatureTest(sourceId, targetId);
        }

        [TestMethod]
        public void Attack_Creature_Drain_Ward()
        {
            int cardAttacker = 37;
            int cardDefender = 64;

            List<int> player1Table = new List<int> { cardAttacker };
            List<int> player2Table = new List<int> { cardDefender };

            SetupTable(player1Table, player2Table);

            int sourceId = 0;
            int targetId = 1;
            RunAttackCreatureTest(sourceId, targetId);
        }

        [TestMethod]
        public void Attack_Creature_Ward_Ward()
        {
            int cardAttacker = 64;
            int cardDefender = 64;

            List<int> player1Table = new List<int> { cardAttacker };
            List<int> player2Table = new List<int> { cardDefender };

            SetupTable(player1Table, player2Table);

            int sourceId = 0;
            int targetId = 1;
            RunAttackCreatureTest(sourceId, targetId);
        }

        [TestMethod]
        public void Attack_Creature_Invalid_Source()
        {
            int cardAttacker = 54;
            int cardDefender = 54;

            List<int> player1Table = new List<int> { cardAttacker };
            List<int> player2Table = new List<int> { cardDefender };

            SetupTable(player1Table, player2Table);
            player1.Table[0].ResetAttacks();

            int invalidSourceId = 2;
            int targetId = 1;

            RunInvalidAttackCreatureTest(invalidSourceId, targetId);
        }

        [TestMethod]
        public void Attack_Creature_Cant_Attack()
        {
            int cardAttacker = 55;
            int cardDefender = 55;

            List<int> player1Table = new List<int> { cardAttacker };
            List<int> player2Table = new List<int> { cardDefender };

            SetupTable(player1Table, player2Table);

            int sourceId = 0;
            int targetId = 1;

            RunInvalidAttackCreatureTest(sourceId, targetId);
        }

        [TestMethod]
        public void Attack_Creature_Invalid_Target()
        {
            int cardAttacker = 54;
            int cardDefender = 54;

            List<int> player1Table = new List<int> { cardAttacker };
            List<int> player2Table = new List<int> { cardDefender };

            SetupTable(player1Table, player2Table);
            player1.Table[0].ResetAttacks();

            int sourceId = 0;
            int invalidTargetId = 2;

            RunInvalidAttackCreatureTest(sourceId, invalidTargetId);
        }        

        [TestMethod]
        public void Attack_Creature_Target_NonGuard()
        {
            int cardAttacker = 0;
            int cardDefenderNonGuard = 0;
            int cardDefender = 54;

            List<int> player1Table = new List<int> { cardAttacker };
            List<int> player2Table = new List<int> { cardDefenderNonGuard, cardDefender };

            SetupTable(player1Table, player2Table);
            player1.Table[0].ResetAttacks();

            int sourceId = 0;
            int targetId = 1;

            RunInvalidAttackCreatureTest(sourceId, targetId);
        }

        [TestMethod]
        public void Attack_Creature_Target_Guard()
        {
            int cardAttacker = 0;
            int cardDefenderNonGuard = 0;
            int cardDefender = 54;

            List<int> player1Table = new List<int> { cardAttacker };
            List<int> player2Table = new List<int> { cardDefenderNonGuard, cardDefender };

            SetupTable(player1Table, player2Table);
            player1.Table[0].ResetAttacks();

            int sourceId = 0;
            int targetId = 2;

            RunInvalidAttackCreatureTest(sourceId, targetId);
        }

        private void RunAttackPlayerTest(int sourceId)
        {
            int target = -1;

            CombatCreature attackCreature = player1.Table[sourceId];
            attackCreature.ResetAttacks();

            int expectedMyHealth = player1.Data.Health + (attackCreature.IsDrain ? attackCreature.Attack : 0);
            int expectedOppHealth = player2.Data.Health - attackCreature.Attack;

            AttackAction action = new AttackAction(sourceId, target);
            bool result = action.Execute(player1, player2);

            Assert.IsTrue(result);
            Assert.AreEqual(expectedMyHealth, player1.Data.Health);
            Assert.AreEqual(expectedOppHealth, player2.Data.Health);
            Assert.IsFalse(attackCreature.CanAttack);
        }

        private void RunInvalidAttackPlayerTest(int sourceId)
        {
            int target = -1;

            AttackAction action = new AttackAction(sourceId, target);
            bool result = action.Execute(player1, player2);

            Assert.IsTrue(result);
            Assert.AreEqual(DEFAULT_MY_HEALTH, player1.Data.Health);
            Assert.AreEqual(DEFAULT_OPP_HEALTH, player2.Data.Health);
        }

        private void RunAttackCreatureTest(int sourceId, int targetId)
        {
            CombatCreature attackCreature = player1.Table[sourceId];
            attackCreature.ResetAttacks();

            CombatCreature defenseCreature = player2.Table[targetId];

            int expectedAttCreatureAttack = !defenseCreature.IsWard ? attackCreature.Attack : 0;
            int expectedDefCreatureAttack = !attackCreature.IsWard ? defenseCreature.Attack : 0;

            int expectedMyHealthChange = attackCreature.IsDrain ? expectedAttCreatureAttack : 0;
            int expectedOppHealthChange = attackCreature.IsTrample ? Math.Max(expectedAttCreatureAttack - defenseCreature.Defense, 0) : 0;

            int expectedMyHealth = player1.Data.Health + expectedMyHealthChange;
            int expectedOppHealth = player2.Data.Health - expectedOppHealthChange;

            int expectedMyCardDefense = attackCreature.Defense - expectedDefCreatureAttack;
            int expectedOppCardDefense = defenseCreature.Defense - expectedAttCreatureAttack;

            bool expectedMyShouldDie = defenseCreature.IsLethal && expectedDefCreatureAttack > 0;
            bool expectedOppShouldDie = attackCreature.IsLethal && expectedAttCreatureAttack > 0;

            bool expectedMyIsDead = expectedMyShouldDie || expectedMyCardDefense <= 0;
            bool expectedOppIsDead = expectedOppShouldDie || expectedOppCardDefense <= 0;

            int expectedMyTableCount = expectedMyIsDead ? player1.Table.Count - 1 : player1.Table.Count;
            int expectedOppTableCount = expectedOppIsDead ? player2.Table.Count - 1 : player2.Table.Count;

            AttackAction action = new AttackAction(sourceId, targetId);
            bool result = action.Execute(player1, player2);

            Assert.IsTrue(result);
            Assert.AreEqual(expectedMyHealth, player1.Data.Health);
            Assert.AreEqual(expectedOppHealth, player2.Data.Health);
            Assert.AreEqual(expectedMyCardDefense, attackCreature.Defense);
            Assert.AreEqual(expectedOppCardDefense, defenseCreature.Defense);
            Assert.AreEqual(expectedMyShouldDie, attackCreature.ShouldDie);
            Assert.AreEqual(expectedOppShouldDie, defenseCreature.ShouldDie);
            Assert.AreEqual(expectedMyIsDead, attackCreature.IsDead);
            Assert.AreEqual(expectedOppIsDead, defenseCreature.IsDead);
            Assert.IsFalse(attackCreature.IsWard);
            Assert.IsFalse(defenseCreature.IsWard);
            Assert.IsFalse(attackCreature.CanAttack);
            Assert.AreEqual(expectedMyTableCount, player1.Table.Count);
            Assert.AreEqual(expectedOppTableCount, player2.Table.Count);
        }

        private void RunInvalidAttackCreatureTest(int sourceId, int targetId)
        {
            player1.Table.TryGetValue(sourceId, out CombatCreature attackCreature);
            player2.Table.TryGetValue(sourceId, out CombatCreature defenseCreature);

            AttackAction action = new AttackAction(sourceId, targetId);
            bool result = action.Execute(player1, player2);

            Assert.IsTrue(result);
            Assert.AreEqual(DEFAULT_MY_HEALTH, player1.Data.Health);
            Assert.AreEqual(DEFAULT_OPP_HEALTH, player2.Data.Health);
            if (attackCreature != null)
            {
                CardManagerTest.AssertCards(manager.AllPossibleCards[attackCreature.Card.Number - 1], attackCreature.Card);
            }
            if (defenseCreature != null)
            {
                CardManagerTest.AssertCards(manager.AllPossibleCards[defenseCreature.Card.Number - 1], defenseCreature.Card);

            }
        }

        private void SetupTable(List<int> player1Table, List<int> player2Table)
        {
            AddCardToPlayerTable(player1, player1Table);
            AddCardToPlayerTable(player2, player2Table);
        }

        private void AddCardToPlayerTable(Player player, List<int> playerTable)
        {
            foreach (int cardNumber in playerTable)
            {
                AddToTable(player, cardNumber);
            }
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
    }
}
