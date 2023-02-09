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
            int perceiveRadious = 10; 
            
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
            
                var averagePosition = sumPositions / neighboursAmount;
                var desiredVelocity = averagePosition - currentRgb.Transform.LocalPosition;
                if(MathfTools.GetVectorMag(desiredVelocity) == 0) return;
                
                desiredVelocity = MathfTools.SetMag(desiredVelocity, 10);
                
                var steeringForce = desiredVelocity - currentRgb.PhysicsVelocity.ValueRO.Linear;
                            
                currentRgb.ApplyImpulse(MathfTools.LimitMag(steeringForce, 100f));
                // currentRgb.PhysicsVelocity.ValueRW.Linear = MathfTools.LimitMag(currentRgb.PhysicsVelocity.ValueRO.Linear, 10f);
            }
        }
    }
}