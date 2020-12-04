Shader "FX/QBackgroundFX" 
{
	Properties 
	{
		_MainTex ("Base", 2D) = "white" {}
		_BackColor	("Back Color", Color) = (1,1,1,1)
		_Color	("Color", Color) = (1,1,1,1)
	}
	
	SubShader 
	{
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
		LOD 200		
		Lighting off
		ZWrite off
		Fog { Mode Off }

		PASS 
		{		
			CGPROGRAM
				#pragma vertex vert
	        	#pragma fragment frag
	        	#pragma fragmentoption ARB_precision_hint_fastest
				
				sampler2D _MainTex;			
				float4 _MainTex_ST;
				fixed4 _BackColor;
				fixed4 _Color;
				
		        struct appdata 
		        {	
		            float4 vertex 	: POSITION;	
		            fixed4 uv1 		: TEXCOORD0;	            
		        };
		        
		        struct v2f 
				{				
		            float4 pos: SV_POSITION;	
		            fixed2 uv1: TEXCOORD0;	
		            fixed2 uv2: TEXCOORD1;	
		            fixed2 uv3: TEXCOORD2;	
		        };
			        	
		        v2f vert (appdata v)	
		        {	
		            v2f o;	
		            o.pos = UnityObjectToClipPos(v.vertex);	
		            o.uv1 = o.uv2 = o.uv3 = v.uv1.xy * _MainTex_ST.xy;
		            o.uv1.x += _Time.x * _MainTex_ST.z; 	            
		            o.uv1.y += _Time.x * _MainTex_ST.w; 	      
		            o.uv2.x += o.uv1.x * 0.5; 	            
		            o.uv2.y += o.uv1.y * 0.5; 	   
		            o.uv3.x += o.uv2.x * 0.5; 	            
		            o.uv3.y += o.uv2.y * 0.5; 	
		            return o;	
		        }	

		        fixed4 frag (v2f IN) : COLOR	
		        {	
		        	fixed mainTex1 = tex2D(_MainTex, IN.uv1).r;
		        	fixed mainTex2 = tex2D(_MainTex, IN.uv2).g;
		        	fixed mainTex3 = tex2D(_MainTex, IN.uv3).b;
		        	fixed sum = mainTex1 + mainTex2 + mainTex3;
		        
		            return _BackColor + lerp(_BackColor, _Color * sum, _Color.a);
		        }			
			ENDCG
		}
	}
}
