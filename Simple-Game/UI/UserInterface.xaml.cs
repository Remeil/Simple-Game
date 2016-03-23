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
        
        private TextBlock e_2;
        
        private TextBlock MaxHp;
        
        private TextBlock e_3;
        
        private TextBlock CurrentMp;
        
        private TextBlock e_4;
        
        private TextBlock MaxMp;
        
        private Border e_5;
        
        private Border e_6;
        
        private TextBlock SystemMessages;
        
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
            this.e_1.Margin = new Thickness(0F, 525F, 777F, 0F);
            this.e_1.Text = "HP:";
            // CurrentHp element
            this.CurrentHp = new TextBlock();
            this.e_0.Children.Add(this.CurrentHp);
            this.CurrentHp.Name = "CurrentHp";
            this.CurrentHp.Margin = new Thickness(28F, 524F, 736F, 0F);
            this.CurrentHp.TextAlignment = TextAlignment.Right;
            Binding binding_CurrentHp_Text = new Binding("CurrentHp");
            binding_CurrentHp_Text.Mode = BindingMode.OneWay;
            this.CurrentHp.SetBinding(TextBlock.TextProperty, binding_CurrentHp_Text);
            // e_2 element
            this.e_2 = new TextBlock();
            this.e_0.Children.Add(this.e_2);
            this.e_2.Name = "e_2";
            this.e_2.Margin = new Thickness(69F, 524F, 717F, 0F);
            this.e_2.Text = "/";
            this.e_2.TextAlignment = TextAlignment.Center;
            // MaxHp element
            this.MaxHp = new TextBlock();
            this.e_0.Children.Add(this.MaxHp);
            this.MaxHp.Name = "MaxHp";
            this.MaxHp.Margin = new Thickness(88F, 524F, 676F, 0F);
            this.MaxHp.TextAlignment = TextAlignment.Right;
            Binding binding_MaxHp_Text = new Binding("MaxHp");
            binding_MaxHp_Text.Mode = BindingMode.OneWay;
            this.MaxHp.SetBinding(TextBlock.TextProperty, binding_MaxHp_Text);
            // e_3 element
            this.e_3 = new TextBlock();
            this.e_0.Children.Add(this.e_3);
            this.e_3.Name = "e_3";
            this.e_3.Margin = new Thickness(129F, 524F, 648F, 0F);
            this.e_3.Text = "MP:";
            // CurrentMp element
            this.CurrentMp = new TextBlock();
            this.e_0.Children.Add(this.CurrentMp);
            this.CurrentMp.Name = "CurrentMp";
            this.CurrentMp.Margin = new Thickness(157F, 524F, 607F, 0F);
            this.CurrentMp.TextAlignment = TextAlignment.Right;
            Binding binding_CurrentMp_Text = new Binding("CurrentMp");
            binding_CurrentMp_Text.Mode = BindingMode.OneWay;
            this.CurrentMp.SetBinding(TextBlock.TextProperty, binding_CurrentMp_Text);
            // e_4 element
            this.e_4 = new TextBlock();
            this.e_0.Children.Add(this.e_4);
            this.e_4.Name = "e_4";
            this.e_4.Margin = new Thickness(198F, 524F, 588F, 0F);
            this.e_4.Text = "/";
            this.e_4.TextAlignment = TextAlignment.Center;
            // MaxMp element
            this.MaxMp = new TextBlock();
            this.e_0.Children.Add(this.MaxMp);
            this.MaxMp.Name = "MaxMp";
            this.MaxMp.Margin = new Thickness(217F, 524F, 547F, 0F);
            this.MaxMp.TextAlignment = TextAlignment.Right;
            Binding binding_MaxMp_Text = new Binding("MaxMp");
            binding_MaxMp_Text.Mode = BindingMode.OneWay;
            this.MaxMp.SetBinding(TextBlock.TextProperty, binding_MaxMp_Text);
            // e_5 element
            this.e_5 = new Border();
            this.e_0.Children.Add(this.e_5);
            this.e_5.Name = "e_5";
            this.e_5.Margin = new Thickness(16F, 56F, 16F, 16F);
            this.e_5.BorderBrush = new SolidColorBrush(new ColorW(211, 211, 211, 255));
            this.e_5.BorderThickness = new Thickness(1F, 1F, 1F, 1F);
            // e_6 element
            this.e_6 = new Border();
            this.e_0.Children.Add(this.e_6);
            this.e_6.Name = "e_6";
            this.e_6.Height = 41F;
            this.e_6.Width = 768F;
            this.e_6.Margin = new Thickness(16F, 10F, 0F, 0F);
            this.e_6.HorizontalAlignment = HorizontalAlignment.Left;
            this.e_6.VerticalAlignment = VerticalAlignment.Top;
            this.e_6.BorderBrush = new SolidColorBrush(new ColorW(211, 211, 211, 255));
            this.e_6.BorderThickness = new Thickness(1F, 1F, 1F, 1F);
            // SystemMessages element
            this.SystemMessages = new TextBlock();
            this.e_6.Child = this.SystemMessages;
            this.SystemMessages.Name = "SystemMessages";
            this.SystemMessages.TextWrapping = TextWrapping.Wrap;
            Binding binding_SystemMessages_Text = new Binding("SystemMessages");
            binding_SystemMessages_Text.Mode = BindingMode.OneWay;
            this.SystemMessages.SetBinding(TextBlock.TextProperty, binding_SystemMessages_Text);
            FontManager.Instance.AddFont("Segoe UI", 12F, FontStyle.Regular, "Segoe_UI_9_Regular");
        }
    }
}
