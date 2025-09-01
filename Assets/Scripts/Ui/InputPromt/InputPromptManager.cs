using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
public enum ControlType
{
	Keyboard = 0,
	Gamepad = 1
}
public class InputPromptManager : MonoBehaviour
{
	public static InputPromptManager Instance { get; private set; }
	[SerializeField] public InputActionAsset inputActions;
	private InputDevice device;
	public ControlType controlType;
	public UnityEvent OnControlChange;
	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			Destroy(gameObject);
			return;
		}
		DontDestroyOnLoad(gameObject);
		controlType = ControlType.Keyboard;
	}

	private void OnEnable()
	{
		foreach (var map in inputActions.actionMaps)
		{
			foreach (var action in map.actions)
			{
				action.performed += OnActionPerformed;
				action.Enable();
			}
		}
	}

	private void OnDisable()
	{
		foreach (var map in inputActions.actionMaps)
		{
			foreach (var action in map.actions)
			{
				action.performed -= OnActionPerformed;
				action.Disable();
			}
		}
	}

	private void OnActionPerformed(InputAction.CallbackContext ctx)
	{
		if (device != ctx.control.device)
		{
			device = ctx.control.device;
			if (device is Gamepad)
			{
				controlType = ControlType.Gamepad;
				Debug.Log("Scheme = Gamepad");
			}
			else if (device is Keyboard || device is Mouse)
			{
				controlType = ControlType.Keyboard;
			}
		}
		OnControlChange.Invoke();
	}
}
