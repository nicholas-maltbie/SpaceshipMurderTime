
using PropHunt.Mixed.Commands;
using PropHunt.Mixed.Systems;
using Unity.Entities;
using Unity.NetCode;

namespace PropHunt.Server.Systems
{
    [UpdateInGroup(typeof(ServerSimulationSystemGroup))]
    public class ServerForwardLoadGameRequest : ComponentSystem
    {
        protected override void OnCreate()
        {
            RequireSingletonForUpdate<NetworkIdComponent>();
        }

        protected override void OnUpdate()
        {
            string selectedMap = "Skeld";
            Entities.ForEach((Entity entity, ref StartGameRequest cmd, ref ReceiveRpcCommandRequestComponent req) =>
            {
                PostUpdateCommands.DestroyEntity(entity);
                UnityEngine.Debug.Log($"We received a command to start the game");
                var loadSceneRequest = PostUpdateCommands.CreateEntity();
                PostUpdateCommands.AddComponent(loadSceneRequest, new SceneLoadCommand { loadScene = selectedMap, unloadScene = GameStateSystem.LobbySceneName });
                PostUpdateCommands.AddComponent(loadSceneRequest, new SendRpcCommandRequestComponent());
                Entity sceneLoaderSingleton = PostUpdateCommands.CreateEntity();
                PostUpdateCommands.AddComponent(sceneLoaderSingleton, new SceneLoaderSystem.SceneLoadInfo
                {
                    sceneToLoad = selectedMap,
                    sceneToUnload = GameStateSystem.LobbySceneName
                });
            });
        }
    }
}