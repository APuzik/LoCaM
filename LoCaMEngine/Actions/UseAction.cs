using LoCaMEngine.Entities;
using LoCaMEngine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoCaMEngine.Actions
{
    public class UseAction : IGameAction
    {
        readonly int id;
        readonly int target;
        public UseAction(int id, int target)
        {
            this.id = id;
            this.target = target;
        }

        public bool Execute(Player player, Player opponent)
        {
            if (!IsActionValid(player, opponent))
                return true;

            Card item = player.Hand[id];
            Player targetPlayer = null;
            if (target == -1)
            {
                opponent.TakeDamage(item.Defense);
            }
            else
            {
                if (item.Type == 1)
                {
                    targetPlayer = player;
                    player.Table[target].Card.AddAbils(item.Abils);
                }
                else if (item.Type == 2 || item.Type == 3)
                {
                    targetPlayer = opponent;
                    opponent.Table[target].Card.RemoveAbils(item.Abils);
                }
                targetPlayer.Table[target].Card.Attack += item.Attack;
                targetPlayer.Table[target].Card.Defense += item.Defense;
            }

            player.TakeDamage(-item.MyHealthChange);
            opponent.TakeDamage(item.OppHealthChange);
            player.NextDrawSize += item.Draw;

            player.Hand.Remove(item.Id);

            return true;
        }

        private bool IsActionValid(Player player, Player opponent)
        {
            if (!player.Hand.ContainsKey(id))
                return false;

            Card item = player.Hand[id];
            if (item.Type == 1)
            {
                if (!player.Table.ContainsKey(target))
                    return false;
            }
            else if (item.Type == 2)
            {
                if (!opponent.Table.ContainsKey(target))
                    return false;
            }
            else if (item.Type == 3)
            {
                if (target != -1 && !opponent.Table.ContainsKey(target))
                    return false;
            }
            else
            {
                return false;
            }

            return true;
        }
    }
}
