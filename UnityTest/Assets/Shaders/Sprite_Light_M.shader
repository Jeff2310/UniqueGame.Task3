Shader "Light_M"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Mask("Texture",2D) = "white"{}
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

		SetTexture[_MainTex]{ combine texture,texture * previous }


		}
	}
}
