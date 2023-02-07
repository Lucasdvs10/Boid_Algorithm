using Unity.Entities;
using UnityEngine;

namespace Ecs.Main.Components.Rules {
    public class AligmentTagAuthoring : MonoBehaviour{
        
    }
    
    public class AlignmentTagBaker : Baker<AligmentTagAuthoring> {
        public override void Bake(AligmentTagAuthoring authoring) {
            AddComponent(new AlignmentTag());
        }
    }
}