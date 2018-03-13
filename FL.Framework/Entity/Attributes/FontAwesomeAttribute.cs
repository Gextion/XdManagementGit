using System;

namespace FL.Framework.Entity.Attributes
{
    /// <summary>
    /// Font Awesome Icon Name
    /// Determina cual icono de la libreria FontAwesome debe ser incluido en el campo del modelo
    /// 
    /// Referencia de Iconos: 
    /// http://fontawesome.io/icons/
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class FontAwesomeAttribute : Attribute
    {
        public string FontAwesomeValue { get; set; }

        public FontAwesomeAttribute(string Value)
        {
            this.FontAwesomeValue = Value;
        }
    }
}
