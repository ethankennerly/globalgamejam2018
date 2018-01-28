using UnityEngine;
using UnityEngine.Tilemaps;

namespace Finegamedesign.Utils
{
    public sealed class WallMap : AEnableBehaviour<WallMap>
    {
        [SerializeField]
        private Tilemap m_Tilemap;

        public Tilemap tilemap
        {
            get { return m_Tilemap; }
        }
    }
}
