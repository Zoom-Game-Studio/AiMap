Shader "Unlit/AssetsChangeGray"
{
	Properties
	{
		//_StartGray("MainTex",bool) = false

		_MainTex("MainTex", 2D) = "white" {}
		_Diffuse("Diffuse", Color) = (1,1,1,1)
		_Specular("Specular", Color) = (1,1,1,1)
		_Gloss("Gloss", Range(1,256)) = 5
		_GrayValue("GrayValue", Range(0,1)) = 0.5
		//遮罩
		_SpecularMask("Specular Mask", 2D) = "white" {}
		_SpecularScale("Specular Scarl",Range(0,2)) = 1

	}

		SubShader
		{
			Tags { "RenderType" = "Opaque" }
			LOD 100

			Pass
			{
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#include "UnityCG.cginc"
				#include "Lighting.cginc"

				sampler2D _MainTex;
				float4 _MainTex_ST;
				fixed4 _Diffuse;
				fixed4 _Specular;
				float _Gloss;
				float _GrayValue;

				sampler2D _SpecularMask;
				float4 _SpecularMask_ST;
				float _SpecularScale;

				struct v2f
				{
					float4 vertex : SV_POSITION;
					fixed3 worldNormal : TEXCOORD0;
					float3 worldPos: TEXCOORD1;
					float2 uv : TEXCOORD2;
					float2 maskUv : TEXCOORD3;
				};

				v2f vert(appdata_base v)
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					fixed3 worldNormal = UnityObjectToWorldNormal(v.normal);
					o.worldNormal = worldNormal;
					o.worldPos = mul(unity_ObjectToWorld, v.vertex);
					o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);//v.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
					o.maskUv = TRANSFORM_TEX(v.texcoord, _SpecularMask);
					return o;
				}

				fixed4 frag(v2f i) : SV_Target
				{
					fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz;

					fixed3 albedo = tex2D(_MainTex, i.uv).rgb;
					//漫反射
					fixed3 worldLightDir = UnityWorldSpaceLightDir(i.worldPos);
					//fixed3 worldLightDir = normalize(_WorldSpaceLightPos0.xyz);
					fixed3 diffuse = _LightColor0.rgb * albedo * _Diffuse.rgb * (dot(worldLightDir,i.worldNormal) * 0.5 + 0.5);

					////高光反射
					////fixed3 reflectDir = normalize(reflect(-worldLightDir,i.worldNormal));
					//fixed3 viewDir = normalize(UnityWorldSpaceViewDir(i.worldPos));
					////fixed3 viewDir = normalize(_WorldSpaceCameraPos.xyz - i.worldPos.xyz);
					//fixed3 halfDir = normalize(worldLightDir + viewDir);
					//fixed3 specular = _LightColor0.rgb * _Specular.rgb * pow(saturate(dot(i.worldNormal,halfDir)),_Gloss);
					//高光遮罩
					fixed4 Mask = tex2D(_SpecularMask, i.uv);
					fixed4 color = fixed4(ambient + diffuse,1);
					//fixed4 color = ambient + diffuse + specularMask;
					
					fixed4 col;
					if (color.r < _GrayValue)
					{
						col = tex2D(_MainTex, i.uv);
						float grey = dot(col.rgb, float3(0.299, 0.587, 0.114));
						col.rgb = float3(grey, grey, grey);
						
					}
					else
					{
						col = color;
					}
					
					clip(Mask.r- col.r);
					//clip(Mask.b - col.b);
					return col;

				}
				ENDCG
			}
			
		}
		
}
