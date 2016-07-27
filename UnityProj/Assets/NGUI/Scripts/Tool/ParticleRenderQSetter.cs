using UnityEngine;

public class ParticleRenderQSetter : MonoBehaviour
{
    public ParticleSystem[] m_particleSys;
    public int[] m_paticleRenderList;
    public int m_defaultRenderQ = 0;
    public UISprite m_useSprite;
	public UITexture m_useTexture;
    public bool m_isGreater = false;
	// Use this for initialization
    void Start()
    {
        SetRenderQ();
    }
    void OnEnable()
    {
        SetRenderQ();
    }
    private void ChangeRenderQ()
    {
        bool result = false;
		if (m_useSprite != null  || m_useTexture != null)
        {
            int newRenderQ = m_defaultRenderQ;
			if (m_useSprite != null && m_useSprite.drawCall != null)
	        {
                newRenderQ = m_isGreater ? m_useSprite.drawCall.renderQueue + 1 : m_useSprite.drawCall.renderQueue - 1;
	        }

			if(m_useTexture != null && m_useTexture.drawCall != null)
			{
				newRenderQ = m_isGreater ? m_useTexture.drawCall.renderQueue + 1 : m_useTexture.drawCall.renderQueue - 1;
			}

	        if (newRenderQ != m_defaultRenderQ)
	        {
	            result = true;
	            m_defaultRenderQ = newRenderQ;
	        }
	        for (int j = 0; j < m_paticleRenderList.Length; j++)
	        {
	            m_paticleRenderList[j] = m_defaultRenderQ;
	        }
            if (result)
            {
                SetRenderQ();
            }
	    }
        
    }

	void SetRenderQ ()
	{
        
	    for (int i = 0; i < m_particleSys.Length; i++)
	    {
            if (m_particleSys[i] != null && m_particleSys[i].renderer.sharedMaterial != null)
	        {
	            if (m_paticleRenderList.Length > i)
	            {
	                m_particleSys[i].renderer.sharedMaterial.renderQueue = m_paticleRenderList[i];
	            }
	            else
	            {
                    m_particleSys[i].renderer.sharedMaterial.renderQueue = m_defaultRenderQ;
	            }
	        }
	    }
    }

    void Update()
    {
        ChangeRenderQ();
    }
}