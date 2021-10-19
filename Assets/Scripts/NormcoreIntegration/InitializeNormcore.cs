using System.Collections.Generic;
using System.Linq;
using Normal.Realtime;
using UnityEngine;
using UnityEngine.XR;

namespace NormcoreIntegration
{
    public class InitializeNormcore : MonoBehaviour
    {
        public static bool VREnabled = true;
        private Realtime _realtime;

        private void Awake()
        {
            var inputDevices = new List<InputDevice>();
            InputDevices.GetDevices(inputDevices);

            if (!inputDevices.Any())
            {
                Debug.LogWarning("No XR device connected. Starting NonVRPLayerCharacter");
                VREnabled = false;
            }
        }

        private void Start()
        {
            _realtime = GetComponent<Realtime>();
            _realtime.Connect(Application.isEditor ? "Dev Room" : "Test Room");
        }
    }
}