namespace UnityStandardAssets.CrossPlatformInput
{
    using System;
    using UnityEngine;

    // helps with managing tilt input on mobile devices
    public class TiltInput : MonoBehaviour
    {
        [SerializeField] private AxisMapping mapping;
        [SerializeField] private AxisOptions tiltAroundAxis = AxisOptions.ForwardAxis;
        [SerializeField] private float fullTiltAngle = 25;
        [SerializeField] private float centreAngleOffset;

        private CrossPlatformInput.VirtualAxis steerAxis;

        // options for the various orientations
        private enum AxisOptions
        {
            ForwardAxis,
            SidewaysAxis
        }

        private void OnEnable()
        {
            if (mapping.Type == AxisMapping.MappingType.NamedAxis)
            {
                steerAxis = new CrossPlatformInput.VirtualAxis(mapping.AxisName);
                CrossPlatformInput.RegisterVirtualAxis(steerAxis);
            }
        }

        private void Update()
        {
            float angle = 0;
            if (Input.acceleration != Vector3.zero)
            {
                switch (tiltAroundAxis)
                {
                    case AxisOptions.ForwardAxis:
                        angle = (Mathf.Atan2(Input.acceleration.x, -Input.acceleration.y) * Mathf.Rad2Deg) +
                                centreAngleOffset;
                        break;
                    case AxisOptions.SidewaysAxis:
                        angle = (Mathf.Atan2(Input.acceleration.z, -Input.acceleration.y) * Mathf.Rad2Deg) +
                                centreAngleOffset;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            var axisValue = (Mathf.InverseLerp(-fullTiltAngle, fullTiltAngle, angle) * 2) - 1;
            switch (mapping.Type)
            {
                case AxisMapping.MappingType.NamedAxis:
                    steerAxis.Update(axisValue);
                    break;
                case AxisMapping.MappingType.MousePositionX:
                    CrossPlatformInput.SetVirtualMousePositionX(axisValue * Screen.width);
                    break;
                case AxisMapping.MappingType.MousePositionY:
                    CrossPlatformInput.SetVirtualMousePositionY(axisValue * Screen.width);
                    break;
                case AxisMapping.MappingType.MousePositionZ:
                    CrossPlatformInput.SetVirtualMousePositionZ(axisValue * Screen.width);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnDisable()
        {
            steerAxis.Remove();
        }

        [Serializable]
        public class AxisMapping
        {
            [SerializeField]
            private MappingType type;

            [SerializeField]
            private string axisName;

            public enum MappingType
            {
                NamedAxis,
                MousePositionX,
                MousePositionY,
                MousePositionZ
            }

            public MappingType Type
            {
                get { return type; }
                set { type = value; }
            }

            public string AxisName
            {
                get { return axisName; }
                set { axisName = value; }
            }
        }
    }
}
