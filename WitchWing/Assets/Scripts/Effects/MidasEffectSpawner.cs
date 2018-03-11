// <copyright file="MidasEffectSpawner.cs" company="Jan Ivar Z. Carlsen, Sindri Jóelsson">
// Copyright (c) 2016 Jan Ivar Z. Carlsen, Sindri Jóelsson. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace WitchWing.Effects
{
    using UnityEngine;

    public class MidasEffectSpawner : MonoBehaviour
    {
        private const string recycleBinName = "RecycledMidasEffects";

        [SerializeField] private Transform pool;
        [SerializeField] private GameObject prefab;

        private Transform recycleBin;

        public void Spawn(Vector3 worldPosition)
        {
            GameObject g;

            if (recycleBin != null && recycleBin.childCount > 0)
            {
                g = recycleBin.GetChild(0).gameObject;
            }
            else
            {
                g = Instantiate(prefab);

                if (g != null)
                {
                    g.name = "MidasEffect";
                }
            }

            if (g != null)
            {
                g.GetComponent<MidasAutoRecycle>().SetSpawner(this);
                g.transform.parent = pool;
                g.transform.position = worldPosition;
                g.SetActive(true);
                return;
            }

            Debug.LogError("Spawn failed!");
        }

        public void Recycle(GameObject midasEffectObject)
        {
            if (midasEffectObject == null)
            {
                Debug.LogError("The GameObject you want to recycle is null.");
                return;
            }

            if (recycleBin == null)
            {
                Debug.Log("Midas recycle bin has been destroyed");
                return;
            }

            midasEffectObject.transform.SetParent(recycleBin);
            midasEffectObject.SetActive(false);
        }

        private void Awake()
        {
            recycleBin = pool.Find(recycleBinName);
            if (recycleBin == null)
            {
                var recycleBinObject = new GameObject(recycleBinName);
                recycleBin = recycleBinObject.transform;
                recycleBin.SetParent(pool);
                recycleBin.position = Vector3.zero;
            }
        }
    }
}
