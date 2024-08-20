using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace GameIdea2.Audio._UI
{
    public class HoverGUI : MonoBehaviour
    {
        [SerializeField] private Vector3 posDelta;
        [SerializeField] private float delay = 0;
        [SerializeField] private CanvasGroup cg;
        [SerializeField] private int sideTolerance = 10;
        
        
        [Header("Texts")]
        [SerializeField] private TMPro.TMP_Text title;
        [SerializeField] private TMPro.TMP_Text desc;
        [SerializeField] private TMPro.TMP_Text mass;

        private RectTransform rt;
        private Coroutine hoverRoutine;

        private void Start()
        {
            rt = GetComponent<RectTransform>();
        }

        private void Update()
        {
            var mousePos = Input.mousePosition;
            var midX = Screen.width / 2;
            var midY = Screen.height / 2;
            
            float anchorX = 0;
            float anchorY = 0;

            float posX;
            float posY;

            anchorX = mousePos.x < midX + sideTolerance?0:1;
            anchorY = mousePos.y < midY - sideTolerance?0:1;

            posX = (mousePos.x < midX + sideTolerance) ? (Input.mousePosition.x + posDelta.x) : (Input.mousePosition.x - Screen.width - posDelta.x);
            posY = (mousePos.y < midY - sideTolerance) ? (Input.mousePosition.y + posDelta.y) : (Input.mousePosition.y - Screen.height - posDelta.y);

            rt.anchorMin = rt.anchorMax = rt.pivot = new Vector2(anchorX, anchorY);
            rt.anchoredPosition = new Vector2(posX, posY); 
        }

        public void ShowHoverGUI(HoverWindowData hoverData)
        {
            if(hoverRoutine != null)
                StopCoroutine(hoverRoutine);

            gameObject.SetActive(true);
            title.text = hoverData.Title;
            desc.text = hoverData.Desc;
            mass.text = hoverData.MassRange;
            hoverRoutine = StartCoroutine(ShowHoverGUIRoutine(true));
        }

        public void HideHoverGUI()
        {
            if(hoverRoutine != null)
                StopCoroutine(hoverRoutine);
            
            gameObject.SetActive(true);
            hoverRoutine = StartCoroutine(ShowHoverGUIRoutine(false));
        }

        IEnumerator ShowHoverGUIRoutine(bool show)
        {
            if (show)
            {
                if(delay > 0)
                    yield return new WaitForSeconds(delay);
                gameObject.SetActive(true);
            }

            float currCgAlpha = cg.alpha;
            float newAlphs = show ? 1 : 0;
            float timeStep = 0;
            while (timeStep <= 1)
            {
                timeStep += Time.deltaTime / 0.25f;
                cg.alpha = Mathf.Lerp(currCgAlpha, newAlphs, timeStep);
                yield return new WaitForEndOfFrame();
            }
            
            if(!show)
                gameObject.SetActive(false);
        }
    }
}