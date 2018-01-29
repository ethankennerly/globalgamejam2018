using Finegamedesign.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Finegamedesign.Tiles
{
    public sealed class MobileTileSystem : System<MobileTileSystem>
    {
        private readonly List<MobileTile> m_Mobiles = new List<MobileTile>();

        private float m_TimeScale = 2f;

        public MobileTileSystem()
        {
            DeltaTimeSystem.onDeltaTime += Update;
            MobileTile.onEnable += OnEnableMobileTile;
            MobileTile.onDisable += OnDisableMobileTile;
            MobileTile.onCollision += OnCollision;
            MobileTile.onTrigger += OnCollision;
        }

        ~MobileTileSystem()
        {
            DeltaTimeSystem.onDeltaTime -= Update;
            MobileTile.onEnable -= OnEnableMobileTile;
            MobileTile.onDisable -= OnDisableMobileTile;
            MobileTile.onCollision -= OnCollision;
            MobileTile.onTrigger -= OnCollision;
        }

        public void OnEnableMobileTile(MobileTile mobile)
        {
            if (m_Mobiles.Contains(mobile))
            {
                return;
            }
            mobile.frontTrigger = GetTrigger(mobile.gameObject);
            AlignFrontTrigger(mobile);
            m_Mobiles.Add(mobile);
        }

        public void OnDisableMobileTile(MobileTile mobile)
        {
            m_Mobiles.Remove(mobile);
        }

        public void OnCollision(MobileTile mobile)
        {
            if (mobile.isColliding)
            {
                return;
            }
            mobile.isColliding = true;
        }

        private void Update(float deltaTime)
        {
            foreach (MobileTile mobile in m_Mobiles)
            {
                Move(mobile, deltaTime);
            }
        }

        // Only responds to collision once per update,
        // because collisions can happen many times.
        private void Move(MobileTile mobile, float deltaTime)
        {
            if (mobile == null)
            {
                return;
            }
            if (mobile.isColliding)
            {
                mobile.isColliding = false;
                Snap(mobile.transform, -mobile.velocity * deltaTime);
                Rotate(mobile, 180f);
            }
            AlignFrontTrigger(mobile);
            Move(mobile.transform, mobile.velocity, deltaTime * m_TimeScale);
        }

        // Avoid getting stuck to wall.
        private static void Snap(Transform transform, Vector2 offset, float snapDistance = 0.1f)
        {
            Vector3 snap = transform.position;
            snap.x = Mathf.Round((offset.x + snap.x) / snapDistance) * snapDistance;
            snap.y = Mathf.Round((offset.x + snap.y) / snapDistance) * snapDistance;
            transform.position = snap;
        }

        private static void Move(Transform transform, Vector2 velocity, float deltaTime)
        {
            Vector3 position = transform.position;
            position.x += velocity.x * deltaTime;
            position.y += velocity.y * deltaTime;
            transform.position = position;
        }

        private static void Rotate(MobileTile mobile, float degrees)
        {
            mobile.velocity = Rotate(mobile.velocity, degrees);
            AlignFrontTrigger(mobile);
        }

        private static Collider2D GetTrigger(GameObject gameObject)
        {
            if (gameObject == null)
            {
                return null;
            }
            Collider2D[] colliders = gameObject.GetComponents<Collider2D>();
            foreach (Collider2D collider in colliders)
            {
                if (collider.isTrigger)
                {
                    return collider;
                }
            }
            return null;
        }

        private static void AlignFrontTrigger(MobileTile mobile)
        {
            if (mobile.frontTrigger == null)
            {
                return;
            }
            mobile.frontTrigger.offset = Align(mobile.frontTrigger.offset, mobile.velocity);
        }

        private static Vector2 Align(Vector2 follower, Vector2 target)
        {
            return target.normalized * follower.magnitude;
        }

        // Copied from:
        // https://answers.unity.com/questions/661383/whats-the-most-efficient-way-to-rotate-a-vector2-o.html
        // Other solutions there were:
        //      return Quaternion.Euler(0f, 0f, degrees) * v;
        //      return Quaternion.AngleAxis(degrees, Vector3.forward) * v;
        private static Vector2 Rotate(Vector2 v, float degrees)
        {
            float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
            float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

            float tx = v.x;
            float ty = v.y;
            v.x = (cos * tx) - (sin * ty);
            v.y = (sin * tx) + (cos * ty);
            return v;
        }
    }
}
