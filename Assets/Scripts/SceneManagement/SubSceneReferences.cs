using System.Collections.Generic;
using Unity.Scenes;
using UnityEngine;

namespace PropHunt.SceneManagement
{
    /// <summary>
    /// Set of static references to subscenes
    /// </summary>
    public class SubSceneReferences : MonoBehaviour
    {
        public static SubSceneReferences Instance { get; private set; }

        public List<SubScene> subScenesInGame;

        private Dictionary<string, SubScene> scenes;

        public bool ContainsScene(string name)
        {
            return this.scenes.ContainsKey(name);
        }

        public SubScene GetSceneByName(string name)
        {
            return scenes[name];
        }

        private void Awake()
        {
            this.scenes = new Dictionary<string, SubScene>();
            foreach (SubScene scene in subScenesInGame)
            {
                scenes[scene.gameObject.name] = scene;
            }
            Instance = this;
        }
    }
}
