// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Lightmapped/Diffuse" {
Properties {
	_Color ("Main Color", Color) = (1,1,1,1)
	_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
	_LightMap ("Lightmap (RGB)", 2D) = "black" {}
	_KPro("Pro", float) = 1.2
	_Average("Average", float) = 0.4
}

SubShader {
	
	Tags
	{
		"RenderType" = "Opaque"
		"Queue" = "Geometry"
	}

	Blend SrcAlpha OneMinusSrcAlpha

	Pass {
		CGPROGRAM
		
		#pragma vertex vert
		#pragma fragment frag
		#pragma fragmentoption ARB_precision_hint_fastest
		#pragma multi_compile_fog

		#include "UnityCG.cginc"

		sampler2D _MainTex;
		half4 _MainTex_ST;
		sampler2D _LightMap;
		half4 _LightMap_ST;

		half4 _Color;
		half _KPro;
		half _Average;

		struct Input {
			half4 pos : SV_POSITION;
			half2 uv_MainTex : TEXCOORD0;
			half2 uv2_LightMap : TEXCOORD1;
			UNITY_FOG_COORDS(2)
		};

		Input vert ( appdata_full v )
		{
			Input o;
			o.pos = UnityObjectToClipPos (v.vertex);
			o.uv_MainTex = TRANSFORM_TEX(v.texcoord, _MainTex);
			o.uv2_LightMap = TRANSFORM_TEX(v.texcoord1, _LightMap);

			UNITY_TRANSFER_FOG(o, o.pos);
			return o; 
		}	
			
		half4 frag ( Input IN ) : COLOR
		{
			half4 diffuse = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			half4 color = diffuse * (tex2D(_LightMap, IN.uv2_LightMap) * _KPro + _Average);

			UNITY_APPLY_FOG(IN.fogCoord, color);
			return color;
		}		

		ENDCG
	}
}
}