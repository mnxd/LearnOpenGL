#version 330 core

uniform vec3 cameraPos;
uniform samplerCube skybox;
uniform sampler2D texture_diffuse1;
uniform sampler2D texture_reflection1;


in vec3 Normal;
in vec3 Position;
in vec2 TexCoords;

out vec4 FragColor;

void main()
{    	
	vec3 I					= normalize(Position - cameraPos);
	vec3 R					= reflect(I, normalize(Normal));

	//反射
	// FragColor			=  vec4(texture(skybox, R).rgb,1.0);

	//模型+环境光反射
    vec4 diffuse_color		= texture(texture_diffuse1, TexCoords);
    float reflect_intensity = texture(texture_reflection1, TexCoords).r;
    vec4 reflect_color		= vec4(0.0, 0.0, 0.0, 0.0);
    if(reflect_intensity > 0.1) 
	{
        reflect_color		= texture(skybox, R) * reflect_intensity;
	}
    // 叠加BUG：但是这里的反射贴图似乎是因为assimp的原因导入失败无法正常显示，尤其是护目镜那个部分
    FragColor				=  diffuse_color+reflect_color;

	
	//折射
	//  float ratio			= 1.00 / 1.52;
	//  vec3 I				= normalize(Position - cameraPos);
	//  vec3 R				= refract(I, normalize(Normal), ratio);
	//  FragColor			= vec4(texture(skybox, R).rgb, 1.0);

}