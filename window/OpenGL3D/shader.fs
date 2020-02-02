#version 330 core
out vec4 FragColor;

in vec2  TexCoord;

uniform sampler2D texture1;
uniform sampler2D texture2;
uniform float mixValue;
void main()
{
	if(gl_FragCoord.x<400)
	FragColor = mix(texture(texture1, TexCoord), texture(texture2, TexCoord), mixValue);
	else
	FragColor = vec4(1.0f,1.0f,0.0f,1.0f);
	//FragColor = mix(texture(texture1, TexCoord), texture(texture2, vec2(1.0 - TexCoord.x, TexCoord.y)), 0.2);
}