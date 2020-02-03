#version 330 core

//注：这里之前不用接口块传递数据时渲染出来的光照效果是错误的。。。暂时没想到是什么原因造成的
in VS_OUT{
	vec2 TexCoords;
	vec3 FragPos;
	vec3 Normal;
}fs_in;

out vec4 FragColor;

uniform sampler2D floorTexture;
uniform vec3 viewPos;
uniform vec3 lightPos;
uniform bool blinn;

void main()
{
	vec3 color=texture(floorTexture,fs_in.TexCoords).rgb;

	//ambient
	float ambientStrength = 0.05;
	vec3 ambient = ambientStrength*color;

	//diffuse
	vec3 norm = normalize(fs_in.Normal);
	vec3 lightDir = normalize(lightPos-fs_in.FragPos);
	float diff = max(dot(lightDir,norm),0.0);
	vec3 diffuse = diff*color;
	
	//specular
	vec3 reflectDir = reflect(-lightDir,norm);
	vec3 viewDir = normalize(viewPos-fs_in.FragPos);
	float spec;
	if(blinn)
	{
		vec3 halfwayDir = normalize(lightDir + viewDir);
		spec = pow(max(dot(norm, halfwayDir), 0.0), 32.0);
	}
	else
	{
        vec3 reflectDir = reflect(-lightDir, norm);
		spec = pow(max(dot(viewDir,reflectDir),0.0),8.0);	
	}
	
	vec3 specular = vec3(0.3) * spec; 
    FragColor = vec4(ambient+diffuse+specular, 1.0);
}


	

