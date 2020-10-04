using Unity.Collections;
using Unity.Entities;
using Unity.NetCode;

namespace PropHunt.Mixed.Components
{

    /// <summary>
    /// Component for identifying entities that are controlled by a player.
    /// </summary>
    [GenerateAuthoringComponent]
    public struct PlayerId : IComponentData
    {
        /// <summary>
        /// Number identifying who is controlling the player.
        /// </summary>
        [GhostField]
        public int playerId;

        /// <summary>
        /// Name identifying who is controlling the player
        /// </summary>
        [GhostField]
        public FixedString64 playerName;
    }

}