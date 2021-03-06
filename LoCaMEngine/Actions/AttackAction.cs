﻿using LoCaMEngine.Entities;
using LoCaMEngine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoCaMEngine.Actions
{
    public class AttackAction : IGameAction
    {
        readonly int id;
        readonly int target;
        public AttackAction(int id, int target)
        {
            this.id = id;
            this.target = target;
        }

        public bool Execute(Player player, Player opponent)
        {
            if (!IsActionValid(player, opponent))
            {
                return true;
            }

            CombatCreature attackCreature = player.Table[id];
            CombatCreature defenseCreature = target >= 0 ? opponent.Table[target] : null;

            int attackCreatureAttack = attackCreature.Attack;

            int damageDealt = 0;

            if (defenseCreature == null)
            {
                damageDealt = attackCreature.AttackPlayer(opponent);
            }
            else
            {
                int exceedDamage = 0;
                (damageDealt, exceedDamage) = attackCreature.AttackCreature(defenseCreature);

                opponent.TakeDamage(exceedDamage);

                if (attackCreature.IsDead)
                    player.Table.Remove(attackCreature.Id);

                if (defenseCreature.IsDead)
                    opponent.Table.Remove(defenseCreature.Id);
            }

            if (attackCreature.IsDrain)
            {
                player.ChangeHealth(damageDealt);
            }

            return true;
        }

        bool IsActionValid(Player player, Player opponent)
        {
            if (!player.Table.ContainsKey(id))
                return false;

            if (!player.Table[id].CanAttack)
                return false;

            if (target != -1 && !opponent.Table.ContainsKey(target))
                return false;

            bool hasGuard = opponent.Table.Any(c => c.Value.IsGuard);
            if (hasGuard)
            {
                if (target == -1 || !opponent.Table[target].IsGuard)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
