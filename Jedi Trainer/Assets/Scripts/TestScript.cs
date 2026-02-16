using UnityEngine;
using UnityEngine.InputSystem;

public class TestScript : MonoBehaviour
{
    public InputActionReference activateAction;

    private void OnEnable()
    {
        activateAction.action.Enable();
        activateAction.action.performed += OnTriggerPressed;
    }

    private void OnDisable()
    {
        activateAction.action.performed -= OnTriggerPressed;
    }

    private void OnTriggerPressed(InputAction.CallbackContext context)
    {
        Debug.Log("Trigger pressed!");
    }
}
