using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MousePointerEffect : MonoBehaviour
{
   public float scaleSpeed;
   public float alphaSpeed;
   public float startAlpha;
   
   public Transform pointerTransform;
   private Image pointerImg;
   
   private bool waitForDeactivation;

   private bool pointerIsAnimating;

   private void OnEnable()
   {
      InputController.PathPointClickedEvent += MouseButtonWasPressedCallback;
   }

   private void OnDisable()
   {
      InputController.PathPointClickedEvent -= MouseButtonWasPressedCallback;
   }

   private void Awake()
   {
      pointerTransform.localScale = new float3(0);
      pointerImg = pointerTransform.GetComponent<Image>();
   }

   private void MouseButtonWasPressedCallback(float3 clickedPos)
   {
      pointerIsAnimating = true;

      pointerTransform.localScale = new float3(0);
      pointerTransform.position = Mouse.current.position.ReadValue();

      var color = pointerImg.color;
      color.a = startAlpha;
      pointerImg.color = color;
   }

   private void Update()
   {
      if (!pointerIsAnimating) 
         return;
      
      float3 tempScale = pointerTransform.localScale;
      tempScale += Time.deltaTime * scaleSpeed;
      pointerTransform.localScale = tempScale;

      var color = pointerImg.color;
      color.a = math.clamp(color.a - Time.deltaTime * alphaSpeed, 0f, 1f);
      pointerImg.color = color;

      if (color.a > 0f)
         return;
         
      pointerIsAnimating = false;

      if (waitForDeactivation)
      {
         waitForDeactivation = false;
      }
   }
}
