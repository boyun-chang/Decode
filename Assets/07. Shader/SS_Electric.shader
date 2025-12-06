Shader"Custom/SS_Electric"
{
	Properties
	{
		_Color("Electric Color", Color) = (1,1,1,1)
		_NColor("Non Electric Color", Color) = (1,1,1,1)

		_Cutoff("Alpha cutoff", Range(0,1)) = 0.5
		_LineA("line1", Range(0,1)) = 0.7 // line upper
		_LineB("line2", Range(0,1)) = 0.4
		//_LineC("line3", Range(0,1)) = 0.4
		_LineWidth("Line Width", Range(0,1)) = 0.02

		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_BumpTex("Normal", 2D) = "bump" {}

		_FadeMap("Fade Map", 2D) = "white" {}

		_MaskA("MaskA", 2D) = "white" {}
		_MaskB("MaskB", 2D) = "white" {}
		_MaskD("MaskD", 2D) = "white" {}
		_MaskE("MaskE", 2D) = "white" {}

		_Glossiness("Smoothness",2D) = "white" {}
		_Metallic("Metallic", 2D) = "white" {}

		_Occlusion("Occlusion",2D) = "white" {}
	}
		SubShader
	{
		Tags { "RenderType" = "Opaque" }
LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _FadeMap;
		sampler2D _BumpTex;
		sampler2D _MaskA;
		sampler2D _MaskB;
		sampler2D _MaskD;
		sampler2D _MaskE;
		sampler2D _Glossiness;
		sampler2D _Metallic;
		sampler2D _Occlusion;

	
        struct Input
        {
            float2 uv_MainTex;
        };

        fixed4 _Color;
        fixed4 _NColor;
        float _Cutoff;
		float _LineA;
		float _LineB;
		//float _LineC;
		float _LineWidth;

		UNITY_INSTANCING_BUFFER_START(Props)
			UNITY_INSTANCING_BUFFER_END(Props)

			void surf(Input IN, inout SurfaceOutputStandard o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
			o.Normal = UnpackNormal(tex2D(_BumpTex, IN.uv_MainTex));
			fixed4 fadeSample = tex2D(_FadeMap, IN.uv_MainTex);
			fixed4 m = tex2D(_Metallic, IN.uv_MainTex);
			o.Metallic = m.r;
			fixed4 g = tex2D(_Glossiness, IN.uv_MainTex);
			o.Smoothness = g.r;

			o.Albedo = c.rgb;
			fixed4 h = tex2D(_MaskE, IN.uv_MainTex);
			bool cut = (fadeSample.r + fadeSample.g + fadeSample.b) / 3.0 < _Cutoff ? false : true; // Ư�� ������ ���� �κ��� �ڸ� -> false
					//o.Albedo = cut ? diffuse.rgb : float3(1,1,1);
			if ((fadeSample.r + fadeSample.g + fadeSample.b) / 3.0 > _LineA &&
				((fadeSample.r + fadeSample.g + fadeSample.b) / 3.0 < _LineA + _LineWidth))
			{
				cut = true;
			}
			if ((fadeSample.r + fadeSample.g + fadeSample.b) / 3.0 > _LineB &&
				((fadeSample.r + fadeSample.g + fadeSample.b) / 3.0 < _LineB + _LineWidth))
			{
				cut = true;
			}
			/*if ((fadeSample.r + fadeSample.g + fadeSample.b) / 3.0 > _LineC &&
				((fadeSample.r + fadeSample.g + fadeSample.b) / 3.0 < _LineC + _LineWidth))
			{
				cut = true;
			}*/


			float3 emission = h.rgb * _Color * abs(sin(_Time.y * 3)) * 8; // mask uv 0.4~1 ����
			o.Emission = cut ? emission.rgb : h.rgb * _NColor;
			//pow(frac(IN.worldPos.g * 10 - _Time.y), 20); // ��ǥ *10 �ϸ� ��������, �ð� ����    ���� �ö�.
			o.Alpha = c.a;
		}
		ENDCG
	}
		FallBack"Diffuse"
}