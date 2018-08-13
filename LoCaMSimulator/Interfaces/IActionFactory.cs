using LoCaMEngine.Interfaces;

namespace LoCaMSimulator.Interfaces
{
    interface IActionFactory
    {
        bool IsDraft { get; set; }
        IGameAction CreateGameAction(string input);
    }
}
