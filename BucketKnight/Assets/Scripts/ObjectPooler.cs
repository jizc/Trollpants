// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObjectPooler.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BucketKnight
{
    using UnityEngine;

    public class ObjectPooler : MonoBehaviour
    {
        #region Fields & properties

        public GameObject[] objectPool;

        private Transform _objectPoolTransform;

        #endregion /Fields & properties

        #region Public methods

        /// <summary>
        /// Spawn an object from a prefab, the ObjectPooler will either instantiate a new object or reuse an old one
        /// </summary>
        /// <param name="prefabName">The name of the prefab (filename without extension)</param>
        /// <param name="parent">The parent of the new GameObject</param>
        /// <param name="worldPosition">Where in world space the object will be spawned</param>
        /// <returns>The spawned <see cref="GameObject"/></returns>
        public GameObject Spawn(string prefabName, Transform parent, Vector3 worldPosition, Quaternion rotation)
        {
            var poolGameObject = _objectPoolTransform.Find(prefabName);
            GameObject g = null;

            if (poolGameObject != null)
            {
                g = poolGameObject.gameObject;
                g.transform.position = worldPosition;
                g.transform.rotation = rotation;
            }
            else
            {
                foreach (var prefab in objectPool)
                {
                    if (prefab.name == prefabName)
                    {
                        g = Instantiate(prefab, worldPosition, rotation) as GameObject;
                        break;
                    }
                }
            }

            if (g != null)
            {
                g.name = prefabName;
                var rigidBody = g.GetComponent<Rigidbody>();
                rigidBody.velocity = Vector3.zero;
                rigidBody.angularVelocity = Vector3.zero;

                g.transform.SetParent(parent);
                g.SetActive(true);
                return g;
            }

#if DEBUG
            Debug.LogError("Spawn failed!");
#endif
            return null;
        }

        #endregion /Public methods

        #region Unity methods

        private void OnSpeedChanged(SpeedChanged speedChangedEvent)
        {
            foreach (Transform movingObject in transform)
            {
                var m = movingObject.GetComponent<MovingObject>();
                if (m != null)
                {
                    m.Speed = -speedChangedEvent.newPlayerSpeed;
                }
            }
        }

        private void Awake()
        {
            _objectPoolTransform = gameObject.transform;
            _objectPoolTransform.name = "ObjectPooler";
        }

        private void OnEnable()
        {
            Events.instance.AddListener<SpeedChanged>(OnSpeedChanged);
        }

        private void OnDisable()
        {
            Events.instance.RemoveListener<SpeedChanged>(OnSpeedChanged);
        }
        #endregion /Unity methods
    }
}
