using System;
using PropHunt.Authoring;
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
    public class SpawnPlayerAvatarSystem : ComponentSystem
    {
        /// <summary>
        /// 
        /// </summary>
        private int characterId;

        protected override void OnCreate()
        {
            RequireSingletonForUpdate<GhostPrefabCollectionComponent>();
        }

        public static int GetPlayerGhostIndex(DynamicBuffer<GhostPrefabBuffer> ghostPrefabBuffers, int searchId, EntityManager manager)
        {
            for (int i = 0; i < ghostPrefabBuffers.Length; i++)
            {
                var found = ghostPrefabBuffers[i].Value;
                // The prefab with a PlayerId will be returned
                if (manager.HasComponent<PlayerPrefabComponent>(found) &&
                    manager.GetComponentData<PlayerPrefabComponent>(found).idGUID == searchId)
                {
                    return i;
                }
            }
            return -1;
        }

        protected override void OnUpdate()
        {
            Entities.WithNone<SendRpcCommandRequestComponent>().ForEach((Entity reqEnt, ref SpawnAvatarCommand req, ref ReceiveRpcCommandRequestComponent reqSrc) =>
            {
                int connectionId = EntityManager.GetComponentData<NetworkIdComponent>(reqSrc.SourceConnection).Value;

                PostUpdateCommands.AddComponent<NetworkStreamInGame>(reqSrc.SourceConnection);
                UnityEngine.Debug.Log(String.Format("Server setting connection {0} to in game", connectionId));

                // Delete original avatar
                Entities.ForEach((Entity ent, ref PlayerId playerId) =>
                {
                    if (playerId.playerId == connectionId)
                    {
                        PostUpdateCommands.DestroyEntity(ent);
                    }
                });

                // Setup the character avatar
                Entity ghostCollection = GetSingletonEntity<GhostPrefabCollectionComponent>();
                DynamicBuffer<GhostPrefabBuffer> ghostPrefabs = EntityManager.GetBuffer<GhostPrefabBuffer>(ghostCollection);
                int ghostId = GetPlayerGhostIndex(ghostPrefabs, req.avatarId, EntityManager);
                var prefab = EntityManager.GetBuffer<GhostPrefabBuffer>(ghostCollection)[ghostId].Value;
                var player = PostUpdateCommands.Instantiate(prefab);
                PostUpdateCommands.SetComponent(player, new PlayerId { playerId = connectionId });
                PostUpdateCommands.SetComponent(player, new Translation { Value = req.position });
                PostUpdateCommands.SetComponent(player, new Rotation { Value = req.attitude });
                PostUpdateCommands.SetComponent(player, new GhostOwnerComponent { NetworkId = connectionId });

                PostUpdateCommands.AddBuffer<PlayerInput>(player);
                PostUpdateCommands.SetComponent(reqSrc.SourceConnection, new CommandTargetComponent { targetEntity = player });

                PostUpdateCommands.DestroyEntity(reqEnt);
            });
        }
    }
}