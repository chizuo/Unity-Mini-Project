using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public delegate void PrimitiveChange(string shape);
    public static event PrimitiveChange OnPrimitiveChange;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ButtonOnClick(string option)
    {
        OnPrimitiveChange?.Invoke(option);
    }
}
