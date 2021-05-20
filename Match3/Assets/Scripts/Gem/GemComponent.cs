using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemComponent : MonoBehaviour
{

    [HideInInspector] public Point positionInBoard;
    [HideInInspector] public BoardHolder BoardHolder;
    public float dragSpeed;
    private Camera _mainCamera;

    
    private Transform _transform;
    private Vector3 _originalPos;
    private Vector3 _originalLocalScale;
    private bool _isSelected;
    private float _xOffset;
    private float _yOffset;
    
    // knows the movement of the mouse
    private Vector3 _lastMousePos;
    // Start is called before the first frame update
    private void Start()
    {
        _mainCamera = Camera.main;
       
        _isSelected = false;
        _transform = transform;
        _originalPos = _transform.position;
        _originalLocalScale = _transform.localScale;
        
        // offsets to check touch position
        Bounds spriteRendererBounds = gameObject.GetComponent<SpriteRenderer>().bounds;
        _xOffset = spriteRendererBounds.size.x * 0.5f;
        _yOffset = spriteRendererBounds.size.y * 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        // Get Touch 
        Vector3 touchPos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            touchPos.z = -2;
            
            // Select the gem 
            if (Input.GetMouseButtonDown(0))
            {
                // check if the gem is around touch pos
                Vector3 gemPos = _transform.position;

                if ((touchPos.x < gemPos.x + _xOffset && touchPos.x > gemPos.x - _yOffset) &&
                    (touchPos.y < gemPos.y + _yOffset && touchPos.y > gemPos.y - _yOffset))
                {
                    ChangeSelected(true);
                }

                _lastMousePos = touchPos;
            }
            
            if (Input.GetMouseButton(0) && (_lastMousePos != touchPos) && _isSelected)
            {
                float step = dragSpeed * Time.fixedTime;
                _transform.position = Vector3.MoveTowards(_transform.position, touchPos, step);
            }

            if (Input.GetMouseButtonUp(0))
            {
                // verify if this is a match3 move 
                
                // if not return the gem to original position and isSelect = false 
                ChangeSelected(false);
                StartCoroutine(ReturnToOriginalPos());
            }
        

    }

    private void ManageTouch()
    {
        if (Input.touchCount > 0)
        {
            // Get Touch 
            Touch touch = Input.GetTouch(0);
            Vector3 touchPos = _mainCamera.ScreenToWorldPoint(touch.position);
            touchPos.z = -2;
            
            // Select the gem 
            if (touch.phase == TouchPhase.Began)
            {
                // check if the gem is around touch pos
                Vector3 gemPos = _transform.position;

                if ((touchPos.x < gemPos.x + _xOffset && touchPos.x > gemPos.x - _yOffset) &&
                    (touchPos.y < gemPos.y + _yOffset && touchPos.y > gemPos.y - _yOffset))
                {
                    ChangeSelected(true);
                }
            }
            
            if (touch.phase == TouchPhase.Moved && _isSelected)
            {
                float step = dragSpeed * Time.fixedDeltaTime;
                _transform.position = Vector3.MoveTowards(_transform.position, touchPos, step);
            }

            if (touch.phase == TouchPhase.Ended)
            {
                // verify if this is a match3 move 
                
                // if not return the gem to original position and isSelect = false 
                ChangeSelected(false);
                StartCoroutine(ReturnToOriginalPos());
            }
        }
    }

    private void ChangeSelected(bool selected)
    {
        if (selected)
        {
            _isSelected = true;
            // change z position 
            _transform.localScale *= 1.2f;
        }
        else
        {
            _isSelected = false;
            _transform.localScale = _originalLocalScale;
        }
    }

    private IEnumerator ReturnToOriginalPos()
    {
        float step = dragSpeed * Time.fixedDeltaTime;
        while (_transform.position != _originalPos)
        {
            _transform.position = Vector3.MoveTowards(_transform.position, _originalPos, step);
            yield return new WaitForEndOfFrame();
        }
    }
}
