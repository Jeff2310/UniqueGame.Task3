Shader "Light_M"
{
	Properties
	{
		_Mask ("Mask", 2D) = "black" {}
		_Lights ("Lights",2DArray) = "white"{}
	}
	SubShader
	{

		Lighting Off

		ZWrite Off

		Blend SrcAlpha OneMinusSrcAlpha

		AlphaTest GEqual[_Cutoff]

		Pass
		{

		SetTexture[_Mask]{ combine texture }

		SetTexture[_Lights]{ combine texture,texture * previous }


		}
	}
}
