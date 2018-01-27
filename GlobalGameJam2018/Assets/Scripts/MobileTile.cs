using UnityEngine;

namespace Finegamedesign.Tiles
{
    public sealed class MobileTile : MonoBehaviour
    {
        public float arrivalTime;
        public float nextTileSpeed;
        public Vector2 velocity = new Vector2(1f, 0f);

        private void OnEnable()
        {
            MobileTileSystem.Instance.OnEnable(this);
        }

        private void OnDisable()
        {
            MobileTileSystem.Instance.OnDisable(this);
        }
    }
}
