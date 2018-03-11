// <copyright file="Controller.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace MiniPlanetRun.Character
{
    using System.Collections;
    using UnityEngine;

    public class Controller : MonoBehaviour
    {
        private const float delayedActionDuration = 0.2f;

        [SerializeField] private SwipeDetector swipeDetector;
        [SerializeField] private PolygonCollider2D runCollider;
        [SerializeField] private PolygonCollider2D slideCollider;

        private Animator currentAnimator;
        private Rigidbody2D body2D;
        private ParticleSystem slideParticles;

        private bool canJump;
        private bool canSlide = true;
        private bool extraJump;

        private bool isJumpingPrematurely;
        private bool isSlidingPrematurely;

        public void ResetCharacter()
        {
            StopAllCoroutines();
            canSlide = true;
            isJumpingPrematurely = false;
            isSlidingPrematurely = false;
            runCollider.enabled = true;
            slideCollider.enabled = false;
            slideParticles.Stop();
        }

        public void SetAnimator(Animator animator)
        {
            currentAnimator = animator;
        }

        private void Awake()
        {
            body2D = GetComponent<Rigidbody2D>();
            slideParticles = GameObject.Find("SlideEffect").GetComponent<ParticleSystem>();
            swipeDetector.Swiped += SwipeHandler;
        }

#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
        private void Update()
        {
            if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
                && !(Input.GetKey(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)))
            {
                Jump();
            }

            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            {
                Slide();
            }
        }
#endif

        private void OnCollisionEnter2D(Collision2D col)
        {
            canJump = true;
            body2D.gravityScale = 1.4f;
            extraJump = col.gameObject.name == "Platform";

            if (isJumpingPrematurely)
            {
                Jump();
            }
        }

        private void OnCollisionExit2D(Collision2D col)
        {
            canJump = false;
            body2D.gravityScale = 1.4f;
        }

        private void SwipeHandler(Direction dir)
        {
            if (enabled == false)
            {
                return;
            }

            switch (dir)
            {
                case Direction.Up:
                    Jump();
                    return;

                case Direction.Down:
                    Slide();
                    return;
            }
        }

        private void Jump()
        {
            if (canJump)
            {
                StopAllCoroutines();
                canSlide = true;
                isJumpingPrematurely = false;
                isSlidingPrematurely = false;
                runCollider.enabled = true;
                slideCollider.enabled = false;
                if (extraJump)
                {
                    body2D.AddForce(Vector2.up * 6.5f, ForceMode2D.Impulse);
                }
                else
                {
                    body2D.AddForce(Vector2.up * 5.2f, ForceMode2D.Impulse);
                }

                extraJump = false;
                canJump = false;
                currentAnimator.SetTrigger("StopSlide");
                currentAnimator.SetTrigger("Jump");
                slideParticles.Stop();
            }
            else
            {
                StartCoroutine(JumpingPrematurely());
            }
        }

        private void Slide()
        {
            if (canSlide)
            {
                StopCoroutine(RaisingColliders());
                isJumpingPrematurely = false;
                isSlidingPrematurely = false;
                body2D.gravityScale = 3f;
                canSlide = false;
                StopAllCoroutines();
                slideCollider.enabled = true;
                runCollider.enabled = false;
                currentAnimator.ResetTrigger("StopSlide");
                currentAnimator.SetTrigger("Slide");
                slideParticles.Play();
                StartCoroutine(StopSlide());
            }
            else
            {
                StartCoroutine(SlidingPrematurely());
            }
        }

        private IEnumerator RaisingColliders()
        {
            yield return new WaitForSeconds(0.12f);
            runCollider.enabled = true;
            slideCollider.enabled = false;
            canSlide = true;
            if (isSlidingPrematurely)
            {
                Slide();
            }
        }

        private IEnumerator JumpingPrematurely()
        {
            isJumpingPrematurely = true;

            yield return new WaitForSeconds(delayedActionDuration);
            isJumpingPrematurely = false;
        }

        private IEnumerator SlidingPrematurely()
        {
            isSlidingPrematurely = true;

            yield return new WaitForSeconds(delayedActionDuration);
            isSlidingPrematurely = false;
        }

        private IEnumerator StopSlide()
        {
            yield return new WaitForSeconds(0.45f);
            currentAnimator.SetTrigger("StopSlide");

            StartCoroutine(RaisingColliders());
            slideParticles.Stop();
        }
    }
}
