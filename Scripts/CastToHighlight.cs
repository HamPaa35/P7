using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CastToHighlight : MonoBehaviour
{

    public static string selectedObject;
    public string internalObject;

    [SerializeField]
    MeshRenderer meshRenderer;
    private Material[] oldMaterials;
    private Material[] materials;
    
    public Material highlightMaterial;
    
    public static CastToHighlight instance;

    private void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void HighlightObject(GameObject gameObject)
    {
        selectedObject = gameObject.name;
        internalObject = gameObject.name;
        
        meshRenderer = gameObject.GetComponent<MeshRenderer>();
        oldMaterials = meshRenderer.materials;
        materials = new Material[oldMaterials.Length + 1];

        for (int i = 0; i < oldMaterials.Length; i++)
        {
            materials[i] = oldMaterials[i];
        }
        
        materials[materials.Length - 1] = highlightMaterial;
        meshRenderer.materials = materials;
    }

    public void DeHighlightObject()
    {
        selectedObject = "";
        internalObject = "";
        
        meshRenderer.materials = oldMaterials;
        meshRenderer = null;
        oldMaterials = null;
        materials = null;
    }
}
