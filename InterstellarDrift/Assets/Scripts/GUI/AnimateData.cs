// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AnimateData.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace InterstellarDrift
{
    using CloudOnce;
    using DG.Tweening;
    using UnityEngine;
    using UnityEngine.UI;

    public class AnimateData : MonoBehaviour
    {
        public StatType Type = StatType.Score;

        public Text BestNumber;
        public Text RoundNumber;
        public Sound RecordBreakSound = Sound.Checkpoint;

        [SerializeField] private float _baseDuration = 1f;
        [SerializeField] private float _maxDuration = 3f;
        [SerializeField] private float _secPerHundredPoints = .05f;

        [SerializeField] private float _animationDuration = 1.75f;
        [SerializeField] private float _scaleRatio = 1.25f;
        [SerializeField] private float _shakeDuration = 0.5f;

        private int personalBest;
        private int sessionResult;
        private bool hasPassedPersonalBest;

        public enum StatType
        {
            Score,
            Time
        }

        public void PlayAnimation()
        {
            if (!RoundNumber || !BestNumber)
            {
                Debug.LogWarning(name + " lacks references to RoundNumber or BestNumber");
                return;
            }

            // Get initial values
            switch (Type)
            {
                case StatType.Score:
                    personalBest = CloudVariables.PersonalBestScore;
                    sessionResult = TrackedData.Instance.SessionData.Score;
                    break;
                case StatType.Time:
                    personalBest = CloudVariables.LongestTimeSurvived;
                    sessionResult = TrackedData.Instance.SessionData.SecondsSurvived;
                    break;
            }

            hasPassedPersonalBest = false;

            // Determine duration of animation
            var add = _secPerHundredPoints * (sessionResult / 100f);
            if (add > _maxDuration)
            {
                add = _maxDuration;
            }

            _animationDuration = _baseDuration + add;

            BestNumber.text = personalBest.ToString();
            RoundNumber.text = "0";

            // Invoke animation
            Invoke("AnimateValues", 1f);
        }

        private void AnimateValues()
        {
            AnimateText(RoundNumber, 0, sessionResult, _animationDuration);
        }

        private void AnimateText(Text textToAnimate, int from, int to, float duration)
        {
            var displayedScore = from;
            var sequence = DOTween.Sequence();

            sequence.Append(DOTween.To(() => displayedScore, x => displayedScore = x, to, duration).OnUpdate(() =>
            {
                textToAnimate.text = string.Empty + displayedScore;
                if (displayedScore >= personalBest && !hasPassedPersonalBest)
                {
                    hasPassedPersonalBest = true;
                    if (SoundManager.Instance)
                    {
                        SoundManager.Instance.PlaySound(RecordBreakSound);
                    }

                    BestNumber.transform.parent.DOPunchScale(BestNumber.transform.parent.localScale * _scaleRatio, _shakeDuration);
                    AnimateText(BestNumber, personalBest, sessionResult, duration - sequence.fullPosition);
                }
            }));
        }
    }
}
