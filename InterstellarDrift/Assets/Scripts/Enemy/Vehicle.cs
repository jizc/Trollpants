// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Vehicle.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace InterstellarDrift
{
    using UnityEngine;

    [RequireComponent(typeof(EffectsShepherd))]
    public class Vehicle : MonoBehaviour
    {
        private readonly ForceMode2D forceMode = ForceMode2D.Impulse;

        [SerializeField] private float _maxVelocity;

        private float forwardForce;

        private Rigidbody2D cachedRigidbody2D;
        private bool isInitialized;

        public EffectsShepherd Effects { get; private set; }

        public void SetMaxVelocity(float value)
        {
            if (value < 0f)
            {
#if DEBUG
                Debug.LogWarning("Tried to set MaxVelocity to less than 0.");
#endif
                return;
            }

            _maxVelocity = value;
            forwardForce = Mathf.Sqrt(_maxVelocity);
        }

        public void Init()
        {
            Effects = GetComponent<EffectsShepherd>();
            cachedRigidbody2D = GetComponent<Rigidbody2D>();
            if (!cachedRigidbody2D)
            {
#if DEBUG
                Debug.LogWarning("Missing Rigidbody2D on " + name);
#endif
                return;
            }

            // IMPORTANT: If drag is 1f, Mass is 1f and ForceMode is set to Impulse, Force^2 = MaxVelocity. Ex: 2f^2 = 4 maxVel.
            // This means that we can calculate the needed constant force from the maximum velocity;
            // we can set a top speed and calculate the force needed to get there.
            cachedRigidbody2D.drag = 1f;

            forwardForce = Mathf.Sqrt(_maxVelocity);

            isInitialized = true;
        }

        public void MoveForward()
        {
            if (!isInitialized)
            {
#if DEBUG
                Debug.LogWarning("Vehicle not initialized!");
#endif
                return;
            }

            // Provides motion in the forward direction
            cachedRigidbody2D.AddForce(transform.up * forwardForce * Time.fixedDeltaTime, forceMode);

            if (Effects)
            {
                Effects.ActivateThruster(Thruster.Bottom);
            }

            if (SoundManager.Instance != null)
            {
                SoundManager.Instance.PlaySound(Sound.MainBoost);
            }
        }

        public void MoveLeft(float turnRatio)
        {
            if (!isInitialized)
            {
#if DEBUG
                Debug.LogWarning("Vehicle not initialized!");
#endif
                return;
            }

            cachedRigidbody2D.AddTorque(turnRatio * Time.fixedDeltaTime, forceMode);

            // Activate the right adjustment-thruster visuals
            if (Effects)
            {
                Effects.ActivateThruster(Thruster.Right);
                Effects.DeactivateThruster(Thruster.Left);
            }

            if (SoundManager.Instance != null)
            {
                SoundManager.Instance.PlaySound(Sound.SideBoost);
            }
        }

        public void MoveRight(float turnRatio)
        {
            if (!isInitialized)
            {
#if DEBUG
                Debug.LogWarning("Vehicle not initialized!");
#endif
                return;
            }

            cachedRigidbody2D.AddTorque(-turnRatio * Time.fixedDeltaTime, forceMode);

            // Activate the left adjustment-thruster visuals
            if (Effects)
            {
                Effects.ActivateThruster(Thruster.Left);
                Effects.DeactivateThruster(Thruster.Right);
            }

            if (SoundManager.Instance != null)
            {
                SoundManager.Instance.PlaySound(Sound.SideBoost);
            }
        }

        public void FixedUpdate()
        {
            if (!isInitialized)
            {
#if DEBUG
                Debug.LogWarning("Vehicle not initialized!");
#endif
                return;
            }

            // Check if the thrusters should be turned off because of too little angular momentum
            // Doesn't make sense to have thrusters firing when there is no apparent force applied in that direction
            if (Mathf.Abs(cachedRigidbody2D.angularVelocity) <= 115f
                && (Effects.IsThrusterActive(Thruster.Left) || Effects.IsThrusterActive(Thruster.Right)))
            {
                TurnOffThrusters();
            }

            // Not moving much? Turn off bottom particle effect
            if (Mathf.Abs(cachedRigidbody2D.velocity.sqrMagnitude) <= 25f && Effects.IsThrusterActive(Thruster.Bottom))
            {
                Effects.DeactivateThruster(Thruster.Bottom);
            }
        }

        public void TurnOffThrusters()
        {
#if DEBUG
            Debug.Log("Deactivating adjustment-thrusters.");
#endif

            if (Effects)
            {
                Effects.DeactivateThruster(Thruster.Left);
                Effects.DeactivateThruster(Thruster.Right);
            }

            if (SoundManager.Instance != null)
            {
                SoundManager.Instance.StopSound(Sound.SideBoost);
            }
        }

        private void Awake()
        {
            Init();
        }
    }
}
