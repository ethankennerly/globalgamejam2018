using Finegamedesign.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Finegamedesign.Tiles
{
    public sealed class ClickSpawnSystem : System<ClickSpawnSystem>
    {
        private List<InventoryObject> m_InventoryObjects = new List<InventoryObject>();

        private Tilemap m_Tilemap;

        private readonly bool m_SpawnAnywhere = false;

        public ClickSpawnSystem()
        {
            if (m_SpawnAnywhere)
            {
                ClickPointSystem.onWorld += TrySpawn;
            }
            else
            {
                ClickPointSystem.onCollisionEnter2D += TrySpawnOnMobileTile;
            }
            InventoryObject.onEnable += OnEnable;
            InventoryObject.onDisable += OnDisable;
            SpawnMap.onEnable += OnEnable;
            SpawnMap.onDisable += OnDisable;
        }

        ~ClickSpawnSystem()
        {
            if (m_SpawnAnywhere)
            {
                ClickPointSystem.onWorld -= TrySpawn;
            }
            else
            {
                ClickPointSystem.onCollisionEnter2D -= TrySpawnOnMobileTile;
            }
            InventoryObject.onEnable -= OnEnable;
            InventoryObject.onDisable -= OnDisable;
            SpawnMap.onEnable -= OnEnable;
            SpawnMap.onDisable -= OnDisable;
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

        private void TrySpawn(Vector3 position)
        {
            TrySpawnInventory(position);
        }

        // Only spawn if clicked mobile tile.
        // Otherwise germ might be placed where no mobile tile goes.
        // Then the level will not end.
        private void TrySpawnOnMobileTile(Collider2D collider)
        {
            if (collider == null
                || collider.gameObject == null
                || collider.gameObject.GetComponent<MobileTile>() == null)
            {
                return;
            }
            TrySpawnInventory(collider.transform.position);
        }

        private void TrySpawnInventory(Vector3 position)
        {
            if (!SetTilePosition(m_Tilemap, ref position))
            {
                return;
            }
            if (!ClickPointSystem.DisableTemporarily())
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
