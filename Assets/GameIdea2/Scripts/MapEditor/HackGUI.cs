using System;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

namespace GameIdea2.Scripts.MapEditor
{
    public class HackGUI : MonoBehaviour
    {
        [SerializeField] private TerrestialBody body;
        [SerializeField] private GameObject targetObject;
        [SerializeField] private float minScaleMult = 1;
        [SerializeField] private float maxScaleMult = 10;
        [SerializeField] private float minDist = 1;
        [SerializeField] private float maxDistFromTarget = 50;

        [SerializeField] private float minZoom = 50;
        [SerializeField] private float maxZoom = 250;
        
        private Vector3 startPos;
        private Vector3 startRot;
        private Vector3 startScale;
        private Camera mainCam;
        private void Start()
        {
            mainCam = Camera.main;
            GetComponent<Canvas>().worldCamera = mainCam;
            startRot = transform.localRotation.eulerAngles;
            startPos = transform.localPosition;
            startScale = transform.localScale;
            targetObject = transform.parent.gameObject;
        }

        private void Update()
        {
            if (Universe.Instance.Simulate)
            {
                this.gameObject.SetActive(false);
                return;
            }

            if (!mainCam)
                mainCam = Camera.main;
            
            var targetRot = targetObject.transform.rotation.eulerAngles;
            var newRot = Quaternion.Euler(new Vector3(startRot.x, -targetRot.y, startRot.z));
            transform.localRotation = newRot;

            var orthPerc = (mainCam.orthographicSize - minZoom) / (maxZoom - minZoom);
            var scaleMult = Mathf.Lerp(minScaleMult, maxScaleMult, orthPerc);
            transform.localScale = startScale * scaleMult;
            transform.position = targetObject.transform.position + new Vector3(0, 0, minDist + (maxDistFromTarget * orthPerc));
        }

        public void RotateMe(float yRot)
        {
            targetObject.transform.Rotate(Vector3.up, yRot);
            body.UpdateStartDir();
        }
        
        public void DeleteMe()
        {
            Destroy(targetObject.gameObject);
        }
    }
}