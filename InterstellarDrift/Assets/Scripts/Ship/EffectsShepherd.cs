// <copyright file="EffectsShepherd.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace InterstellarDrift
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public enum Thruster
    {
        Left,
        Right,
        Bottom
    }

    /// <summary>
    /// Finds all particle systems in gameobjects with the 'Effect' layer, and sorts them into lists based on tags.
    /// Defines methods for activating thrusters at specific sides of the ship. Sides are set based on tags.
    /// </summary>
    public class EffectsShepherd : MonoBehaviour
    {
        private const int layer = 8; // Effect
        private static readonly Color s_boostColor = new Color32(255, 0, 145, 255);

        [SerializeField] private bool _initializeSelf;

        [SerializeField] private List<ParticleSystem> _leftParticles;
        [SerializeField] private List<ParticleSystem> _rightParticles;
        [SerializeField] private List<ParticleSystem> _bottomParticles;

        private bool isInitialized;

        private float thrusterParticleSizeCache;
        private Color thrusterStartColorCache;

        public bool IsBoostParticlesActive { get; private set; }

        public void Init()
        {
            _leftParticles = new List<ParticleSystem>();
            _rightParticles = new List<ParticleSystem>();
            _bottomParticles = new List<ParticleSystem>();

            // 3-deep hierarchy search of transform-children, adding particle systems to lists:
            for (var x = 0; x < transform.childCount; x++)
            {
                var firstChild = transform.GetChild(x);

                for (var y = 0; y < firstChild.childCount; y++)
                {
                    var secondChild = firstChild.GetChild(y);

                    for (var z = 0; z < secondChild.childCount; z++)
                    {
                        var thirdChild = secondChild.GetChild(z);

                        if (thirdChild.gameObject.layer == layer)
                        {
                            GetParticleSystem(thirdChild.gameObject);
                        }
                    }

                    if (secondChild.gameObject.layer == layer)
                    {
                        GetParticleSystem(secondChild.gameObject);
                    }
                }

                if (firstChild.gameObject.layer == layer)
                {
                    GetParticleSystem(firstChild.gameObject);
                }
            }

            // Assure that all particle systems are set to loop
            foreach (var partSys in _leftParticles)
            {
                var main = partSys.main;
                main.loop = true;
            }

            foreach (var partSys in _rightParticles)
            {
                var main = partSys.main;
                main.loop = true;
            }

            foreach (var partSys in _bottomParticles)
            {
                var main = partSys.main;
                main.loop = true;
            }

            isInitialized = true;
        }

        public void ActivateThruster(Thruster toActivate, float duration)
        {
            if (!isInitialized)
            {
                return;
            }

            var thruster = ThrusterToParticleSystem(toActivate);
            StartCoroutine(ActivateParticlesForDuration(thruster, duration));
        }

        public void ActivateThruster(Thruster toActivate)
        {
            if (!isInitialized)
            {
                return;
            }

            foreach (var partSys in ThrusterToParticleSystem(toActivate))
            {
                partSys.Play();
            }
        }

        public void DeactivateThruster(Thruster toDeactivate)
        {
            if (!isInitialized)
            {
                return;
            }

            foreach (var partSys in ThrusterToParticleSystem(toDeactivate))
            {
                partSys.Stop();
            }
        }

        public void DeactivateAllThrusters()
        {
            if (!isInitialized)
            {
                return;
            }

            foreach (var partSys in _leftParticles)
            {
                partSys.Stop();
            }

            foreach (var partSys in _rightParticles)
            {
                partSys.Stop();
            }

            foreach (var partSys in _bottomParticles)
            {
                partSys.Stop();
            }
        }

        public void DeactivateAllThrusters(Thruster exception)
        {
            if (!isInitialized)
            {
                return;
            }

            DeactivateAllThrusters();

            foreach (var partSys in ThrusterToParticleSystem(exception))
            {
                partSys.Play();
            }
        }

        public bool IsThrusterActive(Thruster thruster)
        {
            if (!isInitialized)
            {
                return false;
            }

            var isPlaying = false;

            foreach (var partSys in ThrusterToParticleSystem(thruster))
            {
                if (partSys.isPlaying)
                {
                    isPlaying = partSys.isPlaying;
                }
            }

            return isPlaying;
        }

        public bool IsThrusterInactive(Thruster thruster)
        {
            if (!isInitialized)
            {
                return false;
            }

            var isStopped = false;

            foreach (var partSys in ThrusterToParticleSystem(thruster))
            {
                if (partSys.isStopped)
                {
                    isStopped = partSys.isStopped;
                }
            }

            return isStopped;
        }

        public void ActivateBoostParticles()
        {
            if (!isInitialized)
            {
                return;
            }

            IsBoostParticlesActive = true;

            foreach (var partSys in _bottomParticles)
            {
                var main = partSys.main;
                thrusterStartColorCache = main.startColor.color;
                main.startColor = s_boostColor;

                thrusterParticleSizeCache = main.startSize.constant;
                main.startSize = main.startSize.constant * 2;
            }
        }

        public void DeactivateBoostParticles()
        {
            if (!isInitialized)
            {
                return;
            }

            IsBoostParticlesActive = false;

            foreach (var partSys in _bottomParticles)
            {
                var main = partSys.main;
                main.startColor = thrusterStartColorCache;
                main.startSize = thrusterParticleSizeCache;
            }
        }

        private void Awake()
        {
            if (_initializeSelf)
            {
                Init();
            }
        }

        private List<ParticleSystem> ThrusterToParticleSystem(Thruster toConvert)
        {
            switch (toConvert)
            {
                case Thruster.Left:
                    return _leftParticles;
                case Thruster.Right:
                    return _rightParticles;
                case Thruster.Bottom:
                    return _bottomParticles;
                default:
                    return null;
            }
        }

        private IEnumerator ActivateParticlesForDuration(List<ParticleSystem> particles, float duration)
        {
            if (!isInitialized)
            {
                yield break;
            }

            foreach (var partSys in particles)
            {
                partSys.Play();
            }

            yield return new WaitForSeconds(duration);

            foreach (var partSys in particles)
            {
                partSys.Stop();
            }
        }

        private void GetParticleSystem(GameObject fromTarget)
        {
            switch (fromTarget.tag)
            {
                case "SideLeft":
                    _leftParticles.Add(fromTarget.GetComponent<ParticleSystem>());
                    break;
                case "SideRight":
                    _rightParticles.Add(fromTarget.GetComponent<ParticleSystem>());
                    break;
                case "SideBottom":
                    _bottomParticles.Add(fromTarget.GetComponent<ParticleSystem>());
                    break;
            }
        }
    }
}
