using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

    [HideInInspector] public Slider slider;
    public Image fill;
    public Color fullColor;
    public Color lowColor;

    [SerializeField]
    private Image fillAmount;
    private int maxValue = 100;

    private void Start() {
        fillAmount = GameObject.Find("HealthOverlay").GetComponent<Image>();
    }

    public void ChangeColor() {
        fill.color = Color.Lerp(lowColor, fullColor, slider.value / 50);
    }

    public void SetMaxHealth(int health) {
        maxValue = health;
        fillAmount.fillAmount = health / maxValue;
    }

    public void SetHealth(int health) {
        fillAmount.fillAmount = health / maxValue;
    }
}
