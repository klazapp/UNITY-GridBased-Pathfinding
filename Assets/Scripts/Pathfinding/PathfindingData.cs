using System.Collections.Generic;
using System;

[Serializable]
public class PathfindingData
{
    //No of total points
    public int pointsLength;

    //List of adjacent points list
    public List<int>[] adjacentPoints;
 
    //Final paths
    //private List<int> finalPathPoints;
    public List<PossiblePathPoints> possiblePathPointsList;

    //Pathfinding Data Constructor
    public PathfindingData(int pointsLength)
    {
        //Initialize points length
        this.pointsLength = pointsLength;

        //Initialize adjacent points list
        InitializeAdjacentPointsList();
    }

    //Initialize adjacent points list
    private void InitializeAdjacentPointsList()
    {
        adjacentPoints = new List<int>[pointsLength];

        for (var i = 0; i < pointsLength; i++)
        {
            adjacentPoints[i] = new List<int>();
        }
    }

    //Add adjacent points
    public void AddAdjacentPoints(int u, int v)
    {
        //Add v to u's list.
        adjacentPoints[u].Add(v);
    }

    public List<PossiblePathPoints> GetAllPathPoints(int source, int destination)
    {
        possiblePathPointsList = new List<PossiblePathPoints>();
      
        var pathPointIsVisited = new bool[pointsLength];
       
        var pathPointsList = new List<int>
        {
            source
        };

        CalculatePathPoints(source, destination, pathPointIsVisited, pathPointsList);

        return possiblePathPointsList;
    }

    private void CalculatePathPoints(int u, int d, IList<bool> pathPointIsVisited, ICollection<int> localPathList)
    {
        if (u.Equals(d))
        {
            //Stop traversing if match is found
            possiblePathPointsList.Add(new PossiblePathPoints
            {
                finalPathPoints = new List<int>(localPathList)
            });

            return;
        }

        //Mark the current node
        pathPointIsVisited[u] = true;

        //Continue for all vertices that are adjacent to current vertex
        foreach (var i in adjacentPoints[u])
        {
            if (pathPointIsVisited[i]) 
                continue;
            
            //Store current node in paths
            localPathList.Add(i);
            CalculatePathPoints(i, d, pathPointIsVisited, localPathList);
            //Remove current node in paths
            localPathList.Remove(i);
        }

        //Mark the current node
        pathPointIsVisited[u] = false;
    }
}

[Serializable]
public class PossiblePathPoints
{
    public List<int> finalPathPoints = new();
}