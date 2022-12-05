using System;
using System.Collections.Generic;
using System.Linq;

namespace AOC2022D5
{
    class Program
    {

        private static IList<Stack> WarehouseStacks;

        static void Main(string[] args)
        {
            EnterStartUpConfiguration();
            PrintCurrentStacks();
#if (DEBUG)
            IssueMovementPlan(true);
#elif (RELEASE)
            IssueMovementPlan();
#endif
            PrintAnswer();
        }

        private static void PrintAnswer()
        {
            Console.WriteLine();
            Console.WriteLine("Top Crates Are...");
            foreach (var stack in WarehouseStacks)
            {
                Console.WriteLine($"[{stack.PrintTopCrate()}]");
            }
        }

        //Would load these moves from appsettings or passed Parameter
        private static void IssueMovementPlan(bool printAfterMove = false)
        {
            MoveCrates(1, 2, 1);
            if (printAfterMove) PrintCurrentStacks();
            MoveCrates(3, 1, 3);
            if (printAfterMove) PrintCurrentStacks();
            MoveCrates(2, 2, 1);
            if (printAfterMove) PrintCurrentStacks();
            MoveCrates(1, 1, 2);
            if (printAfterMove) PrintCurrentStacks();
        }

        private static void PrintCurrentStacks()
        {
            foreach(var stack in WarehouseStacks)
            {
                Console.WriteLine(stack.PrintAllCrates());
            }
            Console.WriteLine();
        }
        //Typically i'd load this from a appsettings.json but quick and dirty
        private static void EnterStartUpConfiguration()
        {
            WarehouseStacks = new List<Stack>();
            WarehouseStacks.Add(new Stack(1, "Z,N"));
            WarehouseStacks.Add(new Stack(2, "M,C,D"));
            WarehouseStacks.Add(new Stack(3, "P"));
        }
        private static void MoveCrates (int qty, int stackFrom, int stackTo)
        {
            for(int moves = 0; moves < qty; moves++)
            {
                var stackOrigin = WarehouseStacks.FirstOrDefault(x => x.StackId == stackFrom);
                var stackDestination = WarehouseStacks.FirstOrDefault(x => x.StackId == stackTo);

                if (stackOrigin == null || stackDestination == null)
                {
                    Console.WriteLine($"Unable to move from {stackFrom} to {stackTo}! One or both do not exist");
                }

                var crateGrabed = CraneGrab(stackOrigin);
                if (crateGrabed != default)
                {
                    CraneDrop(crateGrabed, stackDestination);
                }
                else
                {
                    Console.WriteLine($"There are no more crates to grab from stack {stackFrom}, Moves Completed {moves-1}");
                }
            }
        }
        private static bool CraneDrop(string crateGrabed, Stack stackDestination) => stackDestination.AddCrate(crateGrabed);
        private static string CraneGrab(Stack stackOrigin) => stackOrigin.RemoveCrate();

    }

    //Typically would put this in a Seperate File
    internal class Stack
    {
        internal int StackId;
        private IList<string> _crates;
        internal Stack(int stackId,string initialCrates)
        {
            this.StackId = stackId;

            if (initialCrates.Any())
            {
                _crates = new List<string>();
                var crates = initialCrates.Split(',');
                foreach (var crate in crates)
                {
                    _crates.Add(crate);
                }
            }
        }
        internal bool HasCrates { get { return _crates.Any(); } set { } }
        internal string RemoveCrate()
        {
            if (!_crates.Any()) return default;

            var lastCrate = _crates.Last().ToString();
            _crates.Remove(lastCrate);

            return lastCrate;
        }
        internal bool AddCrate(string ID)
        {
            if (!_crates.Contains(ID))
            {
                _crates.Add(ID);
                return true;
            }
            return false;
        }
        internal string PrintTopCrate() => $"Stack {StackId} {(_crates.LastOrDefault() == default ? "Has No Crates" : $"Top Crate is [{_crates.Last()}")}]";
        internal string PrintAllCrates() =>$"Stack {StackId} {(_crates.LastOrDefault() == default ? "Has No Crates" : $"[{String.Join("],[", _crates)}]")}";
    }
}
