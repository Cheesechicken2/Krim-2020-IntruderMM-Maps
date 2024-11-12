Shader "Custom/FlashingBlue"
{
    Properties
    {
        _Color ("Base Color", Color) = (0.118, 0, 1, 0.86) // Base color: #1E00FF, Alpha: 219 (0.86)
        _PulseSpeed ("Pulse Speed", Range(0.1, 5.0)) = 1.0 // Controls the pulsing speed
        _PulseIntensity ("Pulse Intensity", Range(1.0, 2.0)) = 1.5 // Intensity of the pulse
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 200

        Cull Back // Makes the material one-sided

        Blend SrcAlpha OneMinusSrcAlpha // Fade rendering mode
        ZWrite Off // Disable depth writing for transparency
        Lighting Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
            };

            fixed4 _Color;
            float _PulseSpeed;
            float _PulseIntensity;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Create a pulsing effect by modifying the blue intensity
                float pulse = abs(sin(_Time.y * _PulseSpeed)) * (_PulseIntensity - 1.0) + 1.0;
                fixed4 pulsingColor = fixed4(_Color.rgb * pulse, _Color.a);
                return pulsingColor;
            }
            ENDCG
        }
    }
    FallBack "Transparent/Cutout/VertexLit"
}
