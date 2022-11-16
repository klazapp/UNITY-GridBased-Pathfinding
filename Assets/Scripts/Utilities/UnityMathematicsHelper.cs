using System.Runtime.CompilerServices;
using Unity.Mathematics;

namespace UnityMathematicsHelper
{
    public static class mathAdditional
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ClampToBelowValue(int intVal, int clampVal)
        {
            intVal = intVal > clampVal ? clampVal : intVal;
            return intVal;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool CompareFloat3XZPos(float3 pointA, float3 pointB)
        {
            return pointA.x == pointB.x && pointA.z == pointB.z;
        }
    }
}
