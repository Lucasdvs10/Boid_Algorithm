using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

namespace Ecs.Components.BoidRules.Alignment.Aspects {
    readonly partial struct MyPhysicsAspect : IAspect {
        public readonly Entity Entity;
        public readonly TransformAspect Transform;
        public readonly RefRW<PhysicsVelocity> PhysicsVelocity;

        public void ApplyImpulse(float3 impulseVector) {
            PhysicsVelocity.ValueRW.Linear = PhysicsVelocity.ValueRO.Linear + (impulseVector);
        }
    }
}