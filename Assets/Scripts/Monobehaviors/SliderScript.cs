using System;
using UnityEngine;
using UnityEngine.UI;

public class SliderScript : MonoBehaviour
{
    private Slider slider;
    public event Action OnTimerEnd;
    bool isReadyFillingUp = true;
    void Start()
    {
        slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Boool " + isReadyFillingUp);
        if (isReadyFillingUp) 
        {
            Debug.Log("Yeah");
            slider.value = Mathf.MoveTowards(slider.value, slider.maxValue, .1f * Time.unscaledDeltaTime);
            if (slider.value > slider.maxValue - .01f)
            {
                Debug.Log("Slideerrrr");
                isReadyFillingUp = false;
                slider.value = 0f;
                OnTimerEnd?.Invoke();
            }
        }

    }
    public void ResetSlider()
    {
        Debug.Log("Called ResetSlider");
        if(slider != null) 
        {
            Debug.Log("HIIIII");
            slider.value = 0f;
            isReadyFillingUp = true;
            Debug.Log(isReadyFillingUp);
        }
    }
    //private void OnDisable()
    //{
    //    Debug.Log("Disabled");
    //    slider.value = 0f;
    //    isReadyFillingUp = true;
    //}
}
