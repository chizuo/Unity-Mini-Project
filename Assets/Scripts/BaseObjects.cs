using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseObjects : MonoBehaviour
{
    // used for GameObject.CreatePrimitive(PrimitiveType primitiveValue)
    private static int primitiveValue = 3;
    public static int Primitive
    {
        get { return primitiveValue; }
    }

    // counters for GameObject counting for GameObject's name
    private int capsuleCount = 0;
    private int cubeCount = 0;
    private int sphereCount = 0;
    private int shapeCounter = 0;

    // Material references for GameObject skins
    public Material stone;
    public Material block;
    public Material metal;
    private Material material;

    private static string type;
    public static string Shape
    {
        get { return type; }
    }

    public int direction = 1;

    private void OnEnable()
    {
        UIManager.OnPrimitiveChange += OnChange;
    }

    private void OnDisable()
    {
        UIManager.OnPrimitiveChange -= OnChange;
    }

    private void OnChange(string option)
    {
        switch (option)
        {
            case "sphere":
                primitiveValue = 0;
                type = "Sphere";
                break;
            case "capsule":
                primitiveValue = 1;
                type = "Capsule";
                break;
            case "cube":
                primitiveValue = 3;
                type = "Cube";
                break;
            case "stone":
                material = stone;
                break;
            case "block":
                material = block;
                break;
            case "metal":
                material = metal;
                break;
        }
    }

    void Start()
    {
        material = stone;
        type = "Cube";
    }

    // Update is called once per frame
    /*
        This is the main loop that repeats until program exits
    */
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            direction *= -1;
        }
        if (Input.GetKey(KeyCode.X))
        {
            transform.Translate(new Vector3(direction * Time.deltaTime, 0, 0));
        }
        if (Input.GetKey(KeyCode.Y))
        {
            transform.Translate(new Vector3(0, direction * Time.deltaTime, 0));
        }
        if (Input.GetKey(KeyCode.Z))
        {
            transform.Translate(new Vector3(0, 0, direction * Time.deltaTime));
        }
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hitInfo;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo))
            {
                switch (primitiveValue)
                {
                    case 0:
                        type = "Sphere";
                        shapeCounter = ++sphereCount;
                        break;
                    case 1:
                        type = "Capsule";
                        shapeCounter = ++capsuleCount;
                        break;
                    default:
                        type = "Cube";
                        shapeCounter = ++cubeCount;
                        break;
                }

                var shape = GameObject.CreatePrimitive((PrimitiveType)primitiveValue);
                shape.name = string.Format($"My{type}{shapeCounter.ToString()}");
                shape.GetComponent<Renderer>().material = material;
                shape.tag = type;
                shape.AddComponent<ObjectGuide>();
                shape.AddComponent<TriangleExplosion>();

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
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hitInfo;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo))
            {
                GameObject gameObject = hitInfo.transform.gameObject;
                if (gameObject.name != "Base")
                {
                    StartCoroutine(gameObject.GetComponent<TriangleExplosion>().SplitMesh(true));
                    Destroy(GameObject.Find($"{type}_guide"));
                }
            }
        }
    }
}
