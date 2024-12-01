using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class RutvijTestingEnv : MonoBehaviour
{
   public PlayerInput playerInput;

   private void Start()
   {
      playerInput = GetComponent<PlayerInput>();
   }

   public void OnMove(InputAction.CallbackContext value)
   {
      if (value.phase == InputActionPhase.Performed)
      {
         Type t = value.valueType;
         Debug.Log($"OnMove {value.valueType} -> {value.ReadValue<Vector2>()}");
      }
   }
}
