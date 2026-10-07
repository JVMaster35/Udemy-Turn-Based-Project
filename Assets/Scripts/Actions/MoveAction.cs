using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveAction : BaseAction
{
    public event EventHandler OnStartMoving;
    public event EventHandler OnStopMoving;
    public event EventHandler<OnChangeFloorsStartedEventArgs> OnChangedFloorsStarted;
    public class OnChangeFloorsStartedEventArgs : EventArgs
    {
        public GridPosition unitGridPosition;
        public GridPosition targetGridPosition;
    }


    [SerializeField] private int maxMoveDistance = 4;

    private List<Vector3> positionList;
    private int currentPositionIndex;
    private bool isChangingFloors;
    private float differentFloorsTeleportTimer;
    private float differentFloorsTeleportTimerMax = .5f;

    // Update is called once per frame
    void Update()
    {
        if(!isActive) 
        {
            return;
        }

        Vector3 t_Position = positionList[currentPositionIndex];

        if (isChangingFloors)
        {
            //Stop & Teleport Logic
            Vector3 targetSameFloorPosition = t_Position;
            targetSameFloorPosition.y = transform.position.y;

            Vector3 rotateDirection = (targetSameFloorPosition - transform.position).normalized;

            float rotateSpeed = 10f;
            transform.forward = Vector3.Slerp(transform.forward, rotateDirection, Time.deltaTime * rotateSpeed);

            differentFloorsTeleportTimer -= Time.deltaTime;

            if (differentFloorsTeleportTimer < 0f)
            {
                isChangingFloors = false;
                transform.position = t_Position;
            }
        }
        else
        {
            //Regular Move Logic
            Vector3 moveDirection = (t_Position - transform.position).normalized;

            float rotateSpeed = 10f;
            transform.forward = Vector3.Slerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);

            float moveSpeed = 4f;
            transform.position += moveSpeed * Time.deltaTime * moveDirection;
        }

        float stoppingDistance = .1f;
        if (Vector3.Distance(transform.position, t_Position) < stoppingDistance)
        {
            currentPositionIndex++;

            if (currentPositionIndex >= positionList.Count)
            {
                OnStopMoving?.Invoke(this, EventArgs.Empty);
                ActionComplete();
            }
            else
            {
                t_Position = positionList[currentPositionIndex];
                GridPosition t_GridPosition = LevelGrid.Instance.GetGridPosition(t_Position);
                GridPosition unitGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);

                if(t_GridPosition.floor != unitGridPosition.floor)
                {
                    //Different Floors
                    isChangingFloors = true;
                    differentFloorsTeleportTimer = differentFloorsTeleportTimerMax;

                    OnChangedFloorsStarted?.Invoke(this, new OnChangeFloorsStartedEventArgs
                    {
                        unitGridPosition = unitGridPosition,
                        targetGridPosition = t_GridPosition,
                    });

                }
            }
        }

    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        List<GridPosition> pathGridPositionList = PathfindingCube.Instance.FindPath(unit.GetGridPosition(), gridPosition, out int pathLength);

        currentPositionIndex = 0;

        positionList = new();

        foreach(GridPosition pathGridPosition in pathGridPositionList)
        {
            positionList.Add(LevelGrid.Instance.GetWorldPosition(pathGridPosition));
        }

        OnStartMoving?.Invoke(this, EventArgs.Empty);

        ActionStart(onActionComplete);
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        GridPosition unitGridPosition = unit.GetGridPosition();

        for (int x = -maxMoveDistance; x <= maxMoveDistance; x++)
        {
            for (int z = -maxMoveDistance; z <= maxMoveDistance; z++)
            {
                for (int floor = -maxMoveDistance; floor <= maxMoveDistance; floor++)
                {
                    GridPosition offsetGridPosition = new GridPosition(x, z, floor);
                    GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                    if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                    {
                        continue;
                    }

                    if (unitGridPosition == testGridPosition)
                    {
                        // Same Grid Position where the unit is already at
                        continue;
                    }

                    if (LevelGrid.Instance.IsGridPositionAlreadyOccupied(testGridPosition))
                    {
                        // Grid Position already occupied with another Unit
                        continue;
                    }

                    if (!PathfindingCube.Instance.IsWalkableGridPosition(testGridPosition))
                    {
                        // Grid Position is Unwalkable in Pathfinder
                        continue;
                    }

                    if (!PathfindingCube.Instance.HasPath(unitGridPosition, testGridPosition))
                    {
                        // There is no Available Path
                        continue;
                    }

                    int pathfindingDistanceMultipler = 10;
                    if (PathfindingCube.Instance.GetPathLength(unitGridPosition, testGridPosition) > maxMoveDistance * pathfindingDistanceMultipler)
                    {
                        // Path Length is too Long
                        continue;
                    }

                    validGridPositionList.Add(testGridPosition);
                }
            }
        }

        return validGridPositionList;
    }

    public override string GetActionName()
    {
        return "move";
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        int targetCountAtGridPosition = unit.GetAction<ShootAction>().GetTargetCountAtPosition(gridPosition);

        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = targetCountAtGridPosition * 10,
        };
    }



}
