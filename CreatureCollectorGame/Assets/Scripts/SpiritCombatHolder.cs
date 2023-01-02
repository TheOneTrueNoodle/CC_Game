using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace creature
{
    public class SpiritCombatHolder : MonoBehaviour
    {
        public Transform parentOverride;

        public GameObject currentSpiritModel;

        public void UnloadSpirit()
        {
            if(currentSpiritModel != null)
            {
                currentSpiritModel.SetActive(false);
            }
        }
        public void UnloadSpiritAndDestroy()
        {
            if(currentSpiritModel != null)
            {
                Destroy(currentSpiritModel);
            }
        }

        public void LoadSpiritModel(SummonedSpirit summonedSpirit)
        {
            UnloadSpiritAndDestroy();

            if (summonedSpirit == null)
            {
                UnloadSpirit();
                return;
            }

            GameObject model = Instantiate(summonedSpirit.BaseCreature.modelPrefab) as GameObject;
            if(model != null)
            {
                if(parentOverride != null)
                {
                    model.transform.parent = parentOverride;
                }
                else
                {
                    model.transform.parent = transform;
                }

                model.transform.localPosition = Vector3.zero;
                model.transform.localRotation = Quaternion.identity;
                model.transform.localScale = Vector3.one;
            }

            currentSpiritModel = model;
        }
    }
}