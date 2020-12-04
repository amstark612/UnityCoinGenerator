Shader "Unlit/QUnlitLightmap" 
{
	Properties 
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_LightMap("Lightmap", 2D) = "white" {}
	}
	SubShader 
	{
		Tags {"Queue" = "Geometry" "RenderType" = "Opaque"}

		Pass 
		{
			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma multi_compile_fwdbase
				#pragma fragmentoption ARB_fog_exp2
				#pragma fragmentoption ARB_precision_hint_fastest
				
				#include "UnityCG.cginc"
				
				struct v2f
				{
					float4	pos			: SV_POSITION;
					float2	uv			: TEXCOORD0;
					float2	uv2			: TEXCOORD1;
				};

				float4 _MainTex_ST;
				float4 _LightMap_ST;

				v2f vert (appdata_full v)
				{
					v2f o;
					
					o.pos = UnityObjectToClipPos( v.vertex);
					o.uv = TRANSFORM_TEX (v.texcoord, _MainTex).xy;
					o.uv2 = TRANSFORM_TEX (v.texcoord1, _LightMap).xy;
					return o;
				}

				sampler2D _MainTex;
				sampler2D _LightMap;

				fixed4 frag(v2f i) : COLOR
				{
					return tex2D(_MainTex, i.uv) * tex2D(_LightMap, i.uv2);
				}
			ENDCG		
		}
	}
	FallBack "VertexLit"
}