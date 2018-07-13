using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Schema;

namespace BusinessProxy
{
    /// <summary>
    /// 动态表名设置
    /// </summary>
    public class DynamicMappingSource : MappingSource
    {
        class DynamicAttributedMetaModel : MetaModel
        {
            private MetaModel source;
            private const string TypeName = "System.Data.Linq.Mapping.AttributedMetaModel";

            private DynamicMappingSource mappingSource;

            internal DynamicAttributedMetaModel(MappingSource mappingSource, Type contextType)
            {
                this.mappingSource = (DynamicMappingSource)mappingSource;
                var bf = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.CreateInstance;
                var args = new object[] { mappingSource, contextType };
                source = typeof(DataContext).Assembly.CreateInstance(TypeName, false, bf, null, args, CultureInfo.CurrentCulture, null) as MetaModel;
                Debug.Assert(source != null);
            }

            public override MetaTable GetTable(Type rowType)
            {
                if (mappingSource.GetMetaTableName != null)
                {
                    var typeName = "System.Data.Linq.Mapping.AttributedMetaTable";
                    var bf = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.CreateInstance;
                    var attribute = new TableAttribute { Name = mappingSource.GetMetaTableName(rowType) };
                    var args = new object[] { source, attribute, rowType };
                    var metaTable = typeof(DataContext).Assembly.CreateInstance(typeName, false, bf, null, args, CultureInfo.CurrentCulture, null) as MetaTable;
                    return metaTable;
                }
                return source.GetTable(rowType);
            }

            public override MetaFunction GetFunction(MethodInfo method)
            {
                return source.GetFunction(method);
            }

            public override IEnumerable<MetaTable> GetTables()
            {
                return source.GetTables();
            }

            public override IEnumerable<MetaFunction> GetFunctions()
            {
                return source.GetFunctions();
            }

            public override MetaType GetMetaType(Type type)
            {
                return source.GetMetaType(type);
            }

            public override MappingSource MappingSource
            {
                get { return source.MappingSource; }
            }

            public override Type ContextType
            {
                get { return source.ContextType; }
            }

            public override string DatabaseName
            {
                get { return source.DatabaseName; }
            }

            public override Type ProviderType
            {
                get { return source.ProviderType; }
            }
        }

        public Func<Type, string> GetMetaTableName;

        protected override MetaModel CreateModel(Type dataContextType)
        {
            if (dataContextType == null)
            {
                throw new ArgumentNullException("dataContextType");
            }
            return new DynamicAttributedMetaModel(this, dataContextType);
        }
    }
}