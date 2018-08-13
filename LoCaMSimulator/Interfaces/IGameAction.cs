using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoCaMSimulator.Interfaces
{
    public interface IGameAction1
    {
        bool Execute(Player1 player, Player1 opponent);
    }
}
