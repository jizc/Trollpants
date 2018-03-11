// <copyright file="UpgradeButton.cs" company="Jan Ivar Z. Carlsen, Sindri Jóelsson">
// Copyright (c) 2016 Jan Ivar Z. Carlsen, Sindri Jóelsson. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace WitchWing.GUI
{
    using System;
    using Data;
    using Environment;
    using Player;
    using UnityEngine;
    using UnityEngine.UI;

    [RequireComponent(typeof(Button))]
    public class UpgradeButton : MonoBehaviour
    {
        [SerializeField] private ButtonType buttonType;
        [SerializeField] private Upgrade upgradeType;

        private enum ButtonType
        {
            Upgrade,
            Downgrade
        }

        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            switch (buttonType)
            {
                case ButtonType.Upgrade:
                    Merchant.Instance.UpgradeSkill(upgradeType);
                    break;
                case ButtonType.Downgrade:
                    AudioClipPlayer.Instance.PlayClick();
                    Merchant.Instance.DowngradeSkill(upgradeType);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
