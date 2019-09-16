using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleRenderer : MonoBehaviour
{
    public Grapple grapple;
    public GrappleFinder grappleFinder;


    //Grapple Renderer for grappled object
    public MeshRenderer m_RendererGrappled;

    public Material matGrappled;

    //GrappleRenderer for selecting
    public MeshRenderer m_Renderer = null;
    public MeshRenderer prevRenderer;
    public Material mat;
    public Material prevMat;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RenderChosenGrapple();
        RenderGrappledObj();
    }

    private void RenderGrappledObj()
    {
        if (!grapple.isGrappled && m_RendererGrappled == null)
            return;
        if (grapple.grappledObj == null && m_RendererGrappled != null)
        {
            m_RendererGrappled.material = prevMat;
            if(m_Renderer != null)
                m_Renderer.material = mat;
            m_RendererGrappled = null;

        }
        if (grapple.grappledObj != null)
        {
            m_RendererGrappled = grapple.grappledObj.GetComponent<MeshRenderer>();
            m_RendererGrappled.material = matGrappled;
        }
    }

    void RenderChosenGrapple()
    {
        if (grapple.isGrappled)
            return;
        if (grappleFinder.shortestObj == null && m_Renderer != null)
        {
            m_Renderer.material = prevMat;
            m_Renderer = null;
            prevRenderer = null;
        }
        if (grappleFinder.shortestObj != null)
        {
            m_Renderer = grappleFinder.shortestObj.GetComponent<MeshRenderer>();
            //base case
            if (prevRenderer == null)
            {
                prevRenderer = m_Renderer;
                prevMat = prevRenderer.material;
                m_Renderer.material = mat;
                return;
            }
            if (m_Renderer != prevRenderer)
            {

                prevRenderer.material = prevMat;
                m_Renderer.material = mat;
                prevRenderer = m_Renderer;
            }


        }
    }
}
