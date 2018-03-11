
Shader "Hidden/FastBloom" {
    Properties {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _Bloom ("Bloom (RGB)", 2D) = "black" {}
    }
    
    CGINCLUDE

        #include "UnityCG.cginc"

        sampler2D _MainTex;
        sampler2D _Bloom;
                
        uniform half4 _MainTex_TexelSize;
        half4 _MainTex_ST;
        
        uniform half4 _Parameter;
        uniform half4 _OffsetsA;
        uniform half4 _OffsetsB;
        
        #define ONE_MINUS_THRESHHOLD_TIMES_INTENSITY _Parameter.w
        #define THRESHHOLD _Parameter.z

        struct v2f_simple 
        {
            float4 pos : SV_POSITION; 
            half2 uv : TEXCOORD0;

        #if UNITY_UV_STARTS_AT_TOP
                half2 uv2 : TEXCOORD1;
        #endif
        };    
        
        v2f_simple vertBloom ( appdata_img v )
        {
            v2f_simple o;
            
            o.pos = UnityObjectToClipPos (v.vertex);
            o.uv = UnityStereoScreenSpaceUVAdjust(v.texcoord, _MainTex_ST);
            
        #if UNITY_UV_STARTS_AT_TOP
            o.uv2 = o.uv;
            if (_MainTex_TexelSize.y < 0.0)
                o.uv.y = 1.0 - o.uv.y;
        #endif
                        
            return o; 
        }

        struct v2f_tap
        {
            float4 pos : SV_POSITION;
            half2 uv20 : TEXCOORD0;
            half2 uv21 : TEXCOORD1;
            half2 uv22 : TEXCOORD2;
            half2 uv23 : TEXCOORD3;
        };            

        v2f_tap vert4Tap ( appdata_img v )
        {
            v2f_tap o;

            o.pos = UnityObjectToClipPos (v.vertex);
            o.uv20 = UnityStereoScreenSpaceUVAdjust(v.texcoord + _MainTex_TexelSize.xy, _MainTex_ST);
            o.uv21 = UnityStereoScreenSpaceUVAdjust(v.texcoord + _MainTex_TexelSize.xy * half2(-0.5h,-0.5h), _MainTex_ST);
            o.uv22 = UnityStereoScreenSpaceUVAdjust(v.texcoord + _MainTex_TexelSize.xy * half2(0.5h,-0.5h), _MainTex_ST);
            o.uv23 = UnityStereoScreenSpaceUVAdjust(v.texcoord + _MainTex_TexelSize.xy * half2(-0.5h,0.5h), _MainTex_ST);

            return o; 
        }                    
                        
        fixed4 fragBloom ( v2f_simple i ) : SV_Target
        {    
            #if UNITY_UV_STARTS_AT_TOP
            
            fixed4 color = tex2D(_MainTex, i.uv2);
            return color + tex2D(_Bloom, i.uv);
            
            #else

            fixed4 color = tex2D(_MainTex, i.uv);
            return color + tex2D(_Bloom, i.uv);
                        
            #endif
        } 
        
        fixed4 fragDownsample ( v2f_tap i ) : SV_Target
        {                
            fixed4 color = tex2D (_MainTex, i.uv20);
            color += tex2D (_MainTex, i.uv21);
            color += tex2D (_MainTex, i.uv22);
            color += tex2D (_MainTex, i.uv23);
            return max(color/4 - THRESHHOLD, 0) * ONE_MINUS_THRESHHOLD_TIMES_INTENSITY;
        }
                    
    ENDCG
    
    SubShader {
      ZTest Off Cull Off ZWrite Off Blend Off
      
    // 0
    Pass {
    
        CGPROGRAM
        #pragma vertex vertBloom
        #pragma fragment fragBloom
        
        ENDCG
         
        }

    // 1
    Pass { 
    
        CGPROGRAM
        
        #pragma vertex vert4Tap
        #pragma fragment fragDownsample
        
        ENDCG
         
        }
    }    

    FallBack Off
}
