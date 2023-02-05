using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics.Aspects;
using UnityEngine;

namespace Ecs.Components {
    public partial class TeleportAtBoardSystem : SystemBase {

        private WallComp _wallSingleton;
        
        protected override void OnStartRunning() {
            base.OnStartRunning();

            _wallSingleton = SystemAPI.GetSingleton<WallComp>();
        }

        protected override void OnUpdate() {
            foreach (var boid in SystemAPI.Query<TeleportAtBoardTag, RigidBodyAspect>()) {
                var boidPosition = boid.Item2.Position;

                if (Mathf.Abs(boidPosition.x) > Mathf.Abs(_wallSingleton.XWallOffset)) {
                     boid.Item2.Position *= new float3(-0.8f,1,1);
                }
                
                if (Mathf.Abs(boidPosition.y) > Mathf.Abs(_wallSingleton.YWallOffset)) {
                    boid.Item2.Position *= new float3(1, -0.8f, 1);
                }
                
                if (Mathf.Abs(boidPosition.z) > Mathf.Abs(_wallSingleton.zWallOffset)) {
                     boid.Item2.Position *= new float3(1,1,-0.8f);
                }
                
            }
            
            
        }
    }
}