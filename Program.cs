using System;
using System.Data;
using System.Data.Odbc;

namespace cs_con_framework_mysql_03
{
    class Program
    {
        static void Main(string[] args)
        {
            DataSet dataSet = new DataSet();

            OdbcConnection myCon = Helper.CreateConnection();

            // MySQL の処理

            // SQL
            string myQuery =
                @"SELECT * from 社員マスタ order by 社員コード";

            // アタプター作成
            OdbcDataAdapter adapter = new OdbcDataAdapter();

            // SQL 作成
            OdbcCommand command = new OdbcCommand(myQuery, myCon);
            command.CommandType = CommandType.Text;
            adapter.SelectCommand = command;

            // 後で更新用コマンドを取り出す為に必要
            OdbcCommandBuilder commandBuilder = new OdbcCommandBuilder(adapter);

            // データを取り出す
            adapter.Fill(dataSet);

            // テーブルを取得
            DataTable dataTable = dataSet.Tables["Table"];

            foreach(DataRow row in dataTable.Rows)
            {
                int kyuyo = row.Field<int>("給与");
                kyuyo++;
                row["給与"] = kyuyo;
                kyuyo = (int)row["給与"];
                kyuyo++;
                row["給与"] = kyuyo;

                DateTime dt = (DateTime)row["生年月日"];
                // 1日以前に変更
                dt = dt.AddDays(1);
                // 日付のセット
                row["生年月日"] = dt;
            }

            // 更新用のコマンドを取得
            adapter.UpdateCommand = commandBuilder.GetUpdateCommand();
            // 更新実行
            adapter.Update(dataSet);

            myCon.Close();
        }
    }
}
