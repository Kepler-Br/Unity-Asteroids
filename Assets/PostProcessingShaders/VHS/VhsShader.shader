// Shader made by ompuco, converted to shaderlab by me.
// Check out original shader:
//https://www.shadertoy.com/view/XlsczN

Shader "Hidden/VhsShader"
{
    Properties
    {
        _MainTex ("tex2D", 2D) = "white" {}
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
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
                float4 screenPos : TEXCOORD1;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;

            float3 rgb2yiq(float3 c){   
                return float3(
                (0.2989*c.x + 0.5959*c.y + 0.2115*c.z),
                (0.5870*c.x - 0.2744*c.y - 0.5229*c.z),
                (0.1140*c.x - 0.3216*c.y + 0.3114*c.z)
                );
            }

            float3 yiq2rgb(float3 c){                
                return float3(
                (1.0*c.x +    1.0*c.y +    1.0*c.z),
                ( 0.956*c.x - 0.2720*c.y - 1.1060*c.z),
                (0.6210*c.x - 0.6474*c.y + 1.7046*c.z)
                );
            }
            
            float2 Circle(float Start, float Points, float Point) 
            {
                float Rad = (3.141592 * 2.0 * (1.0 / Points)) * (Point + Start);
                //return float2(sin(Rad), cos(Rad));
                return float2(-(.3+Rad), cos(Rad));

            }

            float3 Blur(float2 uv, float f, float d)
            {
                float iTime = _Time[1];
                //  d=abs(d);
                float t = (sin(iTime*5.0+uv.y*5.0))/10.0;
                float b = 1.0;
                //t=sin(iTime*5.0+f)/10.0;
                t=0.0;
                float2 PixelOffset=float2(d+.0005*t,0);
                
                float Start = 2.0 / 14.0;
                float2 Scale = 0.66 * 4.0 * 2.0 * PixelOffset.xy;
                
                float3 N0 = tex2D(_MainTex, uv + Circle(Start, 14.0, 0.0) * Scale).rgb;
                float3 N1 = tex2D(_MainTex, uv + Circle(Start, 14.0, 1.0) * Scale).rgb;
                float3 N2 = tex2D(_MainTex, uv + Circle(Start, 14.0, 2.0) * Scale).rgb;
                float3 N3 = tex2D(_MainTex, uv + Circle(Start, 14.0, 3.0) * Scale).rgb;
                float3 N4 = tex2D(_MainTex, uv + Circle(Start, 14.0, 4.0) * Scale).rgb;
                float3 N5 = tex2D(_MainTex, uv + Circle(Start, 14.0, 5.0) * Scale).rgb;
                float3 N6 = tex2D(_MainTex, uv + Circle(Start, 14.0, 6.0) * Scale).rgb;
                float3 N7 = tex2D(_MainTex, uv + Circle(Start, 14.0, 7.0) * Scale).rgb;
                float3 N8 = tex2D(_MainTex, uv + Circle(Start, 14.0, 8.0) * Scale).rgb;
                float3 N9 = tex2D(_MainTex, uv + Circle(Start, 14.0, 9.0) * Scale).rgb;
                float3 N10 = tex2D(_MainTex, uv + Circle(Start, 14.0, 10.0) * Scale).rgb;
                float3 N11 = tex2D(_MainTex, uv + Circle(Start, 14.0, 11.0) * Scale).rgb;
                float3 N12 = tex2D(_MainTex, uv + Circle(Start, 14.0, 12.0) * Scale).rgb;
                float3 N13 = tex2D(_MainTex, uv + Circle(Start, 14.0, 13.0) * Scale).rgb;
                float3 N14 = tex2D(_MainTex, uv).rgb;
                
                float4 clr = tex2D(_MainTex, uv);
                float W = 1.0 / 15.0;
                
                clr.rgb= 
                (N0 * W) +
                (N1 * W) +
                (N2 * W) +
                (N3 * W) +
                (N4 * W) +
                (N5 * W) +
                (N6 * W) +
                (N7 * W) +
                (N8 * W) +
                (N9 * W) +
                (N10 * W) +
                (N11 * W) +
                (N12 * W) +
                (N13 * W) +
                (N14 * W);
                
                
                return  float3(clr.xyz)*b;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float iTime = _Time[1];
                float4 iResolution = _ScreenParams;
                float4 fragCoord = i.screenPos;
                float4 fragColor = tex2D(_MainTex, i.uv);

                float d = 0.0f;
                // d=.1-round(mod(iTime/3.0,1.0))*.1;
                // float2 uv = fragCoord.xy / iResolution.xy;
                float2 uv = i.uv;
                

                float s = 0.0f;// - texture(iChannel1,vec2(0.01+uv.y/1000.0,1.0)).r)*2.0;
                
                float e = min(.30,pow(max(0.0,cos(uv.y*4.0+.3)-.75)*(s+0.5)*1.0,3.0))*25.0;

                uv.x+=e*abs(s*3.0);
                // float r = texture(iChannel2,vec2(mod(iTime*10.0,mod(iTime*10.0,256.0)*(1.0/256.0)),0.0)).r*(2.0*s);
                float r = 0.0f;
                uv.x+=abs(r*pow(min(.003,(uv.y-.15))*6.0,2.0));
                
                d=.051+abs(sin(s/4.0));
                float c = max(0.0001,.002*d);
                float2 uvo = uv;
                // uv.x+=.1*d;
                fragColor.xyz =Blur(uv,0.0,c+c*(uv.x));
                float y = rgb2yiq(fragColor.xyz).r;
                
                
                
                uv.x+=.01*d;
                c*=6.0;
                fragColor.xyz =Blur(uv,.333,c);
                // texture(iChannel0, uv);
                float ii = rgb2yiq(fragColor.xyz).g;
                
                
                uv.x+=.005*d;
                
                c*=2.50;
                fragColor.xyz =Blur(uv,.666,c);
                float q = rgb2yiq(fragColor.xyz).b;
                
                
                
                fragColor.xyz=yiq2rgb(float3(y,ii,q))-pow(s+e*2.0,3.0);
                fragColor.xyz*=smoothstep(1.0,.999,uv.x-.1);

                return fragColor;
                
                // fragColor.xyz-=min(0.10,pow(uv.x,3.0))*d;
                // fixed4 col = tex2D(_MainTex, i.uv);
                // col.rgb = 1 - col.rgb;
                // return col;
            }
            ENDCG
        }
    }
}
