using System.Collections.Generic;
using Unity.Entities;
using Unity.Scenes;
using UnityEngine;

namespace PropHunt.SceneManagement
{
    /// <summary>
    /// Set of static references to subscenes
    /// </summary>
    public class SubSceneReferences : MonoBehaviour
    {
        /// <summary>
        /// Global instance of the sub scenes
        /// </summary>
        /// <value></value>
        public static SubSceneReferences Instance { get; private set; }

        /// <summary>
        /// List of sub scenes to initialize the game
        /// </summary>
        public List<SubScene> subScenesInGame;

        /// <summary>
        /// map of scene name to subscene
        /// </summary>
        private Dictionary<string, SubScene> scenes;

        /// <summary>
        /// Various parameters for saving how much each scene is loaded
        /// </summary>
        private Dictionary<string, Entity> loadingParameters;

        /// <summary>
        /// Is there a scene saved with this given name
        /// </summary>
        /// <param name="name">Name of the subscene</param>
        /// <returns>true if there is a scene with this name, false otherwise</returns>
        public bool ContainsScene(string name)
        {
            return this.scenes.ContainsKey(name);
        }

        /// <summary>
        /// Gets a subscene by the name of the scene
        /// </summary>
        /// <param name="name">Name of the subscene</param>
        /// <returns>the saved sub scene</returns>
        public SubScene GetSceneByName(string name)
        {
            return scenes[name];
        }

        /// <summary>
        /// Check if a scene is finished loading
        /// </summary>
        /// <param name="name">Name of the subscene</param>
        /// <param name="sceneSystem">system used to modify, load, unload sub scenes</param>
        /// <returns>true if finished loading, false otherwise</returns>
        public bool IsSceneFinished(string name, SceneSystem sceneSystem)
        {
            if (this.loadingParameters.ContainsKey(name))
            {
                return sceneSystem.IsSceneLoaded(this.loadingParameters[name]);
            }
            return false;
        }

        /// <summary>
        /// Load a scene by name and log the setup progress
        /// </summary>
        /// <param name="name">Name of the subscene</param>
        /// <param name="sceneSystem">system used to modify, load, unload sub scenes</param>
        public void LoadScene(string name, SceneSystem sceneSystem)
        {
            this.loadingParameters[name] = sceneSystem.LoadSceneAsync(this.GetSceneByName(name).SceneGUID);
        }

        /// <summary>
        /// Unload a scene by name
        /// </summary>
        /// <param name="name">Name of the subscene</param>
        /// <param name="sceneSystem">system used to modify, load, unload sub scenes</param>
        public void UnloadScene(string name, SceneSystem sceneSystem)
        {
            sceneSystem.UnloadScene(this.GetSceneByName(name).SceneGUID);
        }

        private void Awake()
        {
            this.scenes = new Dictionary<string, SubScene>();
            this.loadingParameters = new Dictionary<string, Entity>();
            foreach (SubScene scene in subScenesInGame)
            {
                scenes[scene.gameObject.name] = scene;
            }
            Instance = this;
        }
    }
}
