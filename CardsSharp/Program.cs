using LoCaMEngine;
using LoCaMEngine.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardsSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] inputs;
            int turn = 0;
            GameAI ai = new GameAI();

            // game loop
            for (; ; turn++)
            {
                ai.ClearGameData();
                for (int i = 0; i < 2; i++)
                {
                    inputs = Console.ReadLine().Split(' ');
                    int playerHealth = int.Parse(inputs[0]);
                    int playerMana = int.Parse(inputs[1]);
                    int playerDeck = int.Parse(inputs[2]);
                    int playerRune = int.Parse(inputs[3]);
                    ai.GameState.players[i].Data.Health = playerHealth;
                    ai.GameState.players[i].Data.MaxMana = playerMana;
                    ai.GameState.players[i].Mana = playerMana;
                    ai.GameState.players[i].Data.DeckSize = playerDeck;
                    ai.GameState.players[i].Data.RunesCount = playerRune;
                    //Console.Error.WriteLine($"{playerHealth} {playerMana} {playerDeck} {playerRune}");
                }

                int opponentHand = int.Parse(Console.ReadLine());
                
                //Console.Error.WriteLine(opponentHand);
                int cardCount = int.Parse(Console.ReadLine());
                //Console.Error.WriteLine(cardCount);

                for (int i = 0; i < cardCount; i++)
                {
                    inputs = Console.ReadLine().Split(' ');
                    int cardNumber = int.Parse(inputs[0]);
                    int instanceId = int.Parse(inputs[1]);
                    int location = int.Parse(inputs[2]);
                    int cardType = int.Parse(inputs[3]);
                    int cost = int.Parse(inputs[4]);
                    int attack = int.Parse(inputs[5]);
                    int defense = int.Parse(inputs[6]);
                    string abilities = inputs[7];
                    int myHealthChange = int.Parse(inputs[8]);
                    int opponentHealthChange = int.Parse(inputs[9]);
                    int cardDraw = int.Parse(inputs[10]);
                    Card card = new Card
                    {
                        Abils = abilities,
                        Attack = attack,
                        Cost = cost,
                        Defense = defense,
                        Draw = cardDraw,
                        Id = instanceId,
                        Location = location,
                        MyHealthChange = myHealthChange,
                        Number = cardNumber,
                        OppHealthChange = opponentHealthChange,
                        Type = cardType
                    };
                    if (location == 0)
                        ai.GameState.myHand.Add(card);
                    else if (location == 1)
                        ai.GameState.myTable.Add(card);
                    else if (location == -1)
                        ai.GameState.oppTable.Add(card);

                    //Console.Error.WriteLine($"{cardNumber} {instanceId} {location} {cardType} {cost} {attack} {defense} {abilities} {myHealthChange} {opponentHealthChange} {cardDraw}");
                }

                // Write an action using Console.WriteLine()
                // To debug: Console.Error.WriteLine("Debug messages...");
                //ai.GameState.players[0].sizeHand = myHand.size();

                ai.SortMyHand();

                List<string> commands = new List<string>();
                if (IsDraft(turn))
                    commands.Add(ai.MakeDraft(ai.GameState.myHand));
                else
                    ai.GetCommands(commands);

                ai.PrintCommands(commands);
            }
        }

        static bool IsDraft(int turn)
        {
            return turn < 30;
        }
    }

    class GameState
    {
        public List<Player> players = new List<Player> { new Player(), new Player() };
        public List<Card> myHand = new List<Card>();
        public List<Card> myTable = new List<Card>();
        public List<Card> oppTable = new List<Card>();
    }

    class GameAI
    {
        public GameState GameState { get; } = new GameState();
        public void PrintCommands(List<string> commands)
        {
            if (commands.Count == 0)
            {
                Console.WriteLine("PASS");
                return;
            }

            Console.Write(commands[0]);
            for (int i = 1; i < commands.Count; i++)
                Console.Write($"; {commands[i]}");

            Console.WriteLine();
        }

        public string MakeDraft(List<Card> draft)
        {
            int selected = 0;

            for (int i = 1; i < draft.Count; i++)
            {
                if (draft[i].Attack == 0)
                {
                    if (draft[selected].Attack == 0)
                    {
                        if (draft[i].Cost < draft[selected].Cost)
                        {
                            selected = i;
                        }
                    }
                }
                else
                {
                    if (draft[selected].Attack == 0)
                    {
                        selected = i;
                    }
                    else
                    {
                        bool isChargeI = draft[i].IsCharge;
                        bool isChargeS = draft[selected].IsCharge;
                        if (isChargeI == isChargeS)
                        {
                            if (draft[i].Cost == draft[selected].Cost)
                            {
                                if (draft[i].Attack == draft[selected].Attack)
                                {
                                    if (draft[i].Defense > draft[selected].Defense)
                                    {
                                        selected = i;
                                    }
                                }
                                else if (draft[i].Attack > draft[selected].Attack)
                                {
                                    selected = i;
                                }
                            }
                            else if (draft[i].Cost < draft[selected].Cost)
                            {
                                selected = i;
                            }
                        }
                        else if (isChargeI)
                        {
                            selected = i;
                        }
                    }
                }

            };

            return $"PICK {selected}";
        }

        public void SortMyHand()
        {
            GameState.myHand.Sort((Card c1, Card c2) =>
                {
                    if (c1.Cost == c2.Cost)
                    {
                        bool isChargeI = c1.IsCharge;
                        bool isChargeS = c2.IsCharge;
                        if (isChargeI == isChargeS)
                        {
                            if (c1.Attack == c2.Attack)
                            {
                                return c1.Defense - c2.Defense;
                            }

                            return c1.Attack - c2.Attack;
                        }
                        else
                        {
                            return -1;
                        }
                    }

                    return c1.Cost - c2.Cost;
                });

        }

        public string GetSummonCommand(Card card)
        {
            return $"SUMMON {card.Id}";
        }

        public void GetSummonCommands(List<string> commands)
        {
            int curMana = GameState.players[0].Mana;

            for (int i = 0; i < GameState.myHand.Count && curMana > 0; i++)
            {
                if (GameState.myHand[i].Cost > curMana)
                    break;

                curMana -= GameState.myHand[i].Cost;
                commands.Add(GetSummonCommand(GameState.myHand[i]));
                if (GameState.myHand[i].IsCharge)
                    GameState.myTable.Add(GameState.myHand[i]);
            }
        }

        public string GetAttackCommand(Card card, int id)
        {
            return $"ATTACK {card.Id} {id}";
        }

        public int GetBestAttacker(int guard)
        {
            int id = -1;
            int bestAttack = 10000;
            for (int i = 0; i < GameState.myTable.Count; i++)
            {
                if (GameState.myTable[i].Attack >= GameState.oppTable[guard].Defense && GameState.myTable[i].Attack < bestAttack)
                {
                    bestAttack = GameState.myTable[i].Attack;
                    id = i;
                }
            }

            return id;
        }

        public void GetAttackCommands(List<string> commands)
        {
            List<int> guards = new List<int>();
            for (int i = 0; i < GameState.oppTable.Count; i++)
            {
                if (GameState.oppTable[i].IsGuard)
                    guards.Add(i);
            }

            guards.Sort((int a, int b) =>
            {
                if (GameState.oppTable[a].Defense == GameState.oppTable[b].Defense)
                {
                    return GameState.oppTable[a].Attack - GameState.oppTable[b].Attack;
                }
                return GameState.oppTable[a].Defense - GameState.oppTable[b].Defense;
            });


            for (int i = 0; i < guards.Count; i++)
            {
                int id = GetBestAttacker(guards[i]);
                if (id < 0)
                    id = 0;

                if (id == i)
                    continue;
                Card tmp = GameState.myTable[i];
                GameState.myTable[i] = GameState.myTable[id];
                GameState.myTable[id] = tmp;
            }

            for (int i = 0; i < GameState.myTable.Count; i++)
            {
                int id = i < guards.Count() ? GameState.oppTable[guards[i]].Id : -1;
                commands.Add(GetAttackCommand(GameState.myTable[i], id));
            }
        }

        public void GetCommands(List<string> commands)
        {
            GetSummonCommands(commands);
            GetAttackCommands(commands);
        }

        internal void ClearGameData()
        {
            GameState.myHand.Clear();
            GameState.myTable.Clear();
            GameState.oppTable.Clear();
        }
    }
}
