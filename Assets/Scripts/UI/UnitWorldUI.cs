using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UnitWorldUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI actionPointsTxt;
    [SerializeField] private Unit unit;
    [SerializeField] private Image healthBarImg;
    [SerializeField] private HealthSystem healthSystem;

    private void Start()
    {
        Unit.OnAnyActionPointsChange += Unit_OnAnyActionPointsChange;
        healthSystem.OnDamaged += HealthSystem_OnDamaged;

        UpdateActionPointText();
        UpdateHealthBar();
    }

    private void UpdateActionPointText()
    {
        actionPointsTxt.text = unit.GetActionPoints().ToString();
    }

    private void UpdateHealthBar()
    {
        healthBarImg.fillAmount = healthSystem.GetHealthNormalized();
    }

    private void Unit_OnAnyActionPointsChange(object sender, EventArgs e)
    {
        UpdateActionPointText();
    }

    private void HealthSystem_OnDamaged(object sender, EventArgs e)
    {
        UpdateHealthBar();
    }
}
