using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public PlaceObjectOnPlaneExtended planeObs;
    public TextMeshProUGUI txtmp;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Button1Clicked()
    {
        int res = planeObs.QueryHighlighted();
        //Update text
        txtmp.text = "Count: "+res.ToString();
    }

    public void Button2Clicked()
    {
        planeObs.DeleteHighlighted();
    }

}
