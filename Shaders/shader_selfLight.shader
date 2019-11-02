// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/SelfLight"
{
	Properties{
		_MainTex("Main (RGB)", 2D) = "white" {}
		_MatCapDiffuse("MatCapDiffuse (RGB)", 2D) = "white" {}
		_Light("Light", Float) = 1
	}

		Subshader{
			Tags{ "RenderType" = "Opaque" "Queue" = "Transparent"}

			// mat cap 

			Pass{
				LOD 200
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma multi_compile_fog

				#include "UnityCG.cginc"

				struct appdata_t
				{
					half4 vertex : POSITION;
					half2 texcoord : TEXCOORD0;
					fixed3 normal : NORMAL;
				};
				struct v2f
				{
					float4 pos : SV_POSITION;
					float4	uv : TEXCOORD0;
					UNITY_FOG_COORDS(1)
				};

				uniform sampler2D _MatCapDiffuse;
				uniform sampler2D _MainTex;
				float _Light;

				v2f vert(appdata_t v)
				{
					v2f o;
					o.pos = UnityObjectToClipPos(v.vertex);
					o.uv.xy = v.texcoord;

					float3 worldNorm = normalize(mul(v.normal, (float3x3)unity_WorldToObject));
					o.uv.zw = mul((float3x3)UNITY_MATRIX_V, worldNorm).xy;

					o.uv.zw = o.uv.zw * 0.5f + 0.5f;
					UNITY_TRANSFER_FOG(o, o.pos);

					return o;
				}

				float4 frag(v2f i) : COLOR
				{
					fixed4 matcapLookup = tex2D(_MatCapDiffuse, i.uv.zw);
					fixed4 color = tex2D(_MainTex, i.uv) *_Light * matcapLookup;
					UNITY_APPLY_FOG(i.fogCoord, color);
					return color;
				}

				ENDCG
			}
		}

			FallBack "Diffuse"

}
