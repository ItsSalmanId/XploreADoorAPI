﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.42000.
// 
#pragma warning disable 1591

namespace FOX.ExternalServices.TelenorSmsService {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2556.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="Service1Soap", Namespace="http://tempuri.org/")]
    public partial class Service1 : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback SendTelenorSMSOperationCompleted;
        
        private System.Threading.SendOrPostCallback SendBulkSMS_GEPOperationCompleted;
        
        private System.Threading.SendOrPostCallback _SendBulkSMS_NetworkOperationCompleted;
        
        private System.Threading.SendOrPostCallback SendTelenorSMS_UrduOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public Service1() {
            this.Url = global::FOX.ExternalServices.Properties.Settings.Default.FOX_ExternalServices_TelenorSmsService_Service1;
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event SendTelenorSMSCompletedEventHandler SendTelenorSMSCompleted;
        
        /// <remarks/>
        public event SendBulkSMS_GEPCompletedEventHandler SendBulkSMS_GEPCompleted;
        
        /// <remarks/>
        public event _SendBulkSMS_NetworkCompletedEventHandler _SendBulkSMS_NetworkCompleted;
        
        /// <remarks/>
        public event SendTelenorSMS_UrduCompletedEventHandler SendTelenorSMS_UrduCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/SendTelenorSMS", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string SendTelenorSMS(string RecipentName, string PhoneNumbers, string SMS_Text, string TeamName) {
            object[] results = this.Invoke("SendTelenorSMS", new object[] {
                        RecipentName,
                        PhoneNumbers,
                        SMS_Text,
                        TeamName});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void SendTelenorSMSAsync(string RecipentName, string PhoneNumbers, string SMS_Text, string TeamName) {
            this.SendTelenorSMSAsync(RecipentName, PhoneNumbers, SMS_Text, TeamName, null);
        }
        
        /// <remarks/>
        public void SendTelenorSMSAsync(string RecipentName, string PhoneNumbers, string SMS_Text, string TeamName, object userState) {
            if ((this.SendTelenorSMSOperationCompleted == null)) {
                this.SendTelenorSMSOperationCompleted = new System.Threading.SendOrPostCallback(this.OnSendTelenorSMSOperationCompleted);
            }
            this.InvokeAsync("SendTelenorSMS", new object[] {
                        RecipentName,
                        PhoneNumbers,
                        SMS_Text,
                        TeamName}, this.SendTelenorSMSOperationCompleted, userState);
        }
        
        private void OnSendTelenorSMSOperationCompleted(object arg) {
            if ((this.SendTelenorSMSCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.SendTelenorSMSCompleted(this, new SendTelenorSMSCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/SendBulkSMS_GEP", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string SendBulkSMS_GEP(string RecipentName, string PhoneNumbers, string SMS_Text, string TeamName) {
            object[] results = this.Invoke("SendBulkSMS_GEP", new object[] {
                        RecipentName,
                        PhoneNumbers,
                        SMS_Text,
                        TeamName});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void SendBulkSMS_GEPAsync(string RecipentName, string PhoneNumbers, string SMS_Text, string TeamName) {
            this.SendBulkSMS_GEPAsync(RecipentName, PhoneNumbers, SMS_Text, TeamName, null);
        }
        
        /// <remarks/>
        public void SendBulkSMS_GEPAsync(string RecipentName, string PhoneNumbers, string SMS_Text, string TeamName, object userState) {
            if ((this.SendBulkSMS_GEPOperationCompleted == null)) {
                this.SendBulkSMS_GEPOperationCompleted = new System.Threading.SendOrPostCallback(this.OnSendBulkSMS_GEPOperationCompleted);
            }
            this.InvokeAsync("SendBulkSMS_GEP", new object[] {
                        RecipentName,
                        PhoneNumbers,
                        SMS_Text,
                        TeamName}, this.SendBulkSMS_GEPOperationCompleted, userState);
        }
        
        private void OnSendBulkSMS_GEPOperationCompleted(object arg) {
            if ((this.SendBulkSMS_GEPCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.SendBulkSMS_GEPCompleted(this, new SendBulkSMS_GEPCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/_SendBulkSMS_Network", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string _SendBulkSMS_Network(string RecipentName, string PhoneNumbers, string SMS_Text, string TeamName) {
            object[] results = this.Invoke("_SendBulkSMS_Network", new object[] {
                        RecipentName,
                        PhoneNumbers,
                        SMS_Text,
                        TeamName});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void _SendBulkSMS_NetworkAsync(string RecipentName, string PhoneNumbers, string SMS_Text, string TeamName) {
            this._SendBulkSMS_NetworkAsync(RecipentName, PhoneNumbers, SMS_Text, TeamName, null);
        }
        
        /// <remarks/>
        public void _SendBulkSMS_NetworkAsync(string RecipentName, string PhoneNumbers, string SMS_Text, string TeamName, object userState) {
            if ((this._SendBulkSMS_NetworkOperationCompleted == null)) {
                this._SendBulkSMS_NetworkOperationCompleted = new System.Threading.SendOrPostCallback(this.On_SendBulkSMS_NetworkOperationCompleted);
            }
            this.InvokeAsync("_SendBulkSMS_Network", new object[] {
                        RecipentName,
                        PhoneNumbers,
                        SMS_Text,
                        TeamName}, this._SendBulkSMS_NetworkOperationCompleted, userState);
        }
        
        private void On_SendBulkSMS_NetworkOperationCompleted(object arg) {
            if ((this._SendBulkSMS_NetworkCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this._SendBulkSMS_NetworkCompleted(this, new _SendBulkSMS_NetworkCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/SendTelenorSMS_Urdu", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string SendTelenorSMS_Urdu(string RecipentName, string PhoneNumbers, string SMS_Text, string TeamName) {
            object[] results = this.Invoke("SendTelenorSMS_Urdu", new object[] {
                        RecipentName,
                        PhoneNumbers,
                        SMS_Text,
                        TeamName});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void SendTelenorSMS_UrduAsync(string RecipentName, string PhoneNumbers, string SMS_Text, string TeamName) {
            this.SendTelenorSMS_UrduAsync(RecipentName, PhoneNumbers, SMS_Text, TeamName, null);
        }
        
        /// <remarks/>
        public void SendTelenorSMS_UrduAsync(string RecipentName, string PhoneNumbers, string SMS_Text, string TeamName, object userState) {
            if ((this.SendTelenorSMS_UrduOperationCompleted == null)) {
                this.SendTelenorSMS_UrduOperationCompleted = new System.Threading.SendOrPostCallback(this.OnSendTelenorSMS_UrduOperationCompleted);
            }
            this.InvokeAsync("SendTelenorSMS_Urdu", new object[] {
                        RecipentName,
                        PhoneNumbers,
                        SMS_Text,
                        TeamName}, this.SendTelenorSMS_UrduOperationCompleted, userState);
        }
        
        private void OnSendTelenorSMS_UrduOperationCompleted(object arg) {
            if ((this.SendTelenorSMS_UrduCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.SendTelenorSMS_UrduCompleted(this, new SendTelenorSMS_UrduCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2556.0")]
    public delegate void SendTelenorSMSCompletedEventHandler(object sender, SendTelenorSMSCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2556.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class SendTelenorSMSCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal SendTelenorSMSCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2556.0")]
    public delegate void SendBulkSMS_GEPCompletedEventHandler(object sender, SendBulkSMS_GEPCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2556.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class SendBulkSMS_GEPCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal SendBulkSMS_GEPCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2556.0")]
    public delegate void _SendBulkSMS_NetworkCompletedEventHandler(object sender, _SendBulkSMS_NetworkCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2556.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class _SendBulkSMS_NetworkCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal _SendBulkSMS_NetworkCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2556.0")]
    public delegate void SendTelenorSMS_UrduCompletedEventHandler(object sender, SendTelenorSMS_UrduCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2556.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class SendTelenorSMS_UrduCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal SendTelenorSMS_UrduCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591