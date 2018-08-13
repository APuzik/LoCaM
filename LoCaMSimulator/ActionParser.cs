using LoCaMEngine.Interfaces;
using LoCaMSimulator.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoCaMSimulator
{
    class ActionParser
    {
        public IActionFactory Factory { get; set; } = new ActionFactory();
        public List<IGameAction> GetActions(string input)
        {
            List<IGameAction> gameActions = new List<IGameAction>();
            string[] actions = input.Split(new char[] { ';' });
            for(int i = 0; i < actions.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(actions[i]))
                    continue;

                gameActions.Add(Factory.CreateGameAction(actions[i].Trim()));
            }

            return gameActions;
        }
    }
}
