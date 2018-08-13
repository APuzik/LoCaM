using LoCaMEngine.Actions;
using LoCaMSimulator;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LoCaMSimulatorTest
{
    [TestClass]
    public class ActionFactoryTest
    {
        [TestMethod]
        public void TestPickAction()
        {
            ActionFactory factory = new ActionFactory();
            PickAction action = factory.CreateGameAction("PICK 0") as PickAction;

            Assert.IsNotNull(action);
            Assert.IsInstanceOfType(action, typeof(PickAction));
        }

        [TestMethod]
        public void TestAttackAction()
        {
            ActionFactory factory = new ActionFactory();
            factory.IsDraft = false;

            AttackAction action = factory.CreateGameAction("ATTACK 0 1") as AttackAction;

            Assert.IsNotNull(action);
            Assert.IsInstanceOfType(action, typeof(AttackAction));
        }

        [TestMethod]
        public void TestUseAction()
        {
            ActionFactory factory = new ActionFactory();
            factory.IsDraft = false;

            UseAction action = factory.CreateGameAction("USE 0 -1") as UseAction;

            Assert.IsNotNull(action);
            Assert.IsInstanceOfType(action, typeof(UseAction));
        }

        [TestMethod]
        public void TestSummonAction()
        {
            ActionFactory factory = new ActionFactory();
            factory.IsDraft = false;

            SummonAction action = factory.CreateGameAction("SUMMON 0") as SummonAction;

            Assert.IsNotNull(action);
            Assert.IsInstanceOfType(action, typeof(SummonAction));
        }

        [TestMethod]
        public void TestPassAction_Draft()
        {
            ActionFactory factory = new ActionFactory();

            PickAction action = factory.CreateGameAction("PASS") as PickAction;

            Assert.IsNotNull(action);
            Assert.IsInstanceOfType(action, typeof(PickAction));
        }

        [TestMethod]
        public void TestPassAction()
        {
            ActionFactory factory = new ActionFactory();
            factory.IsDraft = false;

            PassAction action = factory.CreateGameAction("PASS") as PassAction;

            Assert.IsNotNull(action);
            Assert.IsInstanceOfType(action, typeof(PassAction));
        }
    }
}
