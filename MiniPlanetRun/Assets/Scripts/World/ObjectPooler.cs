// <copyright file="ObjectPooler.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace MiniPlanetRun.World
{
    using UnityEngine;

    public class ObjectPooler : MonoBehaviour
    {
        private Transform objectPoolTransform;

        /// <summary>
        /// Spawn an object from a prefab, the ObjectPooler will either instantiate a new object or reuse an old one
        /// </summary>
        /// <param name="prefabPath">The path where the prefab is stored, the prefab MUST be stored in the Resources folder
        ///  and the string must conform to this example: "Prefabs/Enemies/"</param>
        /// <param name="prefabName">The name of the prefab (filename without extension)</param>
        /// <param name="parent">The parent of the new GameObject</param>
        /// <param name="worldPosition">Where in world space the object will be spawned</param>
        /// <returns>The spawned <see cref="GameObject"/></returns>
        public GameObject Spawn(string prefabPath, string prefabName, Transform parent, Vector3 worldPosition)
        {
            var poolGameObject = objectPoolTransform.Find(prefabName + "s");
            GameObject g;

            if (poolGameObject != null && poolGameObject.childCount > 0)
            {
                g = poolGameObject.GetChild(0).gameObject;
                g.transform.position = worldPosition;
            }
            else
            {
                g = Instantiate(Resources.Load(prefabPath + prefabName), worldPosition, Quaternion.identity) as GameObject;

                if (g != null)
                {
                    g.name = prefabName;
                }
            }

            if (g != null)
            {
                var rb = g.GetComponent<Rigidbody>();
                var rb2D = g.GetComponent<Rigidbody2D>();
                if (rb2D != null)
                {
                    rb2D.WakeUp();
                }
                else if (rb != null)
                {
                    rb.WakeUp();
                }

                g.transform.SetParent(parent);
                g.SetActive(true);
                return g;
            }

#if UNITY_EDITOR
            Debug.LogError("Spawn failed!");
#endif
            return null;
        }

        /// <summary>
        /// Puts the GameObject in the object pool
        /// </summary>
        /// <param name="g">The GameObject you want to recycle</param>
        public void Recycle(GameObject g)
        {
            if (g == null)
            {
#if UNITY_EDITOR
                Debug.LogError("The object you want to recycle is null.");
#endif
                return;
            }

            var rb = g.GetComponent<Rigidbody>();
            var rb2D = g.GetComponent<Rigidbody2D>();
            if (rb2D != null)
            {
                rb2D.Sleep();
            }
            else if (rb != null)
            {
                rb.Sleep();
            }

            if (objectPoolTransform == null)
            {
                Debug.Log("Object pool has been destroyed");
                return;
            }

            var gameObjectTypePool = objectPoolTransform.Find(g.name + "s");
            var t = g.transform;

            g.SetActive(false);

            if (gameObjectTypePool != null)
            {
                t.SetParent(gameObjectTypePool);
                t.localPosition = Vector3.zero;
            }
            else
            {
                var newGameObject = new GameObject { name = g.name + "s" };
                newGameObject.transform.SetParent(objectPoolTransform);
                newGameObject.transform.localPosition = Vector3.zero;

                gameObjectTypePool = newGameObject.transform;
                t.SetParent(gameObjectTypePool);
                t.localPosition = Vector3.zero;
            }
        }

        private void Awake()
        {
            objectPoolTransform = new GameObject("ObjectPools").transform;
        }
    }
}
