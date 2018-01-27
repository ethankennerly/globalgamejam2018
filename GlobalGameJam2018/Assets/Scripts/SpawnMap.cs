using UnityEngine;
using UnityEngine.Tilemaps;

namespace Finegamedesign.Utils
{
    public sealed class SpawnMap : AEnableBehaviour<SpawnMap>
    {
        [SerializeField]
        private Tilemap m_Tilemap;

        public Tilemap tilemap
        {
            get { return m_Tilemap; }
        }
    }
}
