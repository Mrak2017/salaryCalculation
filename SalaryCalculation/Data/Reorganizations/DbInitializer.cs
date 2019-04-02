using SalaryCalculation.Controllers;
using SalaryCalculation.Models.DataReorganizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalaryCalculation.Models
{
    /** Класс для инициализации и заполнения базы данных*/
    public static class DbInitializer
    {
        /** 1) Создание базы данных если не существует
         2) Выполнение реорганизация (миграция) в случае обновления версии программы (ревизии данных)*/
        public static void Initialize(SalaryCalculationDBContext context, int currentVersion)
        {
            context.Database.EnsureCreated();

            ConfigurationController controller = new ConfigurationController(context);
            int lastPassedVersion = controller.GetConfigurationIntOrDefault(ConfigurationController.LAST_DATA_REVISION_CODE, 0);
            string mainClassNamespace = typeof(ReorganizationMain).Namespace;
            for (int i = lastPassedVersion; i < currentVersion; i++)
            {
                string className = mainClassNamespace + "." + ReorganizationMain.SubclassNamePrefix + currentVersion;
                Type type = Type.GetType(className);
                if (type != null)
                {
                    ReorganizationMain reorganization = (ReorganizationMain) Activator.CreateInstance(type, context);
                    reorganization.Run();
                }
                else
                {
                    throw new Exception("Не удалось найти реорганизацию с именем: '" + className + "'");
                }
            }
            if (lastPassedVersion != currentVersion)
            {
                controller.AddOrUpdateConfiguration(ConfigurationController.LAST_DATA_REVISION_CODE, currentVersion.ToString());
            }
        }
    }
}
