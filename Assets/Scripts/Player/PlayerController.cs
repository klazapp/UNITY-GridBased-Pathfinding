using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityMathematicsHelper;

public class PlayerController : MonoBehaviour
{
    private Transform playerTransform;
    private PlayerMovingComponents playerMovingComponents;
    
    //Misc variables
    private const float PLAYER_Y_POS = 1.34f;
    private const float PLAYER_MOVE_SPEED = 6f;
    
    private void OnEnable()
    {
        InputController.PathPointClickedEvent += PathPointClickedCallback;
    }

    private void OnDisable()
    {
        InputController.PathPointClickedEvent -= PathPointClickedCallback;
    }

    private void Start()
    {
        playerMovingComponents = new();
        
        playerTransform = this.transform;

        //Activate pathfinding
        ActivatePathfindingAtPlayerPos();
    }

    private void ActivatePathfindingAtPlayerPos()
    {
        PathfindingManager.Instance.OnSpawnPathPointsAtPlayerPos(playerTransform.position);
        PathfindingManager.Instance.OnActivatePathfinding();
    }

    private void Update()
    {
        if (!playerMovingComponents.playerIsMoving)
            return;

        MovePlayerAlongPath();
        
        CheckAndPrepareNextPath();
    }

    private void PathPointClickedCallback(float3 clickedPos)
    {
        //Retrieves one path to target position
        var paths = PathfindingManager.Instance.OnRetrievePathsToTargetPoint(playerTransform.position, clickedPos);

        //Prepare player to move components 
        playerMovingComponents.pathsToMoveTo = new List<float3>(paths);
        playerMovingComponents.moveLerpT = 0;
        playerMovingComponents.currentPathIndex = 0;
        playerMovingComponents.playerIsMoving = true;
    }

    private void MovePlayerAlongPath()
    {
        float3 playerPos = playerTransform.localPosition;
        //Retrieve target pos from the stored path and the current index 
        var targetPos = playerMovingComponents.pathsToMoveTo[playerMovingComponents.currentPathIndex];
        playerPos = math.lerp(playerPos, targetPos, playerMovingComponents.moveLerpT);
        playerPos.y = PLAYER_Y_POS;
        playerTransform.localPosition = playerPos;
    }
    
    private void CheckAndPrepareNextPath()
    {
        playerMovingComponents.moveLerpT += Time.deltaTime * PLAYER_MOVE_SPEED;

        if (playerMovingComponents.moveLerpT < 1f)
            return;
        
        playerMovingComponents.moveLerpT = 0f;

        //Increment path index until reaching max path length
        playerMovingComponents.currentPathIndex = mathAdditional.ClampToBelowValue(
            playerMovingComponents.currentPathIndex + 1, playerMovingComponents.pathsToMoveTo.Count - 1);
    }
}
