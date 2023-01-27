using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace creature
{
    [System.Serializable]
    public class ActiveCreature
    {
        public Creature BaseCreature;

        public string nickname;

        public int Level;

        public int HPBonus;
        public int speedBonus;
        public int dodgeCountBonus;

        public ActiveCreature(Creature baseCreature, int cLevel)
        {
            BaseCreature = baseCreature;
            Level = cLevel;
        }

        public int HP
        {
            get { return Mathf.FloorToInt((BaseCreature.BaseHP * Level) / 100f) + 10 + HPBonus; }
        }
        public int Speed
        {
            get { return Mathf.FloorToInt((BaseCreature.BaseSpeed * Level) / 100f) + 5 + speedBonus; }
        }
    }
}
