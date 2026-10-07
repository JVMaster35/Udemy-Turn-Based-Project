using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Cinemachine;

public class CameraController : MonoBehaviour
{
    private const float MIN_FOLLOW_Y_OFFSET = 2f;
    private const float MAX_FOLLOW_Y_OFFSET = 15f;

    public static CameraController Instance { get; private set; }


    [SerializeField] private CinemachineCamera _virtualCamera;
    private CinemachineFollow cinemachineFollow;

    private Vector3 targetFollowOffset;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("There's more than one CameraController! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        cinemachineFollow = _virtualCamera.GetComponent<CinemachineFollow>();
        targetFollowOffset = cinemachineFollow.FollowOffset;
    }

    private void Update()
    {
        MoveCamera();
        RotateCamera();

        ZoomCamera();
    }

    private void MoveCamera()
    {
        Vector2 inputMoveDir = InputManager.Instance.GetCameraMoveVector();

        float moveSpeed = 5f;

        Vector3 moveVector = transform.forward * inputMoveDir.y + transform.right * inputMoveDir.x;
        transform.position += moveVector * moveSpeed * Time.deltaTime;
    }

    private void RotateCamera()
    {
        Vector3 rotationVector = new Vector3(0, 0, 0);

        rotationVector.y = InputManager.Instance.GetCameraRotateAmount();

        float rotationSpeed = 50f;
        transform.eulerAngles += rotationVector * rotationSpeed * Time.deltaTime;
    }

    private void ZoomCamera()
    {
        float zoomIncreaseAmount = 1f;

        targetFollowOffset.y += InputManager.Instance.GetCameraZoomAmount() * zoomIncreaseAmount;

        float zoomSpeed = 5f;
        targetFollowOffset.y = Mathf.Clamp(targetFollowOffset.y, MIN_FOLLOW_Y_OFFSET, MAX_FOLLOW_Y_OFFSET);

        cinemachineFollow.FollowOffset =
            Vector3.Lerp(cinemachineFollow.FollowOffset, targetFollowOffset, Time.deltaTime * zoomSpeed);
    }

    public float GetCameraHeight()
    {
        return targetFollowOffset.y;
    }
}
