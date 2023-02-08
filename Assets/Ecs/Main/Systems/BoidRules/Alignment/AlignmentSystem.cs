﻿using Ecs.Main.Components.Rules;
using Ecs.Main.Components.Spawner;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics.Aspects;
using UnityEngine;

namespace Ecs.Components.BoidRules.Alignment {
    public partial struct AlignmentSystem : ISystem {
        //Todo: Por enquanto o número de entidades está escrito na unha devido à minha incapacidade de rodar um sistema antes do outro
        // private SpawnerComp _spawnerComp;
        
        public void OnCreate(ref SystemState state) {
            // _spawnerComp = SystemAPI.GetSingleton<SpawnerComp>();
        }

        public void OnDestroy(ref SystemState state) {
        }

        public void OnUpdate(ref SystemState state) {
            int perceiveRadious = 15;
            var entitiesAmount = 100;
            
            foreach ((AlignmentTag tag, RigidBodyAspect currentRgb) in SystemAPI.Query<AlignmentTag, RigidBodyAspect>() ) {
                var currentEntity = currentRgb.Entity;
                float3 sumVelocities = float3.zero;
                foreach ((AlignmentTag othersTag, RigidBodyAspect otherRgb) in SystemAPI.Query<AlignmentTag, RigidBodyAspect>() ) {
                    var otherEntity = otherRgb.Entity;
                    var distanceBetweenEntities = MathfTools.Distance(currentRgb.Position, otherRgb.Position);

                    if (currentEntity != otherEntity && distanceBetweenEntities <= perceiveRadious) {
                        sumVelocities += otherRgb.LinearVelocity;
                    }
                }

                var desiredVelocity = sumVelocities / entitiesAmount;
                desiredVelocity = MathfTools.SetMag(desiredVelocity, 10);
                var steeringForce = desiredVelocity - currentRgb.LinearVelocity;
                
                currentRgb.ApplyLinearImpulseLocalSpace(MathfTools.LimitMag(steeringForce, 200f));
                currentRgb.LinearVelocity = MathfTools.LimitMag(currentRgb.LinearVelocity, 10f);
            }
        }
    }
}