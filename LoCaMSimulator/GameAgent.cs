using LoCaMEngine.Entities;
using LoCaMSimulator.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace LoCaMSimulator
{
    class GameAgent : IGameAgent
    {
        Process process = new Process();
        public EventWaitHandle Event { get; set; } = new EventWaitHandle(false, EventResetMode.ManualReset);
        public bool IsTimedOut { get; set; } = false;

        public string Name { get; private set; }

        public string Output { get; private set; }

        public Player Player { get; set; } = new Player();

        //        public List<IGameAction> Actions { get; private set; }

        public List<IActionObserver> GameObservers { get; set; } = new List<IActionObserver>();

        public GameAgent(string name)
        {
            Name = name;
        }

        public void Create(string executableName)
        {
            Output = string.Empty;
            process = CreateProcess(executableName, string.Empty);
            
        }
        int c = 1;
        public void Test()
        {            
            process.StandardInput.WriteLine($"{9} {1} {2} {3}");
            process.StandardInput.WriteLine($"{4} {5} {6} {7}");
            process.StandardInput.WriteLine($"{8}");
            process.StandardInput.WriteLine($"{0}");

            c++;
        }
        public string SendTurnData(Player opponent, List<Card> cards)
        {
            Event.Reset();
            Output = "";
            process.StandardInput.WriteLine(Player.Data.ToString());
            Console.WriteLine(Player.Data.ToString());
            process.StandardInput.WriteLine(opponent.Data.ToString());
            Console.WriteLine(opponent.Data.ToString());
            
            process.StandardInput.WriteLine(opponent.Data.HandSize);
            Console.WriteLine(opponent.Data.HandSize);
            process.StandardInput.WriteLine(cards.Count);
            Console.WriteLine(cards.Count);
            foreach (Card card in cards)
            {
                process.StandardInput.WriteLine(card.ToString());
                Console.WriteLine(card.ToString());
            }
            Event.WaitOne();
            Console.WriteLine($"Output: {Output}");
            return Output;// process.StandardOutput.ReadLine();
        }

        private Process CreateProcess(string executableName, string executableParameter)
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo(executableName, executableParameter)
            {
                UseShellExecute = false,
                ErrorDialog = false,
                RedirectStandardError = true,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };

            process.StartInfo = processStartInfo;
            process.OutputDataReceived += new DataReceivedEventHandler(ReadOutput);
            process.ErrorDataReceived += new DataReceivedEventHandler(ReadErrors);

            bool processStarted = process.Start();            

            process.BeginErrorReadLine();
            process.BeginOutputReadLine();

            return process;
        }
        public void KillProcess()
        {
            process.Kill();
        }

        void ReadOutput(object sender, DataReceivedEventArgs e)
        {
            Output = e.Data;
            Event.Set();
            
            //foreach (IActionObserver observer in GameObservers)
            //{
            //    observer.Notify(Player, e.Data);
            //}
        }

        void ReadErrors(object sender, DataReceivedEventArgs e)
        {
            Console.WriteLine($"{Name}:\t{e.Data}");
        }

        public void AddObserver(IActionObserver observer)
        {
            GameObservers.Add(observer);
        }
    }
}
