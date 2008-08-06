/* -*- Mode: C++; tab-width: 8; indent-tabs-mode: t; c-basic-offset: 8 -*- */
/*
 * frameworkelement.h
 *
 * Copyright 2007 Novell, Inc. (http://www.novell.com)
 *
 * See the LICENSE file included with the distribution for details.
 * 
 */

#ifndef __MOON_FRAMEWORKELEMENT_H__
#define __MOON_FRAMEWORKELEMENT_H__

#include <glib.h>

#include "uielement.h"

class FrameworkElement : public UIElement {
 protected:
	virtual ~FrameworkElement () {}

 public:
	static DependencyProperty *HeightProperty;
	static DependencyProperty *WidthProperty;

	// DPs moved here from UIElement in 2.0
	static DependencyProperty *CursorProperty;
	static DependencyProperty *ResourcesProperty;
	static DependencyProperty *TagProperty;
	static DependencyProperty *TriggersProperty;

	// 2.0 only DPs
	static DependencyProperty *ActualHeightProperty;
	static DependencyProperty *ActualWidthProperty;
	static DependencyProperty *HorizontalAlignmentProperty;
	static DependencyProperty *LanguageProperty;
	static DependencyProperty *MarginProperty;
	static DependencyProperty *MaxHeightProperty;
	static DependencyProperty *MaxWidthProperty;
	static DependencyProperty *MinHeightProperty;
	static DependencyProperty *MinWidthProperty;
	static DependencyProperty *VerticalAlignmentProperty;

	FrameworkElement ();
	virtual Type::Kind GetObjectType () { return Type::FRAMEWORKELEMENT; }

	virtual void ComputeBounds ();

	virtual void OnPropertyChanged (PropertyChangedEventArgs *args);

	virtual bool InsideObject (cairo_t *cr, double x, double y);

	virtual void GetSizeForBrush (cairo_t *cr, double *width, double *height);
	
	//
	// Property Accessors
	//
	void SetHeight (double height);
	double GetHeight ();
	
	void SetWidth (double width);
	double GetWidth ();
	
	//
	// 2.0 methods
	//
	
	virtual Size MeasureOverride (Size availableSize)
	{
		return Size (0, 0);
	}
	
	// overrides uielement::MeasureCore
	virtual Size MeasureCore (Size availableSize)
	{
		//
		// We proxy from UIElement.MeasureCore to FrameworkElement.MeasureOverride
		//
		// WPF docs do not shed much light into why there is a difference
		//
		return MeasureOverride (availableSize);
	}
};

G_BEGIN_DECLS

FrameworkElement *framework_element_new (void);

double	framework_element_get_height	(FrameworkElement *framework_element);
void	framework_element_set_height	(FrameworkElement *framework_element, double height);
double	framework_element_get_width	(FrameworkElement *framework_element);
void	framework_element_set_width	(FrameworkElement *framework_element, double width);

void	framework_element_init (void);

G_END_DECLS


#endif /* __MOON_FRAMEWORKELEMENT_H__ */
