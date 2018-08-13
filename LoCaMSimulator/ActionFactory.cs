using LoCaMEngine;
using LoCaMEngine.Actions;
using LoCaMEngine.Interfaces;
using LoCaMSimulator.Interfaces;
using System;

namespace LoCaMSimulator
{
    public class ActionFactory : IActionFactory
    {
        public CardManager CardManager { get; set; }
        public bool IsDraft { get; set; } = true;
        public IGameAction CreateGameAction(string input)
        {
            string[] values = input.Split(new char[] { ' ' });

            if (input.Contains(GameActions.PASS))
            {
                if(IsDraft)
                    return new PickAction(0, CardManager);
                else
                    return new PassAction();
            }
            else if (input.IndexOf(GameActions.PICK) >= 0 && IsDraft)
            {
                int.TryParse(values[1], out int pick);

                return new PickAction(pick, CardManager);
            }
            else if (input.IndexOf(GameActions.SUMMON) >= 0 && !IsDraft)
            {
                int id = int.Parse(values[1]);
                
                return new SummonAction(id);
            }
            else if (input.IndexOf(GameActions.ATTACK) >= 0 && !IsDraft)
            {
                int sourceId = int.Parse(values[1]);
                int targetId = int.Parse(values[2]);

                return new AttackAction(sourceId, targetId);
            }
            else if (input.IndexOf(GameActions.USE) >= 0 && !IsDraft)
            {
                int sourceId = int.Parse(values[1]);
                int targetId = int.Parse(values[2]);

                return new UseAction(sourceId, targetId);
            }
            else
            {
                throw new Exception($"Invalid action specified: {input}. IsDraft: {IsDraft}.");
            }

        }
    }
}
