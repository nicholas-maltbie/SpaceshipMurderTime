using Unity.Entities;
using Unity.Mathematics;

namespace PropHunt.Mixed.Components
{
    /// <summary>
    /// Spawn zone location for identifying spawn points
    /// </summary>
    public struct SpawnZone : IComponentData { }

    /// <summary>
    /// Buffer for storing various spawn points
    /// </summary>
    [InternalBufferCapacity(32)]
    public struct SpawnPoint : IBufferElementData
    {
        /// <summary>
        /// Absolute location of spawn point
        /// </summary>
        public float3 position;

        /// <summary>
        /// Direction player should point
        /// </summary>
        public quaternion attitude;
    }
}