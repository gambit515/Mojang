using UnityEngine;

public class IsJoystickOn : MonoBehaviour
{
    [SerializeField] private GameObject joystick;
    [SerializeField] private GameObject jumpButton;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(joystick.activeSelf)
            jumpButton.SetActive(true);
        else
            jumpButton.SetActive(false);
    }
}
