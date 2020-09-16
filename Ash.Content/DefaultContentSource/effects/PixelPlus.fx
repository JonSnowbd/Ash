sampler s0;

float2 renderTargetSize;
float kValue;

float4 PixelShaderFunction( float2 texCoord:TEXCOORD0 ) : COLOR0
{
	texCoord = texCoord * renderTargetSize;

	float2 alpha = float2(kValue,kValue);
	float2 x = frac(texCoord);
	float2 y = clamp(0.5f / alpha * x, 0.0f, 0.5f) + clamp(0.5f / alpha * (x - 1.0f) + 0.5f, 0.0f, 0.5f);
	
	float2 finalCoord = (floor(texCoord) + y) / renderTargetSize;
	
    float4 color = tex2D(s0, finalCoord);
    return color;
}


technique Technique1
{
    pass Pass1
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}