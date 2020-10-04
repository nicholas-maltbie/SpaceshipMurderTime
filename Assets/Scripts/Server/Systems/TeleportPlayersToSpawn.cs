using Unity.Entities;
using PropHunt.Mixed.Components;
using Unity.NetCode;
using Unity.Transforms;
using Unity.Mathematics;
using PropHunt.Mixed.Commands;
using PropHunt.Server.Systems;
using Unity.Collections;
using PropHunt.SceneManagement;
using Unity.Scenes;

namespace PropHunt.Mixed.Systems
{
    /// <summary>
    /// System to teleport all players to spawn zones
    /// </summary>
    [UpdateInGroup(typeof(ServerSimulationSystemGroup))]
    [UpdateAfter(typeof(ServerStartGameSystem))]
    public class TeleportPlayersToSpawn : ComponentSystem
    {
        /// <summary>
        /// Scene system to manage scenes
        /// </summary>
        private SceneSystem sceneSystem;

        /// <summary>
        /// Request to teleport all players to spawn
        /// </summary>
        public struct TeleportPlayerTimer : IComponentData
        {
            /// <summary>
            /// Target scene to wait for loaded
            /// </summary>
            public FixedString64 targetScene;
        }

        protected override void OnCreate()
        {
            RequireSingletonForUpdate<TeleportPlayerTimer>();
            this.sceneSystem = World.GetOrCreateSystem<SceneSystem>();
        }

        protected override void OnUpdate()
        {
            TeleportPlayerTimer request = GetSingleton<TeleportPlayerTimer>();
            string targetScene = request.targetScene.ToString().Trim();
            // UnityEngine.Debug.Log($"Current scene loaded state: {SubSceneReferences.Instance.IsSceneFinished(targetScene, this.sceneSystem)}");
            if (!SubSceneReferences.Instance.ContainsScene(targetScene))
            {
                PostUpdateCommands.DestroyEntity(GetSingletonEntity<TeleportPlayerTimer>());
            }
            if (!SubSceneReferences.Instance.IsSceneFinished(targetScene, this.sceneSystem))
            {
                return;
            }
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

                var teleportPlayerRequest = PostUpdateCommands.CreateEntity();
                PostUpdateCommands.AddComponent(teleportPlayerRequest, new TeleportPlayerCommand { position = spawnTranslation, attitude = spawnRotation, playerId = playerId.playerId });
                PostUpdateCommands.AddComponent(teleportPlayerRequest, new SendRpcCommandRequestComponent());
            });
        }
    }
}