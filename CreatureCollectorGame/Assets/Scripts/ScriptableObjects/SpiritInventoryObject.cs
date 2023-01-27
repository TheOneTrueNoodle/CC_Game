using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace creature
{
    [CreateAssetMenu(fileName = "New Spirit Inventory", menuName = "Creatures/Spirit Inventory")]
    public class SpiritInventoryObject : ScriptableObject
    {
        public List<ActiveCreature> Container = new List<ActiveCreature>();

        public void AddSpirit(Creature spiritBase, int cLevel)
        {
            ActiveCreature newSpirit = new ActiveCreature(spiritBase, cLevel);
            Container.Add(newSpirit);
        }
    }
}
