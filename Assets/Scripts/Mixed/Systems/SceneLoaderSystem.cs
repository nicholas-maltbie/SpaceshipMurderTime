using Unity.Collections;
using Unity.Entities;
using Unity.Scenes;
using PropHunt.Mixed.Components;
using Unity.NetCode;
using PropHunt.SceneManagement;
using static PropHunt.Mixed.Systems.GameStateSystem;

namespace PropHunt.Mixed.Systems
{
    /// <summary>
    /// System to manage loading and unloading scenes
    /// </summary>
    public class SceneLoaderSystem : ComponentSystem
    {
        /// <summary>
        /// Scene system to manage scenes
        /// </summary>
        private SceneSystem sceneSystem;

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
            this.sceneSystem = World.GetOrCreateSystem<SceneSystem>();
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

            if (toLoad.Length > 0 && SubSceneReferences.Instance.ContainsScene(toLoad))
            {
                SubSceneReferences.Instance.LoadScene(toLoad, this.sceneSystem);
                GameFlow flow = toLoad == GameStateSystem.LobbySceneName ? GameFlow.Lobby : GameFlow.InGame;
                Entity entity = GetSingletonEntity<GameStateSystem.GameState>();
                PostUpdateCommands.SetComponent(entity, new GameStateSystem.GameState
                {
                    loadedScene = toLoad,
                    stage = flow,
                });
            }
            if (toUnload.Length > 0 && SubSceneReferences.Instance.ContainsScene(toUnload))
            {
                SubSceneReferences.Instance.UnloadScene(toUnload, this.sceneSystem);
            }
            
        }
    }
}