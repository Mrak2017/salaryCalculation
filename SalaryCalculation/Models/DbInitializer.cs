using SalaryCalculation.Controllers;
using SalaryCalculation.Models.DataReorganizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalaryCalculation.Models
{
    public static class DbInitializer
    {
        public static void Initialize(SalaryCalculationDBContext context, int version)
        {
            context.Database.EnsureCreated();

            ConfigurationController controller = new ConfigurationController(context);
            var lastPassedVersion = controller.GetSettingIntOrDefault(ConfigurationController.LAST_DATA_REVISION_CODE, 0);
            string mainClassNamespace = typeof(ReorganizationMain).Namespace;
            foreach (int verionNumber in Enumerable.Range(lastPassedVersion, version))
            {
                string className = mainClassNamespace + ".ToVersion" + version;
                Type type = Type.GetType(className);
                if (type != null)
                {
                    ReorganizationMain test = (ReorganizationMain) Activator.CreateInstance(type, context);
                    test.Run();
                }
                else
                {
                    throw new Exception("Не удалось найти реорганизацию с именем: '" + className + "'");
                }
            }
            controller.AddOrUpdateSetting(ConfigurationController.LAST_DATA_REVISION_CODE, version.ToString());
        }
    }
}
