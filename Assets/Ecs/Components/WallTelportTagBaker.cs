using Unity.Entities;
using UnityEngine;

namespace Ecs.Components {
    public class WallTelportTagBaker : MonoBehaviour{
        
    }
    
    public class WallTelepBaker : Baker<WallTelportTagBaker> {
        public override void Bake(WallTelportTagBaker authoring) {
            AddComponent(new TeleportAtBoardTag());
        }
    }
}