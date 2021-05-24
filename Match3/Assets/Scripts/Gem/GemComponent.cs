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
    
    [Header("Gems used on menu screen are dummy")]
    public bool isDummyGem;
    
    // Variables used in boardHolder
    [HideInInspector] public Vector3 originalPos;
    [HideInInspector] public float dragSpeedDeafaultValue;
    [HideInInspector] public SpriteRenderer sp;
    [HideInInspector] public CircleCollider2D CircleCollider;
    // private variables 
    private Transform _transform;
    [HideInInspector] public Vector3 _originalLocalScale;
    private bool _isSelected;
    private float _xOffset;
    private float _yOffset;
    
    // knows the movement of the mouse
    private Vector3 _lastMousePos;
    
    // Start is called before the first frame update
    private void Start()
    {
        //load component values 
        _mainCamera = Camera.main;
        _isSelected = false;
        _transform = transform;
        originalPos = _transform.position;
        _originalLocalScale = _transform.localScale;
        sp = gameObject.GetComponent<SpriteRenderer>();
        CircleCollider = gameObject.GetComponent<CircleCollider2D>();
        dragSpeedDeafaultValue = dragSpeed;
        
        // offsets to check touch position
        Bounds spriteRendererBounds = gameObject.GetComponent<SpriteRenderer>().bounds;
        _xOffset = spriteRendererBounds.size.x * 0.5f;
        _yOffset = spriteRendererBounds.size.y * 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        // cant move the gems when game is paused 
        if(BoardHolder.paused || isDummyGem)
            return;
        
        // Verify if the player is using desktop or mobile 
        if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android)
        {
            ManageTouch();
        }
        else
        {
            ManageMouse();
        }

    }

    private void ManageMouse()
    {
        // Get Touch 
        Vector3 mousePos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = -2;
            
        // Select the gem 
        if (Input.GetMouseButtonDown(0))
        {
            // check if the gem is around touch pos
            Vector3 gemPos = _transform.position;

            if ((mousePos.x < gemPos.x + _xOffset && mousePos.x > gemPos.x - _yOffset) &&
                (mousePos.y < gemPos.y + _yOffset && mousePos.y > gemPos.y - _yOffset))
            {
                ChangeSelected(true);
            }

            _lastMousePos = mousePos;
        }
            
        if (Input.GetMouseButton(0) && (_lastMousePos != mousePos) && _isSelected)
        {
            float step = dragSpeed * Time.fixedTime;
            _transform.position = Vector3.MoveTowards(_transform.position, mousePos, step);
        }

        if (Input.GetMouseButtonUp(0))
        {
            // verify if this is a match3 move 
                
            // if not return the gem to original position and isSelect = false 
            ChangeSelected(false);
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
            }
        }
    }

    // change gem state 
    public void ChangeSelected(bool selected)
    {
        if (selected)
        {
            _isSelected = true;
            // Debug.Log("Point");
            // Debug.Log(positionInBoard.row);
            // Debug.Log(positionInBoard.col);
            _transform.localScale *= 1.2f;
        }
        else
        {
            _isSelected = false;
            _transform.localScale = _originalLocalScale;
            StartCoroutine(ReturnToOriginalPos());
        }
    }

    // CHECK CONTACT WITH OTHER GEMS 
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_isSelected)
        {
            GemComponent otherGemComponent = other.gameObject.GetComponent<GemComponent>();
            BoardHolder.CheckMatch3(this,otherGemComponent);
        }

    }

    // Return to original pos when dragged of or selected is false
    public IEnumerator ReturnToOriginalPos()
    {
        float step = dragSpeed * Time.fixedDeltaTime;
        while (_transform.position != originalPos)
        {
            _transform.position = Vector3.MoveTowards(_transform.position, originalPos, step);
            yield return new WaitForEndOfFrame();
        }
    }
}
