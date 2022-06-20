﻿namespace AutonomerSForm
{
    public static class Constants
    {
        public const string ModuleInfo = nameof(AutonomerSForm);
        public const string DbName = "AutonomerSDB";
        public const string PathToPythonScript = @"Sources\Core\AutonomersPythonProject\sqlconnector.py"; //TODO: Поменяй как появится
        public const string PathToGenerateDbScript = @"Sources\Core\CreateDbScript.sql";
        public const string PathToConfig = "Config.json";
    }
}
