// <copyright file="PotionEffects.cs" company="Jan Ivar Z. Carlsen, Sindri Jóelsson">
// Copyright (c) 2016 Jan Ivar Z. Carlsen, Sindri Jóelsson. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace WitchWing.GUI
{
    using System.Collections;
    using DG.Tweening;
    using Environment;
    using Player;
    using UnityEngine;
    using UnityEngine.UI;

    public class PotionEffects : MonoBehaviour
    {
        private const float wisdomBlinkDuration = 1f;
        private const float potionDuration = 10;

        [SerializeField] private WorldGenerator worldGenerator;
        [SerializeField] private Button wisdomButton;
        [SerializeField] private Image wisdomCooldownImage;
        [SerializeField] private Color32 wisdomColor;
        [SerializeField] private Button midasButton;
        [SerializeField] private Image midasCooldownImage;
        [SerializeField] private Image manaFill;

        public void ActivateWisdomEffect()
        {
            StartCoroutine(ActivateWisdom());
        }

        public void ActivateMidasEffect()
        {
            StartCoroutine(ActivateMidas());
        }

        public void RefreshButtonInteractable()
        {
            wisdomButton.interactable = Player.State.WisdomPotions > 0;
            midasButton.interactable = Player.State.MidasPotions > 0;
        }

        private IEnumerator ActivateWisdom()
        {
            Player.State.IsWisdomActive = true;
            wisdomButton.interactable = false;
            wisdomCooldownImage.DOFillAmount(0f, potionDuration).OnComplete(() =>
            {
                if (!Player.State.IsDead)
                {
                    wisdomButton.interactable = true;
                }
            });
            Player.RefillMana();
            ToggleManaFillColor();

            StartCoroutine(CycleManaColors(potionDuration - wisdomBlinkDuration));
            Invoke("WisdomBlink", potionDuration - wisdomBlinkDuration);

            yield return new WaitForSeconds(potionDuration);

            ToggleManaFillColor();
            Player.State.IsWisdomActive = false;
        }

        private IEnumerator ActivateMidas()
        {
            worldGenerator.MidasEffect();
            Player.State.IsMidasActive = true;
            midasButton.interactable = false;
            midasCooldownImage.DOFillAmount(0f, potionDuration).OnComplete(() =>
            {
                if (!Player.State.IsDead)
                {
                    midasButton.interactable = true;
                }
            });

            yield return new WaitForSeconds(potionDuration);

            Player.State.IsMidasActive = false;
        }

        private IEnumerator CycleManaColors(float seconds)
        {
            var cachedColor = manaFill.color;
            var h = 0f;
            var elapsedTime = 0f;

            while (elapsedTime < seconds)
            {
                elapsedTime += Time.deltaTime;
                if (Equals(h, 1f))
                {
                    h = 0f;
                }

                h = Mathf.MoveTowards(h, 1f, Time.deltaTime);
                manaFill.color = Color.HSVToRGB(h, 1f, 1f);
                yield return null;
            }

            manaFill.color = cachedColor;
        }

        private void WisdomBlink()
        {
            ToggleManaFillColor();
            for (var i = 1; i < 10; i++)
            {
                Invoke("ToggleManaFillColor", wisdomBlinkDuration * 0.1f * i);
            }
        }

        private void ToggleManaFillColor()
        {
            manaFill.color = manaFill.color == wisdomColor ? ManaPool.ManaFillDefault : wisdomColor;
        }
    }
}
