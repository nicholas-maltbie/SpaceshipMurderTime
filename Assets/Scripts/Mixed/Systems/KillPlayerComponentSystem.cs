using Unity.Entities;
using Unity.NetCode;
using Unity.Jobs;
using Unity.Physics;
using Unity.Transforms;
using PropHunt.Mixed.Components;
using Unity.Physics.Extensions;
using System.ComponentModel;
using SpaceshipMurderTime;
using Unity.Rendering;
using System.Drawing;
using System;
using PropHunt.Mixed.Commands;
using System.Linq;
using Unity.Mathematics;
using PropHunt.Server.Systems;
using PropHunt.Authoring;

namespace PropHunt.Mixed.Systems
{
    [UpdateAfter(typeof(KinematicCharacterControllerInput))]
    public class KillPlayerGroup : ComponentSystemGroup { }

    /// <summary>
    /// System group for resolving push forces applied to dynamic objects in the scene
    /// </summary>
    [UpdateInGroup(typeof(KillPlayerGroup))]
    [UpdateAfter(typeof(ApplyKillCommand))]
    public class KillCommandCleanup : SystemBase
    {
        EndSimulationEntityCommandBufferSystem commandBufferSystem;

        protected override void OnCreate()
        {
            this.commandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            var commandBuffer = this.commandBufferSystem.CreateCommandBuffer().AsParallelWriter();

            Entities.ForEach((
                Entity entity,
                int entityInQueryIndex,
                in Kill killComponent) =>
            {
                commandBuffer.RemoveComponent(entityInQueryIndex, entity, ComponentType.ReadOnly<Kill>());
            }
            ).ScheduleParallel();

            this.Dependency.Complete();
            this.commandBufferSystem.AddJobHandleForProducer(this.Dependency);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [UpdateInGroup(typeof(KillPlayerGroup))]
    public class ApplyKillCommand : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities.ForEach((ref PlayerAliveState state, in Kill kill) =>
            {
                if (state.isAlive)
                {
                    state.isAlive = false;
                }
            }
            ).ScheduleParallel();
        }
    }


    /// <summary>
    /// System group for resolving push forces applied to dynamic objects in the scene
    /// </summary>
    /// 
    [UpdateAfter(typeof(ApplyKillCommand))]
    [UpdateBefore(typeof(KillCommandCleanup))]
    [UpdateInGroup(typeof(ClientSimulationSystemGroup))]
    public class UpdatePlayerState : ComponentSystem
    {
        protected override void OnCreate()
        {
            RequireSingletonForUpdate<NetworkIdComponent>();
        }

        protected override void OnUpdate()
        {
            var ecb = EntityManager.World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>().CreateCommandBuffer();
            // Send command to server to spawn new avatar
            int localPlayerId = GetSingleton<NetworkIdComponent>().Value;

            bool isServer = World.GetExistingSystem<ServerSimulationSystemGroup>() != null;

            Entities.ForEach((
                Entity entity,
                ref PlayerAliveState state,
                ref Kill kill,
                ref Translation translation,
                ref Rotation rotation,
                ref PlayerView view,
                ref PlayerId playerId) =>
            {
                if (state.isAlive == false && playerId.playerId == localPlayerId)
                {
                    Entity commandTarget = GetSingletonEntity<CommandTargetComponent>();
                    PostUpdateCommands.SetComponent(commandTarget, new CommandTargetComponent { targetEntity = Entity.Null });
                    // Send request to spawn new avatar
                    var req = PostUpdateCommands.CreateEntity();
                    PostUpdateCommands.AddComponent<SpawnAvatarCommand>(req);
                    PostUpdateCommands.SetComponent(req, new SpawnAvatarCommand
                    {
                        avatarId = PlayerPrefabComponent.GhostCharacterId,
                        position = translation.Value,
                        attitude = rotation.Value,
                    });
                    PostUpdateCommands.AddComponent(req, new SendRpcCommandRequestComponent { TargetConnection = GetSingletonEntity<NetworkStreamInGame>() });
                }
            });
        }

        // private void HideChildren(Entity entity, EntityCommandBuffer ecb)
        // {
        //     if (EntityManager.HasComponent<Child>(entity))
        //     {
        //         foreach (var child in EntityManager.GetBuffer<Child>(entity))
        //         {
        //             HideChildren(child.Value, ecb);
        //         }
        //     }
        //     if (EntityManager.HasComponent<RenderMesh>(entity))
        //     {
        //         PostUpdateCommands.RemoveComponent<UpdateMaterialComponentData>(entity);
        //         var renderMesh = EntityManager.GetSharedComponentData<RenderMesh>(entity);
        //         var material = SharedMaterials.Instance.GetMaterialById(3);

        //         // Set the material.
        //         ecb.SetSharedComponent(entity, new RenderMesh
        //         {
        //             mesh = renderMesh.mesh,
        //             material = material,
        //             subMesh = renderMesh.subMesh,
        //             layer = renderMesh.layer,
        //             castShadows = renderMesh.castShadows,
        //             needMotionVectorPass = renderMesh.needMotionVectorPass,
        //             receiveShadows = renderMesh.receiveShadows,
        //         });
        //     }
        //     if (EntityManager.HasComponent<PhysicsCollider>(entity))
        //     {

        //     }
        // }
    }

    [UpdateInGroup(typeof(ClientSimulationSystemGroup))]
    public class PlayerModeSystem : ComponentSystem
    {
        public enum PlayerVisionState { GHOST, ALIVE };
        // Singleton component to trigger connections once from a control system
        public struct PlayerState : IComponentData
        {
            public PlayerVisionState vision;
        }

        protected override void OnCreate()
        {
            RequireSingletonForUpdate<PlayerState>();
            RequireSingletonForUpdate<NetworkIdComponent>();
            Entity entity = EntityManager.CreateEntity(typeof(PlayerState));
            EntityManager.SetComponentData(entity, new PlayerState { vision = PlayerVisionState.ALIVE });

        }

        public float elapsed = 0;
        protected override void OnUpdate()
        {
            var group = World.GetExistingSystem<GhostPredictionSystemGroup>();
            var tick = group.PredictingTick;
            var isClient = World.GetExistingSystem<ClientSimulationSystemGroup>() != null;

            int localPlayerId = GetSingleton<NetworkIdComponent>().Value;

            Entity stateSingleton = GetSingletonEntity<PlayerState>();
            Entities.ForEach(
                    (Entity entity, DynamicBuffer<PlayerInput> inputBuffer, ref PlayerAliveState state, ref PlayerId player, ref PredictedGhostComponent prediction) =>
                    {

                        if (player.playerId == localPlayerId)
                        {
                            if (state.isAlive == true)
                            {
                                EntityManager.SetComponentData(stateSingleton, new PlayerState { vision = PlayerVisionState.ALIVE });
                            }
                            else
                            {
                                EntityManager.SetComponentData(stateSingleton, new PlayerState { vision = PlayerVisionState.GHOST });
                            }
                        }
                    }
            );
        }
    }

    [UpdateInGroup(typeof(ClientSimulationSystemGroup))]
    public class DebugPlayerKill : ComponentSystem
    {
        protected override void OnCreate()
        {
            RequireSingletonForUpdate<NetworkIdComponent>();
        }

        protected override void OnUpdate()
        {
            var group = World.GetExistingSystem<GhostPredictionSystemGroup>();
            var tick = group.PredictingTick;

            int localPlayerId = GetSingleton<NetworkIdComponent>().Value;

            Entities.ForEach(
                    (Entity entity, DynamicBuffer<PlayerInput> inputBuffer, ref PredictedGhostComponent prediction) =>
                    {
                        if (!GhostPredictionSystemGroup.ShouldPredict(tick, prediction))
                        {
                            return;
                        }

                        // This is a debug which will kill you when you interact
                        inputBuffer.GetDataAtTick(tick, out var input);
                        if (input.IsInteracting)
                        {
                            PostUpdateCommands.AddComponent<Kill>(entity);
                        }
                    }
            );
        }
    }
}