Shader "Sprites/DissolveShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Noise ("Noise",2D) = "white" {}

		_Factor("Factor",range(0,1)) = 0
		_Size("Size",range(0,10)) = 5
		_EdgeColor("Edge Color",Color) = (1,1,1,1)
		_EdgeWidth("Edge Width",float) = 0.1
		
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always
		Blend SrcAlpha OneMinusSrcAlpha
		
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			uniform sampler2D _MainTex;
			uniform sampler2D _Noise;
			//Include tiling and offset data
			//fixed4 _Noise_ST;
			//Include texture size data
			//fixed4 _Noise_TexelSize;

			uniform float _OffsetX;
			uniform float _OffsetY;

			uniform float _Factor;
			uniform float _Size;
			uniform fixed4 _EdgeColor;
			uniform float _EdgeWidth;


			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				fixed4 pure = col;

				float2 offset = float2(_OffsetX, _OffsetY);
				fixed4 noise = tex2D(_Noise,(i.uv+0.5)*_Size);
				
				float f = noise.r;


				_Factor -= _EdgeWidth;

				if (f < _Factor)
				{
					col.a = 0;
				}
				if (f >= _Factor && f < _Factor + _EdgeWidth && pure.a >0)
				{
					col = _EdgeColor;
				}
				return col;
			}
			ENDCG
		}
	}
}
