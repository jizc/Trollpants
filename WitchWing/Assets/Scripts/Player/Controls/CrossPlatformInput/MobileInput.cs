namespace UnityStandardAssets.CrossPlatformInput.PlatformSpecific
{
    using UnityEngine;

    public class MobileInput : VirtualInput
    {
        public override float GetAxis(string name, bool raw)
        {
            if (!VirtualAxes.ContainsKey(name))
            {
                AddAxes(name);
            }

            return VirtualAxes[name].GetValue;
        }

        public override void SetButtonDown(string name)
        {
            if (!VirtualButtons.ContainsKey(name))
            {
                AddButton(name);
            }

            VirtualButtons[name].Pressed();
        }

        public override void SetButtonUp(string name)
        {
            if (!VirtualButtons.ContainsKey(name))
            {
                AddButton(name);
            }

            VirtualButtons[name].Released();
        }

        public override void SetAxisPositive(string name)
        {
            if (!VirtualAxes.ContainsKey(name))
            {
                AddAxes(name);
            }

            VirtualAxes[name].Update(1f);
        }

        public override void SetAxisNegative(string name)
        {
            if (!VirtualAxes.ContainsKey(name))
            {
                AddAxes(name);
            }

            VirtualAxes[name].Update(-1f);
        }

        public override void SetAxisZero(string name)
        {
            if (!VirtualAxes.ContainsKey(name))
            {
                AddAxes(name);
            }

            VirtualAxes[name].Update(0f);
        }

        public override void SetAxis(string name, float value)
        {
            if (!VirtualAxes.ContainsKey(name))
            {
                AddAxes(name);
            }

            VirtualAxes[name].Update(value);
        }

        public override bool GetButtonDown(string name)
        {
            if (VirtualButtons.ContainsKey(name))
            {
                return VirtualButtons[name].GetButtonDown;
            }

            AddButton(name);
            return VirtualButtons[name].GetButtonDown;
        }

        public override bool GetButtonUp(string name)
        {
            if (VirtualButtons.ContainsKey(name))
            {
                return VirtualButtons[name].GetButtonUp;
            }

            AddButton(name);
            return VirtualButtons[name].GetButtonUp;
        }

        public override bool GetButton(string name)
        {
            if (VirtualButtons.ContainsKey(name))
            {
                return VirtualButtons[name].GetButton;
            }

            AddButton(name);
            return VirtualButtons[name].GetButton;
        }

        public override Vector3 MousePosition()
        {
            return VirtualMousePosition;
        }

        private static void AddButton(string name)
        {
            // we have not registered this button yet so add it, happens in the constructor
            CrossPlatformInput.RegisterVirtualButton(new CrossPlatformInput.VirtualButton(name));
        }

        private static void AddAxes(string name)
        {
            // we have not registered this button yet so add it, happens in the constructor
            CrossPlatformInput.RegisterVirtualAxis(new CrossPlatformInput.VirtualAxis(name));
        }
    }
}
