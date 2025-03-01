﻿#if NORMCORE

using UnityEngine;

namespace Normal.Realtime.Examples
{
    public class VoiceMouthMove : MonoBehaviour
    {
        public Transform mouth;

        private RealtimeAvatarVoice _voice;
        private float _mouthSize;

        private void Awake()
        {
            // Get a reference to the RealtimeAvatarVoice component
            _voice = GetComponent<RealtimeAvatarVoice>();
        }

        private void Update()
        {
            // Use the current voice volume (a value between 0 - 1) to calculate the target mouth size (between 0.1 and 1.0)
            var targetMouthSize = Mathf.Lerp(0.1f, 1.0f, _voice.voiceVolume);

            // Animate the mouth size towards the target mouth size to keep the open / close animation smooth
            _mouthSize = Mathf.Lerp(_mouthSize, targetMouthSize, 30.0f * Time.deltaTime);

            // Apply the mouth size to the scale of the mouth geometry
            var localScale = mouth.localScale;
            localScale.y = _mouthSize;
            mouth.localScale = localScale;
        }
    }
}

#endif