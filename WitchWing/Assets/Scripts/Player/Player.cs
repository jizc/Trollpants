// <copyright file="Player.cs" company="Jan Ivar Z. Carlsen, Sindri Jóelsson">
// Copyright (c) 2016 Jan Ivar Z. Carlsen, Sindri Jóelsson. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace WitchWing.Player
{
    using System;
    using CloudOnce;
    using Controls;
    using Data;
    using Environment;
    using GUI;
    using UnityEngine;
    using UnityEngine.UI;

    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(PlayerState))]
    [RequireComponent(typeof(PlayerEffects))]
    public class Player : MonoBehaviour
    {
        private const float timeBonusDrainDelay = 25f;
        private const float timeBonusDrainInterval = 0.1f;
        private const float animationSmoothTime = 0.1f;

        private static Player s_instance;

        [SerializeField] private BoostInput boostInput;
        [SerializeField] private GameObject playerGeometry;
        [SerializeField] private Image manaFill;
        [SerializeField] private PotionEffects potionEffects;

        private Animator animator;
        private TiltController controller;
        private ManaPool manaPool;

        private float currentTimeBonusDrainDelay;
        private float currentTimeBonusDrainInterval;
        private int timeBonusDrainAmount;

        private float smoothedRotation;
        private float angularVelocity;
        private bool isBoosting;
        private PlayerState state;
        private PlayerEffects effects;

        public static PlayerState State
        {
            get { return s_instance.state; }
        }

        public static Transform Transform
        {
            get { return s_instance.transform; }
        }

        public static bool IsBoosting
        {
            get
            {
                return s_instance.isBoosting;
            }

            private set
            {
                if (value == s_instance.isBoosting)
                {
                    return;
                }

                if (value)
                {
                    s_instance.effects.EnableBoostTrail();
                    AudioClipPlayer.PlayBoostStart();
                }
                else
                {
                    s_instance.effects.EnableNormalTrail();
                    AudioClipPlayer.PlayBoostEnd();
                }

                s_instance.isBoosting = value;
            }
        }

        public static void Kill()
        {
            if (State.IsDead)
            {
                return;
            }

            State.IsDead = true;
            s_instance.playerGeometry.SetActive(false);
            s_instance.effects.PlayCrashAnimation();
            s_instance.effects.DisableTrail();
        }

        public static void Revive()
        {
            s_instance.playerGeometry.SetActive(true);
            s_instance.effects.EnableNormalTrail();
            RefillMana();
            StartNewTimeBonusDrain();
            State.IsWisdomActive = false;
            State.IsMidasActive = false;
            State.IsDead = false;
        }

        public static void RefillMana()
        {
            s_instance.manaPool.ResetManaAmount();
        }

        public static void StartNewTimeBonusDrain()
        {
            var bonusAmount = Mathf.Clamp(GameState.DifficultyLevel * 100, 0, 2000);
            State.TimeBonus = bonusAmount;
            s_instance.timeBonusDrainAmount = Mathf.RoundToInt(bonusAmount / 100f);
            s_instance.currentTimeBonusDrainDelay = timeBonusDrainDelay;
            s_instance.currentTimeBonusDrainInterval = 0f;
        }

        public void DrinkWisdomPotion()
        {
            if (State.WisdomPotions > 0)
            {
                AudioClipPlayer.PlayPotion();
                State.WisdomPotions--;
                s_instance.potionEffects.ActivateWisdomEffect();
                Cloud.Storage.Save();
            }
        }

        public void DrinkMidasPotion()
        {
            if (State.MidasPotions > 0)
            {
                AudioClipPlayer.PlayPotion();
                State.MidasPotions--;
                s_instance.potionEffects.ActivateMidasEffect();
                Cloud.Storage.Save();
            }
        }

        /// <summary>
        /// Get the altitude the player wants the avatar to be.
        /// </summary>
        /// <param name="altitudeRange">The height range the avatar is allowed to move on the Y axis.</param>
        /// <returns>The altitude the player wants the avatar to be.</returns>
        private static float GetTargetAltitude(float altitudeRange = 3f)
        {
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
            var normalizedMousePosition = Input.mousePosition.y / Screen.height;
            var altitude = Mathf.Clamp((normalizedMousePosition - 0.5f) * -2, -1f, 1f);
#else
            var altitude = CrossPlatformInput.GetAxis("Vertical");
#endif
            altitude *= altitudeRange;

            // Round off the tilt input
            return Mathf.Round(altitude * 100f) / 100f;
        }

        private void Awake()
        {
            s_instance = this;
            controller = new TiltController(transform);
            animator = GetComponent<Animator>();
            state = GetComponent<PlayerState>();
            effects = GetComponent<PlayerEffects>();
            manaPool = new ManaPool(manaFill);
        }

        private void Update()
        {
            if (GameState.IsPaused && !TutorialCoordinator.IsControlTutorialActive)
            {
                return;
            }

            HandleBoosting();
            if (State.IsDead)
            {
                return;
            }

            MoveTowardsDesiredAltitude();
            if (TutorialCoordinator.IsControlTutorialActive)
            {
                return;
            }

            DrainTimeBonus();
        }

        private void LateUpdate()
        {
            AnimatePlayer();
            manaPool.UpdateGUI();
        }

        private void HandleBoosting()
        {
            if (boostInput.IsPressingBoost && !State.IsDead)
            {
                manaPool.DrainMana();
                IsBoosting = manaPool.CurrentManaAmount > 0f;
            }
            else
            {
                IsBoosting = false;
                if (Math.Abs(State.ManaAmount - manaPool.CurrentManaAmount) < 0.0001f)
                {
                    return;
                }

                manaPool.RegenerateMana();
            }
        }

        private void MoveTowardsDesiredAltitude()
        {
            var altitude = GetTargetAltitude();
            var altitudeInput = PlayerSettings.IsYAxisInverted ? altitude : -altitude;
            controller.Move(
                altitudeInput,
                State.VerticalSpeed);
        }

        private void DrainTimeBonus()
        {
            if (State.TimeBonus == 0)
            {
                return;
            }

            if (currentTimeBonusDrainDelay > 0f)
            {
                currentTimeBonusDrainDelay -= Time.deltaTime;
                return;
            }

            currentTimeBonusDrainInterval -= Time.deltaTime;

            if (currentTimeBonusDrainInterval > 0f)
            {
                return;
            }

            State.TimeBonus = Mathf.Max(0, State.TimeBonus - timeBonusDrainAmount);
            currentTimeBonusDrainInterval = timeBonusDrainInterval;
        }

        private void AnimatePlayer()
        {
            animator.speed = WorldScroller.ScrollSpeed / WorldScroller.BaseScrollSpeed;
            smoothedRotation = Mathf.SmoothDamp(smoothedRotation, controller.TargetRotationZ, ref angularVelocity, animationSmoothTime);
            animator.SetFloat("Rotation", smoothedRotation);
        }
    }
}
