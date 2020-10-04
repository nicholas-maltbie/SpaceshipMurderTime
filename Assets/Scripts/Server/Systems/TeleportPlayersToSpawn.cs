using Unity.Collections;
using Unity.Entities;
using Unity.Scenes;
using PropHunt.Mixed.Components;
using Unity.NetCode;
using Unity.Transforms;
using Unity.Mathematics;
using System.Numerics;
using PropHunt.Mixed.Commands;

namespace PropHunt.Mixed.Systems
{
    /// <summary>
    /// System to teleport all players to spawn zones
    /// </summary>
    [UpdateInGroup(typeof(ServerSimulationSystemGroup))]
    public class TeleportPlayersToSpawn : ComponentSystem
    {
        private float elapsed = 0;
        /// <summary>
        /// Request to teleport all players to spawn
        /// </summary>
        public struct TeleportPlayerTimer : IComponentData
        {
            /// <summary>
            /// delay in seconds to teleport players to spawn zones
            /// </summary>
            public float delay;
        }

        protected override void OnCreate()
        {
            RequireSingletonForUpdate<TeleportPlayerTimer>();
        }

        protected override void OnUpdate()
        {
            this.elapsed += Time.DeltaTime;
            TeleportPlayerTimer request = GetSingleton<TeleportPlayerTimer>();

            if (this.elapsed < request.delay)
            {
                return;
            }

            this.elapsed = 0;
            PostUpdateCommands.DestroyEntity(GetSingletonEntity<TeleportPlayerTimer>());

            var spawnZoneEntity = GetSingletonEntity<SpawnZone>();
            DynamicBuffer<SpawnPoint> spawnPoints = EntityManager.GetBuffer<SpawnPoint>(spawnZoneEntity);

            Entities.ForEach((
                Entity entity,
                ref PlayerId playerId,
                ref Translation translation,
                ref Rotation rotation) =>
            {
                float3 spawnTranslation = spawnPoints[playerId.playerId % spawnPoints.Length].position;
                quaternion spawnRotation = spawnPoints[playerId.playerId % spawnPoints.Length].attitude;
                translation.Value = spawnTranslation;
                rotation.Value = spawnRotation;
                
                UnityEngine.Debug.Log($"Sending command to teleport player {playerId.playerId} to position {spawnTranslation} and rotation {spawnRotation}");
                var teleportPlayerRequest = PostUpdateCommands.CreateEntity();
                PostUpdateCommands.AddComponent(teleportPlayerRequest, new TeleportPlayerCommand { position = spawnTranslation, attitude = spawnRotation, playerId = playerId.playerId});
                PostUpdateCommands.AddComponent(teleportPlayerRequest, new SendRpcCommandRequestComponent());
            });
        }
    }
}