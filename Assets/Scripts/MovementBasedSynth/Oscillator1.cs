using System.Reflection;
using UnityEngine;

namespace MovementBasedSynth
{
    public class Oscillator1 : MonoBehaviour
    {
        public double frequency = 440;
        public float gain = 0.0f;
        private double _increment;
        private double _phase;
        private double _samplingFrequency = 48000.0;
        private GameObject _leftHand, _rightHand;

        private void Awake()
        {
            _leftHand = GameObject.Find("LeftHandAnchor");
            _rightHand = GameObject.Find("RightHandAnchor");
        }

        private void OnTriggerEnter(Collider other)
        {
            var trail = other.gameObject.GetComponent<TrailRenderer>();
            trail.enabled = true;
        }

        private void OnTriggerStay(Collider other)
        {
            var trail = other.gameObject.GetComponent<TrailRenderer>();
        }

        private void OnTriggerExit(Collider other)
        {
            var trail = other.gameObject.GetComponent<TrailRenderer>();
            trail.Clear();
            trail.enabled = false; 
        }


        private void OnAudioFilterRead(float[] data, int channels)
        {
            _increment = frequency * 2.0 * Mathf.PI / _samplingFrequency;

            for (var i = 0; i < data.Length; i += channels)
            {
                _phase += _increment;
                data[i] = gain * Mathf.Sin((float)_phase);

                if (channels == 2) data[i + 1] = data[i];
                if (_phase > Mathf.PI * 2) _phase = 0.0;
            }
        }
    }
}
