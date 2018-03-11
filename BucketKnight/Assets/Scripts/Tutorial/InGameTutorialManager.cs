// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InGameTutorialManager.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BucketKnight
{
    using UnityEngine;
    using UnityEngine.UI;

    public class InGameTutorialManager : MonoBehaviour
    {
        public GameObject[] tutorialObjects;
        public GameObject[] nonTutorialObjects;

        public int numberOfCoinsToPickUp;
        public int numberOfHazardsToDodge;

        private Text _coinText;
        private Text _dodgeText;

        private int _coinCounter;
        private int _dodgeCounter;

        private bool _playerLost;

        private void Awake()
        {
            _coinText = transform.Find("CoinText").GetComponent<Text>();
            _dodgeText = transform.Find("DodgeText").GetComponent<Text>();
        }

        private void OnEnable()
        {
            Events.instance.AddListener<InGameCoinsNumberChanged>(OnCoinsNumberChanged);
            Events.instance.AddListener<SuccessfulDodge>(OnSuccessfulDodge);
            Events.instance.AddListener<GameRestarted>(OnGameRestarted);
            Events.instance.AddListener<PlayerLost>(OnPlayerLostEvent);
        }

        private void Start()
        {
            SavedData.TutorialHasBeenLaunchedOnce = true;

            ////Events.instance.Raise(new TutorialToggled(true)); //<-- uncomment to debug tutorial level

            SetActives();
            if (!SavedData.PlayTutorial)
            {
                Events.instance.Raise(new GameResumed());
                gameObject.SetActive(false);
                return;
            }

            Events.instance.Raise(new GamePaused());
            Events.instance.Raise(
                new ShowInfoBox("To familiarize yourself with the controls, try picking up " + numberOfCoinsToPickUp +
                                " coins."));
            Events.instance.AddListener<ConfirmationBoxAnswered>(OnTutorialInfoReadResume);

            _coinCounter = 0;
            _coinText.gameObject.SetActive(true);
            _dodgeText.gameObject.SetActive(false);
        }

        private void SetActives()
        {
            foreach (var nonTutorialObject in nonTutorialObjects)
            {
                nonTutorialObject.SetActive(!SavedData.PlayTutorial);
            }
            foreach (var tutorialObject in tutorialObjects)
            {
                tutorialObject.SetActive(SavedData.PlayTutorial);
            }
        }

        #region Event handlers

        private void OnTutorialInfoReadResume(ConfirmationBoxAnswered confirmationBoxAnsweredEvent)
        {
            Events.instance.RemoveListener<ConfirmationBoxAnswered>(OnTutorialInfoReadResume);
            Events.instance.Raise(new GameResumed());
        }

        private void OnTutorialInfoReadRestart(ConfirmationBoxAnswered confirmationBoxAnsweredEvent)
        {
            Events.instance.RemoveListener<ConfirmationBoxAnswered>(OnTutorialInfoReadRestart);
            Events.instance.Raise(new GameRestarted());
        }

        private void OnTutorialFinished(ConfirmationBoxAnswered confirmationBoxAnsweredEvent)
        {
            Events.instance.RemoveListener<ConfirmationBoxAnswered>(OnTutorialFinished);
            Events.instance.Raise(new TutorialToggled(false));
            SetActives();
            Events.instance.Raise(new GameRestarted());
            gameObject.SetActive(false);
        }

        private void OnCoinsNumberChanged(InGameCoinsNumberChanged coinsNumberChangedEvent)
        {
            if (_coinCounter < numberOfCoinsToPickUp && coinsNumberChangedEvent.coins != 0)
            {
                _coinCounter++;
                _coinText.text = "Coins: " + _coinCounter;
            }
            else
            {
                return;
            }

            if (_coinCounter == numberOfCoinsToPickUp)
            {
                _coinText.gameObject.SetActive(false);
                _dodgeText.gameObject.SetActive(true);
                Events.instance.Raise(new GamePaused());
                Events.instance.Raise(new ShowInfoBox(
                    "You got " + numberOfCoinsToPickUp + " coins! Now try using the barrel roll to dodge " +
                    numberOfHazardsToDodge + " hazards. "
                    +
                    "To do a barrel roll, tap the screen with both fingers if you are using touch controls, or one finger if you are using tilt controls."));
                Events.instance.AddListener<ConfirmationBoxAnswered>(OnTutorialInfoReadRestart);
                GameObject.Find("TutorialHazardSpawner").GetComponent<Spawner>().bridgeShouldSpawn = true;
            }
        }

        private void OnSuccessfulDodge(SuccessfulDodge successfulDodgeEvent)
        {
            if (_coinCounter >= numberOfCoinsToPickUp && _dodgeCounter < numberOfHazardsToDodge)
            {
                _dodgeCounter++;
                _dodgeText.text = "Dodges: " + _dodgeCounter;
            }
            else
            {
                return;
            }

            if (_dodgeCounter == numberOfHazardsToDodge)
            {
                Events.instance.Raise(new GamePaused());
                Events.instance.Raise(new ShowInfoBox(
                    "Congratulations on completing the tutorial! Remember; the barrel roll will only protect you while you are actually under the water, so time it carefully. Now lets start river adventuring!"));
                Events.instance.AddListener<ConfirmationBoxAnswered>(OnTutorialFinished);
            }
        }

        private void OnGameRestarted(GameRestarted gameRestartedEvent)
        {
            if (_coinCounter < numberOfCoinsToPickUp && !_playerLost)
            {
                _coinCounter = 0;
                _coinText.text = "Coins: 0";
            }

            _playerLost = false;
        }

        private void OnPlayerLostEvent(PlayerLost playerLostEvent)
        {
            _playerLost = true;
            Events.instance.Raise(
                new ShowInfoBox(
                    "While adventuring you only have 3 lives. Hitting obstacles will remove one or more of them, so be careful!"));
            Events.instance.AddListener<ConfirmationBoxAnswered>(OnTutorialInfoReadRestart);
        }

        #endregion

        private void OnDisable()
        {
            Events.instance.RemoveListener<InGameCoinsNumberChanged>(OnCoinsNumberChanged);
            Events.instance.RemoveListener<SuccessfulDodge>(OnSuccessfulDodge);
            Events.instance.RemoveListener<GameRestarted>(OnGameRestarted);
            Events.instance.RemoveListener<PlayerLost>(OnPlayerLostEvent);
        }
    }
}
