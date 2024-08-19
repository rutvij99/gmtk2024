using UnityEngine;

namespace GameIdea2.Audio.CustomPlay
{
    [CreateAssetMenu(menuName = "GMTK24/APIDataAsset", fileName = "DefaultAPIDataAsset")]
    public class Api : ScriptableObject
    {
        [SerializeField] private string fetchListEndpoint;
        [SerializeField] private string fetchLevelEndpoint;
        [SerializeField] private string uploadLevelEndpoint;
        
        public string FetchListAPI => fetchListEndpoint;
        public string FetchLevelAPI => fetchLevelEndpoint;
        public string UploadLevelAPI => uploadLevelEndpoint;
    }
}