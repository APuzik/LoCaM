using LoCaMEngine;
using LoCaMEngine.Entities;
using LoCaMEngine.Interfaces;
using LoCaMSimulator.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LoCaMSimulator
{
    class GameReferee : IActionObserver
    {
        const string DRAW = "Draw...";
        const string PLAYER1 = "Player1";
        const string PLAYER2 = "Player2";
        readonly string PLAYER1_WINS = $"{PLAYER1} wins!";
        readonly string PLAYER2_WINS = $"{PLAYER2} wins!";
        const int CARDS_IN_DECK = 30;
        const int TURNS_TO_PLAY = 50;

        List<IGameAgent> agents;

        GameState gameState;
        GameEngine gameEngine = new GameEngine();

        public ActionParser ActionParser { get; set; }
        int currentTurn = 0;
        int timeoutMs = 100;

        public GameReferee()
        {
            gameState = new GameState();

            ActionParser = new ActionParser
            {
                Factory = new ActionFactory
                {
                    CardManager = gameState.CardManager
                }
            };

            agents = new List<IGameAgent>
            {
                new GameAgent("agents[0]"),
                new GameAgent("agents[1]"),
            };

            gameState.Player1 = agents[0].Player;
            gameState.Player2 = agents[1].Player;
        }

        public string Winner
        {
            get
            {
                if (agents[0].IsTimedOut && agents[1].IsTimedOut)
                {
                    return $"Both players are timed out! {DRAW}";
                }
                else if (agents[0].IsTimedOut)
                {
                    return $"{PLAYER1} is timed out! {PLAYER2_WINS}";
                }
                else if (agents[1].IsTimedOut)
                {
                    return $"{PLAYER2} is timed out! {PLAYER1_WINS}";
                }
                else
                {
                    if (!agents[0].Player.IsAlive && !agents[1].Player.IsAlive)
                    {
                        return DRAW;
                    }
                    else if (!agents[0].Player.IsAlive)
                    {
                        return PLAYER2_WINS;
                    }
                    else if (!agents[1].Player.IsAlive)
                    {
                        return PLAYER1_WINS;
                    }
                }

                return "Undefined!";
            }
        }

        bool IsGameEnded { get => agents[0].IsTimedOut || agents[1].IsTimedOut || !agents[0].Player.IsAlive || !agents[1].Player.IsAlive; }
        bool IsDraft { get => currentTurn < CARDS_IN_DECK; }

        public void StartGame()
        {
            foreach (IGameAgent agent in agents)
            {
                agent.AddObserver(this);
            }


            const int MAX_TURNS_HARDLIMIT = (2 * CARDS_IN_DECK + 2 * CARDS_IN_DECK + 2 * 10) * 10;
            currentTurn = 0;

            timeoutMs = 1000;
            for (; currentTurn < CARDS_IN_DECK; currentTurn++)
            {
                if (IsGameEnded)
                    return;
                DraftNext();

                if (currentTurn > 1)
                    timeoutMs = 100;
            }

            ActionParser.Factory.IsDraft = false;
            gameEngine.Shaffle(agents[0].Player, agents[1].Player);
            gameEngine.MakeInitialDraw(agents[0].Player, agents[1].Player);
            
            timeoutMs = 1000;
            for (; currentTurn < MAX_TURNS_HARDLIMIT && !IsGameEnded; currentTurn++)
            {
                if (IsTimeToFinish())
                {
                    agents[currentTurn % 2].Player.RemoveRune();
                }
                MakeTurn(agents[currentTurn % 2], agents[(currentTurn + 1) % 2]);
                if (currentTurn > CARDS_IN_DECK + 2)
                    timeoutMs = 100;
            }
        }

        private bool IsTimeToFinish()
        {
            return currentTurn >= (CARDS_IN_DECK + TURNS_TO_PLAY) * 2;
        }

        private void MakeTurn(IGameAgent agent, IGameAgent opponent)
        {
            gameEngine.InitTurn(agent.Player);
            
            if (agent.Player.IsAlive)
            {
                List<Card> cards = gameState.GetVisibleCards(agent.Player, opponent.Player);
                string output = agent.SendTurnData(opponent.Player, cards);
                PerformActions(agent.Player, output);
                //Task<bool> response = WaitForResponseAsync(agent.Event);
                //if (!response.Result)
                //{
                ////   agent.IsTimedOut = true;
                //}
            }
        }


        private void DraftNext()
        {
            List<Card> draftCards = gameState.CardManager.GetDraft();
            string output = agents[0].SendTurnData(agents[1].Player, draftCards);
            PerformActions(agents[0].Player, output);
            //Task<bool> response1 = WaitForResponseAsync(agents[0].Event);

            output = agents[1].SendTurnData(agents[0].Player, draftCards);
            PerformActions(agents[1].Player, output);
            //Task<bool> response2 = WaitForResponseAsync(agents[1].Event);

            //if (!response1.Result)
            //{
            //    agents[0].IsTimedOut = true;
            //}
            //if (!response2.Result)
            //{
            //    agents[1].IsTimedOut = true;
            //}
        }

        private Task<bool> WaitForResponseAsync(EventWaitHandle responseReceivedEvent)
        {
            responseReceivedEvent.Reset();

            return responseReceivedEvent.WaitOneAsync(timeoutMs);

            //var tcs = new TaskCompletionSource<bool>();
            //var rwh = ThreadPool.RegisterWaitForSingleObject(responseReceivedEvent, delegate { tcs.TrySetResult(responseReceivedEvent.WaitOne(timeoutMs)); }, null, -1, true);
            //var t = tcs.Task;
            //t.ContinueWith((antecedent) => rwh.Unregister(null));

            //return t;
        }

        internal void CreateGameProcesses(string executableName1, string executableName2)
        {
            agents[0].Create(executableName1);
            agents[1].Create(executableName2);
        }

        public void Notify(Player player, string playerOutput)
        {

        }

        public void PerformActions(Player player, string playerOutput)
        {
            Player opponent = gameState.GetOpponent(player);

            List<IGameAction> actions = ActionParser.GetActions(playerOutput);
            if (actions.Count == 0)
                throw new Exception("Invalid action specified.");

            if (IsDraft)
            {
                actions[0].Execute(player, opponent);
            }
            else
            {
                foreach (IGameAction action in actions)
                {
                    if (!action.Execute(player, opponent))
                    {
                        break;
                    }
                }
            }
        }

        internal void KillProcesses()
        {
            agents[0].KillProcess();
            agents[1].KillProcess();
        }
    }

    static class WaitHandleExtension
    {
        public static Task<bool> WaitOneAsync(this WaitHandle waitHandle, int timeout)
        {
            if (waitHandle == null)
                throw new ArgumentNullException("waitHandle");

            var tcs = new TaskCompletionSource<bool>();
            var rwh = ThreadPool.RegisterWaitForSingleObject(waitHandle, (state, isTimedOut) => { tcs.TrySetResult(!isTimedOut); }, null, timeout, true);
            var t = tcs.Task;
            t.ContinueWith((antecedent) => rwh.Unregister(null));

            return t;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            //string executableName1 = @"G:\AS\codinggame\LoCaMSimulator\Debug\CardsCpp.exe";//
            //string executableName2 = @"G:\AS\codinggame\LoCaMSimulator\Debug\CardsCpp1.exe";//
            string executableName1 = @"..\..\..\CardsSharp\bin\Debug\CardsSharp.exe";//@"G:\AS\codinggame\LoCaMSimulator\Debug\CardsCpp.exe";//
            string executableName2 = @"..\..\..\CardsSharp\bin\Debug\CardsSharp.exe";// @"G:\AS\codinggame\LoCaMSimulator\Debug\CardsCpp1.exe";//

            GameReferee referee = new GameReferee();
            referee.CreateGameProcesses(executableName1, executableName2);
            referee.StartGame();
            Console.WriteLine($"Winner is {referee.Winner}");
            referee.KillProcesses();
            Console.ReadLine();
        }
    }
}
