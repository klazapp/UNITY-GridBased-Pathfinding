using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : TMonoSingleton<InputController>
{
    [Header("Layer Mask")] 
    [SerializeField]
    private LayerMask layerMask;

    //Events
    public delegate void PathPointClickedAction(float3 clickedPosition);
    public static event PathPointClickedAction PathPointClickedEvent;

    private InputControlComponents inputControlComponents;

    protected override void Awake()
    {
        base.Awake();
        inputControlComponents = new InputControlComponents();    
    }
    
    private void OnEnable()
    {
        inputControlComponents ??= new InputControlComponents();    
        
        inputControlComponents.BaseMap.MouseClick.started += StartedClickCallback;
        inputControlComponents.Enable();
    }
    private void OnDisable()
    {
        inputControlComponents.BaseMap.MouseClick.started -= StartedClickCallback;
        inputControlComponents.Disable();
    }

    private void StartedClickCallback(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.started)
            DetectClickedObject();
    }

    private void DetectClickedObject()
    {
        if (Camera.main is null)
            return;

        //Retrieve ray from mouse click
        var ray = Camera.main.ScreenPointToRay(inputControlComponents.BaseMap.MousePosition.ReadValue<Vector2>());

        //Retrieve hit object data 
        if (Physics.Raycast(ray, out var hitData, layerMask))
        {
            PathPointClickedEvent?.Invoke(hitData.transform.position);
        }
    }
}
