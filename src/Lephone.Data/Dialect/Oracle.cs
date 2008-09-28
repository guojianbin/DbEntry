﻿using System;
using System.Data;
using Lephone.Data.SqlEntry;
using Lephone.Data.Builder;
using Lephone.Data.Common;
using Lephone.Util.Logging;

namespace Lephone.Data.Dialect
{
    public class Oracle : SequencedDialect
    {
        public Oracle()
        {
            TypeNames[DataType.String] = "CLOB";
            TypeNames[DataType.DateTime] = "TIMESTAMP";
            TypeNames[DataType.Date] = "DATE";
            TypeNames[DataType.Time] = "DATE"; //TODO: Is it right?
            TypeNames[DataType.Boolean] = "NUMBER(1,0)";

            TypeNames[DataType.Byte] = "NUMBER(3,0)";
            TypeNames[DataType.SByte] = "";
            TypeNames[DataType.Decimal] = "NUMBER(19,5)";
            TypeNames[DataType.Double] = "DOUBLE PRECISION";
            TypeNames[DataType.Single] = "FLOAT(24)";

            TypeNames[DataType.Int32] = "NUMBER(10,0)";
            TypeNames[DataType.UInt32] = "NUMBER(10,0)";
            TypeNames[DataType.Int64] = "NUMBER(20,0)";
            TypeNames[DataType.UInt64] = "NUMBER(20,0)";
            TypeNames[DataType.Int16] = "NUMBER(5,0)";
            TypeNames[DataType.UInt16] = "NUMBER(5,0)";

            TypeNames[DataType.Binary] = "BLOB";

            TypeNames[typeof(string)] = "VARCHAR2";
        }

        public override string DbNowString
        {
            get { return "sysdate"; }
        }

        public override string GetUserId(string ConnectionString)
        {
            string [] ss = ConnectionString.Split(';');
            foreach (string s in ss)
            {
                string[] ms = s.Split('=');
                if (ms[0].Trim().ToLower() == "user id")
                {
                    return ms[1].Trim().ToUpper();
                }
            }
            return null;
        }

        public override IDataReader GetDataReader(IDataReader dr, Type ReturnType)
        {
            return new StupidDataReader(dr, ReturnType);
        }

        public override bool NotSupportPostFix
        {
            get { return true; }
        }

        protected override string GetSelectSequenceSql(string TableName)
        {
            return string.Format("select {0}_SEQ.nextval from dual", TableName.ToUpper());
        }

        public override bool NeedCommitCreateFirst
        {
            get { return false; }
        }

        public override bool SupportDirctionOfEachColumnInIndex
        {
            get { return false; }
        }

        public override string UnicodeTypePrefix
        {
            get { return "N"; }
        }

        public override string IdentityColumnString
        {
            get { return "NOT NULL"; }
        }

        public override string GetCreateSequenceString(string TableName)
        {
            return string.Format("CREATE SEQUENCE {0}_SEQ INCREMENT BY 1;\n", TableName.ToUpper());
        }

        protected override string QuoteSingle(string name)
        {
            return base.QuoteSingle(name.ToUpper());
        }

        public override string NullString
        {
            get { return ""; }
        }

        public override bool ExecuteEachLine
        {
            get { return true; }
        }

        public override void ExecuteDropSequence(DataProvider dp, string TableName)
        {
            string sql = string.Format("DROP SEQUENCE {0}_SEQ;\n", TableName.ToUpper());
            Logger.SQL.Trace(sql);
            dp.ExecuteNonQuery(sql);
        }

        protected override SqlStatement GetPagedSelectSqlStatement(SelectStatementBuilder ssb)
        {
            SqlStatement Sql = base.GetNormalSelectSqlStatement(ssb);
            Sql.SqlCommandText = string.Format("select * from ( select row_.*, rownum rownum_ from ( {0} ) row_ where rownum <= {1} ) where rownum_ >= {2}",
                Sql.SqlCommandText, ssb.Range.EndIndex, ssb.Range.StartIndex);
            return Sql;
        }

        public override char ParamterPrefix
        {
            get { return ':'; }
        }
    }
}