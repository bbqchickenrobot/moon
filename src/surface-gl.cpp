/* -*- Mode: C++; tab-width: 8; indent-tabs-mode: t; c-basic-offset: 8 -*- */
/*
 * surface-gl.cpp
 *
 * Copyright 2010 Novell, Inc. (http://www.novell.com)
 *
 * See the LICENSE file included with the distribution for details.
 *
 */

#include <config.h>

#include "surface-gl.h"

namespace Moonlight {

GLSurface::GLSurface ()
{
	size[0]       = 0;
	size[1]       = 0;
	texture       = 0;
	textureYUV[0] = 0;
	textureYUV[1] = 0;
	textureYUV[2] = 0;
	data          = NULL;
}

GLSurface::GLSurface (GLsizei width, GLsizei height)
{
	size[0]       = width;
	size[1]       = height;
	texture       = 0;
	textureYUV[0] = 0;
	textureYUV[1] = 0;
	textureYUV[2] = 0;
	data          = NULL;
}

GLSurface::~GLSurface ()
{
	if (data)
		g_free (data);

	if (texture)
		glDeleteTextures (1, &texture);

	if (textureYUV[0])
		glDeleteTextures (3, textureYUV);
}

cairo_surface_t *
GLSurface::Cairo ()
{
	int stride = size[0] * 4;

	if (!data) {
		data = (unsigned char *) g_malloc0 (stride * size[1]);

		// derived class should implement read back of texture image
		g_assert (texture == 0 && !IsPlanar ());
	}

	return cairo_image_surface_create_for_data (data,
						    CAIRO_FORMAT_ARGB32,
						    size[0],
						    size[1],
						    stride);
}

GLuint
GLSurface::Texture ()
{
	GLuint name = texture;

	if (!texture)
		glGenTextures (1, &texture);

	if (name != texture || data) {
		glBindTexture (GL_TEXTURE_2D, texture);
		glTexImage2D (GL_TEXTURE_2D,
			      0,
			      GL_RGBA,
			      size[0],
			      size[1],
			      0,
			      GL_BGRA,
			      GL_UNSIGNED_BYTE,
			      data);
		glBindTexture (GL_TEXTURE_2D, 0);
	}

	if (data) {
		g_free (data);
		data = NULL;
	}

	return texture;
}

bool
GLSurface::IsPlanar ()
{
	return (textureYUV[0] && textureYUV[1] && textureYUV[2]);
}

GLint
GLSurface::TextureYUV (int i)
{
	if (!textureYUV[i]) {
		int width[] = { size[0], size[0] / 2, size[0] / 2 };
		int height[] = { size[1], size[1] / 2, size[1] / 2 };
		int i;

		glGenTextures (3, textureYUV);

		for (i = 0; i < 3; i++) {
			GLfloat border[][4] = {
				{ 0.0625f, 0.0625f, 0.0625f, 0.0625f },
				{   0.5f ,    0.5f,    0.5f,    0.5f },
				{   0.5f ,    0.5f,    0.5f,    0.5f }
			};

			glBindTexture (GL_TEXTURE_2D, textureYUV[i]);
			glTexImage2D (GL_TEXTURE_2D,
				      0,
				      GL_LUMINANCE,
				      width[i],
				      height[i],
				      0,
				      GL_LUMINANCE,
				      GL_UNSIGNED_BYTE,
				      NULL);
			glTexParameterfv (GL_TEXTURE_2D,
					  GL_TEXTURE_BORDER_COLOR,
					  border[i]);
		}
		glBindTexture (GL_TEXTURE_2D, 0);
	}

	return textureYUV[i];
}

GLsizei
GLSurface::Width ()
{
	return size[0];
}

GLsizei
GLSurface::Height ()
{
	return size[1];
}

};
