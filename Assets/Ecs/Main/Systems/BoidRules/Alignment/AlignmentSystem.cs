using System.Linq;
using Ecs.Main.Components.Rules;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics.Aspects;
using UnityEngine;

namespace Ecs.Components.BoidRules.Alignment {
    public partial struct AlignmentSystem : ISystem {
        public void OnCreate(ref SystemState state) {
        }

        public void OnDestroy(ref SystemState state) {
        }

        public void OnUpdate(ref SystemState state) {
            int perceiveRadious = 15;
            // int numEntities = SystemAPI.Query<RefRO<AlignmentTag>>().Count();

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

                var desiredVelocity = sumVelocities / 3f;
                desiredVelocity = SetMag(desiredVelocity, 10);
                var steeringForce = desiredVelocity - currentRgb.LinearVelocity;
                
                currentRgb.ApplyLinearImpulseLocalSpace(steeringForce);
            }
        }
        

        private float Distance(float3 v1, float3 v2) {
            return Mathf.Sqrt((v1.x - v2.x) * (v1.x - v2.x) + (v1.y - v2.y) * (v1.y - v2.y) +
                              (v1.z - v2.z) * (v1.z - v2.z));
        }

        private float3 SetMag(float3 oldVector, int newMagnitude) {
            var oldMag = Mathf.Sqrt(Mathf.Pow(oldVector.x, 2) + Mathf.Pow(oldVector.y, 2) + Mathf.Pow(oldVector.z, 2));
            
            if(oldMag == 0) return float3.zero;
            
            return (oldVector / oldMag) * newMagnitude;
        }
        
    }
}