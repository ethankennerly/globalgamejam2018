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

        private Tilemap m_WallMap;

        public MobileTileSystem()
        {
            DeltaTimeSystem.onDeltaTime += Update;
            MobileTile.onEnable += OnEnableMobileTile;
            MobileTile.onDisable += OnDisableMobileTile;
            WallMap.onEnable += OnEnableWallMap;
            WallMap.onDisable += OnDisableWallMap;
        }

        ~MobileTileSystem()
        {
            DeltaTimeSystem.onDeltaTime -= Update;
            MobileTile.onEnable -= OnEnableMobileTile;
            MobileTile.onDisable -= OnDisableMobileTile;
            WallMap.onEnable -= OnEnableWallMap;
            WallMap.onDisable -= OnDisableWallMap;
        }

        public void OnEnableMobileTile(MobileTile mobile)
        {
            if (m_Mobiles.Contains(mobile))
            {
                return;
            }
            mobile.System = this;
            m_Mobiles.Add(mobile);
        }

        public void OnDisableMobileTile(MobileTile mobile)
        {
            mobile.System = null;
            m_Mobiles.Remove(mobile);
        }

        private void OnEnableWallMap(WallMap map)
        {
            m_WallMap = map.tilemap;
        }

        private void OnDisableWallMap(WallMap map)
        {
            m_WallMap = null;
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
            if (!mobile.isColliding)
            {
                mobile.isColliding = HasTileInFront(m_WallMap, mobile);
            }
            if (mobile.isColliding)
            {
                mobile.isColliding = false;
                Snap(mobile.transform, -mobile.velocity * deltaTime);
                mobile.velocity = Rotate(mobile.velocity, 180f);
            }
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

        private static bool HasTileInFront(Tilemap tilemap, MobileTile mobile)
        {
            Vector3 front = GetInFront(mobile, 0.75f);
            return HasTile(tilemap, front);
        }

        private static Vector3 GetInFront(MobileTile mobile, float distance)
        {
            Vector3 position = mobile.transform.position;
            position = new Vector3(position.x, position.y, position.z);
            Vector2 velocity = new Vector2(mobile.velocity.x, mobile.velocity.y);
            velocity.Normalize();
            velocity *= distance;
            position.x += velocity.x;
            position.y += velocity.y;
            return position;
        }

        private static bool HasTile(Tilemap tilemap, Vector3 position)
        {
            if (tilemap == null)
            {
                return false;
            }
            position.z = tilemap.transform.position.z;
            Vector3Int cell = tilemap.WorldToCell(position);
            if (!tilemap.HasTile(cell))
            {
                return false;
            }
            return true;
        }

        // Causes mobiles to follow each other.
        private static bool HasCollider2D(Vector3 point, GameObject ignoredObject)
        {
            Vector2 origin = new Vector2(point.x, point.y);
            RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.zero);
            return hit != null && hit.collider != null && hit.collider.gameObject != ignoredObject;
        }
    }
}
