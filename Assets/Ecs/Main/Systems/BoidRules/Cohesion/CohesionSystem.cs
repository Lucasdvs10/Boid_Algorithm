using Ecs.Components.BoidRules.Alignment;
using Ecs.Components.BoidRules.Alignment.Aspects;
using Ecs.Main.Components.Rules;
using Unity.Entities;
using Unity.Mathematics;

namespace Ecs.Components.BoidRules.Cohesion {
    public partial struct CohesionSystem : ISystem{
        public void OnCreate(ref SystemState state) {
        }

        public void OnDestroy(ref SystemState state) {
        }

        public void OnUpdate(ref SystemState state) {
            int perceiveRadious = 20; 
            
            foreach ((CohesionTag tag, MyPhysicsAspect currentRgb) in SystemAPI.Query<CohesionTag, MyPhysicsAspect>() ) { 
                float3 sumPositions = float3.zero;
                int neighboursAmount = 0;
                
                foreach ((CohesionTag othersTag, MyPhysicsAspect otherRgb) in SystemAPI.Query<CohesionTag, MyPhysicsAspect>()) {
                    var distanceBetweenEntities = MathfTools.Distance(currentRgb.Transform.LocalPosition,
                        otherRgb.Transform.LocalPosition);
            
                    if (distanceBetweenEntities > 0 && distanceBetweenEntities <= perceiveRadious) {
                        sumPositions += otherRgb.Transform.LocalPosition;
                        neighboursAmount++;
                    }
                }
            
                if(neighboursAmount == 0) return;
                var averagePosition = sumPositions / neighboursAmount;
                var desiredVelocity = averagePosition - currentRgb.Transform.LocalPosition;
                if(MathfTools.GetVectorMag(desiredVelocity) == 0) return;
                
                desiredVelocity = MathfTools.SetMag(desiredVelocity, 40f);
                
                var steeringForce = desiredVelocity - currentRgb.PhysicsVelocity.ValueRO.Linear;
                            
                currentRgb.ApplyImpulse(MathfTools.LimitMag(steeringForce, 10f));
            }
        }
    }
}