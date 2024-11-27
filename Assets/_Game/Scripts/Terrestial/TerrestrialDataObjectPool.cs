using System.Collections.Generic;
using GameIdea2.Terrestial;

namespace GameIdea2.Scripts.Terrestial
{
    public static class TerrestrialDataObjectPool
    {
        private static Dictionary<string, TerrestrialDataObject> ObjectPool;

        public static Dictionary<string, TerrestrialDataObject> GetPool()
        {
            if (ObjectPool == null)
                ObjectPool = new Dictionary<string, TerrestrialDataObject>();
            
            return ObjectPool; 
        }
    }
}