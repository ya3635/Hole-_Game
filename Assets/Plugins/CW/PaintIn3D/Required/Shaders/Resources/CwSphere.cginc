#include "../../../../PaintCore/Required/Resources/CwShared.cginc"
#include "../../../../PaintCore/Required/Resources/CwMasking.cginc"
#include "../../../../PaintCore/Required/Resources/CwBlendModes.cginc"
#include "../../../../PaintCore/Required/Resources/CwExtrusions.cginc"
#include "../../../../PaintCore/Required/Resources/CwOverlap.cginc"

float4    _Coord;
float4    _Channels;
float4x4  _Matrix;
float4    _Color;
float     _Opacity;
float     _Hardness;
float     _In3D;

sampler2D _TileTexture;
float4x4  _TileMatrix;
float4x4 _ReferenceMatrix;
float     _TileOpacity;
float     _TileTransition;

struct a2v
{
	float4 vertex    : POSITION;
	float3 normal    : NORMAL;
	float2 texcoord0 : TEXCOORD0;
	float2 texcoord1 : TEXCOORD1;
	float2 texcoord2 : TEXCOORD2;
	float2 texcoord3 : TEXCOORD3;
};

struct v2f
{
	float4 vertex   : SV_POSITION;
	float2 texcoord : TEXCOORD0;
	float3 local_position : TEXCOORD1;
	float3 tile     : TEXCOORD2;
	float3 weights  : TEXCOORD3;
	float3 mask     : TEXCOORD4;
	float4 vpos     : TEXCOORD5;
	float3 world_pos : TEXCOORD6;
};

void Vert(a2v i, out v2f o)
{
	float2 texcoord    = i.texcoord0 * _Coord.x + i.texcoord1 * _Coord.y + i.texcoord2 * _Coord.z + i.texcoord3 * _Coord.w;

	float4 myWorldPos = mul(unity_ObjectToWorld, i.vertex);
	float3 myWorldNormal = normalize(mul((float3x3)unity_ObjectToWorld, i.normal));

	float4 local_position    = mul(_ReferenceMatrix, myWorldPos);
	float3 local_normal = normalize(mul((float3x3)_ReferenceMatrix, myWorldNormal));

	o.world_pos = lerp(float3(texcoord, 0.0f), myWorldPos.xyz, _In3D);
	o.world_pos = mul((float3x3)_Matrix, o.world_pos);
	
	o.vertex   = float4(texcoord.xy * 2.0f - 1.0f, 0.5f, 1.0f);

	o.local_position = local_position;
	//o.local_position = mul((float3x3)_Matrix, o.local_position);

	o.texcoord = texcoord;
	float3 tile_scale = float3(
		length(_TileMatrix[0].xyz),
		length(_TileMatrix[1].xyz),
		length(_TileMatrix[2].xyz)
		);

	o.tile     = mul(_TileMatrix, local_position).xyz;
	o.mask     = mul(_MaskMatrix, myWorldPos).xyz;
	o.vpos     = mul(_DepthMatrix, myWorldPos);

	o.weights = pow(abs(local_normal), _TileTransition);
	o.weights /= o.weights.x + o.weights.y + o.weights.z;

#if UNITY_UV_STARTS_AT_TOP
	o.vertex.y = -o.vertex.y;
#endif
}

float CW_GetStrength(float distance)
{
	float strength = 1.0f;
	strength -= pow(saturate(distance), _Hardness);
	strength *= _Opacity;
	return strength;
}

float CW_GetStrength(v2f i, float distance)
{
	float strength = CW_GetStrength(distance);
	#if CW_LINE_CLIP || CW_QUAD_CLIP
		#if CW_LINE_CLIP
			float3 f_position = i.world_pos - _Position;
		#elif CW_QUAD_CLIP
			float3 f_position = i.world_pos - CW_GetClosestPosition_Edge(_Position, _EndPosition, i.world_pos);
		#endif
		float f_strength = CW_GetStrength(length(f_position));

		return CW_GetOverlapStrength(strength, f_strength);
	#else
		return strength;
	#endif
}

float4 Frag(v2f i) : SV_TARGET
{
	float3 position = i.world_pos - CW_GetClosestPosition(i.world_pos);
	float  distance = length(position);

	// You can remove this to improve performance if you don't care about overlapping UV support
	if (distance > 1.0f)
	{
		discard;
	}

	float  strength = CW_GetStrength(i, distance);
	float4 color    = _Color;

	// Fade mask
	strength *= CW_GetMask(i.mask);

	// Fade local mask
	strength *= CW_GetLocalMask(i.texcoord);

	// Fade depth
	strength *= CW_GetDepthMask(i.vpos);

	// Mix in tiling
	float4 textureX = tex2D(_TileTexture, i.tile.yz) * i.weights.x;
	float4 textureY = tex2D(_TileTexture, i.tile.xz) * i.weights.y;
	float4 textureZ = tex2D(_TileTexture, i.tile.xy) * i.weights.z;
	color *= lerp(float4(1.0f, 1.0f, 1.0f, 1.0f), textureX + textureY + textureZ, _TileOpacity);

	return CW_Blend(color, strength, i.texcoord, _Channels);
}