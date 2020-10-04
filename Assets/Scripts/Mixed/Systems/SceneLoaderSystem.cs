using Unity.Collections;
using Unity.Entities;
using Unity.Scenes;
using PropHunt.Mixed.Components;

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
            string toUnload = loadInfo.sceneToLoad.ToString().Trim();
            PostUpdateCommands.DestroyEntity(loaderSingleton);

            Entities.ForEach((Entity entity, SubScene subScene, ref SceneIdentifier loading) =>
            {
                string sceneName = loading.sceneName.ToString().Trim();

                // If we want to load it and it is not already loaded
                if (toLoad == sceneName && !EntityManager.HasComponent<RequestSceneLoaded>(entity))
                {
                    // Request scene unload
                    PostUpdateCommands.AddComponent<RequestSceneLoaded>(entity);
                    // If loading lobby, ensure game state is updated
                    Entity gameStateEntity = GetSingletonEntity<PropHunt.Mixed.Systems.GameStateSystem.GameState>();
                    GameStateSystem.GameStage stage = sceneName == GameStateSystem.LobbySceneName ? GameStateSystem.GameStage.Lobby : GameStateSystem.GameStage.InGame;
                    PostUpdateCommands.SetComponent(gameStateEntity, new GameStateSystem.GameState {
                        stage = stage,
                        loadedScene = sceneName
                    });
                    UnityEngine.Debug.Log($"Loading scene of name {sceneName}");
                }
                // If we want to unload it and it is already loaded
                else if (toUnload == sceneName && EntityManager.HasComponent<RequestSceneLoaded>(entity))
                {
                    // Request scene loaded
                    PostUpdateCommands.RemoveComponent<RequestSceneLoaded>(entity);
                    UnityEngine.Debug.Log($"Unloading scene of name {sceneName}");
                }
            });

            PostUpdateCommands.RemoveComponent<SceneLoadInfo>(loaderSingleton);
        }
    }
}