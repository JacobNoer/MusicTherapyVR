using Normal.Realtime;
using UnityEngine;

namespace Drum
{
    public class DrumPlay : MonoBehaviour
    {
        private DrumSync _drumSync;
        private GameObject leftHand, rightHand;
        private GameObject myObject, DrumSkin;
        private RealtimeView myView;

        private Color DrumSkinOriginal = new Color(204, 204, 169, 255);
        private Color DrumSkinHit = new Color(89, 89, 89, 255);

        [SerializeField] public bool _play;
        private bool _prevPlay;
        private Color _color;
        private Color _prevColor;
        private AudioSource source;
        private MeshRenderer meshRenderer;

        private void Awake()
        {
            //DrumSkin = GameObject.Find("Skin");
            _drumSync = GetComponent<DrumSync>();
            //meshRenderer = DrumSkin.GetComponent<MeshRenderer>();
            leftHand = GameObject.Find("LeftHandAnchor");
            rightHand = GameObject.Find("RightHandAnchor");
            myObject = GameObject.Find("drumCollider"); // skift navn hvis det andet object
            myView = myObject.GetComponent<RealtimeView>();
            source = GetComponent<AudioSource>();
            //_color = DrumSkinOriginal;
            _play = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            myView.RequestOwnership();
            if (other.gameObject == leftHand || other.gameObject == rightHand)
                _play = !_play;
            //color();
        }

        private void color()
        {
            if (_play == true)
            {
                _color = DrumSkinOriginal;
            }
            else if (_play == false)
            {
                _color = DrumSkinHit;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            myView.ClearOwnership();
        }

        private void Update()
        {
            if (_color != _prevColor)
            {
                _drumSync.SetColor(_color);
                _prevColor = _color;
            }

            if (_play != _prevPlay)
            {
                _drumSync.SetPlay(_play);
                _prevPlay = _play;
            }
        }
    }
}