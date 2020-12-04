Shader "FX/QBlockFX" 
{
	Properties 
	{
		_MainTex ("Base", 2D) = "white" {}
		_Color   ("Color", Color) = (1,1,1,1)
		_Alpha	 ("Alpha", Range(0,1)) = 1.0
	}
	
	SubShader 
	{
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
		LOD 200		
		Lighting off
		ZWrite off
		Cull off
		Fog { Mode Off }
		Blend One One
		
		PASS 
		{		
			CGPROGRAM
				#pragma vertex vert
	        	#pragma fragment frag
	        	#pragma fragmentoption ARB_precision_hint_fastest
	        	#include "UnityCG.cginc"
				
				sampler2D _MainTex;			
				float4 _MainTex_ST;
				fixed4 _Color;
				float _Alpha;
				
		        struct appdata 
		        {	
		            float4 vertex 	: POSITION;	
		            fixed2 uv1 		: TEXCOORD0;	            
		        };
		        
		        struct v2f 
				{				
		            float4 pos: SV_POSITION;	
		            fixed2 uv1: TEXCOORD0;	
		            fixed2 uv2: TEXCOORD1;	
		        };
			        	
		        v2f vert (appdata v)	
		        {	
		            v2f o;	
		            o.pos = UnityObjectToClipPos(v.vertex);	
		            o.uv1 = o.uv2 = TRANSFORM_TEX (v.uv1, _MainTex).xy;
		            o.uv1.x -= _Time.x * 5;     
		            o.uv2.x -= _Time.y;     
		            return o;	
		        }	

		        fixed4 frag (v2f IN) : COLOR	
		        {	
		        	fixed4 mainTex = tex2D(_MainTex, IN.uv1);	    
		        	fixed4 mainTex2 = tex2D(_MainTex, IN.uv2);
		        	float alphaSqr = _Alpha * _Alpha;
		            return 2 * _Color * pow(mainTex.g + mainTex2.r, 1 + 10 * (1 - alphaSqr)) * alphaSqr;
		        }			
			ENDCG
		}
	}
}
