/*
 * stylus.h
 *
 * Author:
 *   Rolf Bjarne Kvinge  (RKvinge@novell.com)
 *   Sebastien Pouliot  <sebastien@ximian.com>
 *
 * Copyright 2007 Novell, Inc. (http://www.novell.com)
 *
 * See the LICENSE file included with the distribution for details.
 * 
 */

#ifndef MOON_STYLUS_H
#define MOON_STYLUS_H

#include "canvas.h"

G_BEGIN_DECLS

typedef enum {
	TabletDeviceTypeMouse,
	TabletDeviceTypeStylus,
	TabletDeviceTypeTouch
} TabletDeviceType;


class StylusInfo : public DependencyObject {
 public:
	StylusInfo () { }
	virtual ~StylusInfo () { };
	
	virtual Type::Kind GetObjectType () { return Type::STYLUSINFO; };

	static DependencyProperty* DeviceTypeProperty;
	static DependencyProperty* IsInvertedProperty;
};

StylusInfo* stylus_info_new ();
TabletDeviceType stylus_info_get_device_type (StylusInfo* stylus_info);
void	stylus_info_set_device_type	(StylusInfo* stylus_info, TabletDeviceType type);
bool	stylus_info_get_inverted	(StylusInfo* stylus_info);
void	stylus_info_set_inverted	(StylusInfo* stylus_info, bool inverted);


class StylusPoint : public DependencyObject {
 public:
	StylusPoint () { }
	virtual ~StylusPoint () { };
	
	virtual Type::Kind GetObjectType () { return Type::STYLUSPOINT; };

	static DependencyProperty* PressureFactorProperty;
	static DependencyProperty* XProperty;
	static DependencyProperty* YProperty;
};

StylusPoint* stylus_point_new ();
double	stylus_point_get_x (StylusPoint *stylus_point);
void	stylus_point_set_x (StylusPoint *stylus_point, double x);
double	stylus_point_get_y (StylusPoint *stylus_point);
void	stylus_point_set_y (StylusPoint *stylus_point, double y);
double	stylus_point_get_pressure_factor (StylusPoint *stylus_point);
void	stylus_point_set_pressure_factor (StylusPoint *stylus_point, double pressure);

class DrawingAttributes : public DependencyObject {
 public:
	DrawingAttributes () { }
	virtual ~DrawingAttributes () { };
	
	virtual Type::Kind GetObjectType () { return Type::DRAWINGATTRIBUTES; };

	void Render (cairo_t *cr, StylusPointCollection* collection);
	static void RenderWithoutDrawingAttributes (cairo_t *cr, StylusPointCollection* collection);

	static DependencyProperty* ColorProperty;
	static DependencyProperty* OutlineColorProperty;
	static DependencyProperty* HeightProperty;
	static DependencyProperty* WidthProperty;
};

DrawingAttributes* drawing_attributes_new ();
Color*	drawing_attributes_get_color (DrawingAttributes* da);
void	drawing_attributes_set_color (DrawingAttributes* da, Color *color);
Color*	drawing_attributes_get_outline_color (DrawingAttributes* da);
void	drawing_attributes_set_outline_color (DrawingAttributes* da, Color *color);
double	drawing_attributes_get_height (DrawingAttributes* da);
void	drawing_attributes_set_height (DrawingAttributes* da, double height);
double	drawing_attributes_get_width (DrawingAttributes* da);
void	drawing_attributes_set_width (DrawingAttributes* da, double width);

class Stroke : public DependencyObject {
 public:
	Stroke () { }
	virtual ~Stroke () { };
	
	virtual Type::Kind GetObjectType () { return Type::STROKE; };

	static DependencyProperty* DrawingAttributesProperty;
	static DependencyProperty* StylusPointsProperty;
};

Stroke* stroke_new ();
DrawingAttributes* stroke_get_drawing_attributes (Stroke *stroke);
void stroke_set_drawing_attributes (Stroke *stroke, DrawingAttributes *attributes);
StylusPointCollection* stroke_get_stylus_points (Stroke *stroke);
void stroke_set_stylus_points (Stroke *stroke, StylusPointCollection* collection);


class InkPresenter : public Canvas {
 public:
	InkPresenter () { }
	virtual ~InkPresenter () { };
	
	virtual Type::Kind GetObjectType () { return Type::INKPRESENTER; };

	virtual void RenderChildren (cairo_t *cr, int x, int y, int width, int height);
	virtual bool OnChildPropertyChanged (DependencyProperty *prop, DependencyObject *child);

	static DependencyProperty* StrokesProperty;
};

InkPresenter* ink_presenter_new ();
StrokeCollection* ink_presenter_get_strokes (InkPresenter *ink_presenter);
void ink_presenter_set_strokes (InkPresenter *ink_presenter, StrokeCollection* collection);


void stylus_init (void);

G_END_DECLS

#endif /* MOON_STYLUS_H */
