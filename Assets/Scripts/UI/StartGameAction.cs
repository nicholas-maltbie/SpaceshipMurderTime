using UnityEngine;
using PropHunt.Client.Systems;

namespace PropHunt.UI
{
    /// <summary>
    /// Action to handle disconnecting from the server
    /// </summary>
    public class StartGameAction : MonoBehaviour
    {

        /// <summary>
        /// Action to send start game request.
        /// </summary>
        public void StartGame()
        {
            ClientSendStartGameRpcRequest.RequestStartGame();
        }
    }
}