using Ecs.Components.BoidRules.Alignment;
using Ecs.Main.Components.Rules;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics.Aspects;

namespace Ecs.Components.BoidRules.Cohesion {
    public partial struct CohesionSystem : ISystem{
        public void OnCreate(ref SystemState state) {
        }

        public void OnDestroy(ref SystemState state) {
        }

        public void OnUpdate(ref SystemState state) {
            int perceiveRadious = 15; 
            var entitiesAmount = 100;
                        
            foreach ((CohesionTag tag, RigidBodyAspect currentRgb) in SystemAPI.Query<CohesionTag, RigidBodyAspect>() ) { 
                var currentEntity = currentRgb.Entity;
                float3 sumPositions = float3.zero;
                foreach ((CohesionTag othersTag, RigidBodyAspect otherRgb) in SystemAPI.Query<CohesionTag, RigidBodyAspect>()
                 ) {
                    var otherEntity = otherRgb.Entity;
                    var distanceBetweenEntities = MathfTools.Distance(currentRgb.Position, otherRgb.Position);
            
                    if (currentEntity != otherEntity && distanceBetweenEntities <= perceiveRadious) {
                        sumPositions += otherRgb.Position;
                    }
                }
            
                var avaragePosition = sumPositions / entitiesAmount;
                var desiredVelocity = avaragePosition - currentRgb.Position;
                
                desiredVelocity = MathfTools.SetMag(desiredVelocity, 10);
                var steeringForce = desiredVelocity - currentRgb.LinearVelocity;
                            
                currentRgb.ApplyLinearImpulseLocalSpace(MathfTools.LimitMag(steeringForce, 200f));
                currentRgb.LinearVelocity = MathfTools.LimitMag(currentRgb.LinearVelocity, 10f);
            }
        }
    }
}