using UnityEngine;

namespace Animations.Scripts
{
    public class MenuButtonController : MonoBehaviour
    {
        // Use this for initialization
        public int index;
        [SerializeField] private bool keyDown;
        [SerializeField] private int maxIndex;
        public AudioSource audioSource;

        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
        }

        // Update is called once per frame
        private void Update()
        {
            if (Input.GetAxis("Vertical") != 0)
            {
                if (!keyDown)
                {
                    if (Input.GetAxis("Vertical") < 0)
                    {
                        if (index < maxIndex)
                            index++;
                        else
                            index = 0;
                    }
                    else if (Input.GetAxis("Vertical") > 0)
                    {
                        if (index > 0)
                            index--;
                        else
                            index = maxIndex;
                    }

                    keyDown = true;
                }
            }
            else
            {
                keyDown = false;
            }
        }
    }
}