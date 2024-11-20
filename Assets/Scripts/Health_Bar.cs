using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health_Bar : MonoBehaviour
{
    public Slider slider;      // Thanh trượt
    public Gradient gradient;  // Gradient dùng cho Player
    public Image fill;         // Hình ảnh fill của thanh máu

    private bool isPlayer;     // Xác định là Player hay Monster

    // Hàm thiết lập giá trị tối đa cho thanh máu
    public void InitializeHealthBar(int maxHealth, bool isPlayer)
    {
        this.isPlayer = isPlayer; // Gán giá trị cờ
        slider.maxValue = maxHealth; // Đặt giá trị tối đa
        slider.value = maxHealth;    // Đặt giá trị hiện tại

        if (isPlayer)
        {
            fill.color = gradient.Evaluate(1f); // Gradient xanh cho Player
        }
        else
        {
            fill.color = Color.red; // Màu đỏ cố định cho Monster
        }
    }

    // Hàm cập nhật giá trị máu
    public void SetHealth(int currentHealth)
    {
        slider.value = currentHealth; // Cập nhật giá trị thanh trượt

        if (isPlayer)
        {
            fill.color = gradient.Evaluate(slider.normalizedValue); // Gradient xanh theo máu
        }
        else
        {
            fill.color = Color.red; // Màu đỏ cố định cho Monster
        }
    }
}
