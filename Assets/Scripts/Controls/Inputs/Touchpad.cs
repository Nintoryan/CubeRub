using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CubeRub.Controls.Inputs
{
  public class Touchpad : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
  {
    public event Action Pressed;
    public event Action Dragged;
    public event Action Upped;
    
    public Vector3 PressingPosition { get; private set; }
    public Vector3 DraggedPosition { get; private set; }

    public void OnPointerDown(PointerEventData eventData)
    {
      PressingPosition = eventData.position;
      Pressed?.Invoke();
    }
    
    public void OnDrag(PointerEventData eventData)
    {
      DraggedPosition = eventData.position;
      Dragged?.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
      Upped?.Invoke();
    }
  };
}
