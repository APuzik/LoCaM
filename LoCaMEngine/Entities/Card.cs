using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoCaMEngine.Entities
{
    public class Card
    {
        public int Number { get; set; }
        public int Id { get; set; }
        public int Location { get; set; }
        public int Type { get; set; }
        public int Cost { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public string Abils
        {
            get => new string(abilities.ToArray());
            set
            {
                foreach (char c in value)
                {
                    if (c == '-')
                        continue;
                    abilities.Add(c);
                }
            }
        }
        public int MyHealthChange { get; set; }
        public int OppHealthChange { get; set; }
        public int Draw { get; set; }

        public bool IsCharge { get => abilities.Contains('C'); }
        public bool IsGuard { get => abilities.Contains('G'); }
        public bool IsTrample { get => abilities.Contains('B'); }
        public bool IsLethal { get => abilities.Contains('L'); }
        public bool IsDrain { get => abilities.Contains('D'); }
        public bool IsWard { get => abilities.Contains('W'); }

        public override string ToString()
        {
            return $"{Number} {Id} {Location} {Type} {Cost} {Attack} {Defense} {Abils} {MyHealthChange} {OppHealthChange} {Draw}";
        }

        public void AddAbils(string abils)
        {
            foreach(char abil in abils)
            {
                abilities.Add(abil);
            }
        }

        public void RemoveAbils(string abils)
        {
            foreach (char abil in abils)
            {
                abilities.Remove(abil);
            }
        }

        HashSet<char> abilities = new HashSet<char>();

        public Card Clone()
        {
            return new Card
            {
                Abils = Abils,
                Attack = Attack,
                Cost = Cost,
                Defense = Defense,
                Draw = Draw,
                Id = -1,
                Location = Location,
                MyHealthChange = MyHealthChange,
                Number = Number,
                OppHealthChange = OppHealthChange,
                Type = Type
            };
        }
    }
}
