using UnityEngine;
using System.Collections;

[RequireComponent(typeof (Camera))]
public class CameraAdapter : MonoBehaviour
{
    public float Aspect
    {
        get
        {
            return aspect;
        }
        set 
        {
            if (value > 0f)
            {
                aspect = value;
            }
            RefreshCameraRect();
        }
    }

    public bool UseUIRootDefaultAspect = true;

    private float aspect = 0f;

    private bool defaultExcute = false;

    private Camera m_camera;
    private int m_screenWidth;
    private int m_screenHeight;
    // Use this for initialization
    void Start()
    {
        Initiate();
        m_screenWidth = Screen.width;
        m_screenHeight = Screen.height;
    }

    void Update()
    {
        if (m_screenWidth != Screen.width && m_screenHeight != Screen.height)
        {
            m_screenWidth = Screen.width;
            m_screenHeight = Screen.height;
            RefreshCameraRect();
        }
    }

    private void Initiate()
    {
        SetRefreshCameraRect();
    }
    public void ResetRefreshCameraRect()
    {
        if (UseUIRootDefaultAspect)
        {
            Aspect = 16 / (float)9;
        }
    }

    private void SetRefreshCameraRect()
    {
        if (!defaultExcute)
        {
            defaultExcute = true;
            ResetRefreshCameraRect();
        }
    }

    public void RefreshCameraRect()
    {
        if (m_camera == null)
        {
            m_camera = GetComponent<Camera>();
        }
        if (aspect > 0f)
        {
            //Camera camera = GetComponent<Camera>();
            //float targetH = camera.pixelWidth / (camera.pixelHeight * aspect);
            //float targetV = (camera.pixelHeight * aspect) / camera.pixelWidth;
            int defaultScreenWith = Screen.width;
            int defaultScreenHeight = Screen.height;
            float aspectNow = defaultScreenWith/(float) defaultScreenHeight;
            float targetH = 1f;
            float targetV = 1f;
            if (aspectNow > aspect)
            {
                targetV =(defaultScreenHeight * aspect) / defaultScreenWith;
            }
            else
            {
               targetH = defaultScreenWith / (defaultScreenHeight * aspect);
            }
            int defaultMask = m_camera.cullingMask;
            m_camera.cullingMask = LayerMask.GetMask("Nothing");
            Color defaultColor = m_camera.backgroundColor;
            m_camera.backgroundColor=new Color(0,0,0);
            if (m_camera.gameObject.activeInHierarchy)
            {
                m_camera.Render();
            }
            m_camera.backgroundColor = defaultColor;
            m_camera.cullingMask = defaultMask;
            if (targetH < 1f || targetV<1f)
            {
                Rect rect = new Rect((1f - targetV) / 2f, (1f - targetH) / 2f, targetV, targetH);
                m_camera.rect = rect;
            }
            else
            {
                m_camera.pixelRect = new Rect(m_camera.pixelRect.x,
                   m_camera.pixelRect.y, Screen.width, Screen.height);
                m_camera.rect = new Rect(0, 0, 1, 1);
            }
        }
    }
}
