﻿using Ecs.Main.Components.Rules;
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
                EntityManager.SetComponentData(entity, new PhysicsVelocity{Angular = 0, Linear = new float3(rn.NextFloat(-20f,20), rn.NextFloat(-20f,20), rn.NextFloat(-20f,20))});

                EntityManager.AddComponent<AlignmentTag>(entity);
                EntityManager.AddComponent<CohesionTag>(entity);
            }
        }

        protected override void OnUpdate() {
        }
    }
}