using UnityEngine;
using System.Collections;
using Mono.Data.Sqlite;

public class Test : MonoBehaviour
{
    string name = null;
    string email = null;
    string path = null;

    void Start()
    {

        //创建数据库名称为xuanyusong.db
        string appDBPath = Application.persistentDataPath + "/xuanyusong.db";
        DbAccess db = new DbAccess(@"Data Source=" + appDBPath);

        path = appDBPath;

        if (!db.IsTableExit("momo"))
        {
            //请注意 插入字符串是 已经要加上'宣雨松' 不然会报错
            db.CreateTable("momo", new string[] { "name", "qq", "email", "blog" }, new string[] { "text", "text", "text", "text" });
            //我在数据库中连续插入三条数据
            db.InsertInto("momo", new string[] { "'宣雨松'", "'289187120'", "'xuanyusong@gmail.com'", "'www.xuanyusong.com'" });
            db.InsertInto("momo", new string[] { "'雨松MOMO'", "'289187120'", "'000@gmail.com'", "'www.xuanyusong.com'" });
            db.InsertInto("momo", new string[] { "'哇咔咔'", "'289187120'", "'111@gmail.com'", "'www.xuanyusong.com'" });
        }
        

        //然后在删掉两条数据
        db.Delete("momo", new string[] { "email", "email" }, new string[] { "'xuanyusong@gmail.com'", "'000@gmail.com'" });

        //注解1
        using (SqliteDataReader sqReader = db.SelectWhere("momo", new string[] { "name", "email" }, new string[] { "qq" }, new string[] { "=" }, new string[] { "289187120" }))
        {

            while (sqReader.Read())
            {
                //目前中文无法显示
                Debug.Log("xuanyusong" + sqReader.GetString(sqReader.GetOrdinal("name")));

                Debug.Log("xuanyusong" + sqReader.GetString(sqReader.GetOrdinal("email")));

                name = sqReader.GetString(sqReader.GetOrdinal("name"));
                email = sqReader.GetString(sqReader.GetOrdinal("email"));

            }

            sqReader.Close();
        }

        db.CloseSqlConnection();
    }

    void OnGUI()
    {
        if (name != null)
        {
            GUILayout.Label(name);
        }

        if (email != null)
        {
            GUILayout.Label(email);
        }

        if (path != null)
        {
            GUILayout.Label(path);
        }
    }
}