using Normal.Realtime;
using UnityEngine;

namespace Sync
{
    public class ColorTest : MonoBehaviour
    {
        [SerializeField] private Color _color = default;
        private Color _previousColor = default;

        [SerializeField] private float _gain;
        private float _previousGain;

        //private TestSync _colorSync;
        private GameObject leftHand, rightHand;
        private Color newColor = new Color(0, 0, 0, 1);
        private Color newColor2 = new Color(1, 1, 1, 1);
        private GameObject myObject;
        private RealtimeView myView;
        private TestSync _colorSync;
        private float volume = 0.1f;

        private void Awake()
        {
            //Get a reference to the color sync component
            _colorSync = GetComponent<TestSync>();

            leftHand = GameObject.Find("LeftHandAnchor");
            rightHand = GameObject.Find("RightHandAnchor");
            myObject = GameObject.Find("Sphere");
            myView = myObject.GetComponent<RealtimeView>();
        }


        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == leftHand || other.gameObject == rightHand)
            {
                myView.RequestOwnership();
                _color = newColor;
                _gain = volume;
                Debug.Log("HAND");
                if (other.gameObject == leftHand)
                    OVRInput.SetControllerVibration(1, 1, OVRInput.Controller.LTouch);
                else
                    OVRInput.SetControllerVibration(1, 1, OVRInput.Controller.RTouch);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject == leftHand || other.gameObject == rightHand)
            {
                _color = newColor2;
                _gain = 0;
                if (other.gameObject == leftHand)
                    OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.LTouch);
                else
                    OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.RTouch);
                myView.ClearOwnership();
            }
        }

        private void Update()
        {
            // If the color has changed (via the inspector), call SetColor on the color sync component.
            if (_color != _previousColor)
            {
                _colorSync.SetColor(_color);
                _previousColor = _color;
            }

            if (_gain != _previousGain)
            {
                _colorSync.SetGain(_gain);
                _previousGain = _gain;
            }
        }
    }
}