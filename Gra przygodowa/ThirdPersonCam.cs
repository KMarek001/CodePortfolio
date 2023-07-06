using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCam : MonoBehaviour
{
    [Header("References")]
    private Transform orientation;
    private GameObject player;
    private Transform playerTransform;
    private Transform playerObj;

    public float rotationSpeed;

    private void Start()
    {
        player = GameObject.Find("Player");
        playerTransform = player.transform;

        orientation = playerTransform.Find("Orientation");
        playerObj = playerTransform.Find("PlayerObj");
        

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        Vector3 viewDir = playerTransform.position - new Vector3(transform.position.x, playerTransform.position.y, transform.position.z);
        orientation.forward = viewDir.normalized;

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (inputDir != Vector3.zero)
            playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
    }
}
