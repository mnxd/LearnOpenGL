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

	//����
	// FragColor			=  vec4(texture(skybox, R).rgb,1.0);

	//ģ��+�����ⷴ��
    vec4 diffuse_color		= texture(texture_diffuse1, TexCoords);
    float reflect_intensity = texture(texture_reflection1, TexCoords).r;
    vec4 reflect_color		= vec4(0.0, 0.0, 0.0, 0.0);
    if(reflect_intensity > 0.1) 
	{
        reflect_color		= texture(skybox, R) * reflect_intensity;
	}
    // ����BUG����������ķ�����ͼ�ƺ�����Ϊassimp��ԭ����ʧ���޷�������ʾ�������ǻ�Ŀ���Ǹ�����
    FragColor				=  diffuse_color+reflect_color;

	
	//����
	//  float ratio			= 1.00 / 1.52;
	//  vec3 I				= normalize(Position - cameraPos);
	//  vec3 R				= refract(I, normalize(Normal), ratio);
	//  FragColor			= vec4(texture(skybox, R).rgb, 1.0);

}