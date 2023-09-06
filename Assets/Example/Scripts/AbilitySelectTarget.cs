using System;
using System.Collections;
using System.Collections.Generic;
using Example;
using UAS;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class AbilitySelectTarget : MonoBehaviour
{
    public Image cursor;
    public new Camera camera;
    public LayerMask unitLayerMask;
    public LayerMask floorLayerMask;
    private bool m_IsSelectingUnit;
    private bool m_IsSelectingPoint;

    private Vector2 m_InputPosition;

    private Action<IUnit> m_OnUnitSelected;

    private Action<Vector3?> m_OnPointSelected;
    // public Sprite cursor;
    // Start is called before the first frame update
    void Start()
    {
        cursor.gameObject.SetActive(false);
    }
    public void OnSelectTargetMove(InputAction.CallbackContext value)
    {
        if(!m_IsSelectingUnit && !m_IsSelectingPoint)
            return;
        
        m_InputPosition = value.ReadValue<Vector2>();
        
        RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)cursor.transform.parent, m_InputPosition,
            null, out var localPoint);
        ((RectTransform)cursor.transform).anchoredPosition = localPoint;
    }

    public void OnSelectTargetTap(InputAction.CallbackContext value)
    {
        if(!m_IsSelectingUnit && !m_IsSelectingPoint)
            return;
        
        if (value.performed)
        {
            Ray ray = camera.ScreenPointToRay(m_InputPosition);
            if (m_IsSelectingUnit)
            {
                if (Physics.Raycast(ray, out var hit, 100f, unitLayerMask))
                {
                    IUnit unit = hit.collider.GetComponent<IUnit>();
                    if (unit != null)
                    {
                        EndSelectingUnit(unit);
                    }
                    else
                    {
                        EndSelectingUnit(null);
                    }
                }
                else
                {
                    EndSelectingUnit(null);
                }
            }
            else if (m_IsSelectingPoint)
            {
                if (Physics.Raycast(ray, out var hit, 100f, floorLayerMask))
                {
                    EndSelectingPoint(hit.point);
                }
                else
                {
                    EndSelectingPoint(null);
                }
            }

        }
    }

    public void StartSelectingUnit(Action<IUnit> onUnitSelected)
    {
        cursor.gameObject.SetActive(true);
        m_IsSelectingUnit = true;
        this.m_OnUnitSelected = onUnitSelected;
    }

    public void StartSelectingPoint(Action<Vector3?> onPointSelected)
    {
        cursor.gameObject.SetActive(true);
        m_IsSelectingPoint = true;
        this.m_OnPointSelected = onPointSelected;
    }

    private void EndSelectingUnit(IUnit target)
    {
        m_IsSelectingUnit = false;
        m_OnUnitSelected.Invoke(target);
        cursor.gameObject.SetActive(false);
    }

    private void EndSelectingPoint(Vector3? point)
    {
        m_IsSelectingUnit = false;
        m_OnPointSelected.Invoke(point);
        cursor.gameObject.SetActive(false);
    }

    // private void EndSelecting()
    // {
    //     
    // }
}
