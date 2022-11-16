using Unity.Mathematics;
using UnityEngine;

public class IndicatorManager : MonoBehaviour
{
    private void OnEnable()
    {
        InputController.PathPointClickedEvent += PathPointClickedCallback;
    }

    private void OnDisable()
    {
        InputController.PathPointClickedEvent -= PathPointClickedCallback;
    }
    
    private void PathPointClickedCallback(float3 clickedPosition)
    {
        clickedPosition.y = 2f;
        this.transform.localPosition = clickedPosition;
    }
}
