using Unity.Entities;
using UnityEngine;
using PropHunt.Mixed.Components;

namespace PropHunt.Authoring
{
    /// <summary>
    /// Behaviour to create a a spawn point
    /// </summary>
    public class SpawnPointAuthoringComponent : MonoBehaviour, IConvertGameObjectToEntity
    {
        /// <summary>
        /// Various positions for spawning players
        /// </summary>
        public Transform[] positions;

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            DynamicBuffer<SpawnPoint> points = dstManager.AddBuffer<SpawnPoint>(entity);

            foreach (Transform pos in positions)
            {
                points.Add(new SpawnPoint() { position = pos.position, attitude = pos.rotation });
                // GameObject.DestroyImmediate(pos.gameObject);
            }

            dstManager.AddComponentData(entity, new SpawnZone { });
        }
    }
}