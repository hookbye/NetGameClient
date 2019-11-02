// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/CitySea"
{
	Properties
	{
		_WaterNormalTex ("Normal Map", 2D) = "white" {}
		_NormalSize ("Normal Size", Range(0.1, 100)) = 1
		_HighLightTex ("High Light Texture", 2D) = "white" {}
		_HighLightSize ("High Light Size", Range(0.1, 100)) = 20
		_Speed ("Speed", Range(0, 0.1)) = 0.05
		_OffsetFactor ("Offset Factor", Range(0, 0.2)) = 0.01
		_RefractionFactor ("Refraction Factor", Range(0, 0.2)) = 1
		_TintColor0 ("Tint Color 0", Color) = (0, 0, 0, 0)
		_TintColor1 ("Tint Color 1", Color) = (0, 0, 0, 0)
		_Shineness ("Shineness", Range(-300, 300)) = 0.5
		_Specular ("Specular", Range(0, 9000)) = 1
	}


	SubShader
	{
		LOD 500
		Tags { "RenderType"="Opaque" "Queue"="Transparent-10" }

		GrabPass { }

		Pass
		{
			Fog { Mode Off}
			ZWrite Off
			Blend Off

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest

			#define WATER_REFREACT
			#define WATER_SPECULAR

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
				float4 proj1 : TEXCOORD1;
				float2 infoUV : TEXCOORD2;
			};

			sampler2D _WaterNormalTex;
			half4 _WaterNormalTex_ST;

			sampler2D _HighLightTex;
			half4 _HighLightTex_ST;

			sampler2D _GrabTexture : register(s0);
			half4 _GrabTexture_ST;

			float _Speed;
			float _RefractionFactor;
			float _OffsetFactor;
			float _HighLightSize;
			float _NormalSize;
			fixed4 _TintColor0;
			fixed4 _TintColor1;
			float _Shineness;
			float _Specular;

			float3 waveNormal(float2 uv)
			{
				float3 normal = tex2D(_WaterNormalTex, uv).xyz;
				normal = normal * 2.0 - 1.0;
				return normal;
			}


			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _WaterNormalTex);
				o.proj1 = ComputeScreenPos(o.vertex);
				o.infoUV = TRANSFORM_TEX(v.uv, _HighLightTex);

#if UNITY_UV_STARTS_AT_TOP
				o.proj1.y = (o.vertex.w - o.vertex.y) * 0.5;
#endif
				return o;
			}

			
			fixed4 frag(v2f i) : SV_Target
			{
				float2 uv = i.proj1.xy / i.proj1.w;
#if defined(WATER_REFREACT) || defined(WATER_SPECULAR)
				float normalOffset = -_Time.y * _Speed;
				float3 normalVal = normalize(  waveNormal(i.uv + float2(normalOffset, 0))
			    	                         + waveNormal(float2(i.uv.x - normalOffset + 0.3, i.uv.y - normalOffset)));
#endif

#ifdef WATER_REFREACT
				float3 inVec = normalize(float3(uv, 0));
				float3 refractVec = refract(inVec, normalVal, _RefractionFactor);
				uv += refractVec.xy * _OffsetFactor;
#endif

#ifdef WATER_SPECULAR
				float3 lightdir = float3(-0.7239723275323889, 0.6828958834170762, 0.09755655477386801);
				float factorCandidate = reflect(lightdir, -normalVal).y;
				float reflectiveFactor = max(0.0, factorCandidate);
				float specularVal = pow(reflectiveFactor, _Shineness) * _Specular - 0.08;
#endif

				fixed2 seaLerps = tex2D(_HighLightTex, i.infoUV).rg;
				fixed4 waterColor = lerp(_TintColor1, _TintColor0, seaLerps.g);
				fixed4 col = lerp(waterColor, tex2D(_GrabTexture, uv), seaLerps.r);

#ifdef WATER_SPECULAR
				col	+= fixed4(specularVal, specularVal, specularVal, 0);
#endif
				return col;
			}
			ENDCG
		}
	}


	SubShader
	{
		LOD 300
		Tags { "RenderType"="Transparent" "Queue"="Transparent" }

		Pass
		{
			Fog { Mode Off}
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma alpha
			#define WATER_SPECULAR

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
				float4 proj1 : TEXCOORD1;
				float2 infoUV : TEXCOORD2;
			};

			sampler2D _WaterNormalTex;
			half4 _WaterNormalTex_ST;

			sampler2D _HighLightTex;
			half4 _HighLightTex_ST;

			float _Speed;
			float _RefractionFactor;
			float _OffsetFactor;
			float _HighLightSize;
			float _NormalSize;
			fixed4 _TintColor0;
			fixed4 _TintColor1;
			float _Shineness;
			float _Specular;

			float3 waveNormal(float2 uv)
			{
				float3 normal = tex2D(_WaterNormalTex, uv).xyz;
				normal = normal * 2.0 - 1.0;
				return normal;
			}


			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _WaterNormalTex);
				o.proj1 = ComputeScreenPos(o.vertex);
				o.infoUV = TRANSFORM_TEX(v.uv, _HighLightTex);

#if UNITY_UV_STARTS_AT_TOP
				o.proj1.y = (o.vertex.w - o.vertex.y) * 0.5;
#endif
				return o;
			}

			
			fixed4 frag(v2f i) : SV_Target
			{
				float2 uv = i.proj1.xy / i.proj1.w;
#ifdef WATER_SPECULAR
				float normalOffset = -_Time.y * _Speed;
				float3 normalVal = normalize(  waveNormal(i.uv + float2(normalOffset, 0))
			    	                         + waveNormal(float2(i.uv.x - normalOffset + 0.3, i.uv.y - normalOffset)));
#endif


#ifdef WATER_SPECULAR
				float3 lightdir = float3(-0.7239723275323889, 0.6828958834170762, 0.09755655477386801);
				float factorCandidate = 0.6828958834170762 - 2.0 * dot(lightdir, -normalVal) * -normalVal.y;
				float reflectiveFactor = max(0.0, factorCandidate);
				float specularVal = pow(reflectiveFactor, _Shineness) * _Specular - 0.08;
#endif

				fixed2 seaLerps = tex2D(_HighLightTex, i.infoUV).rg;
				fixed4 waterColor = lerp(_TintColor1, _TintColor0, seaLerps.g);
				fixed4 col = waterColor;
				col.a = 1.0 - seaLerps.r;

#ifdef WATER_SPECULAR
				col	+= fixed4(specularVal, specularVal, specularVal, 0);
#endif
				return col;
			}
			ENDCG
		}
	}
}
