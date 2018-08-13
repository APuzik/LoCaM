using LoCaMEngine.Entities;
using LoCaMEngine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoCaMEngine.Actions
{
    public class PickAction : IGameAction
    {
        int pick = 0;
        CardManager cardManager;
        public PickAction(int pick, CardManager cardManager)
        {
            this.pick = pick;
            this.cardManager = cardManager;
        }

        public bool Execute(Player player, Player opponent)
        {
            if (!IsValidAction())
            {
                return true;
            }

            Card newInstance = cardManager.CreateCardFromDraft(pick);
            player.Deck.Add(newInstance);

            return true;
        }

        private bool IsValidAction()
        {
            if (pick < 0 || pick > 2)
                throw new ArgumentException("Invalid action format. \"PICK\" argument should be 0, 1 or 2.");

            return true;
        }
    }
}
