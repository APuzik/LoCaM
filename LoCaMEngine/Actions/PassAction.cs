using LoCaMEngine.Entities;
using LoCaMEngine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoCaMEngine.Actions
{
    public class PassAction : IGameAction
    {
        public bool Execute(Player player, Player opponent)
        {
            return false;
        }
    }
}
