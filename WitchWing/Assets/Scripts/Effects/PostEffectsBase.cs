// <copyright file="PostEffectsBase.cs" company="Jan Ivar Z. Carlsen, Sindri Jóelsson">
// Copyright (c) 2016 Jan Ivar Z. Carlsen, Sindri Jóelsson. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace WitchWing.Effects
{
    using System.Collections.Generic;
    using UnityEngine;

    [ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
    public class PostEffectsBase : MonoBehaviour
    {
        private readonly List<Material> createdMaterials = new List<Material>();

        private bool isSupported = true;

        protected bool IsSupported
        {
            get { return isSupported; }
            set { isSupported = value; }
        }

        protected Material CheckShaderAndCreateMaterial(Shader shader, Material materialToCreate)
        {
            if (!shader)
            {
                Debug.Log("Missing shader in " + ToString());
                enabled = false;
                return null;
            }

            if (shader.isSupported && materialToCreate && materialToCreate.shader == shader)
            {
                return materialToCreate;
            }

            if (!shader.isSupported)
            {
                NotSupported();
                Debug.Log("The shader " + shader + " on effect " + ToString() +
                          " is not supported on this platform!");
                return null;
            }

            materialToCreate = new Material(shader);
            createdMaterials.Add(materialToCreate);
            materialToCreate.hideFlags = HideFlags.DontSave;

            return materialToCreate;
        }

        protected virtual bool CheckResources()
        {
            Debug.LogWarning("CheckResources () for " + ToString() + " should be overwritten.");
            return isSupported;
        }

        protected void CheckSupport(bool needDepth = false)
        {
            isSupported = true;

            if (!SystemInfo.supportsImageEffects)
            {
                NotSupported();
                return;
            }

            if (needDepth && !SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.Depth))
            {
                NotSupported();
                return;
            }

            if (needDepth)
            {
                GetComponent<Camera>().depthTextureMode |= DepthTextureMode.Depth;
            }
        }

        protected void ReportAutoDisable()
        {
            Debug.LogWarning("The image effect " + ToString() +
                             " has been disabled as it's not supported on the current platform.");
        }

        private void NotSupported()
        {
            enabled = false;
            isSupported = false;
        }

        private void Start()
        {
            CheckResources();
        }

        private void OnEnable()
        {
            isSupported = true;
        }

        private void OnDestroy()
        {
            RemoveCreatedMaterials();
        }

        private void RemoveCreatedMaterials()
        {
            while (createdMaterials.Count > 0)
            {
                var material = createdMaterials[0];
                createdMaterials.RemoveAt(0);
#if UNITY_EDITOR
                DestroyImmediate(material);
#else
                Destroy(material);
#endif
            }
        }
    }
}
