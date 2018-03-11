// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnemyShipSupervisor.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace InterstellarDrift
{
    using UnityEngine;

    public enum Controller
    {
        OneClick,
        TwoButton,
        Keyboard,
        Enemy
    }

    [RequireComponent(typeof(Rigidbody2D))]
    public class EnemyShipSupervisor : MonoBehaviour
    {
        [SerializeField] private bool _initializeSelf;
        [SerializeField] private Controller _desiredController;

        private EffectsShepherd effects;
        private BaseController controller;
        private DetectionRadius detectionRadius;

        public Controller DesiredController
        {
            get { return _desiredController; }
            set { _desiredController = value; }
        }

        public void Init()
        {
            if (effects == null)
            {
                effects = gameObject.AddComponent<EffectsShepherd>();
                effects.Init();
            }

            controller = AddControllerOfType(DesiredController);
            controller.Init(effects);
            controller.SetMaxVelocity(100f);

            detectionRadius = gameObject.AddComponent<DetectionRadius>();
            detectionRadius.Init(10);
        }

        private void Awake()
        {
            if (_initializeSelf)
            {
                Init();
            }
        }

        private BaseController AddControllerOfType(Controller type)
        {
            switch (type)
            {
                case Controller.OneClick:
                    return gameObject.AddComponent<OneClickController>();
                case Controller.TwoButton:
                    return gameObject.AddComponent<TwoButtonController>();
                case Controller.Keyboard:
                    return gameObject.AddComponent<KeyboardController>();
                case Controller.Enemy:
                    return gameObject.AddComponent<EnemyController>();
                default:
                    return null;
            }
        }
    }
}
