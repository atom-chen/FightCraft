using System.Collections;

namespace Tables
{
    public class TableReader
    {

        #region 唯一实例

        private TableReader() { }

        private TableReader _Instance = null;
        public TableReader Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new TableReader();

                return _Instance;
            }
        }

        #endregion
        #region Logic

//
        public static EquipItem EquipItem { get; internal set; }
//
        public static CommonItem CommonItem { get; internal set; }

        public static void ReadTables()
        {
            //读取所有表
            EquipItem = new EquipItem(TableReadBase.GetTableText("EquipItem"), false);
            CommonItem = new CommonItem(TableReadBase.GetTableText("CommonItem"), false);

            //初始化所有表
            EquipItem.CoverTableContent();
            CommonItem.CoverTableContent();
        }

        #endregion
    }
}
