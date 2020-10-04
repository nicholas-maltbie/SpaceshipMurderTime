using System;
using PropHunt.Mixed.Commands;
using PropHunt.Mixed.Components;
using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;
using Unity.Physics.Systems;
using Unity.Transforms;
using UnityEngine;

namespace PropHunt.Server.Systems
{
    /// <summary>
    /// When server receives go in game request, go in game and delete request
    /// </summary>
    [UpdateBefore(typeof(BuildPhysicsWorld))]
    [UpdateInGroup(typeof(ServerSimulationSystemGroup))]
    public class JoinGameServerSystem : ComponentSystem
    {
        /// <summary>
        /// 
        /// </summary>
        private int characterId;

        protected override void OnCreate()
        {
            RequireSingletonForUpdate<GhostPrefabCollectionComponent>();
            RequireSingletonForUpdate<SpawnZone>();
        }

        protected int GetPlayerGhostIndex(DynamicBuffer<GhostPrefabBuffer> ghostPrefabBuffers)
        {
            for (int i = 0; i < ghostPrefabBuffers.Length; i++)
            {
                var found = ghostPrefabBuffers[i].Value;
                // The prefab with a PlayerId will be returned
                if (EntityManager.HasComponent<PlayerId>(found))
                {
                    return i;
                }
            }
            return -1;
        }

        protected override void OnUpdate()
        {
            var spawnZoneEntity = GetSingletonEntity<SpawnZone>();
            DynamicBuffer<SpawnPoint> spawnPoints = EntityManager.GetBuffer<SpawnPoint>(spawnZoneEntity);

            Entities.WithNone<SendRpcCommandRequestComponent>().ForEach((Entity reqEnt, ref JoinGameRequest req, ref ReceiveRpcCommandRequestComponent reqSrc) =>
            {
                int connectionId = EntityManager.GetComponentData<NetworkIdComponent>(reqSrc.SourceConnection).Value;

                PostUpdateCommands.AddComponent<NetworkStreamInGame>(reqSrc.SourceConnection);
                UnityEngine.Debug.Log(String.Format("Server setting connection {0} to in game", connectionId));

                // Setup the character avatar
                Entity ghostCollection = GetSingletonEntity<GhostPrefabCollectionComponent>();
                DynamicBuffer<GhostPrefabBuffer> ghostPrefabs = EntityManager.GetBuffer<GhostPrefabBuffer>(ghostCollection);
                int ghostId = GetPlayerGhostIndex(ghostPrefabs);
                var prefab = EntityManager.GetBuffer<GhostPrefabBuffer>(ghostCollection)[ghostId].Value;
                var player = PostUpdateCommands.Instantiate(prefab);
                PostUpdateCommands.SetComponent(player, new PlayerId { playerId = connectionId, playerName = req.username });
                PostUpdateCommands.SetComponent(player, new GhostOwnerComponent { NetworkId = connectionId });

                float3 spawnTranslation = spawnPoints[connectionId % spawnPoints.Length].position;
                quaternion spawnRotation = spawnPoints[connectionId % spawnPoints.Length].attitude;
                float verticalRotation = ((Quaternion)spawnRotation).eulerAngles.y;
                PostUpdateCommands.SetComponent(player, new Translation { Value = spawnTranslation });
                PostUpdateCommands.SetComponent(player, new Rotation { Value = spawnRotation });

                PostUpdateCommands.AddBuffer<PlayerInput>(player);
                PostUpdateCommands.SetComponent(reqSrc.SourceConnection, new CommandTargetComponent { targetEntity = player });

                PostUpdateCommands.DestroyEntity(reqEnt);
            });
        }
    }
}