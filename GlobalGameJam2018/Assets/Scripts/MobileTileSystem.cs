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
            DeltaTimeView.OnDeltaTime += Update;
        }

        ~MobileTileSystem()
        {
            DeltaTimeView.OnDeltaTime -= Update;
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
    }
}
