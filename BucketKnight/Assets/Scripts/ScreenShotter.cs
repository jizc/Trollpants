// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ScreenShotter.cs" company="Jan Ivar Z. Carlsen">
// Copyright (c) 2018 Jan Ivar Z. Carlsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BucketKnight
{
    using System;
    using System.IO;
    using UnityEngine;

    public class ScreenShotter : MonoBehaviour
    {
        private bool _takeHiResShot;
        private Camera _camera;
        private float _scaleheight;
        private float _windowaspect;
        private float _targetaspect;

        private void Start()
        {
            _camera = GetComponent<Camera>();
        }

        public static string ScreenShotName(int width, int height)
        {
            return string.Format("{0}/screenshots/{1}x{2}/screen_{3}.png",
                Application.dataPath,
                width, height,
                DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
        }

        public void TakeHiResShot()
        {
            _takeHiResShot = true;
        }

        public void ScreenShot(int resWidth, int resHeight)
        {
            var rt = new RenderTexture(resWidth, resHeight, 24);
            _camera.targetTexture = rt;
            var screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
            _camera.Render();
            RenderTexture.active = rt;
            screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
            _camera.targetTexture = null;
            RenderTexture.active = null; // JC: added to avoid errors
            Destroy(rt);
            var bytes = screenShot.EncodeToPNG();
            var filename = ScreenShotName(resWidth, resHeight);
            File.WriteAllBytes(filename, bytes);
            Debug.Log(string.Format("Took screenshot to: {0}", filename));
            _takeHiResShot = false;
        }

        private void LateUpdate()
        {
            _takeHiResShot |= Input.GetKeyDown("k");
            if (_takeHiResShot)
            {
                ScreenShot(2208, 1242);
                ScreenShot(1334, 750);
                ScreenShot(1136, 640);
            }
        }

        private void SetAspectRatio(float x, float y)
        {
            // set the desired aspect ratio (the values in this example are
            // hard-coded for 16:9, but you could make them into public
            // variables instead so you can set them at design time)
            _targetaspect = x / y;

            // determine the game window's current aspect ratio
            _windowaspect = Screen.width / (float)Screen.height;

            // current viewport height should be scaled by this amount
            _scaleheight = _windowaspect / _targetaspect;

            SetCameraRatio();
        }

        private void SetCameraRatio()
        {
            // if scaled height is less than current height, add letterbox
            if (_scaleheight < 1.0f)
            {
                var rect = _camera.rect;

                rect.width = 1.0f;
                rect.height = _scaleheight;
                rect.x = 0;
                rect.y = (1.0f - _scaleheight) / 2.0f;

                _camera.rect = rect;
            }
            else // add pillarbox
            {
                var scalewidth = 1.0f / _scaleheight;

                var rect = _camera.rect;

                rect.width = scalewidth;
                rect.height = 1.0f;
                rect.x = (1.0f - scalewidth) / 2.0f;
                rect.y = 0;

                _camera.rect = rect;
            }
        }
    }
}
