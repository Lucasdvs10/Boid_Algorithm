using Unity.Mathematics;
using UnityEngine;

namespace Ecs.Components.BoidRules.Alignment {
    public class MathfTools {
        public static float Distance(float3 v1, float3 v2) {
            return Mathf.Sqrt((v1.x - v2.x) * (v1.x - v2.x) + (v1.y - v2.y) * (v1.y - v2.y) +
                              (v1.z - v2.z) * (v1.z - v2.z));
        }

        public static float3 LimitMag(float3 oldVector, float maxMag) {
            return SetMag(oldVector, Mathf.Clamp(GetVectorMag(oldVector), 0f, maxMag));
        }

        public static float3 SetMag(float3 oldVector, float newMagnitude) {
            var oldMag = GetVectorMag(oldVector);
            
            if(oldMag == 0) return float3.zero;
            
            return (oldVector / oldMag) * newMagnitude;
        }

        private static float GetVectorMag(float3 vector) {
            var oldMag = Mathf.Sqrt(Mathf.Pow(vector.x, 2) + Mathf.Pow(vector.y, 2) + Mathf.Pow(vector.z, 2));
            return oldMag;
        }
    }
}