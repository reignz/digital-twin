fixed4 blend5(fixed4 pixel0, fixed4 pixel1, fixed4 pixel2, fixed4 pixel3, fixed4 mask)
{
	fixed inv_mask = 1.0 - mask.a;
	return  max(pixel0 * (mask.r - inv_mask),0) + max(pixel1 * (mask.g - inv_mask),0) + max(pixel2 * (mask.b - inv_mask),0) + max(pixel3 * inv_mask,0);
}

fixed3 blend(fixed3 pixel0, fixed3 pixel1, fixed3 pixel2, fixed3 pixel3,  fixed4 mask)
{
    fixed inv_mask = 1.0 - mask.a;
	return  pixel0 * max((mask.r - inv_mask),0) + pixel1 * max((mask.g - inv_mask),0) + pixel2 * max((mask.b - inv_mask),0) + pixel3 * max(inv_mask,0);
}

// float noise (float2 pf) {
//     float2 p = floor(pf);
//     float2 f = frac(pf);
//     f = f * f * (3.0 - 2.0 * f);
//     float n = p.x + p.y * 57.0;
//     float4 noise = float4(n, n + 1, n + 57.0, n + 58.0);
//     noise = frac(sin(noise)*437.585453);
//     return lerp(lerp(noise.x, noise.y, f.x), lerp(noise.z, noise.w, f.x), f.y);
// }