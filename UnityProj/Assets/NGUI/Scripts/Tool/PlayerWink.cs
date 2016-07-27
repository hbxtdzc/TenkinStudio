using UnityEngine;
using System.Collections;

public class PlayerWink : MonoBehaviour {

    public string PathPre = "PlayerFace/";
    public string RulesNormalEnd = "_Face";
    public string RulesEnd = "_Face_Closeeye";
    public string RulesObjEnd = "_Face";
    private float IntervalTime0 = 1f;
    private float IntervalTime1 = 3f;
    private float m_curUseTime;
    private float CircleTime = 0.1f;
    private Texture TextureNormal;
    private Texture Texture1;
    private Material ChangeObj;
    private float m_lastTime;
    private bool m_isNormal;
    private string m_curMaterialName;
    private string m_name;

    void Awake()
    {
        m_name = gameObject.name.Replace("(Clone)", "");
    }

	// Use this for initialization
	void Start ()
	{
        //m_name = gameObject.name.Replace("(Clone)", "");
        Material MaterialNormal = Resources.Load<Material>(PathPre + m_name + RulesNormalEnd);
	    if (MaterialNormal != null)
	    {
	        TextureNormal = MaterialNormal.mainTexture;
	    }
        Material Material1 = Resources.Load<Material>(PathPre + m_name + RulesEnd);
	    if (Material1 != null)
	    {
	        Texture1 = Material1.mainTexture;
	    }
        Transform[] objChildren = gameObject.GetComponentsInChildren<Transform>(true);
        //MaterialNormal = (Material)Instantiate(MaterialNormalPrrefab);
        //Material1 = (Material)Instantiate(Material1Prefab);
        for (int i = 0; i < objChildren.Length; ++i)
        {
            //Debug.LogError(objChildren[i].name + "          " + (name + RulesObjEnd));
            if ((objChildren[i].name == m_name + RulesObjEnd))
            {
                SkinnedMeshRenderer meshRender = objChildren[i].GetComponent<SkinnedMeshRenderer>();
                if (meshRender.materials.Length > 0)
                {
                    ChangeObj = meshRender.materials[0];
                }
                break;
            }
        }
        m_isNormal = true;
        SetTexture(TextureNormal);	    
	    m_curUseTime = IntervalTime0;
        m_lastTime = Time.time;
	    m_curMaterialName = "";
	    if (ChangeObj != null)
	    {
	        m_curMaterialName = ChangeObj.name.Replace(" (Instance)", "");
	    }
	}

    private void SetTexture(Texture texture)
    {
        if (ChangeObj != null)
        {
            if (ChangeObj != null && m_curMaterialName == (m_name + RulesNormalEnd) && texture != null)
            {
                if (ChangeObj.HasProperty("_MainTex"))
                {
                    ChangeObj.SetTexture("_MainTex", texture);
                }
            }
        }
    }
	// Update is called once per frame
	void Update ()
    {
        if (Time.time - m_lastTime >= m_curUseTime && ChangeObj != null && m_curMaterialName == (m_name + RulesNormalEnd))
	    {
	        if (Time.time - m_lastTime >= (m_curUseTime + CircleTime) && !m_isNormal)
	        {
	            m_isNormal = true;
	            SetTexture(TextureNormal);
	            m_curUseTime = m_curUseTime <= IntervalTime0 ? IntervalTime1 : IntervalTime0;
	            m_lastTime = Time.time;
	        }
	        else if (m_isNormal)
	        {
	            m_isNormal = false;
	            SetTexture(Texture1);
	        }

	    }
    }
}
