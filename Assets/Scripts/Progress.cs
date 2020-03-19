using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Progress : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image image;
    public float fillValue = 0;
    public TextMeshProUGUI progressText;
    public float currentPercentage = 0;
    // Start is called before the first frame update
    void Start()
    {
        
        fillValue = PlayerPrefs.GetInt("CurrentLevel",0);
        if (fillValue < 10)
        {
            progressText.text = "00" + fillValue.ToString();
        }
        else if (fillValue > 10 && fillValue < 100)
        {
            progressText.text = "0" + fillValue.ToString();
        }
        else
        {
            progressText.text = fillValue.ToString();
        }
        slider.value = fillValue;
        currentPercentage = (fillValue * 100) / (151 * 100);
        image.color = gradient.Evaluate(currentPercentage);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
