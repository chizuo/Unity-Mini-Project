using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGuide : MonoBehaviour
{
    private GameObject shape;
    private bool hit;
    private RaycastHit hitInfo;
    private string guide;
    private Material material;
    private Color yellow;
    private Color green;
    private Color color;

    void OnMouseEnter()
    {
        shape = GameObject.CreatePrimitive((PrimitiveType)BaseObjects.Primitive);
        shape.name = $"{BaseObjects.Shape}_guide";
        shape.GetComponent<Collider>().enabled = false;
        material = new Material(Shader.Find("Standard"));
        yellow = new Color(1, 0.92f, 0.016f, 0.5f);
        green = new Color(0, 1, 0, 0.5f);
        material.SetFloat("_Mode", 3);
        material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        material.SetInt("_ZWrite", 0);
        material.DisableKeyword("_ALPHATEST_ON");
        material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        material.EnableKeyword("_ALPHABLEND_ON");
        material.renderQueue = 3000;
    }

    void OnMouseOver()
    {
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo))
        {
            color = hitInfo.transform.name.Equals("Base") ? yellow : green;
            material.SetColor("_Color", color);
            shape.GetComponent<MeshRenderer>().material = material;

            if (hitInfo.transform.name.Equals("Base"))
            {
                float y = BaseObjects.Shape == "Capsule" ? 1.0f : 0.5f;
                shape.transform.position = new Vector3(hitInfo.point.x, hitInfo.point.y + y, hitInfo.point.z);
            }
            else
            {
                shape.transform.position = hitInfo.transform.position + hitInfo.normal;
                shape.transform.up = hitInfo.normal;
                if (BaseObjects.Shape == "Capsule")
                {
                    if (hitInfo.transform.tag.Equals("Capsule") && hitInfo.normal.y != 0.0f)
                        shape.transform.position += hitInfo.normal * 0.75f;
                    else
                        shape.transform.position += hitInfo.normal * 0.5f;
                }
            }
        }
    }

    void OnMouseExit()
    {
        Destroy(this.shape);
    }
}