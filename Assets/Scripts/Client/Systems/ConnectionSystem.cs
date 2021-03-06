using PropHunt.Mixed.Systems;
using Unity.Entities;
using Unity.NetCode;
using Unity.Scenes;
using UnityEngine;
using static PropHunt.Game.ClientGameSystem;

namespace PropHunt.Client.Systems
{
    /// <summary>
    /// System to clear all ghosts on the client
    /// </summary>
    [UpdateBefore(typeof(ConnectionSystem))]
    [UpdateInGroup(typeof(ClientSimulationSystemGroup))]
    public class ClearClientGhostEntities : SystemBase
    {
        protected EndSimulationEntityCommandBufferSystem commandBufferSystem;

        public struct ClientClearGhosts : IComponentData { };

        protected override void OnCreate()
        {
            RequireSingletonForUpdate<ClientClearGhosts>();
            this.commandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            var buffer = this.commandBufferSystem.CreateCommandBuffer().AsParallelWriter();
            // Also delete the existing ghost objects
            Entities.ForEach((
                Entity ent,
                int entityInQueryIndex,
                ref GhostComponent ghost) =>
            {
                buffer.DestroyEntity(entityInQueryIndex, ent);
            }).ScheduleParallel();
            this.commandBufferSystem.CreateCommandBuffer().DestroyEntity(GetSingletonEntity<ClientClearGhosts>());
        }
    }

    /// <summary>
    /// Secondary class to create connection object ot connect to the server
    /// </summary>
    [UpdateInGroup(typeof(ClientSimulationSystemGroup))]
    public class CreateConnectObjectSystem : ComponentSystem
    {
        protected override void OnUpdate()
        {
            if (ConnectionSystem.connectRequested)
            {
                EntityManager.CreateEntity(typeof(InitClientGameComponent));
                ConnectionSystem.connectRequested = false;
                // Load lobby state when connecting
                string currentScene = GetSingleton<GameStateSystem.GameState>().loadedScene.ToString().Trim();
                if (currentScene == GameStateSystem.LobbySceneName)
                {
                    currentScene = "";
                }
                Entity sceneLoaderSingleton = PostUpdateCommands.CreateEntity();
                PostUpdateCommands.AddComponent(sceneLoaderSingleton, new SceneLoaderSystem.SceneLoadInfo
                {
                    sceneToUnload = currentScene,
                    sceneToLoad = GameStateSystem.LobbySceneName,
                });
            }
        }
    }

    /// <summary>
    /// System to handle disconnecting client from the server
    /// </summary>
    [UpdateBefore(typeof(GhostReceiveSystem))]
    [UpdateInGroup(typeof(ClientSimulationSystemGroup))]
    public class ConnectionSystem : ComponentSystem
    {
        /// <summary>
        /// Has a disconnect been requested
        /// </summary>
        public static bool disconnectRequested;

        /// <summary>
        /// Has a connection been requested
        /// </summary>
        public static bool connectRequested;

        /// <summary>
        /// Is the client currently connected
        /// </summary>
        public static bool IsConnected { get; private set; }

        /// <summary>
        /// Invoke whenever a disconnect is requested
        /// </summary>
        public static void DisconnectFromServer()
        {
            ConnectionSystem.disconnectRequested = true;
        }

        /// <summary>
        /// Invoke whenever a connect is requested
        /// </summary>
        public static void ConnectToServer()
        {
            ConnectionSystem.connectRequested = true;
        }

        protected override void OnUpdate()
        {
            Entities.ForEach((Entity ent, ref NetworkStreamConnection conn) =>
            {
                if (EntityManager.HasComponent<NetworkStreamInGame>(ent))
                {
                    ConnectionSystem.IsConnected = true;
                }
                if (EntityManager.HasComponent<NetworkStreamDisconnected>(ent))
                {
                    ConnectionSystem.IsConnected = false;
                }
            });
            if (ConnectionSystem.disconnectRequested)
            {
                Debug.Log("Attempting to disconnect");
                Entities.ForEach((Entity ent, ref NetworkStreamConnection conn) =>
                {
                    EntityManager.AddComponent(ent, typeof(NetworkStreamRequestDisconnect));
                    // Unload current scene when disconnecting
                    // Works when you don't unload current scene???
                    // string currentScene = GetSingleton<GameStateSystem.GameState>().loadedScene.ToString().Trim();
                    // Entity sceneLoaderSingleton = PostUpdateCommands.CreateEntity();
                    // PostUpdateCommands.AddComponent(sceneLoaderSingleton, new SceneLoaderSystem.SceneLoadInfo {
                    //     sceneToUnload = currentScene
                    // });
                    EntityManager.CreateEntity(ComponentType.ReadOnly(typeof(ClearClientGhostEntities.ClientClearGhosts)));
                });
                ConnectionSystem.disconnectRequested = false;
            }
        }
    }

}