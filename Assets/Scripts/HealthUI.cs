using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private Image currentDisplayedImage;

    [SerializeField] private Sprite fullHealth;
    [SerializeField] private Sprite ThirdHealth;
    [SerializeField] private Sprite SecondHealth;
    [SerializeField] private Sprite FirstHealth;
    [SerializeField] private Sprite NoHealth;

    [SerializeField] private GameObject playerHealth;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch(playerHealth.GetComponent<Health>().currHP)
        {
            case 5:
                currentDisplayedImage.sprite = fullHealth;
                break;
            case 4:
                currentDisplayedImage.sprite = ThirdHealth;
                break;
            case 3:
                currentDisplayedImage.sprite = SecondHealth;
                break;
            case 2:
                currentDisplayedImage.sprite = FirstHealth;
                break;
            case 1:
                currentDisplayedImage.sprite = NoHealth;
                break;
        }
    }
}
