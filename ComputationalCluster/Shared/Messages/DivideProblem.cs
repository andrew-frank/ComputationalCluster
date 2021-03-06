﻿namespace ComputationalCluster.Shared.Messages.DivideProblemNamespace 
{
    //------------------------------------------------------------------------------
    // <auto-generated>
    //     This code was generated by a tool.
    //     Runtime Version:4.0.30319.18444
    //
    //     Changes to this file may cause incorrect behavior and will be lost if
    //     the code is regenerated.
    // </auto-generated>
    //------------------------------------------------------------------------------

    using System.Xml.Serialization;

    // 
    // This source code was auto-generated by xsd, Version=4.0.30319.33440.
    // 

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.mini.pw.edu.pl/ucc/")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.mini.pw.edu.pl/ucc/", IsNullable = false)]
    public partial class DivideProblem : Message
    {
        private string problemTypeField;
        private ulong idField;
        private byte[] dataField;
        private ulong computationalNodesField;
        private ulong nodeIDField;

        /// <remarks/>
        public string ProblemType {
            get {
                return this.problemTypeField;
            }
            set {
                this.problemTypeField = value;
            }
        }

        /// <remarks/>
        public ulong Id {
            get {
                return this.idField;
            }
            set {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "base64Binary")]
        public byte[] Data {
            get {
                return this.dataField;
            }
            set {
                this.dataField = value;
            }
        }

        /// <remarks/>
        public ulong ComputationalNodes {
            get {
                return this.computationalNodesField;
            }
            set {
                this.computationalNodesField = value;
            }
        }

        /// <remarks/>
        public ulong NodeID {
            get {
                return this.nodeIDField;
            }
            set {
                this.nodeIDField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.mini.pw.edu.pl/ucc/")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.mini.pw.edu.pl/ucc/", IsNullable = false)]
    public partial class NewDataSet {

        private DivideProblem[] itemsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("DivideProblem")]
        public DivideProblem[] Items {
            get {
                return this.itemsField;
            }
            set {
                this.itemsField = value;
            }
        }
    }
}