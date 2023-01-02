using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using player;

namespace creature
{
    public class SpiritCombatManager : MonoBehaviour
    {
        SpiritCombatHolder spiritCombatHolder;
        AnimatorHandler animatorHandler;

        private void Awake()
        {
            spiritCombatHolder = GetComponentInChildren<SpiritCombatHolder>();
            animatorHandler = GetComponent<AnimatorHandler>();
        }
        public void LoadSpirit(SummonedSpirit spirit)
        {
            spiritCombatHolder.LoadSpiritModel(spirit);
            animatorHandler.Initialize();
        }
    }
}