using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoCaMSimulator.Interfaces
{
    public interface IActionValidator
    {
        bool Validate(IGameAction1 action, Player1 player1, Player1 player2);
    }
}
