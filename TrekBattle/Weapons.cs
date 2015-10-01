using System;
using System.Text;
using System.IO;

namespace TrekBattle
{
    class Weapons
    { 
        private int weaponBase;
        private int weaponRand;
        private Random rand;

        private void initWeapons()
        {
            weaponBase = weaponRand = -1;
        }

        public Weapons(Random r)
        {
            rand = r;
            initWeapons();
        }

        public int getDamage()
        {
            return weaponBase + rand.Next(weaponRand);
        }

        public void readWeapon(StreamReader fin)
        {
            initWeapons();

            string weapon = fin.ReadLine();

            // read in weapon base
            bool result = (weapon != null && Int32.TryParse(weapon, out weaponBase));
            if (result == false || weaponBase < 1)
                throw new Exception("Invalid weapon base");

            // read in weapon rand
            weapon = fin.ReadLine();
            result = (weapon != null && Int32.TryParse(weapon, out weaponRand));
            if (result == false || weaponRand < 1)
                throw new Exception("Invalid weapon random");
        }
    }
}
