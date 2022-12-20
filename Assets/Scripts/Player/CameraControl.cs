using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {
    [SerializeField] private Transform player;

    private float xRotation = 0;
    
    private void Start() {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update() {
        CameraUpdates();

        //Cursor.lockState = CursorLockMode.Locked;
    }

    private void CameraUpdates() {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        player.Rotate(Vector3.up * mouseX);

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90, 90);

        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
    }
}
