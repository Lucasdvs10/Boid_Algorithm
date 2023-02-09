using Ecs.Components.BoidRules.Alignment;
using Ecs.Components.BoidRules.Alignment.Aspects;
using Ecs.Main.Components.Rules;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Ecs.Components.BoidRules.Cohesion {
    public partial struct CohesionSystem : ISystem{
        public void OnCreate(ref SystemState state) {
        }

        public void OnDestroy(ref SystemState state) {
        }

        public void OnUpdate(ref SystemState state) {
            int perceiveRadious = 5; 
            
            foreach ((CohesionTag tag, MyPhysicsAspect currentRgb) in SystemAPI.Query<CohesionTag, MyPhysicsAspect>() ) { 
                var currentEntity = currentRgb.Entity;
                float3 sumPositions = float3.zero;
                int neighboursAmount = 1;
                
                foreach ((CohesionTag othersTag, MyPhysicsAspect otherRgb) in SystemAPI.Query<CohesionTag, 
                MyPhysicsAspect>()
                 ) {
                    var otherEntity = otherRgb.Entity;
                    var distanceBetweenEntities = MathfTools.Distance(currentRgb.Transform.LocalPosition,
                        otherRgb.Transform.LocalPosition);
            
                    if (currentEntity != otherEntity && distanceBetweenEntities <= perceiveRadious) {
                        sumPositions += otherRgb.Transform.LocalPosition;
                        neighboursAmount++;
                    }
                }
            
                var avaragePosition = sumPositions / neighboursAmount;
                var desiredVelocity = avaragePosition - currentRgb.Transform.LocalPosition;
                Debug.Log(avaragePosition);
                desiredVelocity = MathfTools.SetMag(desiredVelocity, 10);
                var steeringForce = desiredVelocity - currentRgb.PhysicsVelocity.ValueRO.Linear;
                            
                currentRgb.ApplyImpulse(MathfTools.LimitMag(steeringForce, 5f));
                currentRgb.PhysicsVelocity.ValueRW.Linear = MathfTools.LimitMag(currentRgb.PhysicsVelocity.ValueRO.Linear, 10f);
            }
        }
    }
}