using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace creature
{
    [CreateAssetMenu(menuName = "Creatures/Base Creature")]
    public class Creature : ScriptableObject
    {
        [Header("Creature Information")]
        public Sprite creatureIcon;
        public string creatureName;
        public GameObject modelPrefab;

        [Header("Base Stats")]
        public int baseHP;
        public int baseSpeed;
        public int baseDodgeCount;

        //[Header("Creature Possible Moves")]
        //public List<> possibleMoves;
    }
}