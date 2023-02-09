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
            int perceiveRadious = 10;
            
            foreach ((AlignmentTag tag, MyPhysicsAspect currentRgb) in SystemAPI.Query<AlignmentTag, MyPhysicsAspect>() ) {
                var currentEntity = currentRgb.Entity;
                float3 sumVelocities = float3.zero;
                int neighboursAmount = 1;
                
                foreach ((AlignmentTag othersTag, MyPhysicsAspect otherRgb) in SystemAPI.Query<AlignmentTag, MyPhysicsAspect>() ) {
                    var otherEntity = otherRgb.Entity;
                    var distanceBetweenEntities = MathfTools.Distance(currentRgb.Transform.LocalPosition, otherRgb.Transform.LocalPosition);

                    if (currentEntity != otherEntity && distanceBetweenEntities <= perceiveRadious) {
                        sumVelocities += otherRgb.PhysicsVelocity.ValueRO.Linear;
                        neighboursAmount++;
                    }
                }

                var desiredVelocity = sumVelocities / neighboursAmount;
                
                if(MathfTools.GetVectorMag(desiredVelocity) == 0) return;
                
                desiredVelocity = MathfTools.SetMag(desiredVelocity, 10);
                var steeringForce = desiredVelocity - currentRgb.PhysicsVelocity.ValueRO.Linear;
                
                
                currentRgb.ApplyImpulse(MathfTools.LimitMag(steeringForce, 10f));
                currentRgb.PhysicsVelocity.ValueRW.Linear = MathfTools.LimitMag(currentRgb.PhysicsVelocity.ValueRO.Linear, 10f);
            }
        }
    }
}