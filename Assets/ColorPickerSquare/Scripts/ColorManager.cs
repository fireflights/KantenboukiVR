﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorManager : MonoBehaviour {

    public Color color = Color.red;
	
	// Update is called once per frame
	void OnColorChange (HSBColor color) {
        this.color = color.ToColor();
	}
}
