using LoCaMSimulator.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoCaMSimulator.Actions
{
    public class PassAction1 : IGameAction1
    {
        public bool Execute(Player player, Player opponent)
        {
            return false;
        }
    }
}
