using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

    [HideInInspector] public Slider slider;
    public Image fill;
    public Color fullColor;
    public Color lowColor;

    private void Start() {
        slider = GetComponent<Slider>();
    }

    public void SetMaxHealth(int health) {
        slider.maxValue = health;
        slider.value = health;
    }

    public void SetHealth(int health) {
        slider.value = health;
    }

    public void ChangeColor() {
        fill.color = Color.Lerp(lowColor, fullColor, slider.value / 50);
    }
}
