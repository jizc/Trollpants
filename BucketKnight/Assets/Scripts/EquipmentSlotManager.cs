// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EquipmentSlotManager.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BucketKnight
{
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    public class EquipmentSlotManager : MonoBehaviour
    {
        public PowerupAssetList powerupAssetManager;
        public PowerupSlot slot1;
        public PowerupSlot slot2;
        private Image _background;
        private List<GameObject> _childObjects = new List<GameObject>();

        private void OnEnable()
        {
            Events.instance.AddListener<PowerupSlotOneEquipped>(OnPowerupSlotOneEquipped);
            Events.instance.AddListener<PowerupSlotTwoEquipped>(OnPowerupSlotTwoEquipped);

            _background = GetComponent<Image>();

            Restart();
            SetEquipSlotVisibility();
        }

        private void Awake()
        {
            foreach (Transform childTransform in transform)
            {
                _childObjects.Add(childTransform.gameObject);
            }
        }

        private void Restart()
        {
            SetPowerupOne(SavedData.PowerupInSlotOne);
            SetPowerupTwo(SavedData.PowerupInSlotTwo);
        }

        private void SetPowerupOne(Enums.PowerupType powerup)
        {
            if (powerup == Enums.PowerupType.None)
            {
                slot1.RemovePowerup();
            }
            else
            {
                slot1.AddPowerup(powerupAssetManager.GetSprite(powerup));
            }
        }

        private void SetPowerupTwo(Enums.PowerupType powerup)
        {
            if (powerup == Enums.PowerupType.None)
            {
                slot2.RemovePowerup();
            }
            else
            {
                slot2.AddPowerup(powerupAssetManager.GetSprite(powerup));
            }
        }

        private void SetEquipSlotVisibility()
        {
            _background.enabled = SavedData.PowerupInSlotOne != Enums.PowerupType.None ||
                                 SavedData.PowerupInSlotTwo != Enums.PowerupType.None;
            ShowChildren(SavedData.PowerupInSlotOne != Enums.PowerupType.None ||
                              SavedData.PowerupInSlotTwo != Enums.PowerupType.None);

        }

        public void ShowChildren(bool show)
        {
            foreach (var childObject in _childObjects)
            {
                childObject.SetActive(show);
            }
        }

        public void ActivatePowerupSlotOne()
        {
            if (SavedData.PowerupInSlotOne != Enums.PowerupType.None)
            {
                if (SavedData.GameIsPaused)
                {
                    SellPowerupOne();
                }
                else
                {
                    ActivatePowerupOne();
                }
            }
        }

        public void ActivatePowerupSlotTwo()
        {
            if (SavedData.PowerupInSlotTwo != Enums.PowerupType.None)
            {
                if (SavedData.GameIsPaused)
                {
                    SellPowerupTwo();
                }
                else
                {
                    ActivatePowerupTwo();
                }
            }
        }

        private void SellPowerupOne()
        {
            Events.instance.Raise(new SellEquipmentOne());
            Events.instance.Raise(new PowerupSlotOneEquipped(Enums.PowerupType.None));
        }

        private void SellPowerupTwo()
        {
            Events.instance.Raise(new SellEquipmentTwo());
            Events.instance.Raise(new PowerupSlotTwoEquipped(Enums.PowerupType.None));
        }

        private void ActivatePowerupOne()
        {
            Events.instance.Raise(new PowerupPickup(SavedData.PowerupInSlotOne, Enums.PowerupActivationType.Equipment, 1));
        }

        private void ActivatePowerupTwo()
        {
            Events.instance.Raise(new PowerupPickup(SavedData.PowerupInSlotTwo, Enums.PowerupActivationType.Equipment, 2));
        }

        #region Event handlers

        private void OnPowerupSlotOneEquipped(PowerupSlotOneEquipped e)
        {
            SetPowerupOne(e.powerupType);
            SetEquipSlotVisibility();
        }

        private void OnPowerupSlotTwoEquipped(PowerupSlotTwoEquipped e)
        {
            SetPowerupTwo(e.powerupType);
            SetEquipSlotVisibility();
        }

        private void OnGameRestarted(GameRestarted e)
        {
            Restart();
        }

        #endregion

        private void OnDisable()
        {
            Events.instance.RemoveListener<PowerupSlotOneEquipped>(OnPowerupSlotOneEquipped);
            Events.instance.RemoveListener<PowerupSlotTwoEquipped>(OnPowerupSlotTwoEquipped);
        }
    }
}
