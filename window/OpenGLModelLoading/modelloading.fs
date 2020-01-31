#version 330 core
out vec4 FragColor;

in vec2 TexCoords;
in vec3 FragPos;  
in vec3 Normal;  

struct Light {
    vec3 position;

    vec3 ambient;
    vec3 diffuse;
    vec3 specular;

	float constant;
	float linear;
	float quadratic;
};

uniform vec3 viewPos;
uniform Light light;
uniform sampler2D texture_diffuse1;
uniform sampler2D texture_specular1;

void main()
{    
	float distance    = length(light.position - FragPos);
	float attenuation = 1.0 / (light.constant + light.linear * distance + light.quadratic * (distance * distance));

    // ambient
    vec3 ambient      = light.ambient * texture(texture_diffuse1, TexCoords).rgb;
	
    // diffuse 
    vec3 norm         = normalize(Normal);
    vec3 lightDir     = normalize(light.position - FragPos);
    float diff        = max(dot(norm, lightDir), 0.0);
    vec3 diffuse      = light.diffuse * diff * texture(texture_diffuse1, TexCoords).rgb;  
    
    // specular
    vec3 viewDir      = normalize(viewPos - FragPos);
    vec3 reflectDir   = reflect(-lightDir, norm);  
    float spec        = pow(max(dot(viewDir, reflectDir), 0.0), 32);
    vec3 specular     = light.specular * spec * texture(texture_specular1, TexCoords).rgb;  
        
	ambient           *= attenuation; 
	diffuse			  *= attenuation;
	specular		  *= attenuation;
    vec3 result       = ambient + diffuse + specular;
    FragColor         = vec4(result, 1.0);
}