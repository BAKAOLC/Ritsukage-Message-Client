﻿#pragma checksum "..\..\..\lib\MessageItem.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "EDF373748783D571CABA1463C95B080A495490DEFC55E53A51C0995E2C54C885"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

using Ritsukage_Message_Client.lib;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Integration;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace Ritsukage_Message_Client.lib {
    
    
    /// <summary>
    /// MessageItem
    /// </summary>
    public partial class MessageItem : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 10 "..\..\..\lib\MessageItem.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid MessageBody;
        
        #line default
        #line hidden
        
        
        #line 13 "..\..\..\lib\MessageItem.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label ExitLabel;
        
        #line default
        #line hidden
        
        
        #line 14 "..\..\..\lib\MessageItem.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button ExitButton;
        
        #line default
        #line hidden
        
        
        #line 17 "..\..\..\lib\MessageItem.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label Title;
        
        #line default
        #line hidden
        
        
        #line 21 "..\..\..\lib\MessageItem.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock Content;
        
        #line default
        #line hidden
        
        
        #line 23 "..\..\..\lib\MessageItem.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image Cover;
        
        #line default
        #line hidden
        
        
        #line 25 "..\..\..\lib\MessageItem.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid ExtendInfoGrid;
        
        #line default
        #line hidden
        
        
        #line 26 "..\..\..\lib\MessageItem.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label ExtendInfo;
        
        #line default
        #line hidden
        
        
        #line 27 "..\..\..\lib\MessageItem.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button ExtendInfoButton;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/Ritsukage Message Client;component/lib/messageitem.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\lib\MessageItem.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 8 "..\..\..\lib\MessageItem.xaml"
            ((Ritsukage_Message_Client.lib.MessageItem)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Enter);
            
            #line default
            #line hidden
            return;
            case 2:
            this.MessageBody = ((System.Windows.Controls.Grid)(target));
            return;
            case 3:
            this.ExitLabel = ((System.Windows.Controls.Label)(target));
            return;
            case 4:
            this.ExitButton = ((System.Windows.Controls.Button)(target));
            
            #line 14 "..\..\..\lib\MessageItem.xaml"
            this.ExitButton.Click += new System.Windows.RoutedEventHandler(this.Button_ExitButton_Click);
            
            #line default
            #line hidden
            
            #line 14 "..\..\..\lib\MessageItem.xaml"
            this.ExitButton.MouseEnter += new System.Windows.Input.MouseEventHandler(this.Label_ExitLabel_Hold);
            
            #line default
            #line hidden
            
            #line 14 "..\..\..\lib\MessageItem.xaml"
            this.ExitButton.MouseLeave += new System.Windows.Input.MouseEventHandler(this.Label_ExitLabel_Leave);
            
            #line default
            #line hidden
            return;
            case 5:
            this.Title = ((System.Windows.Controls.Label)(target));
            return;
            case 6:
            this.Content = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 7:
            this.Cover = ((System.Windows.Controls.Image)(target));
            return;
            case 8:
            this.ExtendInfoGrid = ((System.Windows.Controls.Grid)(target));
            return;
            case 9:
            this.ExtendInfo = ((System.Windows.Controls.Label)(target));
            return;
            case 10:
            this.ExtendInfoButton = ((System.Windows.Controls.Button)(target));
            
            #line 27 "..\..\..\lib\MessageItem.xaml"
            this.ExtendInfoButton.Click += new System.Windows.RoutedEventHandler(this.Button_ExtendInfoButton_Click);
            
            #line default
            #line hidden
            
            #line 27 "..\..\..\lib\MessageItem.xaml"
            this.ExtendInfoButton.MouseEnter += new System.Windows.Input.MouseEventHandler(this.Label_ExtendInfo_Hold);
            
            #line default
            #line hidden
            
            #line 27 "..\..\..\lib\MessageItem.xaml"
            this.ExtendInfoButton.MouseLeave += new System.Windows.Input.MouseEventHandler(this.Label_ExtendInfo_Leave);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

