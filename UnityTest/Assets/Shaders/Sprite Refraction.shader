// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Sprites/Refract" {

	Properties{
		_MainTex("Background", 2D) = "white" {}   
		_BackgroundScrollX("X Offset", float) = 0    
		_BackgroundScrollY("Y Offset", float) = 0
		_BackgroundScaleX("X Scale", float) = 1.0    
		_BackgroundScaleY("Y Scale", float) = 1.0
		_Refraction("Refraction", range(-1.0,1.0)) = 1.0   

		_DistortionMap("Distortion Map", 2D) = "" {}    
		_DistortionScrollX("X Offset", float) = 0
		_DistortionScrollY("Y Offset", float) = 0
		_DistortionScaleX("X Scale", float) = 1.0
		_DistortionScaleY("Y Scale", float) = 1.0
		_DistortionPower("Distortion Power", range (-1.0,1.0)) = 0.08

		_AnimXSpeed("Animation Speed On X Axis", range (-1.0,1.0)) = 0.1
		_AnimYSpeed("Animation Speed On Y Axis", range(-1.0,1.0)) = 0.1
	}

		SubShader{
		Tags{ "Queue" = "Geometry" "RenderType" = "Opaque" }

		Pass{

		Cull Off
		ZTest LEqual
		ZWrite On
		AlphaTest Off
		Lighting Off
		ColorMask RGBA
		Blend SrcAlpha OneMinusSrcAlpha

		CGPROGRAM
//#pragma target 4.0
#pragma fragment frag
#pragma vertex vert
#include "UnityCG.cginc"

	uniform sampler2D _MainTex;
	uniform sampler2D _DistortionMap;
	uniform float _BackgroundScrollX;
	uniform float _BackgroundScrollY;
	uniform float _DistortionScrollX;
	uniform float _DistortionScrollY;
	uniform float _DistortionPower;
	uniform float _BackgroundScaleX;
	uniform float _BackgroundScaleY;
	uniform float _DistortionScaleX;
	uniform float _DistortionScaleY;
	uniform float _Refraction;

	uniform float _AnimXSpeed;
	uniform float _AnimYSpeed;

	struct AppData {
		float4 vertex : POSITION;
		half2 texcoord : TEXCOORD0;
	};

	struct VertexToFragment {
		float4 pos : POSITION;
		half2 uv : TEXCOORD0;
	};

	VertexToFragment vert(AppData v) {
		VertexToFragment o;
		o.pos = UnityObjectToClipPos(v.vertex);
		o.uv = v.texcoord.xy;
		return o;
	}


	fixed4 frag(VertexToFragment i) : SV_Target{
		fixed xScrollValue = _AnimXSpeed * _Time.y;
		fixed yScrollValue = _AnimYSpeed * _Time.y;
		_DistortionScrollX += xScrollValue;
		_DistortionScrollY += yScrollValue;

		float2 bgOffset = float2(_BackgroundScrollX,_BackgroundScrollY);
		float2 disOffset = float2(_DistortionScrollX,_DistortionScrollY);
		float2 disScale = float2(_DistortionScaleX,_DistortionScaleY);
		float2 bgScale = float2(_BackgroundScaleX,_BackgroundScaleY);

		float4 disTex = tex2D(_DistortionMap, disScale * i.uv + disOffset);

		float2 offsetUV = (-_Refraction * (disTex * _DistortionPower - (_DistortionPower*0.5)));

		return tex2D(_MainTex, bgScale * i.uv + bgOffset + offsetUV);

		
	}

		ENDCG
	}
	}
}
