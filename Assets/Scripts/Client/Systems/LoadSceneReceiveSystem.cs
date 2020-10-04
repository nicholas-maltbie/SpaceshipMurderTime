
using System.Diagnostics;
using PropHunt.Mixed.Commands;
using PropHunt.Mixed.Systems;
using Unity.Entities;
using Unity.NetCode;

namespace PropHunt.Client.Systems
{
    [UpdateInGroup(typeof(ClientSimulationSystemGroup))]
    public class ClientSceneLoadRpcReceiveSystem : ComponentSystem
    {
        protected override void OnCreate()
        {
            RequireSingletonForUpdate<NetworkIdComponent>();
        }

        protected override void OnUpdate()
        {
            Entities.ForEach((Entity entity, ref SceneLoadCommand cmd, ref ReceiveRpcCommandRequestComponent req) =>
            {
                PostUpdateCommands.DestroyEntity(entity);
                UnityEngine.Debug.Log($"We received a command - Load Scene {cmd.loadScene} - Unload Scene { cmd.unloadScene }");
                Entity sceneLoaderSingleton = PostUpdateCommands.CreateEntity();
                PostUpdateCommands.AddComponent(sceneLoaderSingleton, new SceneLoaderSystem.SceneLoadInfo {
                    sceneToLoad = cmd.loadScene,
                    sceneToUnload = cmd.unloadScene
                });
            });
        }
    }
}