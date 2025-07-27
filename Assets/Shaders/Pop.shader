Shader "Custom/Pop" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _PopColor ("Pop Color", Color) = (1,0.5,0.5,1)
        _PopIntensity ("Pop Intensity", Range(0,2)) = 1
    }
    SubShader {
        Tags { "RenderType"="Opaque" }
        
        CGPROGRAM
        #pragma surface surf Lambert

        sampler2D _MainTex;
        fixed4 _PopColor;
        float _PopIntensity;

        struct Input {
            float2 uv_MainTex;
        };

        void surf (Input IN, inout SurfaceOutput o) {
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
            o.Albedo = c.rgb * _PopColor.rgb * _PopIntensity;
            o.Alpha = c.a;
        }
        ENDCG
    }
}
