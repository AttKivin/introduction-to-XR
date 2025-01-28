using UnityEngine;
using UnityEngine.InputSystem;

public class LightSwitch : MonoBehaviour
{
    private Light lightComponent;

    public InputActionReference switchAction; // Assign button in Inspector

    void Start()
    {
        lightComponent = GetComponent<Light>();

        // Enable the input action
        switchAction.action.Enable();
        switchAction.action.performed += ctx => ChangeLightColor();
    }

    void ChangeLightColor()
    {
        // Change the light to a random color
        lightComponent.color = new Color(Random.value, Random.value, Random.value);
    }

    void OnDestroy()
    {
        switchAction.action.performed -= ctx => ChangeLightColor();
    }
}
