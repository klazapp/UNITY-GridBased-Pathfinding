using System.Collections.Generic;
using Unity.Mathematics;

public struct PlayerMovingComponents
{
    public bool playerIsMoving;
        
    public List<float3> pathsToMoveTo;
    public int currentPathIndex;
    public float moveLerpT;
}
