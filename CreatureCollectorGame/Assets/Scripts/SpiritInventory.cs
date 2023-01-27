using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace creature
{
    public class SpiritInventory : MonoBehaviour
    {
        SpiritCombatManager spiritCombatManager;
        public SpiritInventoryObject spiritInventory;

        public ActiveCreature activeSpiritOne;
        public ActiveCreature activeSpiritTwo;
        public ActiveCreature activeSpiritThree;

        private void Awake()
        {
            spiritCombatManager = GetComponentInChildren<SpiritCombatManager>();

            if(spiritInventory.Container.Count != 0)
            {
                activeSpiritOne = spiritInventory.Container[0];
            }
        }

        private void Start()
        {
            spiritCombatManager.LoadSpirit(activeSpiritOne);
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                activeSpiritOne.HPBonus++;
                for(int i = 0; i < spiritInventory.Container.Count; i++)
                {
                    if(spiritInventory.Container[i].BaseCreature == activeSpiritOne.BaseCreature)
                    {
                        spiritInventory.Container[i] = activeSpiritOne;
                    }
                }
            }
        }
    }
}
