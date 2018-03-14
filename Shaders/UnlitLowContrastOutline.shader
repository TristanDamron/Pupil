//Shader for rendering high contrast outlines to account for micromovements of the head.
//Use this to make objects appear less blurry.
Shader "Pupil/Unlit/LowContrastOutline"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_OutlineWidth ("Outline Width", Range(0, 1)) = 0.02
		_Alpha ("Alpha Blur", Range(0, 1)) = 1	
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		//Outline Pass
		Pass {						
			Cull Front

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag			
			#include "UnityCG.cginc"			

			struct appdata
			{
				float4 vertex : POSITION;
				float4 uv : TEXCOORD0;
				float4 normal : NORMAL;
				float4 color : COLOR;
			};

			struct v2f
			{
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;				
			};
			
			sampler2D _MainTex;
			float4 _MainTex_ST;						
			float4 _OutlineColor;
			float _OutlineWidth;
			float _Alpha;

			v2f vert (appdata v)
			{
				v2f o;
				v.vertex.xyz += v.normal * _OutlineWidth;				
				o.vertex = UnityObjectToClipPos(v.vertex);	
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);										
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			float4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				//Get the high contrast color based on the texture
				_OutlineColor = (col/2) / _Alpha;
				return _OutlineColor;
			}			
			
			ENDCG
		}

		//Texture Pass
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
}
