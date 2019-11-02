// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// - Unlit
// - Per-vertex (virtual) camera space specular light

Shader "Custom/WindUnlit"{
	Properties{
		_MainTex("Base (RGB) Gloss (A)", 2D) = "white" {}
		_Wind("Wind params",Vector) = (-0.5, 0.6,-0.5,1)
		_WindEdgeFlutter("Wind edge fultter factor", float) = 4
		_WindEdgeFlutterFreqScale("Wind edge fultter freq scale",float) = 0.6
		_uvScale("UV Scale", float) = 0.7
	}

	SubShader{
		Tags {"Queue" = "Transparent" "RenderType" = "Transparent" "DisableBatching" = "True"}
		LOD 100
		Blend SrcAlpha OneMinusSrcAlpha
		Cull Off
		ZWrite Off
		ZTest Off

		Pass{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog

			#include "UnityCG.cginc"
			#include "TerrainEngine.cginc"
			sampler2D _MainTex;
			half4 _MainTex_ST;

			float _WindEdgeFlutter;
			float _WindEdgeFlutterFreqScale;
			float _uvScale;

			struct appdata {				float4 vertex : POSITION;				half2 texcoord : TEXCOORD0;				half2 texcoord1 : TEXCOORD1;				fixed4 color : COLOR0;			};

			struct v2f {
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				fixed4 color : COLOR0;
			};

			inline float4 AnimateVertex2(float4 pos, float4 animParams, float4 wind, float2 time)
			{
				// animParams stored in color
				// animParams.x = branch phase
				// animParams.y = edge flutter factor
				// animParams.z = primary factor
				// animParams.w = secondary factor

				float fDetailAmp = 0.1f;
				float fBranchAmp = 0.3f;

				// Phases (object, vertex, branch)
				float fObjPhase = dot(unity_ObjectToWorld[3].xyz, 1);
				float fBranchPhase = fObjPhase + animParams.x;

				float fVtxPhase = dot(pos.xyz, animParams.y + fBranchPhase);

				// x is used for edges; y is used for branches
				float2 vWavesIn = time + float2(fVtxPhase, fBranchPhase);

				// 1.975, 0.793, 0.375, 0.193 are good frequencies
				float4 vWaves = (frac(vWavesIn.xxyy * float4(1.975, 0.793, 0.375, 0.193)) * 2.0 - 1.0);
				vWaves = SmoothTriangleWave(vWaves);
				float2 vWavesSum = vWaves.xz + vWaves.yw;

				// Edge (xz) and branch bending (y)
				float3 bend = animParams.y * fDetailAmp * 0.4;// normal.xyz; //
				bend.y = animParams.w * fBranchAmp;
				pos.xyz += ((vWavesSum.xyx * bend) + (wind.xyz * vWavesSum.y * animParams.w)) * wind.w;

				// Primary bending
				// Displace position
				pos.xyz += animParams.z * wind.xyz;

				return pos;
			}

			v2f vert(appdata v)
			{
				float bendingFact = (0.9 - v.texcoord1.y);
				if (bendingFact < 0)
				{
					bendingFact = 0;
				}

				bendingFact *= _uvScale;

				float4	wind;
				wind.xyz = mul((float3x3)unity_WorldToObject, _Wind.xyz);
				wind.w = _Wind.w  * bendingFact;

				float4	windParams = float4(0, _WindEdgeFlutter, bendingFact.xx);
				float2 	windTime = _Time.y * float2(_WindEdgeFlutterFreqScale, 1);
				float4	mdlPos = AnimateVertex2(v.vertex, windParams, wind, windTime);

				v2f o;
				o.pos = UnityObjectToClipPos(mdlPos);
				o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
				o.color = v.color;

				UNITY_TRANSFER_FOG(o, o.pos);

				return o;
			}

			fixed4 frag(v2f i) : COLOR
			{
				fixed4 color = tex2D(_MainTex, i.uv) * i.color;
				UNITY_APPLY_FOG(i.fogCoord, color);
				return color;
			}

			ENDCG
		}
	}
}


