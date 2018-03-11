// <copyright file="CameraZoomer.cs" company="Jan Ivar Z. Carlsen, Sindri Jóelsson">
// Copyright (c) 2016 Jan Ivar Z. Carlsen, Sindri Jóelsson. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace WitchWing.Environment
{
    using UnityEngine;

    public class CameraZoomer
    {
        private readonly Vector3 cameraStartPos = new Vector3(0f, 0f, -5.7f);
        private readonly Vector3 boostCameraStartPos = new Vector3(0f, -0.5f, -6.55f);
        private readonly Transform cameraTransform;
        private readonly float zDeltaMax;

        private Vector3 currentStandardCameraZoomPos;
        private Vector3 currentBoostCameraZoomPos;

        public CameraZoomer(Transform cameraTransform)
        {
            this.cameraTransform = cameraTransform;

            currentStandardCameraZoomPos = cameraStartPos;
            currentBoostCameraZoomPos = boostCameraStartPos;
            zDeltaMax = Mathf.Abs(boostCameraStartPos.z - cameraStartPos.z);
            cameraTransform.localPosition = cameraStartPos;
        }

        public float CameraZBias
        {
            get { return (cameraTransform.localPosition.z - cameraStartPos.z) / zDeltaMax; }
        }

        public void Reset()
        {
            currentStandardCameraZoomPos = cameraStartPos;
            currentBoostCameraZoomPos = boostCameraStartPos;
            cameraTransform.localPosition = cameraStartPos;
        }

        public void AdjustCameraZoom(float xDelta, bool isBoosting)
        {
            SetCameraZoomPositions(xDelta);

            var cameraTargetPos = isBoosting
                ? currentBoostCameraZoomPos
                : currentStandardCameraZoomPos;

            cameraTransform.localPosition = Vector3.MoveTowards(
                    cameraTransform.position,
                    cameraTargetPos,
                    Time.deltaTime);
        }

        private void SetCameraZoomPositions(float xDelta)
        {
            var vector3Delta = new Vector3(xDelta, 0f, 0f);
            currentStandardCameraZoomPos += vector3Delta;
            currentBoostCameraZoomPos += vector3Delta;
        }
    }
}
