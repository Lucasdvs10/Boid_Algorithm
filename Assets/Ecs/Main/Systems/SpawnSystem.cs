using Ecs.Components.BoidRules.Alignment;
using Ecs.Main.Components.Rules;
using Ecs.Main.Components.Spawner;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace Ecs.Components {
    public partial class SpawnSystem : SystemBase{
        protected override void OnStartRunning() {
            base.OnStartRunning();

            var spawnerComp = SystemAPI.GetSingleton<SpawnerComp>();

            NativeArray<Entity> entitiesArray = new NativeArray<Entity>(spawnerComp.NumberToSpawn, Allocator.Temp);
            
            EntityManager.Instantiate(spawnerComp.Entity, entitiesArray);
            
            Random rn = new Random();
            rn.InitState();
            foreach (var entity in entitiesArray) {
                EntityManager.AddComponent<PhysicsVelocity>(entity);
                EntityManager.SetComponentData(entity, LocalTransform.FromPosition(rn.NextFloat(-50f,50f), rn.NextFloat(-50f,50f), rn.NextFloat(-50f,50f)));
                float3 velVector = new float3(rn.NextFloat(-1f, 1), rn.NextFloat(-1f, 1), rn.NextFloat(-1f, 1f));
                velVector = MathfTools.SetMag(velVector, 10);
                EntityManager.SetComponentData(entity, new PhysicsVelocity{Angular = 0, Linear = velVector});
                
                EntityManager.AddComponent<AlignmentTag>(entity);
                EntityManager.AddComponent<CohesionTag>(entity);
                EntityManager.AddComponent<SeparationTag>(entity);
            }
        }

        protected override void OnUpdate() {
        }
    }
}