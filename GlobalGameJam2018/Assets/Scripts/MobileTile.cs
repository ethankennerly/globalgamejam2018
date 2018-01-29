using Finegamedesign.Utils;
using UnityEngine;

namespace Finegamedesign.Tiles
{
    public sealed class MobileTile : AEnableBehaviour<MobileTile>
    {
        public float arrivalTime;
        public float nextTileSpeed;
        public Vector2 velocity = new Vector2(1f, 0f);
        public bool isColliding;
        public Animator animator { get; private set; }

        public MobileTileSystem System { get; set; }

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        // Moving parallel to a wall will stay in collision, not reenter.
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (System == null)
            {
                return;
            }
            System.OnCollision(this);
        }
    }
}
