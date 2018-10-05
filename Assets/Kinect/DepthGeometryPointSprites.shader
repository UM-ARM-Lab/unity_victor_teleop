Shader "Custom/DepthGeometryPointSprites"
{
	Properties
	{
		_MainTex("Depth", 2D) = "white" {}
		_ColorTex("Color", 2D) = "white" {}
		//_WorldScale("WorldScale", Float) = 1000.0
			//_OriginOffset ("Origin Offset", Vector) = (0, 0, 0)
			//_OriginEuler = ("Origin Euler", Vector) = (0, 0, 0)
	}

		SubShader
		{
			Pass
			{
				Tags { "RenderType" = "Opaque" }
				LOD 200

				CGPROGRAM
					#pragma target 5.0
					#include "UnityCG.cginc" 
					#include "KinectCommon.cginc"
					#pragma vertex VS_Empty
					#pragma geometry GS_Main			
					#pragma fragment FS_Passthrough				

					//float _WorldScale;
					Texture2D _MainTex;
					Texture2D _ColorTex;
					//float3 _OriginOffset;
					float4x4 transformationMatrix;

					[maxvertexcount(4)]
					void GS_Main(point EMPTY_INPUT p[1], uint primID : SV_PrimitiveID, inout TriangleStream<POSCOLOR_INPUT> triStream)
					{
						//if (primID != 960*270 + 480)
						//	return;
						POSCOLOR_INPUT output;
						output.color.a = 1.0;

						//With Loading Raw
						
						int3 textureCoordinates = int3(primID % DepthWidth, primID / DepthWidth, 0);
						int3 colorCoordinates = int3(textureCoordinates.x, DepthHeight - textureCoordinates.y, 0);
						//float depth = DepthFromPacked4444(_MainTex.Load(textureCoordinates));
						//float raw_depth = _MainTex.Load(textureCoordinates);
						// don't output quads for pixels with invalid depth data
						float depth_raw = _MainTex.Load(colorCoordinates);
						//float4 depth_encoded = _MainTex.Load(tc2);

						//if (depth_encoded.z == 0)
						//	return;

						//uint depth_raw_u = _MainTex.Load(textureCoordinates);
						//uint depth = _MainTex.Load(textureCoordinates);
						//float depth = 0.1;
						// color based on depth
						//if(depth_raw_u==0)
						//	return;

						output.color.rgb = _ColorTex.Load(textureCoordinates);
						



						//With Compression
						/*
						int3 textureCoordinates = int3(primID % DepthWidth, primID / DepthWidth, 0);
						int3 tc2 = int3(primID % DepthWidth, primID / DepthWidth, 0);
						float depth_raw = _MainTex.Load(textureCoordinates);
						output.color.rgb = _ColorTex.Load(textureCoordinates);
						output.color.a = 1.0;
						*/

						// convert to meters and scale to world				
						//float worldScaledDepth = depth;// * MillimetersToMetersScale * _WorldScale;
						//float worldScaledDepth = 1.0 / (4500 * depth * -0.0030711016 + 3.3309495161);
						//float worldScaledDepth = ((float)depth) / 1000;
						//float depth = depth_raw * 66;
						float depth = depth_raw *65;
						//depth /= 1000;
						//float4 worldPos = float4(-textureCoordinates.y, worldScaledDepth, textureCoordinates.x, 1.0);
						float4 worldPosRos = float4(
							(textureCoordinates.x - cx)*depth / fx,
							(textureCoordinates.y - cy)*depth / fy,
							depth,
							1);


						//float4 worldPos = float4(-worldPosRos.y, worldPosRos.z, worldPosRos.x, 1.0);
						float4 worldPos = float4(worldPosRos.y, worldPosRos.z, worldPosRos.x, 1.0);

						worldPos = mul(transformationMatrix, worldPos);


						float4 viewPos = mul(UNITY_MATRIX_V, worldPos);

						for (int i = 0; i < 4; ++i)
						{
							// expand vertices in view space so that they're always the same size no matter what direction you're looking at them from
							float4 viewPosExpanded = viewPos + (quadOffsets[i] * XYSpread * depth);
							output.pos = mul(UNITY_MATRIX_P, viewPosExpanded);
							triStream.Append(output);
						}
					}

					ENDCG
			}
		}
}
