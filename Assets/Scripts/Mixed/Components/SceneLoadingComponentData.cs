using Unity.Collections;
using Unity.Entities;
using Unity.NetCode;
using UnityEngine;

namespace PropHunt.Mixed.Components
{
    /// <summary>
    /// Component for identifying and loading a scene
    /// </summary>
    public struct SceneLoading : IComponentData
    {
        /// <summary>
        /// Name of the entity
        /// </summary>
        public FixedString64 sceneName;
    }
}