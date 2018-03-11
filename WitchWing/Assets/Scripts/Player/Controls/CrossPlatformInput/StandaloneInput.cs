namespace UnityStandardAssets.CrossPlatformInput.PlatformSpecific
{
    using System;
    using UnityEngine;

    public class StandaloneInput : VirtualInput
    {
        private const string notPossibleForStandAlone =
                "This is not possible to be called for standalone input. Please check your platform and code where this is called.";

        public override float GetAxis(string name, bool raw)
        {
            return raw ? Input.GetAxisRaw(name) : Input.GetAxis(name);
        }

        public override bool GetButton(string name)
        {
            return Input.GetButton(name);
        }

        public override bool GetButtonDown(string name)
        {
            return Input.GetButtonDown(name);
        }

        public override bool GetButtonUp(string name)
        {
            return Input.GetButtonUp(name);
        }

        public override void SetButtonDown(string name)
        {
            throw new Exception(notPossibleForStandAlone);
        }

        public override void SetButtonUp(string name)
        {
            throw new Exception(notPossibleForStandAlone);
        }

        public override void SetAxisPositive(string name)
        {
            throw new Exception(notPossibleForStandAlone);
        }

        public override void SetAxisNegative(string name)
        {
            throw new Exception(notPossibleForStandAlone);
        }

        public override void SetAxisZero(string name)
        {
            throw new Exception(notPossibleForStandAlone);
        }

        public override void SetAxis(string name, float value)
        {
            throw new Exception(notPossibleForStandAlone);
        }

        public override Vector3 MousePosition()
        {
            return Input.mousePosition;
        }
    }
}
