#version 330 core
out vec4 FragColor;

in vec3 Normal;
in vec3 Position;
in vec2 TexCoords;

uniform vec3 cameraPos;
uniform samplerCube skybox;
uniform sampler2D texture_diffuse1;

void main()
{    
	
    vec3 I = normalize(Position - cameraPos);
    vec3 R = reflect(I, normalize(Normal));

	//BUG::�����2d����Ĳ�������ͺ���������������mix�޷��õ���ȷ�Ľ�������Ǹ��Բ������������ȷ�ġ�
	FragColor =  mix(texture(texture_diffuse1,TexCoords), texture(skybox, R),1);

  //  float ratio = 1.00 / 1.52;
  //  vec3 I = normalize(Position - cameraPos);
  //  vec3 R = refract(I, normalize(Normal), ratio);
  //  FragColor = vec4(texture(skybox, R).rgb, 1.0);
}