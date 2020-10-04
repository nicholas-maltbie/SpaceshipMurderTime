
using PropHunt.Mixed.Components;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace PropHunt.Authoring
{
    /// <summary>
    /// Mono Behaviour to create scene loading component
    /// </summary>
    public class SceneIdentifierAuthoringComponent : MonoBehaviour
    {
        /// <summary>
        /// Scene name
        /// </summary>
        public string sceneName;

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddComponentData(entity, new SceneIdentifier()
            {
                sceneName = new FixedString64(sceneName)
            });
        }
    }
}