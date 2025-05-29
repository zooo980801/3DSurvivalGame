using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] private float moveSpeed;   // 플레이어 이동 속도
    [SerializeField] private float jumpPower;   // 플레이어 점프력
    [SerializeField] private LayerMask groundLayer; // 바닥 체크를 위한 레이어마스크
    private Vector2 moveDirection;              // 이동 방향 

    public float MoveSpeed { get { return moveSpeed; } set { moveSpeed = value; } }

    [Header("Look")]
    [SerializeField] private Transform mainCamera;  // 메인 카메라
    [SerializeField] private Transform cameraContainer; // 카메라 회전을 담당하는 오브젝트
    [SerializeField] private float lookSensitivity; // 마우스 감도
    [SerializeField] private float minXLook;    // 내려다볼 수 있는 각도
    [SerializeField] private float maxXLook;    // 올려다볼 수 있는 각도
    [SerializeField] private LayerMask cameraCollision;

    private float camCurXRot;       //  현재 카메라의 상하 회전값
    private Vector2 mouseDelta;     // 프레임마다 입력된 마우스 이동값
    private float firstPersonZ = 0f;    // 1인칭 위치
    private float thirdPersonZ = -12f;   // 3인칭 위치
    private bool isFirstPerson = true;  // 현재 몇인칭인지 확인

    private Rigidbody _rigidbody;
    private PlayerAnimationHandler animationHandler;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        animationHandler = GetComponent<PlayerAnimationHandler>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;   // 화면 중앙에 마우스 커서 고정
    }

    private void FixedUpdate()
    {
        Movement();     // 플레이어 이동
    }

    private void LateUpdate()
    {
        Look();         // 카메라 화면 회전
    }

    #region 플레이어 이동
    void Movement()
    {
        Vector3 dir = transform.forward * moveDirection.y + transform.right * moveDirection.x;  // 입력값 기준으로 이동 방향을 계산
        dir *= moveSpeed;               // 이속 적용
        dir.y = _rigidbody.velocity.y;  // y축 속도 유지 
        _rigidbody.velocity = dir;      // 최종 이동 벡터 적용
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            moveDirection = context.ReadValue<Vector2>();
            animationHandler.Walk(true);
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            moveDirection = Vector2.zero;   // 이동 방향 초기화
            animationHandler.Walk(false);
        }
    }
    #endregion

    private void Update()
    {
        Debug.DrawRay(cameraContainer.position, mainCamera.transform.position - cameraContainer.position);
    }

    #region 카메라 화면 회전
    void Look()
    {
        camCurXRot += mouseDelta.y * lookSensitivity;   // 상하 회전값 계산
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);   // 회전 각도 제한
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);  // 카메라 상하 회전

        // 플레이어 오브젝트 좌우로 회전
        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);

        if (!isFirstPerson)
        {
            Vector3 rayDir = mainCamera.transform.position - cameraContainer.position;

            RaycastHit hit;
            if (Physics.Raycast(cameraContainer.position, rayDir, out hit, 8f, cameraCollision))
            {
                mainCamera.position = hit.point - rayDir.normalized;
            }
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }
    #endregion

    #region 점프
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            if (OnGround()) // 플레이어가 바닥에 있을 경우
            {
                _rigidbody.AddForce(Vector3.up * jumpPower, ForceMode.Impulse); // 점프
            }
        }
    }

    bool OnGround()
    {
        Ray[] rays = new Ray[4] // 플레이어 아래 방향으로 4개의 레이를 쏨
        {
            new Ray(transform.position + (transform.forward * 0.2f + transform.up * 0.01f), Vector3.down),  // 앞
            new Ray(transform.position + (-transform.forward * 0.2f + transform.up * 0.01f), Vector3.down), // 뒤
            new Ray(transform.position + (transform.right * 0.2f + transform.up * 0.01f), Vector3.down),    // 오른쪽
            new Ray(transform.position + (-transform.right * 0.2f + transform.up * 0.01f), Vector3.down)    // 왼쪽
        };

        for (int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], 0.1f, groundLayer))    // 각 레이가 바닥 레이어에 닿았을 경우
            {
                return true;
            }
        }

        return false; 
    }
    #endregion

    #region 1인칭 3인칭 전환
    public void OnCameraToggle(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            isFirstPerson = !isFirstPerson;                         // 1인칭 3인칭 시점 전환
            Vector3 pos = mainCamera.localPosition;                 // 현재 카메라 위치 가져오기
            pos.z = isFirstPerson ? firstPersonZ : thirdPersonZ;    // 시점에 따라 z축 위치 설정
            mainCamera.localPosition = pos;                         // 변경된 위치를 카메라에 적용
        }
    }
    #endregion
}
