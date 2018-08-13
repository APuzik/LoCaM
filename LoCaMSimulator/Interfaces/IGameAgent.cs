using LoCaMEngine.Entities;
using System.Collections.Generic;
using System.Threading;

namespace LoCaMSimulator.Interfaces
{
    interface IGameAgent
    {
        Player Player { get; set; }
        void Create(string executableName);
        string SendTurnData(Player opponent, List<Card> cards);
        void AddObserver(IActionObserver observer);
        EventWaitHandle Event { get; }
        bool IsTimedOut { get; set; }
        string Output { get; }
        void KillProcess();
    }
}
