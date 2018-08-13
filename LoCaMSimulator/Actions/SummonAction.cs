using LoCaMSimulator.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoCaMSimulator.Actions
{
    public class SummonAction1 : IGameAction1
    {
        int id;
        public SummonAction(int id)
        {
            this.id = id;
        }

        public bool Execute(Player player, Player opponent)
        {
            if (!IsActionValid(player))
            {
                return true;
            }

            Card card = player.Hand[id];
            player.TakeDamage(-card.MyHealthChange);
            opponent.TakeDamage(card.OppHealthChange);
            player.NextDrawSize += card.Draw;

            player.Hand.Remove(card.Id);
            player.Table.Add(card.Id, new CombatCreature(card));
            card.Location = 1;

            return true;
        }

        private bool IsActionValid(Player player)
        {
            if (player.Table.Count == Player.MAX_ON_TABLE)
                return false;

            if (!player.Hand.ContainsKey(id))
                return false;

            Card card = player.Hand[id];
            if (player.Mana < card.Cost)
                return false;

            return true;
        }
    }
}
