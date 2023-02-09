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
            //todo: a resposta do nosso problema está no separation, eu acredito
            // talvez esteja no cohesion tbm, não tenho certeza
            
            foreach ((SeparationTag tag, MyPhysicsAspect currentRgb) in SystemAPI.Query<SeparationTag, MyPhysicsAspect>() ) { 
                var currentEntity = currentRgb.Entity;
                float3 sumOpositeDirection = float3.zero;
                
                foreach ((SeparationTag othersTag, MyPhysicsAspect otherRgb) in SystemAPI.Query<SeparationTag, MyPhysicsAspect>()) {
                    var otherEntity = otherRgb.Entity;
                    var distanceBetweenEntities = MathfTools.Distance(currentRgb.Transform.LocalPosition,
                        otherRgb.Transform.LocalPosition);
            
                    if (distanceBetweenEntities > 0 && distanceBetweenEntities <= perceiveRadious) {
                        sumOpositeDirection += (currentRgb.Transform.LocalPosition - otherRgb.Transform
                        .LocalPosition) / distanceBetweenEntities;
                    }
                }
            
                var desiredVelocity = MathfTools.SetMag(sumOpositeDirection, 1);
                
                if(MathfTools.GetVectorMag(desiredVelocity) == 0) return;
                
                desiredVelocity = MathfTools.SetMag(desiredVelocity, 10);
                
                var steeringForce = desiredVelocity - currentRgb.PhysicsVelocity.ValueRO.Linear;
                            
                currentRgb.ApplyImpulse(MathfTools.LimitMag(steeringForce, 10f));
                // currentRgb.PhysicsVelocity.ValueRW.Linear = MathfTools.LimitMag(currentRgb.PhysicsVelocity.ValueRO.Linear, 10f);
            }
        }
    }
}