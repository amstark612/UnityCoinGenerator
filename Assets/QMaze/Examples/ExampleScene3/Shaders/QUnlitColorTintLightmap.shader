Shader "Unlit/QUnlitColorTintLightmap" 
{
	Properties 
	{
		_TopColor 	("Top Color"   , Color) = (1,1,1,1)
		_BottomColor("Bottom Color", Color) = (0,0,0,1)
		_LeftColor	("Left Color"  , Color) = (1,0,0,1)
		_RightColor	("Right Color" , Color) = (0,1,0,1)
		_Size 		("Size" 	   , Vector)= (0,0,1,1)
		_LightMap	("Lightmap"	   , 2D   ) = "white" {}
		_TopDif		("Top Dif"	   , Range(1,10)) = 1
		_SideDist	("Side Dist"   , Range(0,1)) = 0
	}
	SubShader 
	{
		Tags {"Queue" = "Geometry" "RenderType" = "Opaque"}

		Pass 
		{
			Blend SrcColor Zero 
			
			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag	
				#include "UnityCG.cginc"							
				
				struct appdata 
		        {	
		            float4 vertex 	: POSITION;	
		            fixed3 normal	: NORMAL;
		            float2 uv2		: TEXCOORD1;
		        };
		        
				struct v2f
				{
					float4 pos		: SV_POSITION;
					float2 uv2		: TEXCOORD0;
					float4 hColor	: TEXCOORD1;
					float4 vColor	: COLOR;
				};
				
				float4 _TopColor;
				float4 _BottomColor;
				float4 _LeftColor;
				float4 _RightColor;
				float4 _Size;
				float _SideDist;
				float _TopDif;
				sampler2D _LightMap;

				v2f vert (appdata v)
				{
					v2f o;					
					o.pos = UnityObjectToClipPos( v.vertex);
					o.uv2 = v.uv2;
					
					float sx = clamp(v.vertex.x - _Size.x, 0, _Size.z) / _Size.z;
					float sy = clamp(v.vertex.y - _Size.y, 0, _Size.w) / _Size.w;
					
					o.hColor = lerp(_LeftColor, _RightColor, sx);
					o.vColor = lerp(_BottomColor, _TopColor, sy);
					o.hColor.a = dot(normalize(v.normal), fixed3(0, 1, 0));	
					o.vColor.a = pow(_SideDist, _TopDif);
					
					return o;
				}

				float4 frag(v2f i) : COLOR
				{			
					float4 mainColor = lerp(i.hColor *  (1 - _SideDist) + i.hColor * i.vColor * _SideDist, i.vColor,  i.hColor.a);																
					return  mainColor * pow(lerp(1, i.vColor, _SideDist), _TopDif) * tex2D(_LightMap, i.uv2); 
				}
			ENDCG		
		}
	}
}