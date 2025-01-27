﻿Shader "Simple Vertex Lit" {
	Properties{
		_MainTex("Base (RGB)", 2D) = "white" {}
	}

	SubShader {
		Pass {
			Lighting On
			ColorMaterial AmbientAndDiffuse
		AlphaToMask On
			SetTexture[_MainTex] {
				combine texture * primary DOUBLE
			}
		}
	}
}