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
            int perceiveRadious = 10; 
            
            foreach ((CohesionTag tag, MyPhysicsAspect currentRgb) in SystemAPI.Query<CohesionTag, MyPhysicsAspect>() ) { 
                var currentEntity = currentRgb.Entity;
                float3 sumOpositePositions = float3.zero;
                int neighboursAmount = 1;
                
                foreach ((CohesionTag othersTag, MyPhysicsAspect otherRgb) in SystemAPI.Query<CohesionTag, MyPhysicsAspect>()) {
                    var otherEntity = otherRgb.Entity;
                    var distanceBetweenEntities = MathfTools.Distance(currentRgb.Transform.LocalPosition,
                        otherRgb.Transform.LocalPosition);
            
                    if (currentEntity != otherEntity && distanceBetweenEntities <= perceiveRadious) {
                        sumOpositePositions += currentRgb.Transform.LocalPosition - otherRgb.Transform.LocalPosition;
                        neighboursAmount++;
                    }
                }
            
                var avaragePosition = sumOpositePositions / neighboursAmount;
                var desiredVelocity = avaragePosition - currentRgb.Transform.LocalPosition;
                
                if(MathfTools.GetVectorMag(desiredVelocity) == 0) return;
                
                desiredVelocity = MathfTools.SetMag(desiredVelocity, 10);
                
                var steeringForce = desiredVelocity - currentRgb.PhysicsVelocity.ValueRO.Linear;
                            
                currentRgb.ApplyImpulse(MathfTools.LimitMag(steeringForce, 100f));
                currentRgb.PhysicsVelocity.ValueRW.Linear = MathfTools.LimitMag(currentRgb.PhysicsVelocity.ValueRO.Linear, 10f);
            }
        }
    }
}