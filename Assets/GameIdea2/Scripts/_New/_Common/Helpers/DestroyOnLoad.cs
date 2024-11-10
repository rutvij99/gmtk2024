using System;
using UnityEngine;

namespace GravityWell.Common.Helpers
{
   public class DestroyOnLoad : MonoBehaviour
   {
      private void Awake()
      {
         DestroyImmediate(this.gameObject);
      }
   }
}
