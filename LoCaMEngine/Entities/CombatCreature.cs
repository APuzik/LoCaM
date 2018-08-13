using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoCaMEngine.Entities
{
    public class CombatCreature
    {
        public CombatCreature(Card card)
        {
            Card = card;
            if(card.IsCharge)
            {
                AttacksCount = 1;
            }
        }

        public bool CanAttack { get => AttacksCount > 0; }
        public int AttacksCount { get; private set; } = -1;
        public bool IsDead { get => ShouldDie || Card.Defense <= 0; }
        public bool ShouldDie { get; private set; }

        public int Id { get => Card.Id; }
        public int Attack { get => Card.Attack; }
        public int Defense { get => Card.Defense; }
        public bool IsCharge { get => Card.IsCharge; }
        public bool IsGuard { get => Card.IsGuard; }
        public bool IsTrample { get => Card.IsTrample; }
        public bool IsLethal { get => Card.IsLethal; }
        public bool IsDrain { get => Card.IsDrain; }
        public bool IsWard { get => Card.IsWard; }
        

        public int TakeDamage(int damage)
        {
            Card.Defense -= damage;
            return Card.Defense;
        }

        public int AttackPlayer(Player target)
        {
            target.TakeDamage(Attack);
            AttacksCount--;

            return Attack;
        }

        public (int damageDealt, int exceedDamage) AttackCreature(CombatCreature target)
        {
            int attackCreatureAttack = target.IsWard ? 0 : Attack;
            int defenseCreatureAttack = IsWard ? 0 : target.Attack;

            int exceedDamage = IsTrample ? Math.Max(attackCreatureAttack - target.Defense, 0) : 0;

            target.TakeDamage(attackCreatureAttack);
            TakeDamage(defenseCreatureAttack);

            target.ShouldDie = IsLethal && attackCreatureAttack > 0;
            ShouldDie = target.IsLethal && defenseCreatureAttack > 0;

            Card.RemoveAbils("W");
            target.Card.RemoveAbils("W");

            AttacksCount--;
            return (attackCreatureAttack, exceedDamage);
        }

        public void ResetAttacks(int attacksCount = 1)
        {
            AttacksCount = attacksCount;
        }

        public Card Card { get; }
    }
}
