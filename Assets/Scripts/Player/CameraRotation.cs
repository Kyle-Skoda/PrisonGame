using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : PlayerComponent
{
    [SerializeField] private Camera cam;
    [SerializeField] private float xSensitivity = 100;
    [SerializeField] private float ySensitivity = 100;
    [SerializeField] private float cameraLerp = 3;

    private float xRot = 0f;

    public override void Init()
    {
        base.Init();

        if (player.photonView.IsMine)
            cam.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        Vector2 mouseDir = new Vector2(player.Input.MouseInputDelta.x * xSensitivity, player.Input.MouseInputDelta.y * ySensitivity) * Time.deltaTime;

        xRot -= mouseDir.y;
        xRot = Mathf.Clamp(xRot, -90, 90);

        cam.transform.localRotation = Quaternion.Slerp(cam.transform.localRotation, Quaternion.Euler(xRot, 0, 0), Time.deltaTime * cameraLerp);
        transform.Rotate(Vector3.up * mouseDir.x);
    }

    public Camera GetCamera() => cam;
}
