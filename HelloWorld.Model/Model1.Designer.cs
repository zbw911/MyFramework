﻿//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Data.EntityClient;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Serialization;

[assembly: EdmSchemaAttribute()]
namespace HelloWorld.Model
{
    #region 上下文
    
    /// <summary>
    /// 没有元数据文档可用。
    /// </summary>
    public partial class HelloWordEntities : ObjectContext
    {
        #region 构造函数
    
        /// <summary>
        /// 请使用应用程序配置文件的“HelloWordEntities”部分中的连接字符串初始化新 HelloWordEntities 对象。
        /// </summary>
        public HelloWordEntities() : base("name=HelloWordEntities", "HelloWordEntities")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        /// <summary>
        /// 初始化新的 HelloWordEntities 对象。
        /// </summary>
        public HelloWordEntities(string connectionString) : base(connectionString, "HelloWordEntities")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        /// <summary>
        /// 初始化新的 HelloWordEntities 对象。
        /// </summary>
        public HelloWordEntities(EntityConnection connection) : base(connection, "HelloWordEntities")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        #endregion
    
        #region 分部方法
    
        partial void OnContextCreated();
    
        #endregion
    
        #region ObjectSet 属性
    
        /// <summary>
        /// 没有元数据文档可用。
        /// </summary>
        public ObjectSet<TableH> TableH
        {
            get
            {
                if ((_TableH == null))
                {
                    _TableH = base.CreateObjectSet<TableH>("TableH");
                }
                return _TableH;
            }
        }
        private ObjectSet<TableH> _TableH;

        #endregion

        #region AddTo 方法
    
        /// <summary>
        /// 用于向 TableH EntitySet 添加新对象的方法，已弃用。请考虑改用关联的 ObjectSet&lt;T&gt; 属性的 .Add 方法。
        /// </summary>
        public void AddToTableH(TableH tableH)
        {
            base.AddObject("TableH", tableH);
        }

        #endregion

    }

    #endregion

    #region 实体
    
    /// <summary>
    /// 没有元数据文档可用。
    /// </summary>
    [EdmEntityTypeAttribute(NamespaceName="HelloWordModel", Name="TableH")]
    [Serializable()]
    [DataContractAttribute(IsReference=true)]
    public partial class TableH : EntityObject
    {
        #region 工厂方法
    
        /// <summary>
        /// 创建新的 TableH 对象。
        /// </summary>
        /// <param name="id">id 属性的初始值。</param>
        /// <param name="name">name 属性的初始值。</param>
        public static TableH CreateTableH(global::System.Int32 id, global::System.String name)
        {
            TableH tableH = new TableH();
            tableH.id = id;
            tableH.name = name;
            return tableH;
        }

        #endregion

        #region 基元属性
    
        /// <summary>
        /// 没有元数据文档可用。
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=true, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int32 id
        {
            get
            {
                return _id;
            }
            set
            {
                if (_id != value)
                {
                    OnidChanging(value);
                    ReportPropertyChanging("id");
                    _id = StructuralObject.SetValidValue(value);
                    ReportPropertyChanged("id");
                    OnidChanged();
                }
            }
        }
        private global::System.Int32 _id;
        partial void OnidChanging(global::System.Int32 value);
        partial void OnidChanged();
    
        /// <summary>
        /// 没有元数据文档可用。
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String name
        {
            get
            {
                return _name;
            }
            set
            {
                OnnameChanging(value);
                ReportPropertyChanging("name");
                _name = StructuralObject.SetValidValue(value, false);
                ReportPropertyChanged("name");
                OnnameChanged();
            }
        }
        private global::System.String _name;
        partial void OnnameChanging(global::System.String value);
        partial void OnnameChanged();

        #endregion

    
    }

    #endregion

    
}
