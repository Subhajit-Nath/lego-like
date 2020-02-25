using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [SerializeField] private Transform playerBody;
    [SerializeField] private float mouseSensitivity = 100f;
    float xRotation = 0f;

    private void Update()
    {
        float x = Input.GetAxis("Mouse X") * Time.deltaTime * mouseSensitivity;
        float y = Input.GetAxis("Mouse Y") * Time.deltaTime * mouseSensitivity;

        xRotation -= y;
        xRotation = Mathf.Clamp(xRotation, -85f, 85f);
        playerBody.Rotate(Vector3.up * x);
        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
    }
}
