using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private float checkRate = 0.05f;
    private float lastCheckTime;
    [SerializeField] private float maxCheckDistance;
    [SerializeField] private LayerMask layerMask;

    [SerializeField] private GameObject curInteractGameObject;
    private IInteractable curInteractable;

    [SerializeField] private TextMeshProUGUI promptText;
    private Camera camera;

    private PlayerController controller;

    void Start()
    {
        camera = Camera.main;
        controller = GetComponent<PlayerController>();
    }

    void Update()
    {
        if (Time.time - lastCheckTime > checkRate)
        {
            lastCheckTime = Time.time;

            Ray ray = new Ray(controller.CameraContainer.position, camera.transform.forward);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, maxCheckDistance, layerMask))
            {
                if (hit.collider.gameObject != curInteractGameObject)
                {
                    curInteractGameObject = hit.collider.gameObject;
                    curInteractable = hit.collider.GetComponent<IInteractable>();
                    SetPromptText();
                }
            }
            else
            {
                curInteractGameObject = null;
                curInteractable = null;

                promptText.gameObject.SetActive(false);
            }
        }
    }

    private void SetPromptText()
    {
        if (promptText == null)
        {
            Debug.LogError("[PlayerInteraction] promptText가 연결되지 않았습니다.");
            return;
        }

        if (curInteractable == null)
        {
            Debug.LogWarning("[PlayerInteraction] curInteractable이 null입니다.");
            promptText.gameObject.SetActive(false);
            return;
        }
        promptText.gameObject.SetActive(true);
        promptText.text = curInteractable.GetInteractPrompt();
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && curInteractable != null)
        {
            curInteractable.OnInteract();
            curInteractGameObject = null;
            curInteractable = null;
            promptText.gameObject.SetActive(false);
        }
    }
}
