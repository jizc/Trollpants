/*
    SetRenderQueue.cs

    Sets the RenderQueue of an object's materials on Awake. This will instance
    the materials, so the script won't interfere with other renderers that
    reference the same materials.

    http://wiki.unity3d.com/index.php/DepthMask
*/

namespace BucketKnight
{
    using UnityEngine;

    [AddComponentMenu("Rendering/SetRenderQueue")]
    public class SetRenderQueue : MonoBehaviour
    {
        [SerializeField] protected int[] m_queues = { 3000 };

        protected void Awake()
        {
            var materials = GetComponent<Renderer>().materials;
            for (var i = 0; i < materials.Length && i < m_queues.Length; ++i)
            {
                materials[i].renderQueue = m_queues[i];
            }
        }
    }
}
