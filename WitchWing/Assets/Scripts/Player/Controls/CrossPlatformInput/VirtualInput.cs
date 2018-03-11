namespace UnityStandardAssets.CrossPlatformInput
{
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class VirtualInput
    {
        private readonly Dictionary<string, CrossPlatformInput.VirtualAxis> virtualAxes =
            new Dictionary<string, CrossPlatformInput.VirtualAxis>();

        // Dictionary to store the name relating to the virtual axes
        private readonly Dictionary<string, CrossPlatformInput.VirtualButton> virtualButtons =
            new Dictionary<string, CrossPlatformInput.VirtualButton>();

        public Vector3 VirtualMousePosition { get; private set; }

        protected Dictionary<string, CrossPlatformInput.VirtualAxis> VirtualAxes
        {
            get { return virtualAxes; }
        }

        protected Dictionary<string, CrossPlatformInput.VirtualButton> VirtualButtons
        {
            get { return virtualButtons; }
        }

        public bool AxisExists(string name)
        {
            return virtualAxes.ContainsKey(name);
        }

        public bool ButtonExists(string name)
        {
            return virtualButtons.ContainsKey(name);
        }

        public void RegisterVirtualAxis(CrossPlatformInput.VirtualAxis axis)
        {
            // check if we already have an axis with that name and log and error if we do
            if (virtualAxes.ContainsKey(axis.Name))
            {
                Debug.LogError("There is already a virtual axis named " + axis.Name + " registered.");
            }
            else
            {
                // add any new axes
                virtualAxes.Add(axis.Name, axis);
            }
        }

        public void RegisterVirtualButton(CrossPlatformInput.VirtualButton button)
        {
            // check if already have a buttin with that name and log an error if we do
            if (virtualButtons.ContainsKey(button.Name))
            {
                Debug.LogError("There is already a virtual button named " + button.Name + " registered.");
            }
            else
            {
                // add any new buttons
                virtualButtons.Add(button.Name, button);
            }
        }

        public void UnRegisterVirtualAxis(string name)
        {
            // if we have an axis with that name then remove it from our dictionary of registered axes
            if (virtualAxes.ContainsKey(name))
            {
                virtualAxes.Remove(name);
            }
        }

        public void UnRegisterVirtualButton(string name)
        {
            // if we have a button with this name then remove it from our dictionary of registered buttons
            if (virtualButtons.ContainsKey(name))
            {
                virtualButtons.Remove(name);
            }
        }

        // returns a reference to a named virtual axis if it exists otherwise null
        public CrossPlatformInput.VirtualAxis VirtualAxisReference(string name)
        {
            return virtualAxes.ContainsKey(name) ? virtualAxes[name] : null;
        }

        public void SetVirtualMousePositionX(float f)
        {
            VirtualMousePosition = new Vector3(f, VirtualMousePosition.y, VirtualMousePosition.z);
        }

        public void SetVirtualMousePositionY(float f)
        {
            VirtualMousePosition = new Vector3(VirtualMousePosition.x, f, VirtualMousePosition.z);
        }

        public void SetVirtualMousePositionZ(float f)
        {
            VirtualMousePosition = new Vector3(VirtualMousePosition.x, VirtualMousePosition.y, f);
        }

        public abstract float GetAxis(string name, bool raw);

        public abstract bool GetButton(string name);

        public abstract bool GetButtonDown(string name);

        public abstract bool GetButtonUp(string name);

        public abstract void SetButtonDown(string name);

        public abstract void SetButtonUp(string name);

        public abstract void SetAxisPositive(string name);

        public abstract void SetAxisNegative(string name);

        public abstract void SetAxisZero(string name);

        public abstract void SetAxis(string name, float value);

        public abstract Vector3 MousePosition();
    }
}
