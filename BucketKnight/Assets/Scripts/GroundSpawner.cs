// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GroundSpawner.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BucketKnight
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    public class GroundSpawner : MonoBehaviour
    {
        public List<GameObject> groundVariaties;

        private int _numberOfGrounds;
        private float _xPosition;
        private float _yPosition;

        private void OnEnable()
        {
            Events.instance.AddListener<GroundRemoved>(GroundRemoved);
        }

        private void Start()
        {
            _xPosition = 4.5f;
            _yPosition = 0.5f;
            _numberOfGrounds = 0;
        }

        private void Update()
        {
            if (_numberOfGrounds < 2)
            {
                var elementNumber = Random.Range(0, groundVariaties.Count);
                var element = groundVariaties.ElementAt(elementNumber);
                Instantiate(element, new Vector3(_xPosition, _yPosition, _numberOfGrounds * 34), Quaternion.identity);
                _numberOfGrounds++;
            }
        }

        private void GroundRemoved(GroundRemoved groundEvent)
        {
            _numberOfGrounds--;
        }

        private void OnDisable()
        {
            Events.instance.RemoveListener<GroundRemoved>(GroundRemoved);
        }
    }
}
