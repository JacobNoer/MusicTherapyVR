using UnityEngine;

namespace NormcoreIntegration
{
    public class DisableWithoutVR : MonoBehaviour
    {
        // Start is called before the first frame update
        private void Start()
        {
            if (!InitializeNormcore.VREnabled)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
