
using PropHunt.Mixed.Commands;
using PropHunt.Mixed.Components;
using PropHunt.Server.Systems;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;
using Unity.Physics;

namespace PropHunt.Mixed.Systems
{
    /// <summary>
    /// Parse player input and set kinematic character controller to follow movement
    /// commands based on user input.
    /// </summary>
    [BurstCompile]
    [UpdateAfter(typeof(PlayerRotationSystem))]
    public class KinematicCharacterControllerInput : SystemBase
    {
        protected override void OnUpdate()
        {
            var group = World.GetExistingSystem<GhostPredictionSystemGroup>();
            var tick = group.PredictingTick;
            var deltaTime = Time.DeltaTime;

            Entities.ForEach((
                DynamicBuffer<PlayerInput> inputBuffer,
                ref KCCVelocity velocity,
                ref KCCJumping jump,
                in PredictedGhostComponent prediction,
                in PlayerView view,
                in KCCMovementSettings settings) =>
            {
                if (!GhostPredictionSystemGroup.ShouldPredict(tick, prediction))
                {
                    return;
                }

                inputBuffer.GetDataAtTick(tick, out PlayerInput input);

                // Rotate movement vector around current attitude (only care about horizontal)
                float3 inputVector = new float3(input.horizMove, 0, input.vertMove);
                // Don't allow the total movement to be more than the 1x max move speed
                float3 direction = inputVector / math.max(math.length(inputVector), 1);

                float speedMultiplier = input.IsSprinting ? settings.SprintSpeed : settings.moveSpeed;

                quaternion horizPlaneView = quaternion.RotateY(math.radians(view.yaw));

                // Make movement vector based on player input
                velocity.playerVelocity = math.mul(horizPlaneView, direction) * speedMultiplier;
                // including jump action
                jump.attemptingJump = input.IsJumping;
            }).Schedule();
        }
    }
}