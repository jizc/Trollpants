// <copyright file="CharacterCollision.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace MiniPlanetRun.Character
{
    using Audio;
    using CloudOnce;
    using Data;
    using UnityEngine;
    using World;

    public class CharacterCollision : MonoBehaviour
    {
        [SerializeField] private GameManager gameManager;
        [SerializeField] private SessionData sessionData;
        [SerializeField] private ObjectPooler objectPooler;
        [SerializeField] private ParticleSystem points;
        [SerializeField] private ParticleSystem deathEffect;
        [SerializeField] private bool obstacleCollisionOn = true;

        private void OnTriggerEnter2D(Collider2D col)
        {
#if DEBUG
            Debug.Log(name + " entered trigger " + col.name);
#endif

            if (col.name == "Currency")
            {
                sessionData.CherriesThisRun++;
                CloudVariables.TotalCollectedCherries++;
                sessionData.Cherries++;

                AudioClipPlayer.PlayCherry();

                points.transform.position = col.transform.position;
                points.Play();
                objectPooler.Recycle(col.gameObject);
            }
            else if (col.name.StartsWith("Jump") || col.name.StartsWith("Slide"))
            {
                if (!obstacleCollisionOn)
                {
                    return;
                }

                AudioClipPlayer.PlayDeath();

                deathEffect.transform.position = transform.position;
                deathEffect.Play();
                gameManager.PlayerDeath();
            }
        }
    }
}
