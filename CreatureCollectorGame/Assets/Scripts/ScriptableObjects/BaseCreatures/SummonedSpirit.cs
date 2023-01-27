using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace creature
{
    [CreateAssetMenu(menuName = "Creatures/Summoned Creatures")]
    public class SummonedSpirit : ScriptableObject
    {
        public Creature BaseCreature;

        public string nickname;

        public int Level;

        public int HPBonus;
        public int speedBonus;
        public int dodgeCountBonus;
    }
}
