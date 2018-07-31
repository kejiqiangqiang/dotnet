﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Demo
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.ComponentModel;
	using System;
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="EFOS.Master")]
	public partial class LINQtoSQLDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region 可扩展性方法定义
    partial void OnCreated();
    partial void InsertAcc_Role(Acc_Role instance);
    partial void UpdateAcc_Role(Acc_Role instance);
    partial void DeleteAcc_Role(Acc_Role instance);
    partial void InsertAcc_Function(Acc_Function instance);
    partial void UpdateAcc_Function(Acc_Function instance);
    partial void DeleteAcc_Function(Acc_Function instance);
    #endregion
		
		public LINQtoSQLDataContext() : 
				base(global::Demo.Properties.Settings.Default.EFOS_MasterConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public LINQtoSQLDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public LINQtoSQLDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public LINQtoSQLDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public LINQtoSQLDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<Acc_Role> Acc_Role
		{
			get
			{
				return this.GetTable<Acc_Role>();
			}
		}
		
		public System.Data.Linq.Table<Acc_Function> Acc_Function
		{
			get
			{
				return this.GetTable<Acc_Function>();
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.Acc_Role")]
	public partial class Acc_Role : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _FRoleID;
		
		private string _RoleName;
		
		private string _Remarks;
		
    #region 可扩展性方法定义
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnFRoleIDChanging(int value);
    partial void OnFRoleIDChanged();
    partial void OnRoleNameChanging(string value);
    partial void OnRoleNameChanged();
    partial void OnRemarksChanging(string value);
    partial void OnRemarksChanged();
    #endregion
		
		public Acc_Role()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_FRoleID", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int FRoleID
		{
			get
			{
				return this._FRoleID;
			}
			set
			{
				if ((this._FRoleID != value))
				{
					this.OnFRoleIDChanging(value);
					this.SendPropertyChanging();
					this._FRoleID = value;
					this.SendPropertyChanged("FRoleID");
					this.OnFRoleIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_RoleName", DbType="VarChar(200) NOT NULL", CanBeNull=false)]
		public string RoleName
		{
			get
			{
				return this._RoleName;
			}
			set
			{
				if ((this._RoleName != value))
				{
					this.OnRoleNameChanging(value);
					this.SendPropertyChanging();
					this._RoleName = value;
					this.SendPropertyChanged("RoleName");
					this.OnRoleNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Remarks", DbType="VarChar(2000)")]
		public string Remarks
		{
			get
			{
				return this._Remarks;
			}
			set
			{
				if ((this._Remarks != value))
				{
					this.OnRemarksChanging(value);
					this.SendPropertyChanging();
					this._Remarks = value;
					this.SendPropertyChanged("Remarks");
					this.OnRemarksChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.Acc_Function")]
	public partial class Acc_Function : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private string _FunctionCode;
		
		private string _ParentCode;
		
		private string _FunctionName;
		
		private string _Icon;
		
		private string _Url;
		
		private string _JsonParams;
		
		private int _Sort;
		
		private string _Remarks;
		
		private bool _IsUse;
		
    #region 可扩展性方法定义
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnFunctionCodeChanging(string value);
    partial void OnFunctionCodeChanged();
    partial void OnParentCodeChanging(string value);
    partial void OnParentCodeChanged();
    partial void OnFunctionNameChanging(string value);
    partial void OnFunctionNameChanged();
    partial void OnIconChanging(string value);
    partial void OnIconChanged();
    partial void OnUrlChanging(string value);
    partial void OnUrlChanged();
    partial void OnJsonParamsChanging(string value);
    partial void OnJsonParamsChanged();
    partial void OnSortChanging(int value);
    partial void OnSortChanged();
    partial void OnRemarksChanging(string value);
    partial void OnRemarksChanged();
    partial void OnIsUseChanging(bool value);
    partial void OnIsUseChanged();
    #endregion
		
		public Acc_Function()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_FunctionCode", DbType="VarChar(100) NOT NULL", CanBeNull=false, IsPrimaryKey=true)]
		public string FunctionCode
		{
			get
			{
				return this._FunctionCode;
			}
			set
			{
				if ((this._FunctionCode != value))
				{
					this.OnFunctionCodeChanging(value);
					this.SendPropertyChanging();
					this._FunctionCode = value;
					this.SendPropertyChanged("FunctionCode");
					this.OnFunctionCodeChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ParentCode", DbType="VarChar(100)")]
		public string ParentCode
		{
			get
			{
				return this._ParentCode;
			}
			set
			{
				if ((this._ParentCode != value))
				{
					this.OnParentCodeChanging(value);
					this.SendPropertyChanging();
					this._ParentCode = value;
					this.SendPropertyChanged("ParentCode");
					this.OnParentCodeChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_FunctionName", DbType="VarChar(200) NOT NULL", CanBeNull=false)]
		public string FunctionName
		{
			get
			{
				return this._FunctionName;
			}
			set
			{
				if ((this._FunctionName != value))
				{
					this.OnFunctionNameChanging(value);
					this.SendPropertyChanging();
					this._FunctionName = value;
					this.SendPropertyChanged("FunctionName");
					this.OnFunctionNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Icon", DbType="VarChar(50)")]
		public string Icon
		{
			get
			{
				return this._Icon;
			}
			set
			{
				if ((this._Icon != value))
				{
					this.OnIconChanging(value);
					this.SendPropertyChanging();
					this._Icon = value;
					this.SendPropertyChanged("Icon");
					this.OnIconChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Url", DbType="VarChar(400) NOT NULL", CanBeNull=false)]
		public string Url
		{
			get
			{
				return this._Url;
			}
			set
			{
				if ((this._Url != value))
				{
					this.OnUrlChanging(value);
					this.SendPropertyChanging();
					this._Url = value;
					this.SendPropertyChanged("Url");
					this.OnUrlChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_JsonParams", DbType="VarChar(MAX)")]
		public string JsonParams
		{
			get
			{
				return this._JsonParams;
			}
			set
			{
				if ((this._JsonParams != value))
				{
					this.OnJsonParamsChanging(value);
					this.SendPropertyChanging();
					this._JsonParams = value;
					this.SendPropertyChanged("JsonParams");
					this.OnJsonParamsChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Sort", DbType="Int NOT NULL")]
		public int Sort
		{
			get
			{
				return this._Sort;
			}
			set
			{
				if ((this._Sort != value))
				{
					this.OnSortChanging(value);
					this.SendPropertyChanging();
					this._Sort = value;
					this.SendPropertyChanged("Sort");
					this.OnSortChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Remarks", DbType="VarChar(500)")]
		public string Remarks
		{
			get
			{
				return this._Remarks;
			}
			set
			{
				if ((this._Remarks != value))
				{
					this.OnRemarksChanging(value);
					this.SendPropertyChanging();
					this._Remarks = value;
					this.SendPropertyChanged("Remarks");
					this.OnRemarksChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_IsUse", DbType="Bit NOT NULL")]
		public bool IsUse
		{
			get
			{
				return this._IsUse;
			}
			set
			{
				if ((this._IsUse != value))
				{
					this.OnIsUseChanging(value);
					this.SendPropertyChanging();
					this._IsUse = value;
					this.SendPropertyChanged("IsUse");
					this.OnIsUseChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}
#pragma warning restore 1591
