using Unity.Entities;
using UnityEngine;

namespace PropHunt.Authoring
{
    /// <summary>
    /// Component for storing a player prefab ID
    /// </summary>
    public struct PlayerPrefabComponent : IComponentData
    {
        /// <summary>
        /// ID for player character
        /// </summary>
        public static readonly int AliveCharacterId = 8672460;

        /// <summary>
        /// ID for ghost character
        /// </summary>
        public static readonly int GhostCharacterId = 6501349;

        /// <summary>
        /// Id stored for this character
        /// </summary>
        public int idGUID;
    }

    /// <summary>
    /// Authoring component to attach a player id to an entity
    /// </summary>
    public class PlayerPrefabAuthoringComponent : MonoBehaviour, IConvertGameObjectToEntity
    {
        /// <summary>
        /// ID to associate with this prefab
        /// </summary>
        public int elementId;

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddComponentData(entity, new PlayerPrefabComponent { idGUID = elementId } ) ;
        }
    }

}