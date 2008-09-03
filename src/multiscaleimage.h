/* -*- Mode: C++; tab-width: 8; indent-tabs-mode: t; c-basic-offset: 8 -*- */
/*
 * multiscaleimage.h:
 *
 * Contact:
 *   Moonlight List (moonlight-list@lists.ximian.com)
 *
 * Copyright 2007 Novell, Inc. (http://www.novell.com)
 *
 * See the LICENSE file included with the distribution for details.
 */

#ifndef __MULTISCALEIMAGE_H__
#define __MULTISCALEIMAGE_H__

#include <glib.h>
#include <cairo.h>

#include <tilesource.h>
#include <eventargs.h>
#include <control.h>

G_BEGIN_DECLS


G_END_DECLS
/* @SilverlightVersion="2" */
/* @Namespace=System.Windows.Controls */
class MultiScaleImage : public FrameworkElement {

 protected:
	virtual ~MultiScaleImage ();

 public:
	/* @PropertyType=double,DefaultValue=1.0,Version=2.0,GenerateGetter */
	static DependencyProperty *AspectRatioProperty;
	/* @PropertyType=MultiScaleTileSource,Version=2.0,GenerateAccessors */
	static DependencyProperty *SourceProperty;
//	/* @PropertyType=ReadOnlyCollection<MultiScaleSubImage>,Version=2.0,GenerateGetter */
//	static DependencyProperty *SubImagesProperty;
	/* @PropertyType=bool,DefaultValue=true,Version=2.0,GenerateAccessors */
	static DependencyProperty *UseSpringsProperty;
	/* @PropertyType=Point,Version=2.0,GenerateAccessors */
	static DependencyProperty *ViewportOriginProperty;
	/* @PropertyType=double,Version=2.0,GenerateAccessors */
	static DependencyProperty *ViewportWidthProperty;

	/* @GenerateCBinding,GeneratePInvoke */
	MultiScaleImage ();

	virtual Type::Kind GetObjectType () { return Type::MULTISCALEIMAGE; }

	//
	// Overrides
	//
//	virtual void Render (cairo_t *cr, int x, int y, int width, int height);
//	virtual void GetSizeForBrush (cairo_t *cr, double *width, double *height);
//	virtual void OnPropertyChanged (PropertyChangedEventArgs *args);
//	virtual void OnSubPropertyChanged (DependencyProperty *prop, DependencyObject *obj, PropertyChangedEventArgs *subobj_args);
//	virtual Value *GetValue (DependencyProperty *property);
//	virtual Size ArrangeOverride (Size size);

	//
	// Methods
	//
	/* @GenerateCBinding,GeneratePInvoke */
	void ZoomAboutLogicalPoint (double zoomIncrementFactor, double zoomCenterLogicalX, double zoomCenterLogicalY);

	//
	// Property Accessors
	//
	double GetAspectRatio ();
	void SetAspectRatio (double ratio);

	MultiScaleTileSource* GetSource ();
	void SetSource (MultiScaleTileSource* source);

	bool GetUseSprings ();
	void SetUseSprings (bool spring);

	Point* GetViewportOrigin ();
	void SetViewportOrigin (Point* p);

	double GetViewportWidth ();
	void SetViewportWidth (double width);

	//
	// Events
	//
	const static int ImageFailedEvent;
	const static int ImageOpenFailedEvent;
	const static int ImageOpenSucceededEvent;
	const static int MotionFinishedEvent;
	const static int ViewportChangedEvent;
};

#endif /* __MULTISCALIMAGE_H__ */
