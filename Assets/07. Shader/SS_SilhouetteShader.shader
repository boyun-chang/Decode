Shader "Custom/SS_SilhouetteShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
		_Alpha ("Alpha", Range(0, 1)) = 1.0
		_RimColor ("RimColor", Color) = (0,0,0,0)
		_RimWidth ("RimWidth", Range(0.0, 0.01)) = 0.003
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
		_BumpMap ("NormalMap", 2D) = "bump" {}
		_Glossiness("Smoothness", Range(0, 1)) = 0.5
		_Metallic("Metallic", 2D) = "Metallic" {}
		_Occlusion("Occlusion", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent"}
        LOD 200

		// 1st pass
		cull front
		Zwrite off
        CGPROGRAM
        #pragma surface surf Nolight vertex:vert noshadow noambient
        #pragma target 3.0

        sampler2D _MainTex;
		float4 _RimColor;
		float _RimWidth;

        struct Input
        {
            float2 uv_MainTex;
			float4 color:COLOR;
        };

		void vert(inout appdata_full v)
		{
			v.vertex.xyz += v.normal.xyz * _RimWidth;
		}

		void surf(Input IN, inout SurfaceOutput o)
		{
		
		}

		float4 LightingNolight(SurfaceOutput s, float3 lightDir, float atten)
		{
			return float4(_RimColor.rgb, 1);
		}
		ENDCG

		// 2nd pass
		cull back
		Zwrite on
		CGPROGRAM
		#pragma surface surf Standard alpha:fade
        #pragma target 3.0

		sampler2D _MainTex;
		sampler2D _BumpMap;
		sampler2D _Metallic;
		sampler2D _Occlusion;
        fixed4 _Color;
		half _Glossiness;
		float _Alpha;

		struct Input{
			float2 uv_MainTex;
			float2 uv_BumpMap;
			float2 uv_Metallic; 
			float3 viewDir;
		};
        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
			o.Occlusion = tex2D(_Occlusion, IN.uv_MainTex);
			fixed4 m = tex2D(_Metallic, IN.uv_Metallic);
			o.Metallic = m.r;
			o.Smoothness = _Glossiness;
            o.Albedo = c.rgb;
			float rim = dot(o.Normal, IN.viewDir);
			o.Alpha = saturate(pow(1 - rim, 1) + _Alpha);
        }
        ENDCG
    }
    FallBack "Diffuse"
}
