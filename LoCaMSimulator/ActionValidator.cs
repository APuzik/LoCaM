using LoCaMSimulator.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoCaMSimulator
{
    class AttackActionValidator : IActionValidator
    {
        public bool Validate(IGameAction1 action, Player1 player1, Player1 player2)
        { 
        //{
        //    if (!player.Table.ContainsKey(id))
        //        return false;

        //    if (!player.Table[id].CanAttack)
        //        return false;

        //    if (target != -1 && !opponent.Table.ContainsKey(target))
        //        return false;

        //    bool hasGuard = opponent.Table.Any(c => c.Value.IsGuard);
        //    if (hasGuard)
        //    {
        //        if (target == -1 || !opponent.Table[target].IsGuard)
        //        {
        //            return false;
        //        }
        //    }

            return true;
        }
    }
}
