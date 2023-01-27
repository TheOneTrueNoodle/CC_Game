using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace creature
{
    [CreateAssetMenu(menuName = "Creatures/Base Creature")]
    public class Creature : ScriptableObject
    {
        [Header("Creature Information")]
        [SerializeField] Sprite creatureIcon;
        public GameObject modelPrefab;
        [SerializeField] string creatureName;
        [TextArea(3,8)]
        [SerializeField] string description;

        [Header("Base Stats")]
        [SerializeField] int baseHP;
        [SerializeField] int baseSpeed;
        [SerializeField] int baseDodgeCount;

        //[Header("Creature Possible Moves")]
        //public List<> possibleMoves;

        public string Name
        {
            get { return creatureName; }
        }
        public string Description
        {
            get { return description; }
        }
        public int BaseHP
        {
            get { return baseHP; }
        }
        public int BaseSpeed
        {
            get { return baseSpeed; }
        }
        public int BaseDodgeCount
        {
            get { return baseDodgeCount; }
        }

    }
}