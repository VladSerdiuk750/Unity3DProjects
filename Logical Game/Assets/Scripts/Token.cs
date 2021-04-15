﻿using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.EventSystems;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class Token : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Camera m_camera;

    private Vector3 m_pointerPositionBeforeDrag;

    private Vector3 m_positionBeforeDrag;

    private int[] m_dragSpace;

    private int m_tokenType;

    private AudioSource m_audioSource;

    private void Start()
    {
        m_camera = Camera.main;
        
        transform.SetParent(Controller.Instance.Field.transform);
        
        AlignOnGrid();
        
        m_tokenType = Random.Range(0, Controller.Instance.TokenTypes);

        Material myMaterial = gameObject.GetComponent<Renderer>().material;

        myMaterial.SetColor("_Color", Controller.Instance.TokenColors[m_tokenType]);
        
        Controller.Instance.TokensByTypes[m_tokenType].Add(this);
        
        m_audioSource = gameObject.GetComponent<AudioSource>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        m_pointerPositionBeforeDrag = m_camera.ScreenToWorldPoint(eventData.position);
        m_positionBeforeDrag = transform.position;
        GetDragSpace();
        m_audioSource.Play();
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 previousPosition = transform.position;
        
        Vector3 mouseWorldPosition = m_camera.ScreenToWorldPoint(eventData.position);

        Vector3 totalDrag = mouseWorldPosition - m_pointerPositionBeforeDrag;

        if (Mathf.Abs(totalDrag.x) > Mathf.Abs(totalDrag.y))
        {
            float posX = Mathf.Clamp(mouseWorldPosition.x, m_positionBeforeDrag.x - m_dragSpace[1],
                m_positionBeforeDrag.x + m_dragSpace[0]);
            
            transform.position = new Vector3(posX, m_positionBeforeDrag.y, transform.position.z);
        }
        else
        {
            float posY = Mathf.Clamp(mouseWorldPosition.y, m_positionBeforeDrag.y - m_dragSpace[3],
                m_positionBeforeDrag.y + m_dragSpace[2]);
            
            transform.position = new Vector3(m_positionBeforeDrag.x, posY, transform.position.z);
        }

        float currentFrameTokenGrag = Vector3.Distance(previousPosition, transform.position);

        float clampedPitchDrag = Mathf.Clamp(currentFrameTokenGrag * 10, 0.9f, 1.05f);

        m_audioSource.pitch = Mathf.Lerp(m_audioSource.pitch, clampedPitchDrag, 0.5f);

        float clampedVolumeDrag = Mathf.Clamp(currentFrameTokenGrag * 10, 0.2f, 1.2f);

        float interpolatedDrag = Mathf.Lerp(m_audioSource.volume, clampedVolumeDrag - 0.2f, 0.7f);

        m_audioSource.volume = interpolatedDrag * Controller.Instance.Audio.SfxVolume;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        AlignOnGrid();
        m_audioSource.Stop();
        Controller.Instance.TurnDone();
    }

    private void AlignOnGrid()
    {
        Vector3 alignedPosition = transform.position;
        alignedPosition.x = Mathf.Round(transform.position.x);
        alignedPosition.y = Mathf.Round(transform.position.y);
        transform.position = alignedPosition;
    }

    private void GetDragSpace()
    {
        int OddEven = 1;

        if ((int)Controller.Instance.FieldSize % 2 != 0)
        {
            OddEven = 0;
        }
        
        m_dragSpace = new int[] {0, 0, 0, 0};
        int halfField = ((int)Controller.Instance.FieldSize - 1) / 2;

        m_dragSpace[0] = CheckSpace(Vector2.right);

        if (m_dragSpace[0] == -1)
        {
            m_dragSpace[0] = halfField - (int)transform.position.x + OddEven;
        }

        m_dragSpace[1] = CheckSpace(Vector2.left);
        
        if (m_dragSpace[1] == -1)
        {
            m_dragSpace[1] = halfField + (int)transform.position.x;
        }

        m_dragSpace[2] = CheckSpace(Vector2.up);
        
        if (m_dragSpace[2] == -1)
        {
            m_dragSpace[2] = halfField - (int)transform.position.y + OddEven;
        }

        m_dragSpace[3] = CheckSpace(Vector2.down);
        
        if (m_dragSpace[3] == -1)
        {
            m_dragSpace[3] = halfField + (int)transform.position.y;
        }
    }

    private int CheckSpace(Vector2 direction)
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, direction);
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].collider.gameObject != gameObject)
            {
                return Mathf.FloorToInt(hits[i].distance);
            }
        }

        return -1;
    }
}
