using UnityEngine;

public class DoodleBarScript : MonoBehaviour
{
    RectTransform barRectTr;
    void Start()
    {
        barRectTr = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Input.mousePosition;
        //Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y,0));
    }
}
