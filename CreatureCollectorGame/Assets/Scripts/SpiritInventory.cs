using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace creature
{
    public class SpiritInventory : MonoBehaviour
    {
        SpiritCombatManager spiritCombatManager;

        public SummonedSpirit activeSpiritOne;
        public SummonedSpirit activeSpiritTwo;
        public SummonedSpirit activeSpiritThree;

        private void Awake()
        {
            spiritCombatManager = GetComponentInChildren<SpiritCombatManager>();
        }

        private void Start()
        {
            spiritCombatManager.LoadSpirit(activeSpiritOne);
        }
    }
}
