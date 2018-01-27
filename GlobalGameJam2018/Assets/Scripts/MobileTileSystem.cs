using Finegamedesign.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Finegamedesign.Tiles
{
    [Serializable]
    public class MobileTileSystem : System<MobileTileSystem>
    {
        private List<MobileTile> m_Mobiles;

        public MobileTileSystem()
        {
            DeltaTimeSystem.OnDeltaTime += Update;
        }

        ~MobileTileSystem()
        {
            DeltaTimeSystem.OnDeltaTime -= Update;
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
            mobile.velocity = Rotate(mobile.velocity, 180f);
        }

        private void Update(float deltaTime)
        {
            foreach (MobileTile mobile in m_Mobiles)
            {
                Move(mobile, deltaTime);
            }
        }

        private void Move(MobileTile mobile, float deltaTime)
        {
            Move(mobile.transform, mobile.velocity, deltaTime);
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
