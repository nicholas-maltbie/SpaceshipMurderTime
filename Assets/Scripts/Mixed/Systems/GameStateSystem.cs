
using Unity.Collections;
using Unity.Entities;

namespace PropHunt.Mixed.Systems
{
    /// <summary>
    /// Component system for managing current game state
    /// </summary>
    public class GameStateSystem : ComponentSystem
    {
        /// <summary>
        /// Name of lobby scene
        /// </summary>
        public static readonly string LobbySceneName = "LobbyScene";

        /// <summary>
        /// Current sage of the name
        /// </summary>
        public enum GameFlow { Lobby, InGame }

        /// <summary>
        /// Component for current game state
        /// </summary>
        public struct GameState : IComponentData
        {
            /// <summary>
            /// Current stage of the game
            /// </summary>
            public GameFlow stage;

            /// <summary>
            /// Currently (or most recently) loaded scene
            /// </summary>
            public FixedString64 loadedScene;
        }

        protected override void OnCreate()
        {
            Entity gameStateSingleton = EntityManager.CreateEntity(typeof(GameState));
            EntityManager.SetComponentData(gameStateSingleton, new GameState{
                stage = GameFlow.Lobby,
                loadedScene = LobbySceneName,
            });
        }

        protected override void OnUpdate()
        {
            // Nothing to do here
        }
    }
}
