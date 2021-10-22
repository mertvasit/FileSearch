using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Massive
{
    //Written by Yaniv and shahar helped
    // כמאפיין זיהוי FieldID / TableID משמש לסימון 
    // Massive נמצאת במחלקת Attribute-הבדיקה האם למאפיין יש את ה
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class IdentifierAttribute : Attribute
    {
        public IdentifierAttribute() // אם לתכונה יש את המזהה הזה, סימן שאין להכניס למסד הנתונים מידע לגביה והיא תוכנס באופן אוטומטי על ידי המסד
        {
        }
    }
}