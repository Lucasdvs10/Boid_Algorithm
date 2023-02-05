// using Unity.Collections;
// using Unity.Entities;
// using Unity.Mathematics;
// using Unity.Physics.Aspects;
// using Unity.Transforms;
// using UnityEngine;
//
// namespace Ecs.Components {
//     public partial class TestSystem : SystemBase {
//         protected override void OnStartRunning() {
//             foreach (var aspect in SystemAPI.Query<RefRW<LocalTransform>>()) {
//                 aspect.ValueRW.Position = new float3(10, 0, 0);
//             }
//         }
//
//         protected override void OnUpdate() {
//         }
//     }
//     
// }