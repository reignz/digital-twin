fixed4 blend4(fixed4 pixel0, fixed4 pixel1, fixed4 pixel2, fixed4 mask)
{
	return  pixel0 * mask.r + pixel1 * mask.g + pixel2 * mask.b;
}

fixed3 blend(fixed3 pixel0, fixed3 pixel1, fixed3 pixel2, fixed3 mask)
{
	return  pixel0 * mask.r + pixel1 * mask.g + pixel2 * mask.b;
}

fixed blend1(fixed pixel0, fixed pixel1, fixed pixel2, fixed3 mask)
{
	return  pixel0 * mask.r + pixel1 * mask.g + pixel2 * mask.b;
}

float3 blendByDepth(fixed3 pixel0, float depth0, fixed3 pixel1, float depth1, fixed3 pixel2, float depth2, fixed3 mask, float depth)
{
    float ma = max(depth0 + mask.r, depth1 + mask.g);
	ma = max(ma, depth2 + mask.b);
	ma -= depth;

    float b1 = max(depth0 + mask.r - ma, 0);
    float b2 = max(depth1 + mask.g - ma, 0);
    float b3 = max(depth2 + mask.b - ma, 0);

    return (pixel0.rgb * b1 + pixel1.rgb * b2 + pixel2.rgb * b3) / (b1 + b2 + b3);
}

float blend1ByDepth(fixed pixel0, float depth0, fixed pixel1, float depth1, fixed pixel2, float depth2, fixed3 mask, float depth)
{
    float ma = max(depth0 + mask.r, depth1 + mask.g);
	ma = max(ma, depth2 + mask.b);
	ma -= depth;

    float b1 = max(depth0 + mask.r - ma, 0);
    float b2 = max(depth1 + mask.g - ma, 0);
    float b3 = max(depth2 + mask.b - ma, 0);

    return (pixel0 * b1 + pixel1 * b2 + pixel2 * b3) / (b1 + b2 + b3);
}

float noise (float2 pf) {
    float2 p = floor(pf);
    float2 f = frac(pf);
    f = f * f * (3.0 - 2.0 * f);
    float n = p.x + p.y * 57.0;
    float4 noise = float4(n, n + 1, n + 57.0, n + 58.0);
    noise = frac(sin(noise)*437.585453);
    return lerp(lerp(noise.x, noise.y, f.x), lerp(noise.z, noise.w, f.x), f.y);
}