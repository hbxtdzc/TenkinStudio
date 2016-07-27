using UnityEngine;
using System.Collections;

public class UIButtonState : MonoBehaviour
{
    public GameObject m_normalSprite;
    public GameObject m_pressSprite;

	// Use this for initialization
	void Start ()
	{
	    SetState(true);
	}

    void OnPress(bool isPressed)
    {
        SetState(!isPressed);
    }
    private void SetState(bool isNormal)
    {
        if (m_pressSprite != null)
        {
            m_pressSprite.SetActive(!isNormal);
        }
        if (m_normalSprite != null)
        {
            m_normalSprite.SetActive(isNormal); 
        }
    }
}
