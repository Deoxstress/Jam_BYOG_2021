using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class OutlineBeat : MonoBehaviour
{
    public List<Material> outlineFill;
    public Renderer[] renderers;
    public float outlineFillValue;
    // Start is called before the first frame update
    void Awake()
    {
        
    }
    void Start()
    {
        renderers = gameObject.GetComponentsInChildren<Renderer>();
        foreach(Renderer renderer in renderers)
        {
            outlineFill.Add(renderer.materials[2]);
        }
        //outlineFill.Add(gameObject.GetComponent<Renderer>().materials[2]);

        //ChangeFillColor(new Color(1, 0.5293866f, 0f, 1)); //Orange
        ChangeFillColor(new Color(0.6f, 0.6f, 0.6f, 1)); //Grey
        outlineFillValue = 0f;
    }

    // Update is called once per frame
    void Update()
    {

        /*if (Input.GetKeyDown(KeyCode.R))
            outlineFill = gameObject.GetComponent<Renderer>().materials[2]; */          
        if (outlineFill != null)
            foreach (Material outlines in outlineFill)
            {
                if(outlines != null)
                    outlines.SetFloat("_OutlineWidth", outlineFillValue);       
            }
    }

    public void ChangeFillColor(Color color)
    {
        foreach (Material outlines in outlineFill)
        {
            if (outlines != null)
                outlines.SetColor("_OutlineColor", color);
        }
    }
}
