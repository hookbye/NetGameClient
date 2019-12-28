Shader "Unlit/BlurShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_BlurRadius("_BlurRadius",Range(1,10)) = 5
		_TextureSizeX("_TextureSizeX",Float) = 256
		_TextureSizeY("_TextureSizeY",Float) = 256

	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

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
			float _TextureSizeX;
			float _TextureSizeY;
			float _BlurRadius;

			float4 _Blur(float2 uv,float blurRadius,float textureSizeX,float textureSizeY)
			{
				float4 tempColor = float4(0, 0, 0, 0);
				float pixelDisX = 1.0 / textureSizeX;
				float pixelDisY = 1.0 / textureSizeY;
				int count = blurRadius * 2 + 1;
				count *= count;
				for (int x = -blurRadius; x <= blurRadius; x++)
				{
					for (int y = -blurRadius; y <= blurRadius; y++)
					{
						float4 color = tex2D(_MainTex, uv + float2(x*pixelDisX, y*pixelDisY));
						tempColor += color;
					}
				}
				return tempColor / count;
			}
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
				fixed4 col = _Blur(i.uv,_BlurRadius,_TextureSizeX,_TextureSizeY);
				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
}
