using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
public enum BackgroundType { Blue,Brown,Gray,Green,Pink,Yellow }
public class AnimatedBackground : MonoBehaviour
{
    [SerializeField] private Vector2 movementDir;
    private MeshRenderer meshRenderer;

    [Header("Color")]
    [SerializeField] private BackgroundType bgType ;
    [SerializeField] private Texture2D[] texture; 
    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        UpdateBackgroundTetxure();
    }
    private void Update()
    {
        meshRenderer.material.mainTextureOffset += movementDir * Time.deltaTime;
       
    }

    [ContextMenu("Change Background Color")]
    private void UpdateBackgroundTetxure()
    {
        if(meshRenderer == null) meshRenderer =GetComponent<MeshRenderer>();    
        meshRenderer.material.mainTexture = texture[(int)bgType];
    }
}
