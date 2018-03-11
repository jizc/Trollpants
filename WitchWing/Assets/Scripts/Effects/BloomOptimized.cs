// <copyright file="BloomOptimized.cs" company="Jan Ivar Z. Carlsen, Sindri Jóelsson">
// Copyright (c) 2016 Jan Ivar Z. Carlsen, Sindri Jóelsson. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace WitchWing.Effects
{
    using UnityEngine;

    [ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
    [AddComponentMenu("Image Effects/Bloom and Glow/Bloom (Optimized)")]
    public class BloomOptimized : PostEffectsBase
    {
        [SerializeField] [Range(0.0f, 1.5f)] private float threshold = 0.25f;
        [SerializeField] [Range(0.0f, 2.5f)] private float intensity = 0.75f;
        [SerializeField] private Shader fastBloomShader;

        private Material fastBloomMaterial;

        protected override bool CheckResources()
        {
            CheckSupport();

            fastBloomMaterial = CheckShaderAndCreateMaterial(fastBloomShader, fastBloomMaterial);

            if (!IsSupported)
            {
                ReportAutoDisable();
            }

            return IsSupported;
        }

        private void OnDisable()
        {
            if (fastBloomMaterial)
            {
                DestroyImmediate(fastBloomMaterial);
            }
        }

        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if (CheckResources() == false)
            {
                Graphics.Blit(source, destination);
                return;
            }

            const int divider = 4;
            const float widthMod = 0.5f;

            fastBloomMaterial.SetVector("_Parameter", new Vector4(widthMod, 0.0f, threshold, intensity));
            source.filterMode = FilterMode.Bilinear;

            var rtW = source.width / divider;
            var rtH = source.height / divider;

            // downsample
            var renderTexture = RenderTexture.GetTemporary(rtW, rtH, 0, source.format);
            renderTexture.filterMode = FilterMode.Bilinear;
            Graphics.Blit(source, renderTexture, fastBloomMaterial, 1);

            fastBloomMaterial.SetTexture("_Bloom", renderTexture);

            Graphics.Blit(source, destination, fastBloomMaterial, 0);

            RenderTexture.ReleaseTemporary(renderTexture);
        }
    }
}
