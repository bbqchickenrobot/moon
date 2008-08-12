/*
 * style.h:
 *
 * Copyright 2008 Novell, Inc. (http://www.novell.com)
 *
 * See the LICENSE file included with the distribution for details.
 * 
 */

#ifndef __MOON_STYLE_H__
#define __MOON_STYLE_H__

#include <glib.h>

#include "dependencyobject.h"
#include "collection.h"

//
// Style
//
/* @SilverlightVersion="2" */
/* @ContentProperty="Setters" */
/* @Namespace=System.Windows */
class Style : public DependencyObject
{
protected:
	virtual ~Style () {}

public:
	/* @GenerateCBinding */
	Style ();

	virtual Type::Kind GetObjectType () { return Type::STYLE; }

 	/* @PropertyType=SetterBaseCollection,Access=Internal,ManagedFieldAccess=Private,ManagedAccess=Public */
	static DependencyProperty *SettersProperty;

 	/* @PropertyType=Managed,ManagedPropertyType=System.Type,Access=Internal,ManagedAccess=Public */
	static DependencyProperty *TargetTypeProperty;
};

//
// SetterBaseCollection
//
/* @SilverlightVersion="2" */
/* @Namespace=System.Windows */
class SetterBaseCollection : public DependencyObjectCollection {
 protected:
	virtual ~SetterBaseCollection () {};
	
 public:
	/* @GenerateCBinding */
	SetterBaseCollection ();
	
	virtual Type::Kind GetObjectType () { return Type::SETTERBASE_COLLECTION; }
	virtual Type::Kind GetElementType () { return Type::SETTERBASE; }
};

//
// SetterBase
//
/* @SilverlightVersion="2" */
/* @Namespace=System.Windows */
class SetterBase : public DependencyObject
{
protected:
	virtual ~SetterBase () {}

public:
	/* @GenerateCBinding */
	SetterBase ();

	virtual Type::Kind GetObjectType () { return Type::SETTERBASE; }
};

//
// Setter
//
/* @SilverlightVersion="2" */
/* @Namespace=System.Windows */
/* @ManagedDependencyProperties=Manual */
class Setter : public SetterBase
{
protected:
	virtual ~Setter () {}

public:
	/* @GenerateCBinding */
	Setter ();

	virtual Type::Kind GetObjectType () { return Type::SETTER; }

 	/* @PropertyType=string */
	static DependencyProperty *PropertyProperty;
 	/* @PropertyType=DependencyProperty */
	static DependencyProperty *DependencyPropertyProperty;
 	/* @PropertyType=Managed */
	static DependencyProperty *ValueProperty;
};

G_BEGIN_DECLS

G_END_DECLS

#endif // __MOON_TEMPLATE_H__
