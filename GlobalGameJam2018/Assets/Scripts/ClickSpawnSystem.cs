using Finegamedesign.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Finegamedesign.Tiles
{
    [Serializable]
    public class ClickSpawnSystem : System<ClickSpawnSystem>
    {
        private List<InventoryObject> m_InventoryObjects;

        private Tilemap m_Tilemap;

        public ClickSpawnSystem()
        {
            ClickPointSystem.onWorld += TrySpawn;
            InventoryObject.onEnable += OnEnable;
            SpawnMap.onEnable += OnEnable;
        }

        ~ClickSpawnSystem()
        {
            ClickPointSystem.onWorld -= TrySpawn;
            InventoryObject.onEnable -= OnEnable;
            SpawnMap.onEnable += OnEnable;
        }

        private void OnEnable(InventoryObject inventory)
        {
            if (m_InventoryObjects.Contains(inventory))
            {
                return;
            }
            m_InventoryObjects.Add(inventory);
        }

        public void OnDisable(InventoryObject inventory)
        {
            m_InventoryObjects.Remove(inventory);
        }

        private void OnEnable(SpawnMap map)
        {
            m_Tilemap = map.tilemap;
        }

        private void OnDisable(SpawnMap map)
        {
            m_Tilemap = null;
        }

        public void TrySpawn(Vector3 position)
        {
            if (!SetTilePosition(m_Tilemap, ref position))
            {
                return;
            }
            foreach (InventoryObject inventory in m_InventoryObjects)
            {
                if (inventory.numItems < 1)
                {
                    continue;
                }
                --inventory.numItems;
                GameObject.Instantiate(inventory.prefab, position, Quaternion.identity);
            }
        }

        private static bool SetTilePosition(Tilemap tilemap, ref Vector3 position, float offsetZ = -0.01f)
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
            position = tilemap.GetCellCenterWorld(cell);
            position.z += offsetZ;
            return true;
        }
    }
}
