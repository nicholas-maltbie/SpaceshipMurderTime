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
            bool isServer = World.GetExistingSystem<ServerSimulationSystemGroup>() != null;

            if (isServer)
            {
                Entities.ForEach((
                    ref PlayerAliveState state,
                    in Kill kill) =>
                {
                    if( state.isAlive )
                    {
                        state.isAlive = false;
                    }
                    
                }
                ).ScheduleParallel();
            }
        }
    }

    /// <summary>
    /// System group for resolving push forces applied to dynamic objects in the scene
    /// </summary>
    [UpdateAfter(typeof(ApplyKillCommand))]
    [UpdateBefore(typeof(KillCommandCleanup))]
    [UpdateInGroup(typeof(KillPlayerGroup))]
    public class UpdatePlayerState : ComponentSystem
    {
        protected override void OnUpdate()
        {
            var ecb = EntityManager.World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>().CreateCommandBuffer();

            Entities.ForEach(
                (Entity entity, ref PlayerAliveState state, ref Kill kill) =>
                {
                    if(state.isAlive == false)
                    {
                        PostUpdateCommands.RemoveComponent<UpdateMaterialComponentData>(entity);
                        var renderMesh = EntityManager.GetSharedComponentData<RenderMesh>(entity);
                        var material = SharedMaterials.Instance.GetMaterialById(3);

                        // Set the material.
                        ecb.SetSharedComponent(entity, new RenderMesh
                        {
                            mesh = renderMesh.mesh,
                            material = material,
                            subMesh = renderMesh.subMesh,
                            layer = renderMesh.layer,
                            castShadows = renderMesh.castShadows,
                            needMotionVectorPass = renderMesh.needMotionVectorPass,
                            receiveShadows = renderMesh.receiveShadows,
                        });
                    }
                }
            );
        }
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
            Entity entity = EntityManager.CreateEntity(typeof(PlayerState));
            EntityManager.SetComponentData(entity, new PlayerState { vision = PlayerVisionState.ALIVE });
            
            // Debug.Log("Creating client world");
        }

        protected override void OnUpdate()
        {
            int localPlayerId = GetSingleton<NetworkIdComponent>().Value;

            Entity stateSingleton = GetSingletonEntity<PlayerState>();
            Entities.ForEach(
                    (ref PlayerId player, ref PlayerAliveState state) =>
                    {
                        if( player.playerId == localPlayerId )
                        {
                            if (state.isAlive == true )
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
}