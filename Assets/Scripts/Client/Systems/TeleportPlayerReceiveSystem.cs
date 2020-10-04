using PropHunt.Mixed.Commands;
using PropHunt.Mixed.Components;
using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;
using Unity.Transforms;

namespace PropHunt.Client.Systems
{
    [UpdateInGroup(typeof(ClientSimulationSystemGroup))]
    public class TeleportPlayerReceiveSystem : ComponentSystem
    {
        protected override void OnCreate()
        {
            RequireSingletonForUpdate<NetworkIdComponent>();
        }

        protected override void OnUpdate()
        {
            int localPlayerId = GetSingleton<NetworkIdComponent>().Value;

            float3 targetPosition = float3.zero;
            quaternion targetRotation = quaternion.identity;
            bool found = false;

            Entities.ForEach((Entity entity, ref TeleportPlayerCommand cmd, ref ReceiveRpcCommandRequestComponent req) =>
            {
                PostUpdateCommands.DestroyEntity(entity);
                UnityEngine.Debug.Log($"We received a command to teleprot player to pos: {cmd.position} attitude: {cmd.attitude}");
                if (cmd.playerId == localPlayerId)
                {
                    targetPosition = cmd.position;
                    targetRotation = cmd.attitude;
                }
            });

            if (found)
            {
                Entities.ForEach((
                    Entity Entity,
                    ref PlayerId playerId,
                    ref Translation translation,
                    ref Rotation rotation) =>
                {
                    if (playerId.playerId == localPlayerId)
                    {
                        translation.Value = targetPosition;
                        rotation.Value = targetRotation;
                    }
                });
            }
        }
    }
}