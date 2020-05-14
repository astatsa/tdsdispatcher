using System.Windows;
using System.Windows.Interactivity;
using TDSDispatcher.Services;

namespace TDSDispatcher.Behaviors
{
    class PermissionBehavior : Behavior<UIElement>
    {
        public static DependencyProperty PermissionServiceProperty = DependencyProperty.Register(nameof(PermissionService), typeof(PermissionService),
            typeof(PermissionBehavior), new PropertyMetadata(
                (d, e) => ((PermissionBehavior)d).SetVisibility()));
        public static DependencyProperty OperationProperty = DependencyProperty.Register(nameof(Operation), typeof(EntityOperations), 
            typeof(PermissionBehavior));

        public PermissionService PermissionService
        {
            get => (PermissionService)GetValue(PermissionServiceProperty);
            set => SetValue(PermissionServiceProperty, value);
        }

        public EntityOperations Operation
        {
            get => (EntityOperations)GetValue(OperationProperty);
            set => SetValue(OperationProperty, value);
        }

        protected override void OnAttached()
        {
            SetVisibility();
            base.OnAttached();
        }

        private void SetVisibility()
        {
            if (PermissionService != null)
                AssociatedObject.Visibility = PermissionService.HasPermission(Operation) ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
