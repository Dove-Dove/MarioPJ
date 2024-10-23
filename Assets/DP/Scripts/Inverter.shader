Shader "Custom/InvertColorWithoutAlpha"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags {"RenderType"="Transparent"}
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float2 texcoord : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = v.texcoord;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // �ؽ�ó���� �ȼ� ���� �����ɴϴ�
                fixed4 color = tex2D(_MainTex, i.texcoord);
                // RGB ���� ������Ű��, ���� ���� �����մϴ�.
                color.rgb = 1.0 - color.rgb;
                return color;
            }
            ENDCG
        }
    }
}
