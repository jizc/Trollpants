// <copyright file="PopUpSpawner.cs" company="Jan Ivar Z. Carlsen, Sindri Jóelsson">
// Copyright (c) 2016 Jan Ivar Z. Carlsen, Sindri Jóelsson. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace WitchWing.GUI
{
    using System.Collections;
    using UnityEngine;
    using UnityEngine.UI;

    public class PopUpSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject coinPickup;

        private RectTransform parentTransform;

        public void SpawnCoin(int value)
        {
            SpawnPopUp(coinPickup, value);
        }

        private void Awake()
        {
            parentTransform = (RectTransform)transform;
        }

        private void SpawnPopUp(GameObject prefab, int value)
        {
            var poolGameObject = parentTransform.Find(prefab.name + "PopUps");
            GameObject popUpGameObject;

            if (poolGameObject != null && poolGameObject.childCount > 0)
            {
                popUpGameObject = poolGameObject.GetChild(0).gameObject;
            }
            else
            {
                popUpGameObject = Instantiate(prefab);
            }

            if (popUpGameObject != null)
            {
                var rectTransform = popUpGameObject.GetComponent<RectTransform>();
                rectTransform.SetParent(parentTransform);
                rectTransform.anchoredPosition = Vector2.zero;
                rectTransform.localScale = new Vector3(1f, 1f, 1f);
                rectTransform.GetChild(0).Find("#").GetComponent<Text>().text = value.ToString();
                StartCoroutine(PopUpAndRecycle(popUpGameObject, prefab.name, 100f, 600f));
                return;
            }

            Debug.LogError("Spawn failed!");
        }

        private IEnumerator PopUpAndRecycle(GameObject go, string prefabName, float distance, float maxDelta)
        {
            var rect = go.GetComponent<RectTransform>();
            var canvasGroup = go.GetComponent<CanvasGroup>();
            var distanceTraveled = 0f;
            var targetPos = rect.anchoredPosition.y + distance;

            go.SetActive(true);
            while (distanceTraveled < distance)
            {
                canvasGroup.alpha = Mathf.Clamp01(distanceTraveled / (distance * 0.75f));

                var currentY = rect.anchoredPosition.y;
                distanceTraveled = Mathf.MoveTowards(currentY, targetPos, maxDelta * Time.deltaTime);
                rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, distanceTraveled);

                yield return null;
            }

            yield return new WaitForSeconds(0.5f);

            canvasGroup.alpha = 0f;
            Recycle(go, prefabName);
        }

        private void Recycle(GameObject go, string prefabName)
        {
            if (go == null)
            {
                Debug.LogError("The PopUp you want to recycle is null.");
                return;
            }

            var gameObjectTypePool = parentTransform.Find(prefabName + "PopUps");
            var rectTransform = go.GetComponent<RectTransform>();

            go.SetActive(false);

            if (gameObjectTypePool != null)
            {
                var poolRect = gameObjectTypePool.GetComponent<RectTransform>();
                rectTransform.SetParent(poolRect);
            }
            else
            {
                var newGameObject = new GameObject(prefabName + "PopUps", typeof(RectTransform));
                var newRect = newGameObject.GetComponent<RectTransform>();
                newRect.SetParent(parentTransform);
                newRect.anchoredPosition = Vector2.zero;
                rectTransform.SetParent(newRect);
            }

            rectTransform.anchoredPosition = Vector2.zero;
        }
    }
}
