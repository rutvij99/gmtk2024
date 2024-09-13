using System;
using System.Collections;
using GravityWell.Core;
using UnityEngine;

namespace GravityWell.Splash
{
    public class SplashScreen : MonoBehaviour
    {
        private SplashState splashState = null;

        private void Awake()
        {
            if (GameStateMachine.CurrentState.Type == GameStates.Splash)
            {
                splashState = ((SplashState)GameStateMachine.CurrentState);
            }
        }

        private IEnumerator Start()
        {
            if(splashState == null) yield break;
            yield return new WaitForSeconds(3);
            splashState.OnSplashComplete();
        }
    }
}
    
