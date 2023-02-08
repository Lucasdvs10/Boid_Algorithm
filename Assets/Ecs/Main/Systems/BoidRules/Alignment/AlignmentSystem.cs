using Ecs.Main.Components.Rules;
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
                    var distanceBetweenEntities = Distance(currentRgb.Position, otherRgb.Position);

                    if (currentEntity != otherEntity && distanceBetweenEntities <= perceiveRadious) {
                        sumVelocities += otherRgb.LinearVelocity;
                    }
                }

                var desiredVelocity = sumVelocities / entitiesAmount;
                desiredVelocity = SetMag(desiredVelocity, 10);
                var steeringForce = desiredVelocity - currentRgb.LinearVelocity;
                
                currentRgb.ApplyLinearImpulseLocalSpace(LimitMag(steeringForce, 200f));
                currentRgb.LinearVelocity = LimitMag(currentRgb.LinearVelocity, 10f);
            }
        }
        

        private float Distance(float3 v1, float3 v2) {
            return Mathf.Sqrt((v1.x - v2.x) * (v1.x - v2.x) + (v1.y - v2.y) * (v1.y - v2.y) +
                              (v1.z - v2.z) * (v1.z - v2.z));
        }

        private float3 LimitMag(float3 oldVector, float maxMag) {
            return SetMag(oldVector, Mathf.Clamp(GetVectorMag(oldVector), 0f, maxMag));
        }

        private float3 SetMag(float3 oldVector, float newMagnitude) {
            var oldMag = GetVectorMag(oldVector);
            
            if(oldMag == 0) return float3.zero;
            
            return (oldVector / oldMag) * newMagnitude;
        }

        private static float GetVectorMag(float3 vector) {
            var oldMag = Mathf.Sqrt(Mathf.Pow(vector.x, 2) + Mathf.Pow(vector.y, 2) + Mathf.Pow(vector.z, 2));
            return oldMag;
        }
    }
}