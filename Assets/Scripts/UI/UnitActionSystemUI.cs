using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitActionSystemUI : MonoBehaviour
{
    [SerializeField] private Transform actionBtnPrefab;
    [SerializeField] private Transform actionBtnContainerTrans;
    [SerializeField] private TextMeshProUGUI actionPointsTxt;

    private List<ActionBtnUI> actionBtnUIList;

    private void Awake()
    {
        actionBtnUIList = new List<ActionBtnUI>();
    }

    private void Start()
    {
        UnitActionSystem.Instance.OnSelectedUnitChange += UnitActionSystem_OnSelectedUnitChange;
        UnitActionSystem.Instance.OnSelectedActionChange += UnitActionSystem_OnSelectedActionChange;
        UnitActionSystem.Instance.OnActionStart += UnitActionSystem_OnActionStart;

        Unit.OnAnyActionPointsChange += Unit_OnAnyActionPointsChange;

        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChange;

        CreateUnitActionButtons();
        UpdateSelectedVisuals();
    }

    private void CreateUnitActionButtons()
    {
        foreach(Transform btnTransform in actionBtnContainerTrans)
        {
            Destroy(btnTransform.gameObject);
        }

        actionBtnUIList.Clear();

        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();

        foreach(BaseAction baseAction in selectedUnit.GetBaseActions())
        {
            Transform actionBtnTransform = Instantiate(actionBtnPrefab, actionBtnContainerTrans);
            ActionBtnUI actionBtnUI = actionBtnTransform.GetComponent<ActionBtnUI>();
            actionBtnUI.SetBaseAction(baseAction);

            actionBtnUIList.Add(actionBtnUI);
        }
    }

    private void UnitActionSystem_OnSelectedUnitChange(object sender, EventArgs e)
    {
        CreateUnitActionButtons();
        UpdateSelectedVisuals();
        UpdateActionPoints();
    }

    private void UnitActionSystem_OnSelectedActionChange(object sender, EventArgs e)
    {
        UpdateSelectedVisuals();
    }

    private void UnitActionSystem_OnActionStart(object sender, EventArgs e)
    {
        UpdateActionPoints();
    }

    private void Unit_OnAnyActionPointsChange(object sender, EventArgs e)
    {
        UpdateActionPoints();
    }

    private void TurnSystem_OnTurnChange(object sender, EventArgs e)
    {
        UpdateActionPoints();
    }

    private void UpdateSelectedVisuals()
    {
        foreach(ActionBtnUI actionBtnUI in actionBtnUIList)
        {
            actionBtnUI.UpdateSelectedVisual();
        }
    }

    private void UpdateActionPoints()
    {
        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
        actionPointsTxt.text = "Action Points: " + selectedUnit.GetActionPoints();
    }
}
