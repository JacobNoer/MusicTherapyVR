using UnityEngine;
using UnityEngine.SceneManagement;

namespace Animations.Scripts
{
    public class MenuButton : MonoBehaviour
    {
        [SerializeField] private MenuButtonController menuButtonController;
        [SerializeField] private Animator animator;
        [SerializeField] private AnimatorFunctions animatorFunctions;
        [SerializeField] private int thisIndex;

        // Update is called once per frame
        private void Update()
        {
            if (menuButtonController.index == thisIndex)
            {
                animator.SetBool("selected", true);
                if (Input.GetAxis("Submit") == 1)
                {
                    animator.SetBool("pressed", true);

                    if (thisIndex == 0)
                        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                    else if (thisIndex == 1) Application.Quit();
                }
                else if (animator.GetBool("pressed"))
                {
                    animator.SetBool("pressed", false);
                    animatorFunctions.disableOnce = true;
                }
            }
            else
            {
                animator.SetBool("selected", false);
            }
        }
    }
}