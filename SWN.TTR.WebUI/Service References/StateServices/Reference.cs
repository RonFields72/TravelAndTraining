﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18047
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SWN.TTR.WebUI.StateServices {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://swn.com/ods/StateService", ConfigurationName="StateServices.StateServiceSoap")]
    public interface StateServiceSoap {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://swn.com/ods/StateService/GetAll", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute()]
        SWN.TTR.WebUI.StateServices.State[] GetAll();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://swn.com/ods/StateService/GetById", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute()]
        SWN.TTR.WebUI.StateServices.State GetById(string id);
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.18047")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://swn.com/ods/StateService")]
    public partial class State : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string idField;
        
        private string nameField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public string Id {
            get {
                return this.idField;
            }
            set {
                this.idField = value;
                this.RaisePropertyChanged("Id");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public string Name {
            get {
                return this.nameField;
            }
            set {
                this.nameField = value;
                this.RaisePropertyChanged("Name");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface StateServiceSoapChannel : SWN.TTR.WebUI.StateServices.StateServiceSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class StateServiceSoapClient : System.ServiceModel.ClientBase<SWN.TTR.WebUI.StateServices.StateServiceSoap>, SWN.TTR.WebUI.StateServices.StateServiceSoap {
        
        public StateServiceSoapClient() {
        }
        
        public StateServiceSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public StateServiceSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public StateServiceSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public StateServiceSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public SWN.TTR.WebUI.StateServices.State[] GetAll() {
            return base.Channel.GetAll();
        }
        
        public SWN.TTR.WebUI.StateServices.State GetById(string id) {
            return base.Channel.GetById(id);
        }
    }
}