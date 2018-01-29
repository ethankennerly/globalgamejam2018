using Finegamedesign.Utils;
using System;
using UnityEngine;

namespace Finegamedesign.Tiles
{
    public sealed class MobileTile : AEnableBehaviour<MobileTile>
    {
        public static event Action<MobileTile> onCollision;
        public static event Action<MobileTile> onTrigger;

        public Vector2 velocity = new Vector2(1f, 0f);

        [HideInInspector]
        public bool isColliding;

        [HideInInspector]
        public Collider2D frontTrigger;

        public Animator animator { get; private set; }

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        // Moving parallel to a wall will stay in collision, not reenter.
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider == null
                || collision.collider.isTrigger
                || collision.otherCollider == null
                || collision.otherCollider.isTrigger
                || collision.collider.gameObject == collision.otherCollider.gameObject)
            {
                return;
            }
            if (onCollision == null)
            {
                return;
            }
            onCollision(this);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other == null
                || other.isTrigger
                || other.gameObject == gameObject)
            {
                return;
            }
            if (onTrigger == null)
            {
                return;
            }
            onTrigger(this);
        }
    }
}
