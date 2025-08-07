using System;
using System.Collections.Generic;
using Lean.Touch;
using UnityEngine;
using UnityEngine.Serialization;
using DG.Tweening;

public class HoleMovement : MonoBehaviour
{
    [Header("Hole mesh")]
    [SerializeField] MeshFilter meshFilter;
    [SerializeField] MeshCollider meshCollider;
    [FormerlySerializedAs("_scaleFactor")] [SerializeField] private float scaleFactor = 5f;
    
    [Header("Hole vertices radius")]
    [SerializeField] Vector2 moveLimits;
    [SerializeField] public float radius;
    [SerializeField] private Transform holeCenter;
    [SerializeField] private List<float> levelRadii; 
    
    [SerializeField] private PinePie.SimpleJoystick.JoystickController joystick;

    [Space]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float sensitivity = 0.0001f; // hassasiyet ayarı (1 = normal, <1 = yavaş, >1 = hızlı)
    [SerializeField] private Transform circle;

    

    
    Mesh _mesh;
    List<int> _holeVertices;
    List<Vector3> _offsets;
    
    int _holeVerticesCount;

    Vector3 _touch, _targetPos;
    private float _initialRadius;
    private int _currentLevel;
    
    
    void Start()
    {
        _initialRadius = radius;

        
        Game.İsGameOver = false;
       
        _holeVertices = new List<int>();
        _offsets = new List<Vector3>();
        _mesh = meshFilter.mesh;

        FindHoleVertices();

        
        
        meshFilter.transform.localScale = new Vector3(meshFilter.transform.localScale.x  * scaleFactor, meshFilter.transform.localScale.y, meshFilter.transform.localScale.z * scaleFactor);
        
        UpdateHoleVerticesPosition();
        Camera.main.transform.DOMove(new Vector3(0, 7, -4.75f), 2f);


    }
    void Update()
    {
        if (Game.İsGameOver) return;

        if (joystick == null) return;

        Vector2 inputDir = joystick.InputDirection;

        if (inputDir != Vector2.zero)
        {
            float moveX = inputDir.x * moveSpeed * Time.deltaTime * sensitivity;
            float moveY = inputDir.y * moveSpeed * Time.deltaTime * sensitivity;

            MoveHole(moveX, moveY);
            UpdateHoleVerticesPosition();
        }
    }

    private void OnEnable()
    {
        //LeanTouch.OnFingerUpdate += LeanTouchOnOnFingerUpdate;
        
    }

    private void LeanTouchOnOnFingerUpdate(LeanFinger obj)
    {

        var delta = obj.ScaledDelta;
        

        if (!Game.İsGameOver)
        {
            MoveHole(delta.x, delta.y);

            UpdateHoleVerticesPosition();
        }
    }
    
    private void OnDisable()
    {
        //LeanTouch.OnFingerUpdate -= LeanTouchOnOnFingerUpdate;
    }
    
    void MoveHole(float x, float z)
    {
        
        Vector3 input = new Vector3(x, 0f, z); 

        _touch = holeCenter.position + input;

        _targetPos = new Vector3(
            Mathf.Clamp(_touch.x, -moveLimits.x, moveLimits.x),
            holeCenter.position.y,
            Mathf.Clamp(_touch.z, -moveLimits.y, moveLimits.y)
        );

        holeCenter.position = _targetPos;
    }
    

    private void UpdateHoleVerticesPosition()
    {
        Vector3[] vertices = _mesh.vertices;
        
        float scaleFactor = radius / _initialRadius;

        for (int i = 0; i < _holeVerticesCount; i++)
        {
            Vector3 scaledOffset = _offsets[i] * scaleFactor;
            var localPoint = meshFilter.transform.InverseTransformPoint(holeCenter.position + scaledOffset);
            vertices[_holeVertices[i]] = localPoint;
        }

        _mesh.vertices = vertices;
        meshFilter.mesh = _mesh;
        meshCollider.sharedMesh = _mesh;
    }

     private void FindHoleVertices()
    {
        _holeVertices.Clear();
        _offsets.Clear();
        var list = new List<Vector3>();

        foreach (var localVertex in _mesh.vertices)
        {
            Vector3 worldVertex = meshFilter.transform.TransformPoint(localVertex);
            

            list.Add(worldVertex);
        }

        for (int i = 0; i < list.Count; i++)
        {
            float distance = Vector3.Distance(holeCenter.position, list[i]);

            if (distance < radius)
            {
                _holeVertices.Add(i);
                _offsets.Add(list[i] - holeCenter.position);
            }
        }
        _holeVerticesCount = _holeVertices.Count;
    }
    
    
    
    public void GrowHole(float amount)
    {
        radius += amount;
        FindHoleVertices();
    }
    public void SetHoleLevel(int level)
    {
        if (level < 0 || level >= levelRadii.Count) return;
        if (level != _currentLevel)
        {
            _currentLevel = level;

            float newRadius = levelRadii[level];
            
            for (var i = 0; i < _offsets.Count; i++)
            {
                var offset = _offsets[i];
                var newOffset = offset * newRadius;
                _offsets[i] = new Vector3(newOffset.x, offset.y, newOffset.z);
            }               
            circle.transform.localScale *=newRadius ; 
            

        }

        UpdateHoleVerticesPosition();
    }


}


