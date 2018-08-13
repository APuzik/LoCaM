using LoCaMEngine.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoCaMSimulator.Interfaces
{
    interface IActionObserver
    {
        void Notify(Player player, string playerOutput);
    }
}
