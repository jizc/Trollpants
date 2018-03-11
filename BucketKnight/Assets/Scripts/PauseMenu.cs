// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PauseMenu.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BucketKnight
{
    using UnityEngine;

    public class PauseMenu : MonoBehaviour
    {
        //Kode for å animere pausemenyen fra under skjermen til riktig posisjon, funker ikke fordi vi stopper time når man tar pause
        /*
    private Vector3 startPos;
    public Vector3 finalPos;
    private RectTransform position;
    bool moving = false;
    public bool played = false;
    // Use this for initialization
    void Start () {
        position = GetComponent<RectTransform>();
        startPos = position.localPosition;
    }
 
    void Update () {
        if (!played)
        {
            StartCoroutine(MoveFromTo(startPos, finalPos, 0.5f));
        }
    }

    IEnumerator MoveFromTo(Vector3 pointA, Vector3 pointB, float time)
    {
        played = true;
        if (!moving)
        { // Do nothing if already moving
            moving = true; // Set flag to true
            float t = 0f;
            while (t < 1.0f)
            {
                t += Time.deltaTime / time; // Sweeps from 0 to 1 in time seconds
                //GetComponent<RectTransform>().localPosition = your position
                //transform.position = 
                position.localPosition = Vector3.Lerp(pointA, pointB, t); // Set position proportional to t
                yield return null;
            }
            moving = false; // Finished moving
        }
    }
     */
    }
}
