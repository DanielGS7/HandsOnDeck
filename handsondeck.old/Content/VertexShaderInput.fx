struct VertexShaderInput
{
    float4 Position : POSITION;
    float2 TexCoord : TEXCOORD0;
};

struct VertexShaderOutput
{
    float4 Position : SV_POSITION;
    float2 TexCoord : TEXCOORD0;
};

cbuffer ConstantBuffer : register(b0)
{
    matrix WorldViewProjection; // Matrix for transforming vertices
    float3 sunPosition;         // Position of the sun
    float waterLevel;           // Water level for water effects
    float3 sunSetColor;         // Color of the sunset
    float3 midDayColor;         // Color of midday
    float ambientStrength;      // Strength of the ambient lighting
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;
    output.Position = mul(input.Position, WorldViewProjection);
    output.TexCoord = input.TexCoord;
    return output;
}

Texture2D heightMap : register(t0);
Texture2D voronoiTex : register(t1);
SamplerState Sampler : register(s0);

float4 PixelShaderFunction(VertexShaderOutput input) : SV_TARGET
{
    float2 texCoord = input.TexCoord;
    float height = heightMap.Sample(Sampler, texCoord).r;
    float voronoi = voronoiTex.Sample(Sampler, texCoord).r;

    float distSq = pow(sunPosition.x - 0.5, 2) + pow(sunPosition.y - 0.5, 2);
    float4 color = lerp(float4(sunSetColor, 1), float4(midDayColor, 1), 1 - distSq);

    if (height < waterLevel)
        return float4(0, 0, 1, 1);

    return color * ambientStrength; 
}

technique MainTechnique
{
    pass P0
    {
        VertexShader = compile vs_5_0 VertexShaderFunction();
        PixelShader = compile ps_5_0 PixelShaderFunction();
    }
}
