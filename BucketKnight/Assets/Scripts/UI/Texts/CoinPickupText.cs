// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CoinPickupText.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BucketKnight
{
    using System.Collections;
    using UnityEngine;
    using UnityEngine.UI;

    public class CoinPickupText : MonoBehaviour
    {
        private Text _text;

        private void OnEnable()
        {
            Events.instance.AddListener<CoinsPickedUp>(OnCoinsPickedUp);
            _text = GetComponent<Text>();
            _text.enabled = false;
        }

        private void OnCoinsPickedUp(CoinsPickedUp scoreChangedEvent)
        {
            _text.text = "+ " + scoreChangedEvent.coinsPickedUp;
            _text.enabled = true;
            StartCoroutine(DisableAfterSeconds(0.5f));
        }

        private IEnumerator DisableAfterSeconds(float time)
        {
            for (var timer = time; timer >= 0; timer -= Time.deltaTime)
            {
                yield return 0;
            }
            _text.enabled = false;
        }

        private void OnDisable()
        {
            Events.instance.RemoveListener<CoinsPickedUp>(OnCoinsPickedUp);
        }
    }
}
