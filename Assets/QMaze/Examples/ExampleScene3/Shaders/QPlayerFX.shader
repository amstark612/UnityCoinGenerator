Shader "FX/QPlayerFX" 
{
	Properties 
	{
		_MainTex ("Base", 2D) = "white" {}
		_Alpha("Alpha", Range(0, 1)) = 1
	}
	
	SubShader 
	{
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
		LOD 200		
		Lighting off
		Fog { Mode Off } 
		Blend One OneMinusSrcAlpha 
		
		PASS 
		{		
			CGPROGRAM
				#pragma vertex vert
	        	#pragma fragment frag
	        	#pragma fragmentoption ARB_precision_hint_fastest
				#include "UnityCG.cginc" 
				
				sampler2D _MainTex;			
				float4 _MainTex_ST;
				float _Alpha;
				
		        struct appdata 
		        {	
		            float4 vertex 	: POSITION;	
		            fixed2 uv1 		: TEXCOORD0;
		            fixed3 normal	: NORMAL;            
		        };
		        
		        struct v2f 
				{				
		            float4 pos: SV_POSITION;	
		            fixed2 uv1: TEXCOORD0;	
		            fixed2 uv2: TEXCOORD1;
		            fixed color: COLOR;	
		        };
			        	
		        v2f vert (appdata v)	
		        {	
		            v2f o;	
		            o.pos = UnityObjectToClipPos(v.vertex);	
		            
		            float3 viewDir = normalize(ObjSpaceViewDir(v.vertex));	             	            
		            
		            o.uv1 = o.uv2 = v.uv1.xy;
		            o.uv1.x -= _Time.y * 0.1 ; 	            
		            o.uv2.x += _Time.y * 0.25 ;	            
		            o.color = 1 - dot(viewDir, v.normal);
		            
		            return o;	
		        }

		        fixed4 frag (v2f IN) : COLOR	
		        {	
		         	fixed4 mainTex1 = tex2D(_MainTex, IN.uv1);
		         	fixed4 mainTex2 = tex2D(_MainTex, IN.uv2);
		            return (mainTex1 * IN.color + pow((mainTex1 + mainTex2) * (mainTex1.a + mainTex2.a), 2)) * _Alpha;	
		        }			
			ENDCG
		}
	}
}
