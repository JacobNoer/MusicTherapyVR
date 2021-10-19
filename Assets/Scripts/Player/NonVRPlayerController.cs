using NormcoreIntegration;
using UnityEngine;

namespace Player
{
    public class NonVRPlayerController : MonoBehaviour
    {
        private const float StickToGroundForce = 10;
        private const float TransitionSpeed = 20.0f;

        [Header("Movement")] [SerializeField] [Range(0.0f, 10.0f)]
        private float movementSpeed = 6.0f;

        [SerializeField] [Range(0.0f, 20.0f)] private float jumpStrength = 10.0f;
        [SerializeField] [Range(0.0f, 5.0f)] private float gravityMultiplier = 2.0f;

        [Header("Camera")] [SerializeField] [Range(0.0f, 1.0f)]
        private float mouseSensitivity = 0.2f;

        [SerializeField] private bool lockCursor = true;

        [Header("Head-bob")] [SerializeField] private bool headBobEnabled = true;
        [SerializeField] private float bobAmplitude = 0.07f;
        [SerializeField] private float bobFrequency = 12f;

        private float _cameraPitch;
        private CharacterController _characterController;
        private Vector3 _currentDirection = Vector3.zero;
        private bool _isJumping;
        private Camera _playerCamera;
        private Vector3 _restPosition;
        private float _timer;

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
        }

        private void Start()
        {
            _playerCamera = transform.GetChild(1).GetComponent<Camera>();
            if (!InitializeNormcore.VREnabled)
            {
                _restPosition = _playerCamera.transform.localPosition;
                GetComponent<OVRPlayerController>().enabled = false;
                GetComponent<OVRSceneSampleController>().enabled = false;
                GetComponent<OVRDebugInfo>().enabled = false;
            }
            else
            {
                _playerCamera.gameObject.SetActive(false);
                enabled = false;
            }

            if (lockCursor)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

        private void Update()
        {
            UpdateMouseLook();

            var movement = UpdateMovement(); //Cache value to avoid calling update movement more than once per frame
            _characterController.Move(movement * Time.deltaTime);
            if (headBobEnabled) UpdateHeadBob(movement);
        }

        private void UpdateMouseLook()
        {
            var mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

            _cameraPitch -= mouseDelta.y * mouseSensitivity;
            _cameraPitch = Mathf.Clamp(_cameraPitch, -90.0f, 90.0f);

            _playerCamera.transform.localEulerAngles = Vector3.right * _cameraPitch;
            transform.Rotate(Vector3.up * (mouseDelta.x * mouseSensitivity));
        }

        private Vector3 UpdateMovement()
        {
            int horizontalMovement = 0, forwardMovement = 0;

            if (Input.GetKey("w"))
                forwardMovement = 1;
            else if (Input.GetKey("s")) forwardMovement = -1;

            if (Input.GetKey("d"))
                horizontalMovement = 1;
            else if (Input.GetKey("a")) horizontalMovement = -1;

            var inputValues = new Vector2(horizontalMovement, forwardMovement);

            var targetDirection = new Vector3(inputValues.x, inputValues.y, 0.0f);
            targetDirection.Normalize();

            // Always move along the camera forward as it is the direction that it being aimed at
            var transform1 = transform;
            var desiredMove = transform1.forward * targetDirection.y + transform1.right * targetDirection.x;

            // Get a normal for the surface that is being touched to move along it
            Physics.SphereCast(transform1.position, _characterController.radius, Vector3.down, out var hitInfo,
                _characterController.height / 2f, Physics.AllLayers, QueryTriggerInteraction.Ignore);
            desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;

            _currentDirection.x = desiredMove.x * movementSpeed;
            _currentDirection.z = desiredMove.z * movementSpeed;

            if (_characterController.isGrounded)
            {
                _currentDirection.y = -StickToGroundForce;

                if (_isJumping) _currentDirection.y = jumpStrength;
                _isJumping = false;
            }
            else
            {
                _currentDirection += Physics.gravity * (gravityMultiplier * Time.deltaTime);
            }

            return _currentDirection;
        }

        private void UpdateHeadBob(Vector3 move)
        {
            if (!_characterController.isGrounded) return;

            if (Mathf.Abs(move.x) > 0.1f || Mathf.Abs(move.z) > 0.1f)
            {
                _timer += bobFrequency * Time.deltaTime;

                var localPosition = _playerCamera.transform.localPosition;
                localPosition = new Vector3(
                    Mathf.Lerp(localPosition.x, Mathf.Cos(_timer / 2) * bobAmplitude, TransitionSpeed),
                    Mathf.Lerp(localPosition.y, _restPosition.y + Mathf.Sin(_timer) * bobAmplitude, TransitionSpeed),
                    localPosition.z
                );
                _playerCamera.transform.localPosition = localPosition;
            }
            else
            {
                var localPosition = _playerCamera.transform.localPosition;
                localPosition = new Vector3(
                    Mathf.Lerp(localPosition.x, _restPosition.x, TransitionSpeed * Time.deltaTime),
                    Mathf.Lerp(localPosition.y, _restPosition.y, TransitionSpeed * Time.deltaTime),
                    _restPosition.z
                );
                _playerCamera.transform.localPosition = localPosition;
            }
        }
    }
}