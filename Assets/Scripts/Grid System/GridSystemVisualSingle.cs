using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisualSingle : MonoBehaviour
{
    [SerializeField] private MeshRenderer m_Renderer;
    //[SerializeField] private GameObject selectedGameObject;

    public void Show(Material material)
    {
        m_Renderer.enabled = true;
        m_Renderer.material = material;
    }

    public void Hide()
    {
        m_Renderer.enabled = false;
    }

/*    public void ShowSelected()
    {
        selectedGameObject.SetActive(true);
    }

    public void HideSelected()
    {
        selectedGameObject.SetActive(false);
    }*/

}
