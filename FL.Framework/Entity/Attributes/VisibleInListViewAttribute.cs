using System;

/// <summary>
/// Visible In List View Attribute
/// Determina si se muestra una propiedad del modelo en la vista tipo List
/// </summary>
namespace FL.Framework.Entity.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class VisibleInListViewAttribute : Attribute
    {
        /// <summary>
        /// Store Attribute Value
        /// </summary>
        public bool Visible { get; set; }

        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <param name="IsVisible"></param>
        public VisibleInListViewAttribute(bool IsVisible)
        {
            this.Visible = IsVisible;
        }
    }
}
