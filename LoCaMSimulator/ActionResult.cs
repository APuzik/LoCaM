using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoCaMSimulator
{
    class ActionResult
    {
        bool IsValid { get; set; }
        int AttackerDamage { get; set; }
        int DefenderDamage { get; set; }
        int OpponentDamage { get; set; }
    }
}
