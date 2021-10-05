using UnityEngine;

namespace Animations.Scripts
{
    public class AnimatorFunctions : MonoBehaviour
    {
        [SerializeField] private MenuButtonController menuButtonController;
        public bool disableOnce;

        private void PlaySound(AudioClip whichSound)
        {
            if (!disableOnce)
                menuButtonController.audioSource.PlayOneShot(whichSound);
            else
                disableOnce = false;
        }
    }
}