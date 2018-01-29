Shader "onda"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		tempo ("tempo", Range(0, 10)) = 1.
		colore("colore, rosso=0, blu=1", float) = 1.
		velocita("velocita", Range(0, 10)) = 1.
		dimensione ("dimensione", float) = 1.0
		alphaCustom("alphaCustom", Range(0, 1)) = 1.
		alphaTot("alphaTot", Range(0, 1)) = 1.


	}
	SubShader
	{
 		Tags { "Queue"="Transparent" "RenderType"="Transparent" }
		LOD 100
		ZWrite Off
     	Blend SrcAlpha OneMinusSrcAlpha 

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
			float tempo;
			fixed colore;
			fixed velocita;
			float dimensione;
			fixed alphaCustom;
			fixed alphaTot;
			
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
				fixed4 col = tex2D(_MainTex, i.uv);
				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				fixed2 centeredUv = (i.uv-.5)*4;
				centeredUv/=dimensione;
				fixed l = length(centeredUv);
				fixed alpha = (1-smoothstep(l,.4,.41)+step(l,.4))*(1-smoothstep(l,.8,.81)+step(l,.8));
				fixed wave = sin(length(centeredUv)*20-_Time.y*velocita);
				// al posto di 11, 120, 193 qua sotto mettici il colore che vuoi
				fixed4 color = float4(11, 120, 193,255)*colore +(1-colore)*float4(255, 61, 106,255);

				return  alpha*(1+alpha)*abs(wave*wave*wave)*color*alphaCustom;
			}
			ENDCG
		}
	}
}
