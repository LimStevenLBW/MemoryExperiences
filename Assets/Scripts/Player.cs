using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float mouseSensitivity;
    public float yaw;
    public float pitch;
    public Camera playerCamera;
    public InputFieldPrompt inputField;


    // Start is called before the first frame update
    void Start()
    {
       // Cursor.lockState = CursorLockMode.Locked;
    }
    void CameraControl()
    {
        yaw = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * mouseSensitivity;
        pitch -= mouseSensitivity * Input.GetAxis("Mouse Y");
        pitch = Mathf.Clamp(pitch, -80, 80);
        transform.localEulerAngles = new Vector3(0, yaw, 0);
        playerCamera.transform.localEulerAngles = new Vector3(pitch, 0, 0);
   
    }

    // Update is called once per frame
    void Update()
    {
        CameraControl();
        Vector3 direction = Vector3.zero;

        if (inputField.activated) return;

        if (Input.GetKey("w"))
        {
            direction += transform.forward;
        }
        if (Input.GetKey("s"))
        {
            direction -= transform.forward;
        }
        if (Input.GetKey("d"))
        {
            direction += transform.right;
        }
        if (Input.GetKey("a"))
        {
            direction -= transform.right;
        }

        transform.position += direction * 4.5f * Time.deltaTime;
    }
}
