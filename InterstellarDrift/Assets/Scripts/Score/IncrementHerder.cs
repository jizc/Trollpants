// <copyright file="IncrementHerder.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace InterstellarDrift
{
    using DG.Tweening;
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    ///  Duplicates a base increment-textobject, sets the increment-amount in the text-field and sets up the tweening.
    /// </summary>
    public class IncrementHerder : MonoBehaviour
    {
        [SerializeField] private float _animationDuration = .5f;

        private GameObject incrementTextObject;
        private Transform currentActivePopup;

        public void Init()
        {
            // Get the base text-box that has been defined as a child.
            incrementTextObject = transform.GetChild(0).gameObject;
        }

        public void Increment(int amount)
        {
            Popup(amount, "+");
        }

        public void Decrement(int amount)
        {
            Popup(amount, "-");
        }

        private void Popup(int amount, string prefix)
        {
            var obj = Instantiate(incrementTextObject);

            if (currentActivePopup)
            {
                obj.transform.localPosition = currentActivePopup.localPosition;
            }

            var yOffset = obj.GetComponent<RectTransform>().rect.height;
            obj.transform.localPosition -= new Vector3(0, yOffset, 0);
            currentActivePopup = obj.transform;

            obj.transform.SetParent(transform, false);

            obj.GetComponent<Text>().text = prefix + amount;
            obj.SetActive(true);

            var seq1 = DOTween.Sequence();

            seq1.AppendInterval(_animationDuration);
            seq1.Append(
                obj.transform.DOLocalMove(
                new Vector3(obj.transform.localPosition.x, transform.parent.localPosition.y - 50, obj.transform.localPosition.z),
                _animationDuration)
                .OnComplete(() => Destroy(obj)));
        }
    }
}
