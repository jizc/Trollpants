namespace UnityStandardAssets.CrossPlatformInput
{
    using System;
    using PlatformSpecific;
    using UnityEngine;

    public static class CrossPlatformInput
    {
        private static readonly VirtualInput s_touchInput;
        private static readonly VirtualInput s_hardwareInput;

        private static VirtualInput s_activeInput;

        static CrossPlatformInput()
        {
            s_touchInput = new MobileInput();
            s_hardwareInput = new StandaloneInput();
#if MOBILE_INPUT
            s_activeInput = s_touchInput;
#else
            s_activeInput = s_hardwareInput;
#endif
        }

        public enum ActiveInputMethod
        {
            Hardware,
            Touch
        }

        public static Vector3 MousePosition
        {
            get { return s_activeInput.MousePosition(); }
        }

        public static void SwitchActiveInputMethod(ActiveInputMethod activeInputMethod)
        {
            switch (activeInputMethod)
            {
                case ActiveInputMethod.Hardware:
                    s_activeInput = s_hardwareInput;
                    break;
                case ActiveInputMethod.Touch:
                    s_activeInput = s_touchInput;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("activeInputMethod", activeInputMethod, null);
            }
        }

        public static bool AxisExists(string name)
        {
            return s_activeInput.AxisExists(name);
        }

        public static bool ButtonExists(string name)
        {
            return s_activeInput.ButtonExists(name);
        }

        public static void RegisterVirtualAxis(VirtualAxis axis)
        {
            s_activeInput.RegisterVirtualAxis(axis);
        }

        public static void RegisterVirtualButton(VirtualButton button)
        {
            s_activeInput.RegisterVirtualButton(button);
        }

        public static void UnRegisterVirtualAxis(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            s_activeInput.UnRegisterVirtualAxis(name);
        }

        public static void UnRegisterVirtualButton(string name)
        {
            s_activeInput.UnRegisterVirtualButton(name);
        }

        // returns a reference to a named virtual axis if it exists otherwise null
        public static VirtualAxis VirtualAxisReference(string name)
        {
            return s_activeInput.VirtualAxisReference(name);
        }

        // returns the platform appropriate axis for the given name
        public static float GetAxis(string name)
        {
            return GetAxis(name, false);
        }

        public static float GetAxisRaw(string name)
        {
            return GetAxis(name, true);
        }

        // -- Button handling --
        public static bool GetButton(string name)
        {
            return s_activeInput.GetButton(name);
        }

        public static bool GetButtonDown(string name)
        {
            return s_activeInput.GetButtonDown(name);
        }

        public static bool GetButtonUp(string name)
        {
            return s_activeInput.GetButtonUp(name);
        }

        public static void SetButtonDown(string name)
        {
            s_activeInput.SetButtonDown(name);
        }

        public static void SetButtonUp(string name)
        {
            s_activeInput.SetButtonUp(name);
        }

        public static void SetAxisPositive(string name)
        {
            s_activeInput.SetAxisPositive(name);
        }

        public static void SetAxisNegative(string name)
        {
            s_activeInput.SetAxisNegative(name);
        }

        public static void SetAxisZero(string name)
        {
            s_activeInput.SetAxisZero(name);
        }

        public static void SetAxis(string name, float value)
        {
            s_activeInput.SetAxis(name, value);
        }

        public static void SetVirtualMousePositionX(float f)
        {
            s_activeInput.SetVirtualMousePositionX(f);
        }

        public static void SetVirtualMousePositionY(float f)
        {
            s_activeInput.SetVirtualMousePositionY(f);
        }

        public static void SetVirtualMousePositionZ(float f)
        {
            s_activeInput.SetVirtualMousePositionZ(f);
        }

        // private function handles both types of axis (raw and not raw)
        private static float GetAxis(string name, bool raw)
        {
            return s_activeInput.GetAxis(name, raw);
        }

        // virtual axis and button classes - applies to mobile input
        // Can be mapped to touch joysticks, tilt, gyro, etc, depending on desired implementation.
        // Could also be implemented by other input devices - kinect, electronic sensors, etc
        public class VirtualAxis
        {
            public VirtualAxis(string name)
            {
                Name = name;
            }

            public string Name { get; private set; }

            public float GetValue { get; private set; }

            public float GetValueRaw
            {
                get { return GetValue; }
            }

            // removes an axes from the cross platform input system
            public void Remove()
            {
                UnRegisterVirtualAxis(Name);
            }

            // a controller gameobject (eg. a virtual thumbstick) should update this class
            public void Update(float axisValue)
            {
                GetValue = axisValue;
            }
        }

        // a controller gameobject (eg. a virtual GUI button) should call the
        // 'pressed' function of this class. Other objects can then read the
        // Get/Down/Up state of this button.
        public class VirtualButton
        {
            private int lastPressedFrame = -5;
            private int releasedFrame = -5;

            public VirtualButton(string name)
            {
                Name = name;
            }

            public string Name { get; private set; }

            // these are the states of the button which can be read via the cross platform input system
            public bool GetButton { get; private set; }

            public bool GetButtonDown
            {
                get { return lastPressedFrame - Time.frameCount == -1; }
            }

            public bool GetButtonUp
            {
                get { return releasedFrame == Time.frameCount - 1; }
            }

            // A controller gameobject should call this function when the button is pressed down
            public void Pressed()
            {
                if (GetButton)
                {
                    return;
                }

                GetButton = true;
                lastPressedFrame = Time.frameCount;
            }

            // A controller gameobject should call this function when the button is released
            public void Released()
            {
                GetButton = false;
                releasedFrame = Time.frameCount;
            }

            // the controller gameobject should call Remove when the button is destroyed or disabled
            public void Remove()
            {
                UnRegisterVirtualButton(Name);
            }
        }
    }
}
