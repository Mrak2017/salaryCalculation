# Приложение для ведения заработной платы
+ C# 
    + ASP.NET
    + Entity Framework Core 
    + DB-migrations 
+ Angular v7 
    + RxJS
    + Angular Material Design
    + moment.js 
+ SQLite

#### Для старта:
- импортировать проект в visualStudio
- запустить IIS Express или SalaryCalculation (первый запуск может быть долгим, из за выкачивания node_modules для клиента)

### Описание реализованного функционала.

В настройки вынесены дефолтные значения для расчета зар. платы в зависимости от группы сотрудника.

Основной алгоритм расчета и общие требования к приложению описаны в файле "Задание.CSharp.docx"

По сотруднику существует:
- общая информация, которая содержит ФИО, логин, пароль, даты устройства на работу и увольнения (Person).
- информация о группах в которых он состоял на протяжении работы (Person2Group, сроки нахождения в группах не могут пересекаться, на 1 дату может быть только одна активная группа)
- информация о положении сотрудника в иерархии (OrganizationStructureItem, для удобства выборок из таблице реализован материализованный путь, содержащий айди всех его руководителей по порядку от самого верхнего и айдишник самого сотрудника)

Реализован механизм версионного обновления данных в базе, через наследников класса ReorganizationMain.

Реализован клиент, с использованием REST-API, позволяющий выполнить следующий действия:
- отобразить весь список настроек системы
- добавить/удалить/изменить настройку системы
- отобразить весь список сотрудников, с полем поиска по фамилии
- добавить нового сотрудника
- редактировать информацию о сотруднике:
    - основная информация
    - информация о группах сотрудника (просмотр, добавление/редактирование/удаление)
	- информация о руководителе, с возможность его изменения
	- информация о подчиненных всех уровней (в виде иерархического дерева) с возможность перехода на страницу любого сотрудника
	- возможность добавить и удалить непосредственного подчиненного
- расчитать зар плату для сотрудника на выбранную дату (с учетом группы сотрудника и его подчиненных на выбранную дату)
- расчитать зар плату для всех сотрудников фирмы на выбранную дату (с учетом группы сотрудников и их подчиненных на выбранную дату)
