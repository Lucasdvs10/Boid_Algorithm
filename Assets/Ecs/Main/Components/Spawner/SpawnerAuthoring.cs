using Unity.Entities;
using UnityEngine;

namespace Ecs.Main.Components.Spawner {
    public class SpawnerAuthoring : MonoBehaviour {
        public GameObject GameObjectToSpawn;
        public int NumberToSpawn;
    }
    
    
    public class SpawnerBaker : Baker<SpawnerAuthoring> {
        public override void Bake(SpawnerAuthoring authoring) {
            AddComponent(new SpawnerComp {
                Entity = GetEntity(authoring.GameObjectToSpawn),
                NumberToSpawn = authoring.NumberToSpawn
            });
        }
    }
}