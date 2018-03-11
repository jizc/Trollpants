// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ScoreText.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BucketKnight
{
    using UnityEngine;
    using UnityEngine.UI;

    public class ScoreText : MonoBehaviour
    {
        private void OnEnable()
        {
            Events.instance.AddListener<ScoreChanged>(OnScoreChanged);
        }

        private void OnScoreChanged(ScoreChanged scoreChangedEvent)
        {
            GetComponent<Text>().text = string.Empty + scoreChangedEvent.score;
        }

        private void OnDisable()
        {
            Events.instance.RemoveListener<ScoreChanged>(OnScoreChanged);
        }
    }
}
