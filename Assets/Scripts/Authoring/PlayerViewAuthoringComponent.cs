using PropHunt.Mixed.Components;
using Unity.Entities;
using UnityEngine;

namespace PropHunt.Authoring
{
    /// <summary>
    /// Player view authoring component, will attach a palyer
    /// view attribute to an component as it is converted to an entity.
    /// </summary>
    public class PlayerViewAuthoringComponent : MonoBehaviour, IConvertGameObjectToEntity
    {
        /// <summary>
        /// Speed of view rotation in degrees per second
        /// </summary>
        public float viewRotationRate = 180f;

        /// <summary>
        /// offset for where to place camera relative to character
        /// </summary>
        public Vector3 offset = new Vector3(0, 1.7f, 0);

        /// <summary>
        /// Maximum player view pitch
        /// </summary>
        public float maxPitch = 90;

        /// <summary>
        /// Minimum player view pitch
        /// </summary>
        public float minPitch = -90;

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddComponentData(entity, new PlayerView()
            {
                viewRotationRate = this.viewRotationRate,
                offset = offset,
                maxPitch = maxPitch,
                minPitch = minPitch,
            });
        }
    }
}