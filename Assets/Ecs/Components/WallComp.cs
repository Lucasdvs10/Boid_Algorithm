using Unity.Entities;
using Unity.Mathematics;

namespace Ecs.Components {
    public struct WallComp : IComponentData{
        public float3 PivotPosition;
        public float YWallOffset;
        public float XWallOffset;
        public float zWallOffset;

    }
}