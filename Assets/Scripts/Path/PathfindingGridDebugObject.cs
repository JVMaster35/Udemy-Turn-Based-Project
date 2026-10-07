using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PathfindingGridDebugObject : GridDebugObject
{
    [SerializeField]
    private TextMeshPro gCostTxt;
    [SerializeField]
    private TextMeshPro hCostTxt;
    [SerializeField]
    private TextMeshPro fCostTxt;
    [SerializeField]
    private SpriteRenderer isWalkableSpriteRender;

    private PathNode pathNode;

    public override void SetGridObject(object gridObject)
    {
        base.SetGridObject(gridObject);
        pathNode = (PathNode)gridObject;
    }

    protected override void Update()
    {
        base.Update();
        gCostTxt.text = pathNode.GetGCost().ToString();
        hCostTxt.text = pathNode.GetHCost().ToString();
        fCostTxt.text = pathNode.GetFCost().ToString();
        isWalkableSpriteRender.color = pathNode.IsWalkable() ? Color.green : Color.red;
    }
}
