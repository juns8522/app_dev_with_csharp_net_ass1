using System;
using System.Text;
using System.IO;

namespace TrekBattle
{
    class Fleet
    {
        private string fleetName;
        private Ship[] ships;
        private int numShips;
        private int shipsLost;
        private Random rand;

        private void takeShipDamage(int damage)
        {
            // one of the fleet's ships has been hit.
            // randomly select one and apply damage

            int shipHit = rand.Next(numShips);

            ships[shipHit].takeDamage(damage);
        }

        private void removeShip(int i)
        {
            // move all remaining ships to left, filling up array space taken
            // up by destroyed ship
            // this makes sure the unassigned ship at the end is moved left as well
            // thus keeping the sentinal in place

            for (int j = i; j < numShips; j++)
            {
                ships[j] = ships[j + 1];
            }

            // bookkkeeping on ship numbers
            shipsLost++;
            numShips--;
        }

        private void initFleet()
        {
            fleetName = "";
            ships = null;
            shipsLost = numShips = 0;
        }

        public Fleet(Random r)
        {
            rand = r;
            initFleet();
        }

        private void readShips(StreamReader fin)
        {
            // read number of ships in fleet
            bool result = Int32.TryParse(fin.ReadLine(), out numShips);
            if (result == false || numShips < 1)
            {
                throw new Exception("Invalid number of ships");
            }

            // create required ships
            ships = new Ship[numShips + 1];

            // read in ships
            for (int i = 0; i < numShips; i++)
            {
                ships[i] = new Ship(rand);
                ships[i].readShip(fin);
            }

            // place unassigned ship at end of list of ships to act as sentinal
            ships[numShips] = new Ship(rand);
        }

        private bool endOfFleetFile(StreamReader fin)
        {
            string line;

            while (!fin.EndOfStream)
            {
                line = fin.ReadLine().Trim();
                // see if there is any text other than spaces left in file
                if (line.Length > 0) return false;
            }
            return true;
        }

        public void readFleet(string filename)
        {
            // open stream to fleet file
            if (!File.Exists(filename)) throw new Exception(filename + " file not found");
            StreamReader fin = new StreamReader(filename);
            if (fin == null) throw new Exception("Unable to open " + filename);

            // read fleet name
            try
            {
                initFleet();

                fleetName = fin.ReadLine();
                if (fleetName == null || fleetName.Length == 0)
                {
                    throw new Exception("Missing fleet name");
                }

                readShips(fin);

                if (!endOfFleetFile(fin))
                {
                    throw new Exception("More ships than stated");
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message + " in " + filename);
            }
            finally
            {
                fin.Close();
            }
        }

        public int ShipsDestroyed
        {
            get { return shipsLost; }
        }

        public string FleetName
        {
            get { return fleetName; }
        }

        public void attackFleet(Fleet fleet)
        {
            // go through each ship in the fleet, work out how much damage
            // it dishes out and then get the other fleet to apply it to
            // one of its ships

            int damage;

            for (int i = 0; i < numShips; i++)
            {
                damage = ships[i].weaponDamage();
                fleet.takeShipDamage(damage);
            }
        }

        public void finaliseBattleRound(int round)
        {
            // work out if any ships lost and if any can regenerate shields

            bool shipsLost = false;
            int i = 0;

            while (true)
            {
                // if we have dealt with all the ships in the fleet, end loop
                if (ships[i].unassignedShip()) break;

                if (ships[i].shipDestroyed())
                {
                    // the ship has been destroyed. print report
                    if (shipsLost == false)
                    {
                        Console.WriteLine("\nAfter round " + round + " the " + fleetName + " fleet has lost");
                        shipsLost = true;
                    }
                    Console.WriteLine("  " + ships[i].ShipClass + " destroyed");

                    // remove the ship from current fighting ships
                    removeShip(i);
                    // there will be a new ship at position i. no need to move i
                }
                else
                {
                    // the ship was not lost, see if it can regenerate its shield
                    ships[i].regenShield();
                    // move to next ship in fleet
                    i++;
                }
            }
        }

        public bool fleetDestroyed()
        {
            return numShips == 0;
        }

        public void printDamageReport()
        {
            Console.WriteLine("  " + shipsLost + " ships lost");
            Console.WriteLine("  " + numShips + " ships survived");

            for (int i = 0; i < numShips; i++)
            {
                Console.WriteLine("    " + ships[i].ShipClass + " - " + ships[i].damageRating());
            }
        }
    }
}
