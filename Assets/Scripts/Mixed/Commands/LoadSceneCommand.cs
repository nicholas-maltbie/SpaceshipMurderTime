using AOT;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.NetCode;
using Unity.Networking.Transport;

namespace PropHunt.Mixed.Commands
{
    /// <summary>
    /// Scene to load and/or unload
    /// </summary>
    public struct SceneLoadCommand : IRpcCommand
    {
        /// <summary>
        /// Command to load a server string
        /// </summary>
        public FixedString64 loadScene;

        /// <summary>
        /// Name of scene to unload
        /// </summary>
        public FixedString64 unloadScene;
    }

    public struct StartGameRequest : IRpcCommand { }
}