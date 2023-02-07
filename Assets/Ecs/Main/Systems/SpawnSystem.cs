using Ecs.Main.Components.Rules;
using Ecs.Main.Components.Spawner;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Aspects;
using Unity.Transforms;

namespace Ecs.Components {
    public partial class SpawnSystem : SystemBase{
        protected override void OnStartRunning() {
            base.OnStartRunning();

            var spawnerComp = SystemAPI.GetSingleton<SpawnerComp>();

            EntityArchetype archetype = EntityManager.CreateArchetype(RigidBodyAspect.RequiredComponents);

            NativeArray<Entity> entitiesArray = new NativeArray<Entity>(3, Allocator.Temp);

            EntityManager.Instantiate(spawnerComp.Entity, entitiesArray);

            Random rn = new Random();
            
            rn.InitState();
            
            foreach (var entity in entitiesArray) {
                EntityManager.SetComponentData(entity, LocalTransform.FromPosition(rn.NextFloat3(-5f,5f)));
                EntityManager.SetComponentData(entity, new PhysicsVelocity{Angular = 0, Linear = new float3(rn.NextFloat(-20f,20), rn.NextFloat(-20f,20), rn.NextFloat(-20f,20))});

                EntityManager.AddComponent<AlignmentTag>(entity);
            }
        }

        protected override void OnUpdate() {
            
        }
    }
}