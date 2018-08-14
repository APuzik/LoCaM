using LoCaMEngine.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoCaMEngine
{
    public class GameEngine
    {
        public void MakeDraft(Player player1, Player player2)
        {
        }

        public void Shaffle(Player player1, Player player2)
        {
            player1.ShaffleDeck();
            player2.ShaffleDeck();
        }

        public void MakeInitialDraw(Player player1, Player player2)
        {
            for (int i = 0; i < 3; i++)
            {
                player1.DrawCards();
                player2.DrawCards();
            }
            player2.DrawCards();
        }

        public void InitTurn(Player player)
        {
            player.ResetMana();
            player.AllowAttack();
            player.DrawCards();
        }
    }
}
