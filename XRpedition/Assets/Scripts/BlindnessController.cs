using UnityEngine;
using UnityEngine.InputSystem;

public class BlindnessController : MonoBehaviour
{
    public Material cvdMaterial;
    public InputActionReference cvdAction;
    private int mode = 0;
    private readonly int maxModes = 5;

    private void OnEnable()
    {
        cvdAction.action.performed += OnToggle;
        cvdAction.action.Enable();
    }

    private void OnDisable()
    {
        cvdAction.action.performed -= OnToggle;
        cvdAction.action.Disable();
    }

    private void OnToggle(InputAction.CallbackContext context)
    {
        mode = (mode + 1) % maxModes;
        cvdMaterial.SetInt("_Mode", mode);
        
    }
}
