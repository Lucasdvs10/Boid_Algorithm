using Ecs.Components.BoidRules.Alignment;
using Ecs.Components.BoidRules.Alignment.Aspects;
using Ecs.Main.Components.Rules;
using Unity.Entities;
using Unity.Mathematics;

namespace Ecs.Components.BoidRules.Separation {
    public partial struct SeparationSystem : ISystem{
        public void OnCreate(ref SystemState state) {
        }

        public void OnDestroy(ref SystemState state) {
        }

        public void OnUpdate(ref SystemState state) {
            int perceiveRadious = 20; 
            
            foreach ((SeparationTag tag, MyPhysicsAspect currentRgb) in SystemAPI.Query<SeparationTag, MyPhysicsAspect>() ) { 
                float3 sumOpositeDirection = float3.zero;
                int neighboursAmount = 0;
                
                foreach ((SeparationTag othersTag, MyPhysicsAspect otherRgb) in SystemAPI.Query<SeparationTag, MyPhysicsAspect>()) {
                    var distanceBetweenEntities = MathfTools.Distance(currentRgb.Transform.LocalPosition,
                        otherRgb.Transform.LocalPosition);
                    
                    if (distanceBetweenEntities > 0 && distanceBetweenEntities <= perceiveRadious) {
                        sumOpositeDirection += (currentRgb.Transform.LocalPosition - otherRgb.Transform
                        .LocalPosition) / distanceBetweenEntities;
                        neighboursAmount++;
                    }
                }
            
                if(neighboursAmount == 0) return;
                var desiredVelocity = MathfTools.SetMag(sumOpositeDirection, 1);
                
                if(MathfTools.GetVectorMag(desiredVelocity) == 0) return;
                
                desiredVelocity = MathfTools.SetMag(desiredVelocity, 10);
                
                var steeringForce = desiredVelocity - currentRgb.PhysicsVelocity.ValueRO.Linear;
                            
                currentRgb.ApplyImpulse(MathfTools.LimitMag(steeringForce, 10f));
            }
        }
    }
}