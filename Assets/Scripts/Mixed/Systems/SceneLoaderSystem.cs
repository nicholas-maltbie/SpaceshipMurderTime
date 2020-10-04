using System;
using System.Collections.Concurrent;
using Unity.Entities;
using Unity.Scenes;
using static PropHunt.Mixed.Systems.SceneLoaderSystem;

namespace PropHunt.Mixed.Systems
{
    /// <summary>
    /// Class to hold arguments for loading a new Scene
    /// </summary>
    public class RequestSceneLoadEventArgs : EventArgs
    {
        /// <summary>
        /// String identifier for the new scene to load
        /// </summary>
        public string newScene { get; set; }

        /// <summary>
        /// Desired state to set
        /// </summary>
        public LoadState desiredState { get; set; }
    }

    /// <summary>
    /// System to manage loading and unloading scenes
    /// </summary>
    public class SceneLoaderSystem : ComponentSystem
    {
        /// <summary>
        /// Events for requesting a screen change
        /// </summary>
        public static event EventHandler<RequestSceneLoadEventArgs> RequestScreenChange;

        /// <summary>
        /// Load state for requesting a change in scene
        /// </summary>
        public enum LoadState { Load, Unload }

        /// <summary>
        /// Dictionary of current commands to process organized by scene name
        /// </summary>
        private ConcurrentDictionary<string, LoadState> scenesToLoad;

        protected override void OnCreate()
        {
            this.scenesToLoad = new ConcurrentDictionary<string, LoadState>();
            RequestScreenChange += this.HandleSceneChangeRequest;
        }

        /// <summary>
        /// Handle a request to change scenes
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="eventArgs">arguments of screen change</param>
        public void HandleSceneChangeRequest(object sender, RequestSceneLoadEventArgs eventArgs)
        {
            if (this.scenesToLoad.TryAdd(eventArgs.newScene, eventArgs.desiredState))
            {
                UnityEngine.Debug.Log("Failed to properly load scene");
            }
        }

        protected override void OnUpdate()
        {
            Entities.ForEach((Entity entity, SubScene subScene) =>
            {
                if (!scenesToLoad.ContainsKey(subScene.SceneAsset.name))
                {
                    // Ignore subscene if it is not in the dictionary
                    return;
                }
                LoadState desiredState;
                if (!scenesToLoad.TryRemove(subScene.SceneAsset.name, out desiredState))
                {
                    // return in failure state
                    return;
                }

                if (desiredState == LoadState.Load && !EntityManager.HasComponent<RequestSceneLoaded>(entity))
                {
                    // Request scene unload
                    PostUpdateCommands.AddComponent<RequestSceneLoaded>(entity);
                }
                else if (desiredState == LoadState.Unload && EntityManager.HasComponent<RequestSceneLoaded>(entity))
                {
                    // Request scene loaded
                    PostUpdateCommands.RemoveComponent<RequestSceneLoaded>(entity);
                }
            });
        }
    }
}