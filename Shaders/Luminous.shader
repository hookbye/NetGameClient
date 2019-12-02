Shader "Custom/Luminous"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
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
			#include "Lighting.cginc"

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

			bool isBlack(fixed3 col)
			{
				return col.x < 0.01 && col.y < 0.01 && col.z < 0.01;
			}

			bool isGray(fixed3 col)
			{
				if(abs(col.x - col.y) < 0.01 && abs(col.x - col.z)< 0.01)
					return true;
				return false;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
				// col = fixed4(0,0,0,1);
				// fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz;
				fixed4 tarCol = fixed4(_LightColor0.rgb+col,1);
				// if (_LightColor0.r > 0 || _LightColor0.g > 0 || _LightColor0.b > 0)
				// {
				// 	tarCol = fixed4(_LightColor0.rgb,1);
				// }
				if(isBlack(_LightColor0))
				{
					if(isGray(col))
					{
						tarCol = fixed4(0,0,0,0.5);
					}else
					{
						tarCol = fixed4(0.9,0.9,0.9,0.5);
					}
				}
				else
				{
					// tarCol = col;
				}

				
				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, tarCol);
				return tarCol;
			}
			ENDCG
		}
	}
}
