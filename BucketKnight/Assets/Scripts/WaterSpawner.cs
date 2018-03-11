// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WaterSpawner.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BucketKnight
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    public class WaterSpawner : MonoBehaviour
    {
        public List<GameObject> varieties;
        private int _numberOfActive;

        private float _xPosition;
        private float _yPosition;
        public Transform spawnPosition;

        private void OnEnable()
        {
            Events.instance.AddListener<WaterRemoved>(ElementRemoved);
        }

        private void Start()
        {
            _numberOfActive = 0;
        }

        private void Update()
        {
            if (_numberOfActive < 2)
            {
                var elementNumber = Random.Range(0, varieties.Count);
                var element = varieties.ElementAt(elementNumber);
                Instantiate(element,
                    new Vector3(spawnPosition.position.x, spawnPosition.position.y,
                        spawnPosition.position.z - 37.45f * _numberOfActive), Quaternion.identity);
                _numberOfActive++;
            }
        }

        private void ElementRemoved(WaterRemoved elementEvent)
        {
            _numberOfActive--;
        }

        private void OnDisable()
        {
            Events.instance.RemoveListener<WaterRemoved>(ElementRemoved);
        }
    }
}
