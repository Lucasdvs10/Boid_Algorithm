using Ecs.Components.BoidRules.Alignment.Aspects;
using Ecs.Main.Components.Rules;
using Unity.Entities;
using Unity.Mathematics;

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
            int perceiveRadious = 20;
            
            foreach ((AlignmentTag tag, MyPhysicsAspect currentRgb) in SystemAPI.Query<AlignmentTag, MyPhysicsAspect>() ) {
                float3 sumDirections = float3.zero;
                int neighboursAmount = 0;
                
                foreach ((AlignmentTag othersTag, MyPhysicsAspect otherRgb) in SystemAPI.Query<AlignmentTag, MyPhysicsAspect>() ) {
                    var distanceBetweenEntities = MathfTools.Distance(currentRgb.Transform.LocalPosition, otherRgb.Transform.LocalPosition);

                    if (distanceBetweenEntities > 0 && distanceBetweenEntities <= perceiveRadious) {
                        sumDirections += MathfTools.SetMag(otherRgb.PhysicsVelocity.ValueRO.Linear, 1);
                        neighboursAmount++;
                    }
                }

                if(neighboursAmount == 0) return;
                var desiredVelocity = sumDirections;
                
                if(MathfTools.GetVectorMag(desiredVelocity) == 0) return;
                
                desiredVelocity = MathfTools.SetMag(desiredVelocity, 40);
                var steeringForce = desiredVelocity - currentRgb.PhysicsVelocity.ValueRO.Linear;
                
                
                currentRgb.ApplyImpulse(MathfTools.LimitMag(steeringForce, 40f));
            }
        }
    }
}