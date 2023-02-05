// using Ecs.Components;
// using NUnit.Framework;
// using Unity.Entities;
// using Unity.Mathematics;
// using Unity.Transforms;
//
// public class Kappa
// {
//     // A Test behaves as an ordinary method
//     [Test]
//     public void KappaSimplePasses() {
//
//         World newWorld = new World("Meu novo mundo!");
//         
//
//         var myEntity = newWorld.EntityManager.CreateEntity();
//         newWorld.EntityManager.AddComponent<LocalTransform>(myEntity);
//
//         newWorld.GetOrCreateSystem<TestSystem>();
//         newWorld.Update();
//         
//         Assert.AreEqual(new float3(10,0,0), newWorld.EntityManager.GetComponentData<LocalTransform>(myEntity).Position);
//     }
// }
