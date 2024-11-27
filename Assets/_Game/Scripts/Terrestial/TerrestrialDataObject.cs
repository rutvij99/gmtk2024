using UnityEngine;

namespace GameIdea2.Terrestial
{
    [System.Serializable]
    public class TerrestrialDataObject
    {
        private string Id;
        public Vector3 StartLinearVelocity;
        public Vector3 Position;
        public float ObjectScale;
        public float CollisionRadius;
        public float Mass;

        public TerrestrialDataObject(string Id)
        {
            this.Id = Id;
        }
        
        public override bool Equals(object obj)
        {
            if (obj is not TerrestrialDataObject)
                return false;
            
            return (obj as TerrestrialDataObject).Id == this.Id;
        }

        public override string ToString()
        {
            return $"DataObject {Id}";
        }
    }
}