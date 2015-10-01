using System;
using System.Text;
using System.IO;

namespace TrekBattle
{
    class Shield
    {
        private int shieldStrength;
        private int regenRate;
        private int shieldDamage;
        private bool shieldHit;

        private void initShield()
        {
            shieldStrength = regenRate = -1;
            shieldDamage = 0;
            shieldHit = false;
        }

        public Shield()
        {
            initShield();
        }

        public int absorbDamage(int damage)
        {
            // the shield absorbs damage. if it can't absorb all of the
            // damage then return the remainder, otheerwise return 0

            int shieldRemaining = shieldStrength - shieldDamage;

            shieldHit = true;
            if (shieldRemaining >= damage)
            {
                shieldDamage += damage;
                return 0;
            }
            else
            {
                shieldDamage = shieldStrength;
                return (damage - shieldRemaining);
            }
        }

        public void regenShield()
        {
            if (shieldHit == false && shieldDamage > 0)
            {
                shieldDamage -= regenRate;
                if (shieldDamage < 0) shieldDamage = 0;
            }
            shieldHit = false;
        }

        public void readShield(StreamReader fin)
        {
            initShield();

            string shield = fin.ReadLine();

            // read in shield strength
            bool result = (shield != null && Int32.TryParse(shield, out shieldStrength));
            if (result == false || shieldStrength < 1)
                throw new Exception("Invalid shield strength");

            // read in regen rate
            shield = fin.ReadLine();
            result = (shield != null && Int32.TryParse(shield, out regenRate));
            if (result == false || regenRate < 1 || regenRate > shieldStrength)
                throw new Exception("Invalid regen rate");
        }
    }
}
