using UnityEngine;

namespace Finegamedesign.Utils
{
    public sealed class InventoryObject : AEnableBehaviour<InventoryObject>
    {
        public int numItems = 1;
        public GameObject prefab;
    }
}
