using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;


public class RotationSlider : MonoBehaviour
{
    public Slider xRotationSlider; // Assign the slider in the Unity Inspector
    public Slider yRotationSlider; // Assign the slider in the Unity Inspector
    public GameObject targetObject; // The object you want to rotate
    public float xMinRotation = 40f; // Minimum rotation angle
    public float xMaxRotation = 8f; // Maximum rotation angle
    public float yMinRotation = 250f; // Minimum rotation angle
    public float yMaxRotation = 128f; // Maximum rotation angle
    public float keyRotationStep = 0.01f; // The step size for rotation adjustment using arrow keys
    
    void Start()
    {
        if (xRotationSlider != null)
        {
            // Set the slider's min and max values
            xRotationSlider.minValue = 0f;
            xRotationSlider.maxValue = 1f;
    
            // Optionally, initialize the slider's value to reflect the object's current rotation
            float initialRotation = Mathf.InverseLerp(xMinRotation, xMaxRotation, targetObject.transform.eulerAngles.x);
            xRotationSlider.value = initialRotation;
    
            // Add listener for slider value changes
            xRotationSlider.onValueChanged.AddListener(OnXSliderValueChanged);
        }
        
        if (yRotationSlider != null)
        {
            // Set the slider's min and max values
            yRotationSlider.minValue = 0f;
            yRotationSlider.maxValue = 1f;
    
            // Optionally, initialize the slider's value to reflect the object's current rotation
            float initialRotation = Mathf.InverseLerp(yMinRotation, yMaxRotation, targetObject.transform.eulerAngles.y);
            yRotationSlider.value = initialRotation;
    
            // Add listener for slider value changes
            yRotationSlider.onValueChanged.AddListener(OnYSliderValueChanged);
        }
    }
    
    void Update()
    {
        HandleKeyboardInput();
    }
    
    void HandleKeyboardInput()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            // Decrease X rotation
            xRotationSlider.value = Mathf.Clamp(xRotationSlider.value - keyRotationStep, xRotationSlider.minValue, xRotationSlider.maxValue);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            // Increase X rotation
            xRotationSlider.value = Mathf.Clamp(xRotationSlider.value + keyRotationStep, xRotationSlider.minValue, xRotationSlider.maxValue);
        }
    
        if (Input.GetKey(KeyCode.RightArrow))
        {
            // Increase Y rotation
            yRotationSlider.value = Mathf.Clamp(yRotationSlider.value + keyRotationStep, yRotationSlider.minValue, yRotationSlider.maxValue);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            // Decrease Y rotation
            yRotationSlider.value = Mathf.Clamp(yRotationSlider.value - keyRotationStep, yRotationSlider.minValue, yRotationSlider.maxValue);
        }
    }
    
    void OnXSliderValueChanged(float value)
    {
        // Map the slider value (0-1) to the desired rotation range
        float xRotationAngle = Mathf.Lerp(xMinRotation, xMaxRotation, value);
        
        // Apply the rotation to the target object on the y-axis
        targetObject.transform.rotation = Quaternion.Euler(new Vector3(xRotationAngle, this.targetObject.transform.rotation.eulerAngles.y, 0f));
    }
    void OnYSliderValueChanged(float value)
    {
        // Map the slider value (0-1) to the desired rotation range
        float yRotationAngle = Mathf.Lerp(yMinRotation, yMaxRotation, value);
        
        // Apply the rotation to the target object on the y-axis
        targetObject.transform.rotation = Quaternion.Euler(new Vector3(this.targetObject.transform.rotation.eulerAngles.x, yRotationAngle, 0f));
    }
}
