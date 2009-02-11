/* -*- Mode: C++; tab-width: 8; indent-tabs-mode: t; c-basic-offset: 8 -*- */
/*
 * border.h:
 *
 * Copyright 2007 Novell, Inc. (http://www.novell.com)
 *
 * See the LICENSE file included with the distribution for details.
 * 
 */

#ifndef __MOON_BORDER_H__
#define __MOON_BORDER_H__

#include <glib.h>

#include "frameworkelement.h"
#include "cornerradius.h"

//
// Border
//
/* @SilverlightVersion="2" */
/* @ContentProperty="Child" */
/* @Namespace=System.Windows.Controls */
class Border : public FrameworkElement {
protected:
	virtual ~Border () { };
	
public:
	/* @PropertyType=Brush,GenerateAccessors */
	static int BackgroundProperty;
	/* @PropertyType=Brush,GenerateAccessors */
	static int BorderBrushProperty;
	/* @PropertyType=Thickness,DefaultValue=Thickness(0),GenerateAccessors,Validator=BorderThicknessValidator */
	static int BorderThicknessProperty;
	/* @PropertyType=UIElement,GenerateAccessors,ManagedFieldAccess=Internal */
	static int ChildProperty;
	/* @PropertyType=CornerRadius,GenerateAccessors,Validator=CornerRadiusValidator */
	static int CornerRadiusProperty;
	/* @PropertyType=Thickness,DefaultValue=Thickness(0),GenerateAccessors,Validator=BorderThicknessValidator */
	static int PaddingProperty;
	
 	/* @GenerateCBinding,GeneratePInvoke */
	Border ();
	
	virtual bool IsLayoutContainer () { return true; }
	virtual void OnPropertyChanged (PropertyChangedEventArgs *args);

	virtual Size MeasureOverride (Size availableSize);
	virtual Size ArrangeOverride (Size finalSize);

	virtual void Render (cairo_t *cr, Region *region);

	virtual bool InsideObject (cairo_t *cr, double x, double y);
	virtual bool InsideFillOrClip (cairo_t *cr, double x, double y);
	virtual void HitTest (cairo_t *cr, Rect r, List *uielement_list);
	virtual bool CanFindElement () { return GetBackground () != NULL; }
	// property accessors

	Brush *GetBackground ();
	void SetBackground (Brush *value);

	Brush *GetBorderBrush ();
	void SetBorderBrush (Brush *value);

	Thickness *GetBorderThickness ();
	void SetBorderThickness (Thickness *value);

	UIElement *GetChild ();
	void SetChild (UIElement *value);

	CornerRadius *GetCornerRadius ();
	void SetCornerRadius (CornerRadius *value);

	Thickness *GetPadding ();
	void SetPadding (Thickness *value);
	
};

#endif /* __MOON_BORDER_H__ */
