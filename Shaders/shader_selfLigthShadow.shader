// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/SelfLightShadow"
{
	Properties{
		_MainTex("Main (RGB)", 2D) = "white" {}
		_MatCapDiffuse("MatCapDiffuse (RGB)", 2D) = "white" {}
		_Light("Light", Float) = 1

		_GroundY("GroundY", Float) = 0
		_ShadowValue("Shadow", Range(0, 1)) = 0.8
	}

		Subshader{

			Tags{ "RenderType" = "Opaque" "Queue" = "Geometry+500" }

			//shadow pass 
			Pass
			{
				Stencil
				{
					Ref 0
					Comp equal
					Pass incrWrap
					Fail keep
					ZFail keep
				}

				//透明混合模式
				Blend SrcAlpha OneMinusSrcAlpha

				//关闭深度写入
				ZTest always
				ZWrite off

				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma multi_compile_fog

				#include "UnityCG.cginc"
				struct appdata
				{
					float4 vertex : POSITION;
					float4 color : COLOR;
				};

				struct v2f
				{
					float4 vertex : SV_POSITION;
					float2 uv : TEXCOORD0;
					UNITY_FOG_COORDS(1)
				};

				float4 _VertexShadowLightDir;
				float _GroundY;
				float _ShadowValue;

				float4 ShadowProjectPos(float4 vertPos)
				{
					float4 shadowPos;

					//得到顶点的世界空间坐标
					float3 worldPos = mul(unity_ObjectToWorld, vertPos).xyz;

					//灯光方向
					float3 lightDir = normalize(_VertexShadowLightDir.xyz);

					//阴影的世界空间坐标（低于地面的部分不做改变）
					shadowPos.y = min(worldPos.y, _VertexShadowLightDir.w);
					shadowPos.xz = worldPos.xz - lightDir.xz * max(0, worldPos.y - _VertexShadowLightDir.w) / lightDir.y;
					shadowPos.w = worldPos.y - _GroundY;

					return shadowPos;
				}


				v2f vert(appdata v)
				{
					v2f o;

					//得到阴影的世界空间坐标
					float4 shadowPos = ShadowProjectPos(v.vertex);

					//转换到裁切空间
					o.vertex = UnityWorldToClipPos(shadowPos.xyz);
					o.uv.x = shadowPos.w;
					UNITY_TRANSFER_FOG(o, o.vertex);

					return o;
				}

				fixed4 frag(v2f i) : SV_Target
				{
					clip(i.uv.x);
					fixed4 color = fixed4(0.1, 0.1, 0.1, _ShadowValue);
					UNITY_APPLY_FOG(i.fogCoord, color);
					return color;
				}
				ENDCG
			}

			// mat cap 
			Pass{
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
}
