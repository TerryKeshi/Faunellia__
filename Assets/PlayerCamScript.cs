using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamScript : MonoBehaviour
{
    public Transform _player;
    public GameObject _avatar;
    float _distance = 6f; // расстояние от камеры до player

    public float sensetivityX;
    public float sensetivityY;

    public Transform orientation;
    public Inventory inventory;

    public float xRotation;
    public float yRotation;

    private Vector3 _previousPlayerPosition;

    public Camera showingCamera;

    public float _playerHeight;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.KeypadPlus))
        {
            _distance -= 0.3f;
            if (_distance < 0)
                _distance = 0;

            if (_distance < 1f)
                _avatar.SetActive(false);
        }

        if (Input.GetKey(KeyCode.KeypadMinus))
        {
            _distance += 0.3f;
            if (_distance >= 1f)
                _avatar.SetActive(true);
        }

        if (!showingCamera.enabled)
            if (!inventory.opened)
                if (!inventory._marketOpened)
                {
                    float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensetivityX;
                    float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensetivityY;

                    xRotation += mouseX;
                    yRotation -= mouseY;

                    yRotation = Mathf.Clamp(yRotation, -90f, 90f);

                    transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
                    orientation.rotation = Quaternion.Euler(yRotation, xRotation, 0);

                    Vector3 movementDirection = _player.transform.position - _previousPlayerPosition;
                    movementDirection.y = 0;

                    if (movementDirection.magnitude > 0.02f)
                    {
                        float rotationAngle = -Mathf.Atan2(movementDirection.z, movementDirection.x) * Mathf.Rad2Deg;
                        rotationAngle += 90f;
                        while (rotationAngle > 360f)
                            rotationAngle -= 360f;

                        _avatar.transform.rotation = Quaternion.Euler(0, rotationAngle, 0);
                    }

                    _previousPlayerPosition = _player.transform.position;
                }

        Vector3 upper = new Vector3(0, _playerHeight, 0);

        float subDistance = _distance;
        RaycastHit hit;
        if (Physics.Raycast(_player.position + upper, -transform.forward, out hit, _distance))
            subDistance = hit.distance - 0.02f;

        transform.position = _player.position + upper - transform.forward * subDistance;
    }

    public void Rotate(float v)
    {
        xRotation += v;

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(yRotation, xRotation, 0);
    }

    public void StaticRotate(float xRot, float yRot)
    {
        xRotation = xRot;
        yRotation = yRot;
        transform.rotation = Quaternion.Euler(xRot, yRot, 0);
        orientation.rotation = Quaternion.Euler(yRot, xRot, 0);
    }
}
