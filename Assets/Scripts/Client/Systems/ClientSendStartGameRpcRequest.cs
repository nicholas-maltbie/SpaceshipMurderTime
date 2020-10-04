
using System.Diagnostics;
using PropHunt.Mixed.Commands;
using PropHunt.Mixed.Systems;
using Unity.Entities;
using Unity.NetCode;

namespace PropHunt.Client.Systems
{
    /// <summary>
    /// System to forward a start game request to the server
    /// </summary>
    [UpdateInGroup(typeof(ClientSimulationSystemGroup))]
    public class ClientSendStartGameRpcRequest : ComponentSystem
    {
        /// <summary>
        /// Is there a request to start the game?
        /// </summary>
        private static bool requestStartGame = false;

        /// <summary>
        /// have player request start game next frame
        /// </summary>
        public static void RequestStartGame()
        {
            ClientSendStartGameRpcRequest.requestStartGame = true;
        }

        protected override void OnUpdate()
        {
            // Skip if no start to game is requested
            if (!ClientSendStartGameRpcRequest.requestStartGame)
            {
                return;
            }
            UnityEngine.Debug.Log($"Sending start game request");
            var startGameReqEntity = PostUpdateCommands.CreateEntity();
            PostUpdateCommands.AddComponent(startGameReqEntity, new StartGameRequest { });
            PostUpdateCommands.AddComponent(startGameReqEntity, new SendRpcCommandRequestComponent());
            ClientSendStartGameRpcRequest.requestStartGame = false;
        }
    }
}