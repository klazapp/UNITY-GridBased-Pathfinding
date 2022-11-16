using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityMathematicsHelper;
using Random = UnityEngine.Random;

public class PathfindingManager : TMonoSingleton<PathfindingManager>
{
    [Header("Path Points")] 
    [SerializeField]
    private Transform pathPointsHolderTransform;
    private Transform[] pathPointsChildren;
    
    private PathfindingData pathfindingData;

    private void Start()
    {
        //Store all path's individual points
        pathPointsChildren = new Transform[pathPointsHolderTransform.childCount];
        for (var i = 0; i < pathPointsHolderTransform.childCount; i++)
        {
            pathPointsChildren[i] = pathPointsHolderTransform.GetChild(i);
        }
    }

    public void OnSpawnPathPointsAtPlayerPos(float3 playerPos)
    {
        pathPointsHolderTransform.localPosition = playerPos;
    }

    public void OnActivatePathfinding()
    {
        pathfindingData = new PathfindingData(pathPointsChildren.Length);

        //Add all possible adjacent points 
        for (var i = 0; i < pathPointsChildren.Length; i++)
        {
            var iPointPos = pathPointsChildren[i].localPosition;
            
            for (var j = 0; j < pathPointsChildren.Length; j++)
            {
                var jPointPos = pathPointsChildren[j].localPosition;
                
                var distance = math.distance(iPointPos, jPointPos);

                if (distance <= 1f)
                {
                    pathfindingData.AddAdjacentPoints(i, j);
                }
            }
        }
    }

    public IEnumerable<float3> OnRetrievePathsToTargetPoint(float3 startingPoint, float3 targetPoint)
    {
        //Get starting and final index based on position values
        var getStartingIndex = 0;
        var getFinalIndex = 0;
        for (var i = 0; i < pathPointsChildren.Length; i++)
        {
            if (mathAdditional.CompareFloat3XZPos(startingPoint, pathPointsChildren[i].localPosition))
                getStartingIndex = i;
            
            if (mathAdditional.CompareFloat3XZPos(targetPoint, pathPointsChildren[i].localPosition))
                getFinalIndex = i;
        }

        //Retrieves list of possible path points' index
        var pathPoints = pathfindingData.GetAllPathPoints(getStartingIndex, getFinalIndex);

        //Pick random path out of all possibilities
        var randomlyPickedPath = pathPoints[Random.Range(0, pathPoints.Count)];
        var newlyConstructedPath = new List<float3>();

        //Assign path values based on selected list of indexes
        for (var i = 0; i < randomlyPickedPath.finalPathPoints.Count; i++)
        {
            var pointIndex = randomlyPickedPath.finalPathPoints[i];
            newlyConstructedPath.Add(pathPointsChildren[pointIndex].localPosition);
        }
    
        return newlyConstructedPath;
    }
}
