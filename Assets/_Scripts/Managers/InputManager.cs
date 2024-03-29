using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class InputManager : MonoBehaviour
{
    private static InputManager instance;
    public static InputManager Instance { get { return instance; } }

    public UnityEvent<GameObject> objectSelected;

    private PlayerInputActions _playerInputActions;
    private Camera _camera;
    private InputAction _selectAction;

    private void Awake()
    {
        if (instance != null && instance != this) { Destroy(this.gameObject); }
        else { instance = this; }
        
        _playerInputActions = new PlayerInputActions();
        
        EnableInput();
        
        _playerInputActions.Player.Select.performed += SelectOnPerformed;
    }


    private void Start()
    {
        _camera = Camera.main;
        _selectAction = _playerInputActions.Player.Select;
    }

    private void OnDestroy()
    {
        _selectAction.performed -= SelectOnPerformed;
    }
    
    private void SelectOnPerformed(InputAction.CallbackContext obj)
    {
        // Cast a ray from the mouse position into the scene
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Vector2 worldPosition = _camera.ScreenToWorldPoint(mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero);

        // Check if the ray hits a collider
        if (hit.collider != null)
        {
            // The hit.collider variable contains the clicked object's Collider2D
            GameObject selectedObject = hit.collider.gameObject;

            objectSelected.Invoke(selectedObject);
        }
    }
    
    private void EnableInput()
    {
        _playerInputActions.Player.Enable();
    }
    
    private void DisableInput()
    {
        _playerInputActions.Player.Disable();
    }
}