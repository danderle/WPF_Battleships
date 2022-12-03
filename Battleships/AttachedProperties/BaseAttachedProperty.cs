using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Battleships
{
    public abstract class BaseAttachedProperty<Parent, Property>
        where Parent : new()
    {
        #region Actions

        public event Action<DependencyObject, object> ValueUpdated = (sender, e) => { };

        public event Action<DependencyObject, DependencyPropertyChangedEventArgs> ValueChanged = (sender, e) => { };

        #endregion

        #region Singleton

        /// <summary>
        /// Single instance of the parent class
        /// </summary>
        public static Parent Instance { get; private set; } = new Parent();

        #endregion

        #region Dependency Property

        public static readonly DependencyProperty ValueProperty = DependencyProperty.RegisterAttached(
            "Value",
            typeof(Property),
            typeof(BaseAttachedProperty<Parent, Property>),
            new UIPropertyMetadata(default(Property),
            new PropertyChangedCallback(OnValuePropertyChanged),
            new CoerceValueCallback(OnValuePropertyUpdated)));

        /// <summary>
        /// Gets the attached property
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static Property GetValue(DependencyObject d) => (Property)d.GetValue(ValueProperty);

        /// <summary>
        /// Sets the attached property
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static void SetValue(DependencyObject d, Property value) => d.SetValue(ValueProperty, value);

        #endregion

        /// <summary>
        /// the callback method which is called when the property is updated
        /// </summary>
        /// <param name="d"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private static object OnValuePropertyUpdated(DependencyObject d, object value)
        {
            (Instance as BaseAttachedProperty<Parent, Property>)?.OnValueUpdated(d, value);

            (Instance as BaseAttachedProperty<Parent, Property>)?.ValueUpdated(d, value);

            return value;
        }

        /// <summary>
        /// the callback method which is called when the property is changed
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (Instance as BaseAttachedProperty<Parent, Property>)?.OnValueChanged(d, e);

            (Instance as BaseAttachedProperty<Parent, Property>)?.ValueChanged(d, e);
        }

        #region Event methods

        public virtual void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) { }
        public virtual void OnValueUpdated(DependencyObject d, object value) { }

        #endregion
    }
}
