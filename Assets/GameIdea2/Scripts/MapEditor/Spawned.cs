using System;
using System.Collections;
using GameIdea2.Scripts.Planets;
using GameIdea2.Stars;
using UnityEngine;

namespace GameIdea2.Scripts.MapEditor
{
    public class Spawned : MonoBehaviour
    {
        private IEnumerator Start()
        {
            var star = GetComponent<Star>();
            var blackHole = GetComponent<Blackhole>();
            var rp = GetComponent<RockyPlanet>();
            var gp = GetComponent<GassyPlanet>();
            
            if(!(star || blackHole || rp || gp))
                yield break;

            Vector3 currScale = transform.localScale;
            float timeStep = 0;
            while (timeStep <= 1)
            {
                timeStep += Time.deltaTime / 0.25f;
                transform.localScale = Vector3.Lerp(Vector3.one * 1f, currScale, timeStep);
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForEndOfFrame();
            Destroy(this);
        }
    }
}