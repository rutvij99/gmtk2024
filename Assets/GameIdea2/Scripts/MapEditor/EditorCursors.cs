using UnityEngine;

namespace GameIdea2.Scripts.MapEditor
{
    [CreateAssetMenu(menuName = "GMTK24/EditorCursorAsset", fileName = "DefaultEditorCursorAssets")]
    public class EditorCursors : ScriptableObject
    {
        public Texture2D DefaultCursor;
        public Texture2D PanCursor;
        public Texture2D ScaleCursor;
        public Texture2D MoveCursor;
    }
}