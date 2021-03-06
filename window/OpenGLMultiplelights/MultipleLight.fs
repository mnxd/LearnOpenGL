#version 330 core
out vec4 FragColor;

struct Material {
    sampler2D diffuse;
    sampler2D specular;    
    float shininess;
}; 

struct DirectionLight {
	vec3 direction;

	vec3 ambient;
	vec3 diffuse;
	vec3 specular;
};

struct PointLight {
	 vec3  position;
	 vec3 color;
	 vec3 ambient;
	 vec3 diffuse;
	 vec3 specular;

	 float constant;
	 float linear;
     float quadratic;
};

struct SpotLight {
   vec3  position;
   vec3  direction;
   float cutOff;
   float outerCutOff;

   vec3 ambient;
   vec3 diffuse;
   vec3 specular;

   float constant;
   float linear;
   float quadratic;
};

#define NR_POINT_LIGHTS 4

in vec3 FragPos;  
in vec3 Normal;  
in vec2 TexCoords;
  
uniform vec3 viewPos;
uniform Material material;
uniform DirectionLight dirlight;
uniform PointLight pointLights[NR_POINT_LIGHTS];
uniform SpotLight spotLight;

vec3 caculDirLight(DirectionLight light,vec3 normal,vec3 viewDir);
vec3 caculPointLight(PointLight light,vec3 normal,vec3 fragPos,vec3 viewDir);
vec3 caculSpotLight(SpotLight light,vec3 normal,vec3 fragPos,vec3 viewDir);

void main()
{
	vec3 norm = normalize(Normal);
	vec3 viewDir = normalize(viewPos - FragPos);

	vec3 result = caculDirLight(dirlight,norm,viewDir);
	for(int i = 0; i < NR_POINT_LIGHTS; i++)
		result += caculPointLight(pointLights[i],norm,FragPos,viewDir);
	result += caculSpotLight(spotLight,norm,FragPos,viewDir);

	FragColor = vec4(result, 1.0);
} 

vec3 caculDirLight(DirectionLight light,vec3 normal,vec3 viewDir)
{
	// ambient
    vec3 ambient	= light.ambient * texture(material.diffuse, TexCoords).rgb;
  	
    // diffuse 
	vec3 lightDir = normalize(-light.direction);
    float diff		= max(dot(normal, lightDir), 0.0);
    vec3 diffuse	= light.diffuse * diff * texture(material.diffuse, TexCoords).rgb;  
    
    // specular
    vec3 reflectDir = reflect(-lightDir, normal);  
    float spec		= pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);
    vec3 specular	= light.specular * spec * texture(material.specular, TexCoords).rgb;  
        
    return(ambient + diffuse + specular);
}

vec3 caculPointLight(PointLight light,vec3 normal,vec3 fragPos,vec3 viewDir)
{
	float distance    = length(light.position - fragPos);
	float attenuation = 1.0 / (light.constant + light.linear * distance + light.quadratic * (distance * distance));

    // ambient
    vec3 ambient      = light.ambient * texture(material.diffuse, TexCoords).rgb;
	
    // diffuse 
    vec3 lightDir     = normalize(light.position - fragPos);
    float diff        = max(dot(normal, lightDir), 0.0);
    vec3 diffuse      = light.diffuse * diff * (texture(material.diffuse, TexCoords).rgb + light.color);  
    
    // specular
    vec3 reflectDir   = reflect(-lightDir, normal);  
    float spec        = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);
    vec3 specular     = light.specular * spec * texture(material.specular, TexCoords).rgb;  
        
	ambient           *= attenuation; 
	diffuse			  *= attenuation;
	specular		  *= attenuation;
    return(ambient + diffuse + specular);
}

vec3 caculSpotLight(SpotLight light,vec3 normal,vec3 fragPos,vec3 viewDir)
{
	vec3 lightDir		= normalize(light.position - fragPos);
	float theta			= dot(lightDir, normalize(-light.direction));
	float epsilon		= light.cutOff - light.outerCutOff;
	float intensity		= clamp((theta - light.outerCutOff) / epsilon, 0.0, 1.0); 

	float distance		= length(light.position - fragPos);
	float attenuation	= 1.0 / (light.constant + light.linear * distance + light.quadratic * (distance * distance));

	// ambient
	vec3 ambient		= light.ambient * texture(material.diffuse, TexCoords).rgb;
	
	// diffuse 
	float diff			= max(dot(normal, lightDir), 0.0);
	vec3 diffuse		= light.diffuse * diff * texture(material.diffuse, TexCoords).rgb;  
    
	// specular
	vec3 reflectDir		= reflect(-lightDir, normal);  
	float spec			= pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);
	vec3 specular		= light.specular * spec * texture(material.specular, TexCoords).rgb;  
        
	ambient				*= attenuation; 
	diffuse				*= attenuation;
	specular			*= attenuation;
	diffuse				*= intensity;
	specular			*= intensity;
	return (ambient + diffuse + specular);
}