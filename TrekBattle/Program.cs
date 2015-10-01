using System;
using System.Text;
using System.Threading.Tasks;

namespace TrekBattle
{
    class Program
    {
        static void Main(string[] args)
        {
            Fleet fleet1 = null;
            Fleet fleet2 = null;
            int round = 0;

            Console.WriteLine("TrekBattle by Gordon Lingard");

            try
            {
                processAruments(args, out fleet1, out fleet2);
                runBattle(fleet1, fleet2, ref round);
                printBattleReport(fleet1, fleet2, round);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Program failed with following error");
                Console.WriteLine(ex.Message);
            }
        }

        static void processAruments(string[] args, out Fleet fleet1, out Fleet fleet2)
        {
            if (args.Length != 3) throw new Exception("Invalid number of command line arguments entered");

            // read random number generator seed and then create it - first argument
            int seed;
            bool result = Int32.TryParse(args[0], out seed);
            if (result == false || seed < 0) throw new Exception("Invalid seed value entered");
            Random rand = new Random(seed);

            // create and read fleet 1 - second argument
            fleet1 = new Fleet(rand);
            fleet1.readFleet(args[1]);

            // create and read fleet 2 - third argument
            fleet2 = new Fleet(rand);
            fleet2.readFleet(args[2]);
        }

        static void runBattle(Fleet fleet1, Fleet fleet2, ref int round)
        {
            while (!fleet1.fleetDestroyed() && !fleet2.fleetDestroyed())
            {
                round++;

                // run fleet battles
                fleet1.attackFleet(fleet2);
                fleet2.attackFleet(fleet1);

                // finalise battle round
                fleet1.finaliseBattleRound(round);
                fleet2.finaliseBattleRound(round);
            }
        }

        static void printBattleReport(Fleet fleet1, Fleet fleet2, int round)
        {
            if (fleet1.fleetDestroyed() && fleet2.fleetDestroyed())
            {
                Console.WriteLine("\nAfter round " + round + " the battle has been a draw with both sides destroyed");
            }
            else if (fleet1.fleetDestroyed())
            {
                Console.WriteLine("\nAfter round " + round + " the " + fleet2.FleetName + " fleet won");
                Console.WriteLine("  " + fleet1.ShipsDestroyed + " enemy ships destroyed");
                fleet2.printDamageReport();
            }
            else if (fleet2.fleetDestroyed())
            {
                Console.WriteLine("\nAfter round " + round + " the " + fleet1.FleetName + " fleet won");
                Console.WriteLine("  " + fleet2.ShipsDestroyed + " enemy ships destroyed");
                fleet1.printDamageReport();
            }
            else
            {
                Console.WriteLine("\nERROR BUG - battle ended but neither fleet is destroyed");
            }
        }
    }
}
