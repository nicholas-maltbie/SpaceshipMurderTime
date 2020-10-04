using Unity.Mathematics;
using Unity.NetCode;

namespace PropHunt.Mixed.Commands
{
    /// <summary>
    /// Command to teleport players
    /// </summary>
    public struct TeleportPlayerCommand : IRpcCommand
    {
        /// <summary>
        /// Position to send player
        /// </summary>
        public float3 position;

        /// <summary>
        /// Rotation of player
        /// </summary>
        public quaternion attitude;

        /// <summary>
        /// Id of player to teleport
        /// </summary>
        public int playerId;
    }
}
