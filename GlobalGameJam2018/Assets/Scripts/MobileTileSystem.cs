using Finegamedesign.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Finegamedesign.Tiles
{
    public class MobileTileSystem : System<MobileTileSystem>
    {
        private readonly List<MobileTile> m_Mobiles = new List<MobileTile>();

        private float speed = 2f;

        public MobileTileSystem()
        {
            DeltaTimeSystem.onDeltaTime += Update;
            MobileTile.onEnable += OnEnable;
        }

        ~MobileTileSystem()
        {
            DeltaTimeSystem.onDeltaTime -= Update;
            MobileTile.onEnable -= OnEnable;
        }

        public void OnEnable(MobileTile mobile)
        {
            if (m_Mobiles.Contains(mobile))
            {
                return;
            }
            m_Mobiles.Add(mobile);
        }

        public void OnDisable(MobileTile mobile)
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
                mobile.velocity = Rotate(mobile.velocity, 180f);
            }
            Move(mobile.transform, mobile.velocity, deltaTime * speed);
        }

        private static void Move(Transform transform, Vector2 velocity, float deltaTime)
        {
            Vector3 position = transform.position;
            position.x += velocity.x * deltaTime;
            position.y += velocity.y * deltaTime;
            transform.position = position;
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
