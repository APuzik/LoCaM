using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoCaMEngine.Entities
{
    public class Player
    {
        public const int MAX_MANA = 12;
        public const int MAX_HAND_COUNT = 8;
        public const int MAX_ON_TABLE = 6;
        const int RUNE_HP = 5;
        public PlayerData Data { get; set; } = new PlayerData();

        public int Mana{ get; set; }

        public bool IsAlive { get => Data.Health > 0; }
        public int NextDrawSize { get; set; } = 1;
        public List<Card> Deck { get; set; } = new List<Card>();
        public Dictionary<int, Card> Hand { get; set; } = new Dictionary<int, Card>();
        public Dictionary<int, CombatCreature> Table { get; set; } = new Dictionary<int, CombatCreature>();
        public int RunesCount { get => Data.RunesCount; private set => Data.RunesCount = value; }

        public void ResetMana()
        {            
            Data.MaxMana = Math.Min(Data.MaxMana + 1, MAX_MANA);
            Mana = Data.MaxMana;
        }

        public void DrawCards()
        {
            while (Hand.Count < MAX_HAND_COUNT && NextDrawSize > 0 && IsAlive)
            {
                NextDrawSize--;
                if (Deck.Count == 0)
                {
                    RemoveRune();
                }
                else
                {
                    DrawNextCard();
                }
            }

            NextDrawSize = 1;
        }

        private void DrawNextCard()
        {
            Card card = Deck[Deck.Count - 1];
            Deck.RemoveAt(Deck.Count - 1);
            Hand.Add(card.Id, card);
            card.Location = 0;
        }

        private void RemoveRune()
        {
            RunesCount--;
            Data.Health = RunesCount * RUNE_HP;
        }

        public int TakeDamage(int damage)
        {
            Data.Health -= damage;
            
            if (Data.Health < RunesCount * RUNE_HP)
            {
                int oldRunesCount = RunesCount;
                RunesCount = Data.Health / RUNE_HP;

                NextDrawSize += oldRunesCount - RunesCount;
            }
            return Data.Health;
        }

        public void ShaffleDeck()
        {
            Random rnd = new Random((int)DateTime.Now.Ticks);
            Deck = Deck.OrderBy(x => rnd.Next()).ToList();
        }
    }

    public class PlayerData
    {
        public int Health { get; set; } = 30;
        public int MaxMana { get; set; } = 0;
        public int DeckSize { get; set; } = 0;
        public int RunesCount { get; set; } = 5;
        public int HandSize { get; set; } = 0;

        public override string ToString()
        {
            return $"{Health} {MaxMana} {DeckSize} {RunesCount}";
        }

        public string Dump()
        {
            return $"{Health} {MaxMana} {DeckSize} {RunesCount} {HandSize}";
        }
    }
}
