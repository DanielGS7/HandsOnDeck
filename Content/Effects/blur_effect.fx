sampler2D GameTexture : register(s0);
float BlurAmount;
float2 ScreenSize;

static const float Weights[5] = { 0.227027, 0.1945946, 0.1216216, 0.054054, 0.016216 };

float4 GaussianBlur(float2 texCoord, float2 direction)
{
    float4 color = tex2D(GameTexture, texCoord) * Weights[0];
    
    for (int i = 1; i < 5; ++i)
    {
        float2 offset = direction * i * BlurAmount;
        color += tex2D(GameTexture, texCoord + offset) * Weights[i];
        color += tex2D(GameTexture, texCoord - offset) * Weights[i];
    }
    
    return color;
}

float4 HorizontalBlurPS(float2 texCoord : TEXCOORD0) : COLOR0
{
    return GaussianBlur(texCoord, float2(1.0 / ScreenSize.x, 0));
}

float4 VerticalBlurPS(float2 texCoord : TEXCOORD0) : COLOR0
{
    return GaussianBlur(texCoord, float2(0, 1.0 / ScreenSize.y));
}

technique GaussianBlurTechnique
{
    pass HorizontalBlur
    {
        PixelShader = compile ps_3_0 HorizontalBlurPS();
    }
    
    pass VerticalBlur
    {
        PixelShader = compile ps_3_0 VerticalBlurPS();
    }
}