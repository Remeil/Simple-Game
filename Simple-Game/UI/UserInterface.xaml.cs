// -----------------------------------------------------------
//  
//  This file was generated, please do not modify.
//  
// -----------------------------------------------------------
namespace EmptyKeys.UserInterface.Generated {
    using System;
    using System.CodeDom.Compiler;
    using System.Collections.ObjectModel;
    using EmptyKeys.UserInterface;
    using EmptyKeys.UserInterface.Data;
    using EmptyKeys.UserInterface.Controls;
    using EmptyKeys.UserInterface.Controls.Primitives;
    using EmptyKeys.UserInterface.Input;
    using EmptyKeys.UserInterface.Media;
    using EmptyKeys.UserInterface.Media.Animation;
    using EmptyKeys.UserInterface.Media.Imaging;
    using EmptyKeys.UserInterface.Shapes;
    using EmptyKeys.UserInterface.Renderers;
    using EmptyKeys.UserInterface.Themes;
    
    
    [GeneratedCodeAttribute("Empty Keys UI Generator", "1.7.5.0")]
    public partial class UserInterface : UIRoot {
        
        private Grid e_0;
        
        private TextBlock e_1;
        
        private TextBlock CurrentHp;
        
        public UserInterface(int width, int height) : 
                base(width, height) {
            Style style = RootStyle.CreateRootStyle();
            style.TargetType = this.GetType();
            this.Style = style;
            this.InitializeComponent();
        }
        
        private void InitializeComponent() {
            // e_0 element
            this.e_0 = new Grid();
            this.Content = this.e_0;
            this.e_0.Name = "e_0";
            // e_1 element
            this.e_1 = new TextBlock();
            this.e_0.Children.Add(this.e_1);
            this.e_1.Name = "e_1";
            this.e_1.Margin = new Thickness(0F, 0F, 238F, 277F);
            this.e_1.Text = "Current HP:";
            // CurrentHp element
            this.CurrentHp = new TextBlock();
            this.e_0.Children.Add(this.CurrentHp);
            this.CurrentHp.Name = "CurrentHp";
            this.CurrentHp.Margin = new Thickness(67F, 0F, 206F, 277F);
            Binding binding_CurrentHp_Text = new Binding("CurrentHp");
            binding_CurrentHp_Text.Mode = BindingMode.OneWay;
            this.CurrentHp.SetBinding(TextBlock.TextProperty, binding_CurrentHp_Text);
            FontManager.Instance.AddFont("Segoe UI", 12F, FontStyle.Regular, "Segoe_UI_9_Regular");
        }
    }
}
