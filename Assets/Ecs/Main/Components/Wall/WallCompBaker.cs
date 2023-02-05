using Unity.Entities;
using UnityEngine;

namespace Ecs.Components {
    public class WallCompBaker : MonoBehaviour {
        public float YWallOffset;
        public float XWallOffset;
        public float zWallOffset;


        public void OnDrawGizmos() {
            
            Gizmos.DrawWireCube(transform.position, new Vector3(XWallOffset, YWallOffset, zWallOffset)*2f);
            
            Gizmos.DrawCube(transform.position, Vector3.one);
        }
    }
    
    public class WallBaker : Baker<WallCompBaker> {
        public override void Bake(WallCompBaker authoring) {
            AddComponent(new WallComp {
                PivotPosition = authoring.transform.position,
                YWallOffset = authoring.YWallOffset,
                XWallOffset = authoring.XWallOffset,
                zWallOffset = authoring.zWallOffset
            });
        }
    }
}