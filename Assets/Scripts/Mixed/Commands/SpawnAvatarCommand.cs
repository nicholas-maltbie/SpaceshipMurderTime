using Unity.Mathematics;
using Unity.NetCode;
using Unity.Networking.Transport;

namespace PropHunt.Mixed.Commands
{

    /// <summary>
    /// Command to spawn player avatar
    /// </summary>
    public struct SpawnAvatarCommand : IRpcCommand
    {
        /// <summary>
        /// Id of player avatar to spawn
        /// </summary>
        public int avatarId;

        /// <summary>
        /// Position to spawn character
        /// </summary>
        public float3 position;

        /// <summary>
        /// Attitude to rotate character
        /// </summary>
        public quaternion attitude;
    }

}