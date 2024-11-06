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
        private RectTransform parentRt;
        
        private void Start()
        {
            rt = GetComponent<RectTransform>();
            parentRt = HUDManager.instance.gameObject.transform.GetChild(0) as RectTransform;
        }

        private void Update()
        {
            if(!parentRt)
                return;
            
            var mousePos = Input.mousePosition;
            var midX = Screen.width / 2;
            var midY = Screen.height / 2;
            
            float anchorX = mousePos.x < midX + sideTolerance?0:1;
            float anchorY = mousePos.y < midY - sideTolerance?0:1;
            
            rt.anchorMin = rt.anchorMax = rt.pivot = new Vector2(anchorX, anchorY);
            
            RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRt, Input.mousePosition, null,
                out var localPoint);
            transform.localPosition = new Vector3(localPoint.x, localPoint.y, 0) + posDelta * (anchorX == 0?1:-1);
        }

        public void ShowHoverGUI(TileData hoverData)
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
                timeStep += Time.deltaTime / 0.15f;
                cg.alpha = Mathf.Lerp(currCgAlpha, newAlphs, timeStep);
                yield return new WaitForEndOfFrame();
            }
            
            if(!show)
                gameObject.SetActive(false);
        }
    }
}