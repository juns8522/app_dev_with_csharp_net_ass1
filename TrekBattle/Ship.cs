using System;
using System.Text;
using System.IO;

namespace TrekBattle
{
    class Ship
    {
        private string shipClass;
        private Random rand;

        // combat factors
        private Hull shipsHull;
        private Weapons shipsWeapons;
        private Shield shipShields;

        private void initShip()
        {
            shipClass = "";

            shipsHull = new Hull();
            shipsWeapons = new Weapons(rand);
            shipShields = new Shield();
        }

        public Ship(Random r)
        {
            rand = r;
            initShip();
        }

        public string ShipClass
        {
            get { return shipClass; }
        }

        public void readShip(StreamReader fin)
        {
            initShip();
            shipClass = fin.ReadLine();
            if (shipClass == null || shipClass.Length == 0)
            {
                throw new Exception("Ship class name missing");
            }

            try
            {
                shipShields.readShield(fin);
                shipsHull.readHull(fin);
                shipsWeapons.readWeapon(fin);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message + " in ship class " + shipClass);
            }
        }

        public bool unassignedShip()
        {
            // we have created a ship object but not assigned it any stats
            return shipsHull.unassigned();
        }

        public int weaponDamage()
        {
            return shipsWeapons.getDamage();
        }

        public void takeDamage(int damage)
        {
            // the ships hull takes damage minus any absorbed by the shields
            shipsHull.takeDamage(shipShields.absorbDamage(damage));
        }

        public bool shipDestroyed()
        {
            return (shipsHull.destroyed());
        }

        public void regenShield()
        {
            shipShields.regenShield();
        }

        public string damageRating()
        {
            return shipsHull.damageRating();
        }
    }
}
