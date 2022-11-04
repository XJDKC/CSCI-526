using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArrowGuidance : MonoBehaviour
{
    public enum ArrowType { Normal, Player1, Player2 }

    public ArrowType arrowType = ArrowType.Normal;
    public Color normalColor = Color.white;
    public Color player1Color = new Color(92, 200, 231);
    public Color player2Color = new Color(234, 113, 189);

    private Image _image;

    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (_image != null)
        {
            switch (arrowType)
            {
                case ArrowType.Normal:
                    _image.color = normalColor;
                    break;
                case ArrowType.Player1:
                    _image.color = player1Color;
                    break;
                case ArrowType.Player2:
                    _image.color = player2Color;
                    break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
