using System;
using UnityEngine;
using UnityEngine.UI;

public class SliderScript : MonoBehaviour
{
    private Slider slider;
    public event Action onTimerEnd;
    void Start()
    {
        slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = Mathf.MoveTowards(slider.value, slider.maxValue, .1f * Time.deltaTime);
        if (slider.value > slider.maxValue - .01f) 
        {
            onTimerEnd?.Invoke();
        } 
    }
    private void OnDisable()
    {
        slider.value = 0f;
    }
}
