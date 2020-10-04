using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;

namespace SpaceshipMurderTime
{
    [GenerateAuthoringComponent]
    [GhostComponent(PrefabType = GhostPrefabType.All)]
    public struct PlayerAliveState : IComponentData
    {
        /// <summary>
        /// Is the player alive?
        /// </summary>
        [GhostField]
        public bool isAlive;

        /// <summary>
        /// When did they die?
        /// </summary>
        [GhostField(Quantization = 100, Interpolate = true)]
        public float timeOfDeath;

        /// <summary>
        /// Who killed this player?
        /// </summary>
        [GhostField]
        public int killerId;

    }

    [GhostComponent(PrefabType = GhostPrefabType.All)]
    public struct Kill : IComponentData
    {
        /// <summary>
        /// Who killed this player?
        /// </summary>
        [GhostField]
        public int killerId;
    }

}