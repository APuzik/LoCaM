using LoCaMEngine;
using LoCaMEngine.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoCaMSimulator
{
    class GameState
    {
        public Player Player1 { get; set; }
        public Player Player2 { get; set; }
        public List<Card> GetVisibleCards(Player player1, Player player2)
        {
            var cards = new List<Card>();
            cards.AddRange(player1.Hand.Values);
            cards.AddRange(player1.Table.Values.Select(x => { x.Card.Location = 1; return x.Card; }));
            cards.AddRange(player2.Table.Values.Select(x => { x.Card.Location = -1; return x.Card; }));

            return cards;
        }

        internal Player GetOpponent(Player player)
        {
            if (player == Player1)
                return Player2;
            else if (player == Player2)
                return Player1;
            else
                throw new Exception("Invalid player object.");
        }

        public CardManager CardManager { get; set; } = new CardManager();
    }
}
