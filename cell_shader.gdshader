//rgb to hsv from https://godotshaders.com/shader/hsv-adjustment/

shader_type spatial;
render_mode cull_disabled;

uniform sampler2D main_texture : source_color, hint_default_white;
uniform float roughness : hint_range(0.0, 1.0) = 0.0;
uniform float shade_threshold : hint_range(-1.0, 1.0, 0.001) = 0.0;
uniform float shade_softness : hint_range(0.0, 1.0, 0.001) = 0.05;
uniform float shade_darkness : hint_range(0.0, 1.0, 0.05) = 0.2; //supposed to be a %

uniform float edge_strength : hint_range(0.0, 1.0) = 1.0;
uniform vec4 edge_color : source_color = vec4(1.0, 1.0, 1.0, 0.4);

vec3 rgb2hsv(vec3 c) {
    vec4 K = vec4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
    vec4 p = mix(vec4(c.bg, K.wz), vec4(c.gb, K.xy), step(c.b, c.g));
    vec4 q = mix(vec4(p.xyw, c.r), vec4(c.r, p.yzx), step(p.x, c.r));

    float d = q.x - min(q.w, q.y);
    float e = 1.0e-10;
    return vec3(abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
}

vec3 hsv2rgb(vec3 c) {
    vec4 K = vec4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
    vec3 p = abs(fract(c.xxx + K.xyz) * 6.0 - K.www);
    return c.z * mix(K.xxx, clamp(p - K.xxx, 0.0, 1.0), c.y);
}

float SchlickFresnel(float u) {
	float m = 1.0 - u;
	float m2 = m * m;
	return m2 * m2 * m;
}

void vertex() {
	// Called for every vertex the material is visible on.
}

void fragment() {
	// Called for every pixel the material is visible on.
	vec4 _main = texture(main_texture, UV);
	float VdotN = dot(VIEW * edge_strength, NORMAL);
	
	float fresnel = clamp(SchlickFresnel(VdotN), 0.0, 1.0);
	
	// apply glass look
	float a = mix(0.001, 1.0, fresnel);
	//ALPHA = mix(fresnel * edge_color.a, 1.0, a);
	//ALBEDO = mix(edge_color.rgb * edge_color.a, _main.rgb, a);
	
	ALBEDO = _main.rgb + edge_color.rgb * edge_color.a * a;
	ROUGHNESS = roughness;
}

//void light() {
	////Called for every pixel for every light affecting the material.
	////Uncomment to replace the default light processing function with this one.
	//float NdotL = dot(NORMAL, LIGHT);
	////float is_lit = step(shade_threshold, NdotL);
	//vec3 base = ALBEDO.rgb * LIGHT_COLOR / PI;
	//vec3 light_hsv = rgb2hsv(base);
	//vec3 shade = hsv2rgb(vec3(light_hsv.x, light_hsv.y, light_hsv.z * 0.8));
	//
	//float shade_value = smoothstep(shade_threshold - shade_softness, shade_threshold + shade_softness, NdotL);
	//
	//// Shadows
	//shade_value *= ATTENUATION;
	//vec3 diffuse = mix(shade, base, shade_value);
	//DIFFUSE_LIGHT += diffuse;
//}

void light() {
	//Called for every pixel for every light affecting the material.
	//Uncomment to replace the default light processing function with this one.
	float NdotL = dot(NORMAL, LIGHT);
	//float is_lit = step(shade_threshold, NdotL);
	vec3 base = ALBEDO.rgb;
	vec3 light_hsv = rgb2hsv(base);
	vec3 shade = hsv2rgb(vec3(light_hsv.x, light_hsv.y, light_hsv.z * (1.0 - shade_darkness)));
	
	float shade_value = smoothstep(shade_threshold - shade_softness, shade_threshold + shade_softness, NdotL);
	
	// Shadows
	shade_value *= ATTENUATION;
	vec3 diffuse = mix(shade, base, shade_value * LIGHT_COLOR);
	DIFFUSE_LIGHT += diffuse * LIGHT_COLOR / PI;
}
