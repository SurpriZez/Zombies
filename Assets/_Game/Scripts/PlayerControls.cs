using UnityEngine;
using Mirror;

public class PlayerControls : NetworkBehaviour
{
	private CharacterController _characterController;
	
	[SerializeField] private float _mouseSensitivity = 100f;
	[SerializeField] private float _speed = 4f;
	
	
	[SerializeField] private float _gravity = -9.81f;
	[SerializeField] private Vector3 _velocity;
	[SerializeField] private Transform _groundCheck;
	[SerializeField] private float _groundDistance = 0.4f;
	[SerializeField] private LayerMask _groundMask;
	[SerializeField] private float _jumpHeight = 3f;

	private bool _isGrounded;
	private float _xRotation;
	private Camera _camera;
	private void Awake()
	{
		_characterController = GetComponent<CharacterController>();
		_camera = Camera.main;
	}

	void Update()
	{
		if (!isLocalPlayer) return;


		if (Input.GetKeyDown(KeyCode.L))
		{
			Cursor.lockState = Cursor.lockState == CursorLockMode.None ? CursorLockMode.Locked : CursorLockMode.None;
		}


		HandleCameraView();
		HandleMovement();
		HandleJumping();
	}

	private void HandleJumping()
	{
		_isGrounded = Physics.CheckSphere(_groundCheck.position, _groundDistance, _groundMask);
		if (_isGrounded && _velocity.y < 0f)
		{
			_velocity.y = -2f;
		}

		if (Input.GetButtonDown("Jump") && _isGrounded)
		{
			_velocity.y = Mathf.Sqrt(_jumpHeight * -2f * _gravity);
		}
		_velocity.y += _gravity * Time.deltaTime;
		
		_characterController.Move(_velocity * Time.deltaTime);
	}

	private void HandleMovement()
	{
		var moveX = Input.GetAxis("Horizontal");
		var moveZ = Input.GetAxis("Vertical");
		var movementAxis = transform.right * moveX + transform.forward * moveZ;
		_characterController.Move(movementAxis * (_speed * Time.deltaTime));
	}

	
	

	private void HandleCameraView()
	{
		var mouseX = Input.GetAxis("Mouse X") * _mouseSensitivity * Time.deltaTime;
		var mouseY = Input.GetAxis("Mouse Y") * _mouseSensitivity * Time.deltaTime;

		_xRotation -= mouseY;
		_xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

		_camera.transform.localRotation = Quaternion.Euler(_xRotation, 0, 0);
		transform.Rotate(Vector3.up * mouseX);
	}
}