using Unity.Collections;
using Unity.Entities;
using Unity.Scenes;
using PropHunt.Mixed.Components;
using Unity.NetCode;

namespace PropHunt.Mixed.Systems
{
    /// <summary>
    /// System to manage loading and unloading scenes
    /// </summary>
    public class SceneLoaderSystem : ComponentSystem
    {
        /// <summary>
        /// Information to load or unload a scene
        /// </summary>
        public struct SceneLoadInfo : IComponentData
        {
            /// <summary>
            /// Name of scene to load
            /// </summary>
            public FixedString64 sceneToLoad;

            /// <summary>
            /// Name of scene to unload
            /// </summary>
            public FixedString64 sceneToUnload;

            /// <summary>
            /// Unload all types of subscene
            /// </summary>
            public bool unloadAll;
        }

        protected override void OnCreate()
        {
            RequireSingletonForUpdate<SceneLoadInfo>();
        }

        protected override void OnUpdate()
        {
            Entity loaderSingleton = GetSingletonEntity<SceneLoadInfo>();
            SceneLoadInfo loadInfo = EntityManager.GetComponentData<SceneLoadInfo>(loaderSingleton);
            string toLoad = loadInfo.sceneToLoad.ToString().Trim();
            string toUnload = loadInfo.sceneToUnload.ToString().Trim();
            PostUpdateCommands.DestroyEntity(loaderSingleton);

            bool isClient = World.GetExistingSystem<ClientSimulationSystemGroup>() != null;
            UnityEngine.Debug.Log($"client: {isClient}, Processing scene request - load {toLoad} - unload {toUnload}");

            Entities.ForEach((Entity entity, SubScene subScene) =>
            {
                // Skip entities with scene reference components
                if (!loadInfo.unloadAll && EntityManager.HasComponent<SceneReference>(entity))
                {
                    return;
                }
                string sceneName = subScene.SceneAsset.name;

                // If we want to load it and it is not already loaded
                if (toLoad == sceneName && !EntityManager.HasComponent<RequestSceneLoaded>(entity))
                {
                    // Request scene unload
                    EntityManager.AddComponent<RequestSceneLoaded>(entity);
                    // If loading lobby, ensure game state is updated
                    Entity gameStateEntity = GetSingletonEntity<PropHunt.Mixed.Systems.GameStateSystem.GameState>();
                    GameStateSystem.GameFlow stage = sceneName == GameStateSystem.LobbySceneName ? GameStateSystem.GameFlow.Lobby : GameStateSystem.GameFlow.InGame;
                    EntityManager.SetComponentData(gameStateEntity, new GameStateSystem.GameState {
                        stage = stage,
                        loadedScene = sceneName
                    });
                    UnityEngine.Debug.Log($"client: {isClient}, {entity.Index}, Loading scene of name {sceneName}");
                }
                // If we want to unload it and it is already loaded
                else if (toUnload == sceneName && EntityManager.HasComponent<RequestSceneLoaded>(entity))
                {
                    // Request scene loaded
                    EntityManager.RemoveComponent<RequestSceneLoaded>(entity);
                    UnityEngine.Debug.Log($"client: {isClient}, {entity.Index}, Unloading scene of name {sceneName}");
                }
            });
        }
    }
}