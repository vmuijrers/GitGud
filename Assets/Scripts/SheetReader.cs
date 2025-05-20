using UnityEngine;
using Cathei.BakingSheet;
using Cathei.BakingSheet.Unity;
using UnityEngine.InputSystem;
using System.Reflection;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Linq;

public class SheetReader : MonoBehaviour
{
    public string filePath;
    [SerializeField] private SheetContainer SheetContainer;

    [LoadFromKeyAttribute("MELEE")]
    public UnitSheet.Row UnitSheet;

    public int HP => UnitSheet.HP;
    public System.Action OnLoadingCompleted;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        OnLoadingCompleted += LoadAllStatsToUnits;
        BakeSheetFromJSON();
    }

    private void LoadAllStatsToUnits()
    {
        AttributeValueLoader.LoadValuesFromAttributes(this, this);
        Debug.Log("HP: " + UnitSheet.HP);
        //var objs = Resources.FindObjectsOfTypeAll(typeof(ILoadDataFromTable));
        //foreach(ILoadDataFromTable loader in objs)
        //{
        //    loader.LoadData(this);
        //}
    }

    public object GetValueByCellID(string ID, string ColumnID)
    {
        ISheet<UnitSheet.Row> result = SheetContainer.Find<ISheet<UnitSheet.Row>>("Units");
        UnitSheet = result[ID];
        return UnitSheet.GetByString(ColumnID);
    }

    private void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            //ShowData();
            //UnitHP = SheetContainer.Units["MELEE"].HP;
            //Debug.Log($"UnitHP: {UnitHP}");
        }
    }
#if UNITY_EDITOR
    //Reads from Excel and stores to Json (Editor only)
    public async void ConvertExcelToJSON()
    {
        // any ILogger will work, there is built-in UnityLogger
        var logger = new UnityLogger();

        // pass logger to receive logs
        SheetContainer = new SheetContainer(logger);

        var path = Application.dataPath + filePath;

        // create excel converter from path
        var excelConverter = new ExcelSheetConverter(path);

        // create json converter from path
        var jsonConverter = new JsonSheetConverter(path);

        // convert from excel
        await SheetContainer.Bake(excelConverter);

        // save as json
        await SheetContainer.Store(jsonConverter);

        AssetDatabase.Refresh();
    }
#endif

    [ContextMenu("Show Loaded Data")]
    private void ShowData()
    {
        foreach (var unit in SheetContainer.Units)
        {
            Debug.Log($"ID: {unit.Id},Name: {unit.Name}, HP: {unit.HP}, AttackRange: {unit.AttackRange}, Movespeed: {unit.MoveSpeed}");
        }

        foreach (var level in SheetContainer.Level)
        {
            Debug.Log($"ID: {level.Id},NAME: {level.NAME}, MIN_HEIGHT: {level.MIN_HEIGHT}, MAX_HEIGHT: {level.MAX_HEIGHT}, SAND_CHANCE: {level.SAND_CHANCE}, WATER_CHANCE: {level.WATER_CHANCE}, ROCK_CHANCE: {level.ROCK_CHANCE}, GRASS_CHANCE: {level.GRASS_CHANCE}");
        }
    }



    [ContextMenu("Store Sheet As JSON")]
    public void DoConvertExcelToJSON()
    {
        var path = Application.dataPath + filePath;
        Debug.Log($"Path: {path}");
        ConvertExcelToJSON();
    }

    [ContextMenu("Bake Sheet from Excel")]
    public void BakeSheetFromExcel()
    {
        var path = Application.dataPath + filePath;
        Debug.Log($"Path: {path}");
        BakeSheetFromExcel(path);
    }

    [ContextMenu("Bake Sheet from JSON")]
    public void BakeSheetFromJSON()
    {
        var path = Application.dataPath + filePath;
        Debug.Log($"Path: {path}");
        BakeFromJSON(path);
    }

    public async void BakeFromJSON(string path)
    {
        // any ILogger will work, there is built-in UnityLogger
        var logger = new UnityLogger();

        // pass logger to receive logs
        SheetContainer = new SheetContainer(logger);

        // create json converter from path
        var jsonConverter = new JsonSheetConverter(path);

        var task = SheetContainer.Bake(jsonConverter);
        await task;
        if (task.IsCompleted)
        {
            Debug.Log("Loading Completed!");
            ShowData();
            OnLoadingCompleted?.Invoke();
        }
    }

    public async void BakeSheetFromExcel(string path)
    {
        // any ILogger will work, there is built-in UnityLogger
        var logger = new UnityLogger();

        // pass logger to receive logs
        SheetContainer = new SheetContainer(logger);

        // create excel converter from path
        var excelConverter = new ExcelSheetConverter(path);

        // bake sheets from excel converter
        var task = SheetContainer.Bake(excelConverter);
        await task;
        if (task.IsCompleted)
        {
            Debug.Log("Loading Completed!");
            ShowData();
        }
    }
    
}

public class DummyUnit : MonoBehaviour, ILoadDataFromTable
{
    public string ID;
    private int attackRange; 
    private int hp; 

    public void LoadData(SheetReader reader)
    {
        //hp = reader.GetValueByCellID(ID, "HP");
        //var stats = reader.GetValueByCellID(ID);
        //attackRange = stats.AttackRange;
        //hp = stats.HP;
        //etc.
    }
}

public interface ILoadDataFromTable
{
    void LoadData(SheetReader reader);
}

[System.Serializable]
public class UnitSheet : Sheet<UnitSheet.Row>
{
    public class Row : SheetRow
    {
        // use name of matching column
        public string Name { get; private set; }
        public int HP { get; private set; }
        public int MoveSpeed { get; private set; }
        public int AttackRange { get; private set; }

        public object GetByString(string columName)
        {
            switch(columName)
            {
                case "Name": return Name;
                case "HP": return HP;
                case "MoveSpeed": return MoveSpeed;
                case "AttackRange": return AttackRange;
            }
            return null;
        }
    }
}

[System.Serializable]
public class LevelSheet : Sheet<LevelSheet.Row>
{
    public class Row : SheetRow
    {
        // use name of matching column
        public string NAME { get; private set; }
        public int MIN_HEIGHT { get; private set; }
        public int MAX_HEIGHT { get; private set; }
        public int GRASS_CHANCE { get; private set; }
        public int SAND_CHANCE { get; private set; }
        public int WATER_CHANCE { get; private set; }
        public int ROCK_CHANCE { get; private set; }
    }
}
[System.Serializable]
public class SheetContainer : SheetContainerBase
{
    public SheetContainer(UnityLogger logger) : base(logger) { }

    // use name of each matching sheet name from source
    public UnitSheet Units { get; private set; }
    public LevelSheet Level { get; private set; }
}


public static class AttributeValueLoader
{
    public static void LoadValuesFromAttributes(object target, SheetReader sheetReader)
    {
        var types = Assembly.GetAssembly(typeof(SheetReader)).GetTypes();
        foreach(var type in types)
        {
            var fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            foreach (var field in fields)
            {
                var attribute = field.GetCustomAttribute<LoadFromKeyAttribute>();
                if (attribute != null)
                {
                    object value = sheetReader.GetValueByCellID(attribute.Key, attribute.Key);

                    if (value != null && field.FieldType.IsAssignableFrom(value.GetType()))
                    {
                        field.SetValue(target, value);
                    }
                    else
                    {
                        Debug.LogWarning($"Cannot assign value for key '{attribute.Key}' to field '{field.Name}'. Type mismatch or null.");
                    }
                }
            }
        }
        
    }
}