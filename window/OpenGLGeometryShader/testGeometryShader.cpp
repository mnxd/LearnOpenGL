#include <glad/glad.h>
#include <GLFW/glfw3.h>

#include <shader.h>
#include <iostream>



void framebuffer_size_callback(GLFWwindow* window, int width, int height);

const unsigned int SCR_WIDTH = 1280;
const unsigned int SCR_HEIGHT = 720;


int main()
{
	glfwInit();
	glfwWindowHint(GLFW_CONTEXT_VERSION_MAJOR, 3);
	glfwWindowHint(GLFW_CONTEXT_VERSION_MINOR, 3);
	glfwWindowHint(GLFW_OPENGL_PROFILE, GLFW_OPENGL_CORE_PROFILE);

	GLFWwindow *window = glfwCreateWindow(SCR_WIDTH, SCR_HEIGHT, "Test Rectangle", NULL, NULL);

	if (window == NULL)
	{
		std::cout << "failed to init glfw!" << std::endl;
		return -1;
	}
	glfwMakeContextCurrent(window);

	if (!gladLoadGLLoader((GLADloadproc)glfwGetProcAddress))
	{
		std::cout << "failed to init glad!" << std::endl;
		return -1;
	}


	glEnable(GL_DEPTH_TEST);
	Shader shader("geometryShader.vs", "geometryShader.fs", "geometryShader.gs");

	float points[] = {
		-0.5f,  0.5f, 1.0f, 0.0f, 0.0f, // ����
		 0.5f,  0.5f, 0.0f, 1.0f, 0.0f, // ����
		 0.5f, -0.5f, 0.0f, 0.0f, 1.0f, // ����
		-0.5f, -0.5f, 1.0f, 1.0f, 0.0f  // ����
	};

	unsigned int VBO, VAO;
	glGenBuffers(1, &VBO);
	glGenVertexArrays(1, &VAO);
	glBindVertexArray(VAO);
	glBindBuffer(GL_ARRAY_BUFFER, VBO);
	glBufferData(GL_ARRAY_BUFFER, sizeof(points), &points, GL_STATIC_DRAW);
	glEnableVertexAttribArray(0);
	glVertexAttribPointer(0, 2, GL_FLOAT, GL_FALSE, 5 * sizeof(float), 0);
	glEnableVertexAttribArray(1);
	glVertexAttribPointer(1, 3, GL_FLOAT, GL_FALSE, 5 * sizeof(float), (void*)(2 * sizeof(float)));
	glBindVertexArray(0);


	while (!glfwWindowShouldClose(window))
	{

		glClearColor(0.1f, 0.1f, 0.1f, 1.0f);
		glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);

		shader.use();
		glBindVertexArray(VAO);
		glDrawArrays(GL_POINTS, 0, 4);

		glfwSwapBuffers(window);
		glfwPollEvents();
	}

	glDeleteVertexArrays(1, &VAO);
	glDeleteBuffers(1, &VBO);

	glfwTerminate();

	return 0;

}

void framebuffer_size_callback(GLFWwindow* window, int width, int height)
{
	glViewport(0, 0, width, height);
}
