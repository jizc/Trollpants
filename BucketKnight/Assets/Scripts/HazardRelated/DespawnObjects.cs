// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DespawnObjects.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BucketKnight
{
    using UnityEngine;

    public class DespawnObjects : MonoBehaviour
    {
        private void OnEnable()
        {
            Events.instance.AddListener<GameRestarted>(OnGameRestarted);
        }

        private void OnTriggerEnter(Collider col)
        {
            DespawnObject(col.gameObject);
        }

        private void DespawnObject(GameObject objectToDespawn)
        {
            var movingObject = objectToDespawn.GetComponent<MovingObject>();
            if (movingObject != null)
            {
                movingObject.Despawn();
            }
        }

        public void ResetAllObjects()
        {
            var objectList = FindObjectsOfType<MovingObject>();
            foreach (var movingObject in objectList)
            {
                movingObject.Reset();
            }
        }

        #region Event handlers

        private void OnGameRestarted(GameRestarted gameRestartedEvent)
        {
            ResetAllObjects();
        }

        #endregion

        private void OnDisable()
        {
            Events.instance.RemoveListener<GameRestarted>(OnGameRestarted);
        }
    }
}
