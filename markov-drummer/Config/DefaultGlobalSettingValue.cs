using System;

namespace markov_drummer.Config
{
    [AttributeUsage(AttributeTargets.All)]
    public class DefaultGlobalSettingValue : Attribute
    {
        /// <devdoc>
        ///     This is the default value.
        /// </devdoc>
        private readonly object _value;

        /// <summary>
        /// Just initialize with new instance with Activator.CreateInstance
        /// </summary>
        public bool CreateNew { get; set; }

        public virtual object Value
        {
            get
            {
                return _value;
            }
        }

        public DefaultGlobalSettingValue()
        {

        }

        /// <devdoc>
        /// <para>Initializes a new instance of the <see cref='System.ComponentModel.DefaultValueAttribute'/> class using a Unicode
        ///    character.</para>
        /// </devdoc>
        public DefaultGlobalSettingValue(char value)
        {
            this._value = value;
        }
        /// <devdoc>
        /// <para>Initializes a new instance of the <see cref='System.ComponentModel.DefaultValueAttribute'/> class using an 8-bit unsigned
        ///    integer.</para>
        /// </devdoc>
        public DefaultGlobalSettingValue(byte value)
        {
            this._value = value;
        }
        /// <devdoc>
        /// <para>Initializes a new instance of the <see cref='System.ComponentModel.DefaultValueAttribute'/> class using a 16-bit signed
        ///    integer.</para>
        /// </devdoc>
        public DefaultGlobalSettingValue(short value)
        {
            this._value = value;
        }
        /// <devdoc>
        /// <para>Initializes a new instance of the <see cref='System.ComponentModel.DefaultValueAttribute'/> class using a 32-bit signed
        ///    integer.</para>
        /// </devdoc>
        public DefaultGlobalSettingValue(int value)
        {
            this._value = value;
        }
        /// <devdoc>
        /// <para>Initializes a new instance of the <see cref='System.ComponentModel.DefaultValueAttribute'/> class using a 64-bit signed
        ///    integer.</para>
        /// </devdoc>
        public DefaultGlobalSettingValue(long value)
        {
            this._value = value;
        }
        /// <devdoc>
        /// <para>Initializes a new instance of the <see cref='System.ComponentModel.DefaultValueAttribute'/> class using a
        ///    single-precision floating point
        ///    number.</para>
        /// </devdoc>
        public DefaultGlobalSettingValue(float value)
        {
            this._value = value;
        }
        /// <devdoc>
        /// <para>Initializes a new instance of the <see cref='System.ComponentModel.DefaultValueAttribute'/> class using a
        ///    double-precision floating point
        ///    number.</para>
        /// </devdoc>
        public DefaultGlobalSettingValue(double value)
        {
            this._value = value;
        }
        /// <devdoc>
        /// <para>Initializes a new instance of the <see cref='System.ComponentModel.DefaultValueAttribute'/> class using a <see cref='System.Boolean'/>
        /// value.</para>
        /// </devdoc>
        public DefaultGlobalSettingValue(bool value)
        {
            this._value = value;
        }
        /// <devdoc>
        /// <para>Initializes a new instance of the <see cref='System.ComponentModel.DefaultValueAttribute'/> class using a <see cref='System.String'/>.</para>
        /// </devdoc>
        public DefaultGlobalSettingValue(string value)
        {
            this._value = value;
        }

        /// <devdoc>
        /// <para>Initializes a new instance of the <see cref='System.ComponentModel.DefaultValueAttribute'/>
        /// class.</para>
        /// </devdoc>
        public DefaultGlobalSettingValue(object value)
        {
            this._value = value;
        }

    }
}
