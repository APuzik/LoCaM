using LoCaMEngine.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoCaMEngine.Interfaces
{
    public interface IGameAction
    {
        bool Execute(Player player, Player opponent);
    }
}
