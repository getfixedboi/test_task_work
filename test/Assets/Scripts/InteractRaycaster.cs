using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Camera))]
[DisallowMultipleComponent]
public class InteractRaycaster : MonoBehaviour
{
    [SerializeField][Range(0.1f, 30f)] private float _interactDistance;
    private GameObject _mainCamera;
    private Interactable _interactable;
    private GameObject _currentHit;
    private RaycastHit _hit;
    [SerializeField] private Text _guideText;
    private bool _canInteracted = true;
    [SerializeField] private Button _grabButton;

    private Interactable _lastInteractable;

    private void Awake()
    {
        _mainCamera = this.gameObject;
    }


    private void Update()
    {
        if (Physics.Raycast(_mainCamera.transform.position, _mainCamera.transform.forward, out _hit, _interactDistance))
        {
            _currentHit = _hit.collider.gameObject;
            _interactable = _currentHit.GetComponent<Interactable>();

            if (_interactable != null)
            {
                if (_lastInteractable != _interactable)
                {
                    _lastInteractable?.OnLoseFocus();
                    CheckInteraction();
                    _lastInteractable = _interactable;
                }
                else
                {
                    CheckInteraction();
                }

                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (_canInteracted && !_interactable.IsLastInteracted)
                    {
                        StartCoroutine(C_InteractCooldown());
                        _interactable.OnInteract();
                    }
                }
            }
            else
            {
                ResetInteraction();
            }
        }
        else
        {
            ResetInteraction();
        }
    }
    private void CheckInteraction()
    {
        if (!_interactable.IsLastInteracted)
        {
            _interactable.OnFocus();
            _guideText.text = _interactable.InteractText;
            _grabButton.onClick.AddListener(_interactable.OnInteract);
        }
        else
        {
            _interactable.OnLoseFocus();
            _guideText.text = "";
            _grabButton.onClick.RemoveListener(_interactable.OnInteract);
        }
    }
    private void ResetInteraction()
    {
        if (_lastInteractable != null)
        {
            _lastInteractable.OnLoseFocus();
            _lastInteractable = null;
            _grabButton.onClick.RemoveListener(_interactable.OnInteract);
        }

        _guideText.text = "";
    }

    private IEnumerator C_InteractCooldown()
    {
        _canInteracted = false;
        yield return new WaitForSeconds(0.1f);
        _canInteracted = true;
    }
}
