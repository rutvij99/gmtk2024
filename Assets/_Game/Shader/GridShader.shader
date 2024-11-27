Shader "Custom/TransparentDualGridShader"
{
    Properties
    {
        _Color ("Grid Color", Color) = (1,1,1,1)
        _BigGridScale ("Big Grid Scale", Float) = 1.0
        _SmallGridScale ("Small Grid Scale", Float) = 10.0
        _BigGridThickness ("Big Grid Thickness", Float) = 0.2
        _SmallGridThickness ("Small Grid Thickness", Float) = 0.07
        _Transparency ("Transparency", Range(0,1)) = 0.0
        _Tiling ("Tiling", Vector) = (100, 100, 0, 0)
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100

        Pass
        {
            // Disable lighting for this pass
            Lighting Off
            
            Blend SrcAlpha OneMinusSrcAlpha
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            float _BigGridScale;
            float _SmallGridScale;
            float _BigGridThickness;
            float _SmallGridThickness;
            float4 _Color;
            float _Transparency;
            float4 _Tiling;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv * _Tiling.xy;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Calculate big grid
                float2 bigGrid = abs(frac(i.uv * _BigGridScale - 0.5) - 0.5) / fwidth(i.uv);
                float bigLine = min(bigGrid.x, bigGrid.y) * _BigGridThickness;
                float bigGridPattern = 1.0 - smoothstep(0.0, 1.0, bigLine);

                // Calculate small grid
                float2 smallGrid = abs(frac(i.uv * _SmallGridScale - 0.5) - 0.5) / fwidth(i.uv);
                float smallLine = min(smallGrid.x, smallGrid.y) * _SmallGridThickness;
                float smallGridPattern = 1.0 - smoothstep(0.0, 1.0, smallLine);

                // Combine big and small grid
                float gridPattern = max(bigGridPattern, smallGridPattern);

                // Apply transparency to the background and color to the grid
                float alpha = gridPattern * _Color.a * (1.0 - _Transparency);
                return float4(_Color.rgb, alpha);
            }
            ENDCG
        }
    }
    // Removed fallback to "Diffuse"
}
//Shader "Custom/TransparentTripleGridShader"
//{
//    Properties
//    {
//        _Color ("Grid Color", Color) = (1,1,1,1)
//        _BigGridScale ("Big Grid Scale", Float) = 1.0
//        _SmallGridScale ("Small Grid Scale", Float) = 10.0
//        _SmallestGridScale ("Smallest Grid Scale", Float) = 100.0
//        _BigGridThickness ("Big Grid Thickness", Float) = 0.2
//        _SmallGridThickness ("Small Grid Thickness", Float) = 0.07
//        _SmallestGridThickness ("Smallest Grid Thickness", Float) = 0.03
//        _Transparency ("Transparency", Range(0,1)) = 0.0
//        _Tiling ("Tiling", Vector) = (100, 100, 0, 0)
//    }
//    SubShader
//    {
//        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
//        LOD 100
//
//        Pass
//        {
//            Blend SrcAlpha OneMinusSrcAlpha
//            CGPROGRAM
//            #pragma vertex vert
//            #pragma fragment frag
//            #include "UnityCG.cginc"
//
//            struct appdata
//            {
//                float4 vertex : POSITION;
//                float2 uv : TEXCOORD0;
//            };
//
//            struct v2f
//            {
//                float2 uv : TEXCOORD0;
//                float4 vertex : SV_POSITION;
//            };
//
//            float _BigGridScale;
//            float _SmallGridScale;
//            float _SmallestGridScale;
//            float _BigGridThickness;
//            float _SmallGridThickness;
//            float _SmallestGridThickness;
//            float4 _Color;
//            float _Transparency;
//            float4 _Tiling;
//
//            v2f vert (appdata v)
//            {
//                v2f o;
//                o.vertex = UnityObjectToClipPos(v.vertex);
//                o.uv = v.uv * _Tiling.xy;
//                return o;
//            }
//
//            fixed4 frag (v2f i) : SV_Target
//            {
//                // Calculate big grid
//                float2 bigGrid = abs(frac(i.uv * _BigGridScale - 0.5) - 0.5) / fwidth(i.uv);
//                float bigLine = min(bigGrid.x, bigGrid.y) * _BigGridThickness;
//                float bigGridPattern = 1.0 - smoothstep(0.0, 1.0, bigLine);
//
//                // Calculate small grid
//                float2 smallGrid = abs(frac(i.uv * _SmallGridScale - 0.5) - 0.5) / fwidth(i.uv);
//                float smallLine = min(smallGrid.x, smallGrid.y) * _SmallGridThickness;
//                float smallGridPattern = 1.0 - smoothstep(0.0, 1.0, smallLine);
//
//                // Calculate smallest grid
//                float2 smallestGrid = abs(frac(i.uv * _SmallestGridScale - 0.5) - 0.5) / fwidth(i.uv);
//                float smallestLine = min(smallestGrid.x, smallestGrid.y) * _SmallestGridThickness;
//                float smallestGridPattern = 1.0 - smoothstep(0.0, 1.0, smallestLine);
//
//                // Combine all three grids
//                float gridPattern = max(bigGridPattern, max(smallGridPattern, smallestGridPattern));
//
//                // Apply transparency to the background and color to the grid
//                float alpha = gridPattern * _Color.a * (1.0 - _Transparency);
//                return float4(_Color.rgb, alpha);
//            }
//            ENDCG
//        }
//    }
//    FallBack "Diffuse"
//}
