
# **Exercises: Introduction to Entity Framework**
This document defines the **exercise assignments** for the [Databases Advanced - Entity Framework course @ SoftUni](https://softuni.bg/trainings/4540/entity-framework-core-june-2024)
You can check your solutions in [Judge](https://judge.softuni.org/Contests/3917/Entity-Framework-Introduction)
1. ## **Import the SoftUni Database**
Import the **SoftUni** database into SQL Management Studio by **executing** the provided script.

![](Aspose.Words.7c598c50-a9ea-420d-aef7-8c3a635922d0.001.png)
1. ## **Database First**
Model the existing database by using the Database First approach.

First create a new empty **.NET Core** **Console Application** and after it is created open the **Package Manager Console**

![](Aspose.Words.7c598c50-a9ea-420d-aef7-8c3a635922d0.002.png)

It will look something like this:

![](Aspose.Words.7c598c50-a9ea-420d-aef7-8c3a635922d0.003.png)

Use it to run the following commands **one by one**:

|<p>**Install-Package Microsoft.EntityFrameworkCore.Tools –v 6.0.1**</p><p>**Install-Package Microsoft.EntityFrameworkCore.SqlServer –v 6.0.1**</p><p>**Install-Package Microsoft.EntityFrameworkCore.Design -v 6.0.1**</p>|
| :- |

These are the **packages** you will need, in order to **scaffold** our **SoftUniContext** from the **SoftUni** **database**.

**NOTE:** If **Package Manager Console** gives you any **errors** while trying to **execute the above commands**, try using the following ones:

|<p>**Install-Package Microsoft.EntityFrameworkCore.Tools –Version 6.0.1**</p><p>**Install-Package Microsoft.EntityFrameworkCore.SqlServer –Version 6.0.1**</p><p>**Install-Package Microsoft.EntityFrameworkCore.Design -Version 6.0.1**</p>|
| :- |

**NOTE:** For the next steps you will need **public Program class** and a **static void Main()** method.

Next, we must **execute** the **command** to **scaffold** our **context** **class**. It will consist of 4 things:

1. First, the name of the command:

|**Scaffold-DbContext**|
| :- |

1. Second, the connection we will be using (our connection string):

|**-Connection "Server=<ServerName>;Database=<DatabaseName>;Integrated Security=True;"**|
| :- |

For **ServerName**, use the name of your local MS SQL Server instance or "**.**".

For **DatabaseName**, use the name of the database you want to use, in this case – **SoftUni**.

1. Third, we need to declare our service provider, we'll be using **Microsoft.EntityFrameworkCore.SqlServer**:

|**-Provider Microsoft.EntityFrameworkCore.SqlServer**|
| :- |

1. And the fourth thing we'll do, is to give it a directory where all of our models will go (e.g. **Models**):

|**-OutputDir Data/Models**|
| :- |

Our final command will look like this:

|**Scaffold-DbContext -Connection "Server=.;Database=SoftUni;Integrated Security=True;" -Provider Microsoft.EntityFrameworkCore.SqlServer -OutputDir Data/Models**|
| :- |

Execute the **whole command** on a **single line.**

**NOTE:** You should write only one '**\**' in the connection string. Otherwise, an **InvalidOperationException: Instance failure** will be thrown.

Entity Framework Core has successfully **mapped the database schema to C# classes**. Use the **Solution Explorer** in Visual Studio to move the **SoftUniContext** class out of **Models** into** the **Data** folder and move the **Models** folder out of the **Data** folder into the project's directory. Press OK on both of the pop-up windows that will be shown.

![](Aspose.Words.7c598c50-a9ea-420d-aef7-8c3a635922d0.004.png)

![](Aspose.Words.7c598c50-a9ea-420d-aef7-8c3a635922d0.005.png)

This way Visual Studio will also **rename** the **classes** **everywhere** they're used.

However, sometimes Entity Framework Core isn't good enough with names – all classes may have been pluralized. If this is your case, just rename all of our classes properly. Use **right-click ? Rename** or the **F2** shortcut and press **OK** on this **pop**-**up** **window** after renaming each class:

![](Aspose.Words.7c598c50-a9ea-420d-aef7-8c3a635922d0.006.png)

The final result should look like this:

![](Aspose.Words.7c598c50-a9ea-420d-aef7-8c3a635922d0.007.png)

Don't forget to check the **SoftUniContext**'s namespace after moving it and to add a reference to the **Models** and **Data** namespaces:

**Make** **sure** that your namespaces are **exactly** the same as these:

|<p>**SoftUni<br>SoftUni.Data**</p><p>**SoftUni.Models**</p>|
| :- |

Finally, we want to clean up the packages we won't be using anymore from the package manager GUI or by running these commands **one by one**:

|<p>**Uninstall-Package Microsoft.EntityFrameworkCore.Tools -r**</p><p>**Uninstall-Package Microsoft.EntityFrameworkCore.Design -RemoveDependencies**</p>|
| :- |
1. ## **Employees <a name="ole_link42"></a>Full Information**
**NOTE:** You will need method <a name="ole_link37"></a><a name="ole_link38"></a><a name="ole_link75"></a>**public static string GetEmployeesFullInformation(SoftUniContext context)** and **public StartUp** class. 

<a name="ole_link39"></a><a name="ole_link40"></a><a name="ole_link76"></a>Now we can use **SoftUniContext** to extract data from our database. Your first task is to extract **all employees** and** return their **first**, **last** and **middle** name, their **job** **title** and **salary**, rounded to **2** **symbols** after the decimal separator, all of those separated with a space. Order them by **employee** **id**.
### <a name="ole_link1"></a><a name="ole_link2"></a>**Example**

|**Output**|
| :-: |
|Guy Gilbert R Production Technician 12500.00|
|Kevin Brown F Marketing Assistant 13500.00|
|Roberto Tamburello  Engineering Manager 43300.00|
|…|
1. ## **Employees with Salary Over 50 000**
**NOTE:** You will need method <a name="ole_link41"></a><a name="ole_link43"></a>**public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)** and **public StartUp** class. 

<a name="ole_link47"></a>Your task is to extract **all employees** with **salary** over **50000**. Return their **first names and salaries** in format **"{firstName} – {salary}". Salary** must be rounded to **2** **symbols,** after the decimal separator. Sort them **alphabetically** by **first** name.
### <a name="ole_link3"></a><a name="ole_link4"></a>**Example**

|**Output**|
| :-: |
|Brian - 72100.00|
|Dylan - 50500.00|
|James - 84100.00|
|…|

Use **Express** **Profiler** and check if the query Entity Framework Core sent is correct (there is only one query, but there may be more that are performed by EF for checks).

![](Aspose.Words.7c598c50-a9ea-420d-aef7-8c3a635922d0.008.png)
1. ## **Employees from Research and Development**
**NOTE:** You will need method <a name="ole_link48"></a><a name="ole_link49"></a>**public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)** and **public StartUp** class. 

<a name="ole_link50"></a><a name="ole_link51"></a>Extract all employees from the <a name="ole_link5"></a><a name="ole_link6"></a><a name="ole_link52"></a><a name="ole_link53"></a>**Research and Development** department. Order them by **salary** (in ascending order), then by **first name** (in descending order). Return only their **first name**, **last name**, **department name** and **salary** rounded to **2** **symbols,** after the decimal separator in the format shown below:
###
### **Example**

|**Output**|
| :-: |
|<a name="ole_link54"></a><a name="ole_link55"></a>Gigi Matthew from <a name="ole_link15"></a><a name="ole_link16"></a><a name="ole_link30"></a>Research and Development - $40900.00|
|Diane Margheim from Research and Development - $40900.00|
|Michael Raheem from Research and Development - $42500.00|
|…|

Use Express Profiler and check if the made query by Entity Framework is correct (there is only one query).

![](Aspose.Words.7c598c50-a9ea-420d-aef7-8c3a635922d0.009.png)
1. ## <a name="ole_link7"></a><a name="ole_link8"></a>**Adding a New Address and Updating Employee**
**NOTE:** You will need method <a name="ole_link56"></a><a name="ole_link57"></a>**public static string AddNewAddressToEmployee(SoftUniContext context)** and **public StartUp** class. 

Create a new address with the **text** "<a name="ole_link58"></a><a name="ole_link59"></a>**Vitoshka 15**" and **TownId** = **4**. Set that address to the employee with last the name "<a name="ole_link60"></a><a name="ole_link61"></a>**Nakov**".

Then order by **descending** all the employees by their **Address'** **Id**, take **10** rows and from them, take the **AddressText**. Return the results each on a new line:
### <a name="ole_link11"></a><a name="ole_link12"></a>**Example**

|**Output**|
| :-: |
|<a name="ole_link31"></a><a name="ole_link32"></a>Vitoshka 15|
|163 Nishava Str, ent A, apt. 1|
|…|

After this **restore** your **database** for the tasks ahead!
### **Hints**
Create the address and find the employee with last name equal to "**Nakov**" in order to assign the address to him.
1. ## **Employees and Projects**
**NOTE:** You will need method <a name="ole_link62"></a><a name="ole_link63"></a>**public static string GetEmployeesInPeriod(SoftUniContext context)** and **public StartUp** class. 

<a name="ole_link64"></a><a name="ole_link65"></a>Find the first **10** employees and print each employee's **first name**, **last name, manager's first name** and **last name. If they** have **projects** started in the period **2001 - 2003** (inclusive), print them with information about their name, start and end date. Then return **all** of their **projects** in the format "--<**ProjectName**> - <**StartDate**> - <**EndDate**>", each on a **new** **row**. If a project has no end date, print "**not finished**" instead.
### **Constraints**
Use date format: "<a name="ole_link66"></a><a name="ole_link67"></a><a name="ole_link68"></a>**M/d/yyyy h:mm:ss tt**".
### <a name="ole_link44"></a>**Example**

|**Output**|
| :-: |
|Guy Gilbert - Manager: Jo Brown|
|<a name="ole_link69"></a><a name="ole_link70"></a>--Half-Finger Gloves - 6/1/2002 12:00:00 AM - 6/1/2003 12:00:00 AM|
|--Women's Tights - 6/1/2002 12:00:00 AM - 6/1/2003 12:00:00 AM|
|Kevin Brown - Manager: David Bradley|
|Roberto Tamburello – Manager: Teeri Duffy|
|--Classic Vest – 6.1.2003 12:00:00 - not finished|
|…|
1. ## **Addresses by Town**
**NOTE:** You will need method <a name="ole_link71"></a><a name="ole_link72"></a>**public static string GetAddressesByTown(SoftUniContext context)** and **public StartUp** class. 

<a name="ole_link73"></a><a name="ole_link74"></a>Find all addresses, **ordered** by the **number of employees** who live there (**descending**), then by **town name** (**ascending**) and finally by **address** **text** (**ascending**). Take only the **first 10 addresses**. For each address return it in the format "<**AddressText**>, <**TownName**> - <**EmployeeCount**> employees"
### <a name="ole_link45"></a>**Example**

|**Output**|
| :-: |
|7726 Driftwood Drive, Monroe - 2 employees|
|163 Nishava Str, ent A, apt. 1, Sofia - 2 employees|
|…|
1. ## **Employee 147**
**NOTE:** You will need method <a name="ole_link77"></a><a name="ole_link78"></a>**public static string GetEmployee147(SoftUniContext context)** and **public StartUp** class. 

<a name="ole_link79"></a><a name="ole_link80"></a>Get the **employee with id 147.** Return only his/her **first name**, **last name**, **job title** and **projects** (print only their names). The projects should be **ordered** **by** **name** (**ascending**). Format of the output.
### <a name="ole_link46"></a>**Example**

|**Output**|
| :-: |
|Linda Randall - Production Technician|
|HL Touring Handlebars|
|…|
1. ## **Departments with More Than 5 Employees**
**NOTE:** You will need method <a name="ole_link81"></a><a name="ole_link82"></a>**public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)** and **public StartUp** class. 

Find **all departments** with more than **5 employees**. Order them by **employee count** (**ascending**), then by **department** **name** (**alphabetically**). For each department, print the **department name** and the **manager's first** and **last name** on the **first row**. Then print the **first** **name**, the **last** **name** and the **job** **title** of every **employee** on a new row. Order the employees by **first** **name** (**ascending**), then by **last** **name** (**ascending**).
Format of the output: For each department print it in the format "<**DepartmentName**> - <**ManagerFirstName**>  <**ManagerLastName**>" and for each employee print it in the format "<**EmployeeFirstName**> <**EmployeeLastName**> - <**JobTitle**>".
### <a name="ole_link17"></a><a name="ole_link18"></a>**Example**

|**Output**|
| :-: |
|Engineering – Terri Duffy|
|Gail Erickson - Design Engineer|
|Jossef Goldberg - Design Engineer|
|…|
1. ## **Find Latest 10 Projects**
**NOTE:** You will need **method <a name="ole_link83"></a><a name="ole_link84"></a>public static string GetLatestProjects(SoftUniContext context)** and **public StartUp** class. 

<a name="ole_link85"></a><a name="ole_link86"></a>Write a program that returns information about the **last 10 started projects**. **Sort** **them by name** lexicographically and return **their name, description and start date**, each on a new row. 
### **Constraints**
Use date format: "<a name="ole_link35"></a><a name="ole_link36"></a><a name="ole_link87"></a><a name="ole_link88"></a><a name="ole_link89"></a>**M/d/yyyy h:mm:ss tt**".
### <a name="ole_link33"></a><a name="ole_link34"></a>**Example**

|**Output**|
| :-: |
|All-Purpose Bike Stand<br>Research, design and development of All-Purpose Bike Stand. Perfect all-purpose bike stand for working on your bike at home. Quick-adjusting clamps and steel construction.<br>9/1/2005 12:00:00 AM|
|…|
1. ## **Increase Salaries**
**NOTE:** You will need method <a name="ole_link90"></a><a name="ole_link91"></a>**public static string IncreaseSalaries(SoftUniContext context)** and **public StartUp** class. 

<a name="ole_link92"></a><a name="ole_link93"></a>Write a program that increases salaries of all employees that are in the <a name="ole_link19"></a><a name="ole_link20"></a><a name="ole_link21"></a>**Engineering**, <a name="ole_link22"></a><a name="ole_link23"></a>**Tool Design**, <a name="ole_link24"></a><a name="ole_link25"></a>**Marketing,** or <a name="ole_link26"></a><a name="ole_link27"></a>**Information Services** department by **12%**. Then **return first name, last name and salary** (2 symbols after the decimal separator)** for those employees, whose salary was increased. Order them by **first** **name** (**ascending**), then by **last** **name** (**ascending**). Format of the output.
### **Example**

|**Output**|
| :-: |
|Ashvini Sharma ($36400.00)|
|Dan Bacon ($30688.00)|
|…|
1. ## **Find Employees by First Name Starting with "Sa"**
**NOTE:** You will need method <a name="ole_link94"></a><a name="ole_link95"></a>**public static string** **GetEmployeesByFirstNameStartingWithSa(SoftUniContext context)** and **public StartUp** class. 

<a name="ole_link96"></a><a name="ole_link97"></a>Write a program that finds all employees whose first name starts with "**Sa**". Return their **first**, **last** **name**, their **job** **title** and **salary** rounded to **2** **symbols** after the decimal separator in the format given in the example below. Order them by the **first** **name**, then by **last** **name** (**ascending**).
### **Constraints**
### Find a way to make your query **case-insensitive.**	
### **Example**

|**Output**|
| :-: |
|Sairaj Uddin - Scheduling Assistant - ($16000.00)|
|Samantha Smith - Production Technician - ($14000.00)|
|…|
1. ## <a name="ole_link9"></a><a name="ole_link10"></a><a name="ole_link13"></a><a name="ole_link14"></a>**Delete Project by Id**
<a name="_hlk127371548"></a>**NOTE:** You will need method <a name="ole_link98"></a><a name="ole_link99"></a>**public static string DeleteProjectById(SoftUniContext context)** and **public StartUp** class. 

<a name="ole_link28"></a><a name="ole_link29"></a>Let's **delete** the project with id **2**. Then, take 10 projects and return their names, each on a new line. Remember to restore your database after this task.
### **Example**

|**Output**|
| :-: |
|Classic Vest|
|Full-Finger Gloves|
|…|
### **Hints**
If we try to delete the project directly:

![](Aspose.Words.7c598c50-a9ea-420d-aef7-8c3a635922d0.010.png)

This happens:

![](Aspose.Words.7c598c50-a9ea-420d-aef7-8c3a635922d0.011.png)

<a name="_hlk127371564"></a>The project is **referenced** by the join (many-to-many) table **EmployeesProjects**. Therefore we cannot safely delete it. First, we need to remove any references to that row in the **Projects** table. 

This is done by removing the project from all employees who reference it.

![](Aspose.Words.7c598c50-a9ea-420d-aef7-8c3a635922d0.012.png)
1. ## **Remove Town**
<a name="_hlk127371572"></a>**NOTE:** You will need method <a name="ole_link100"></a><a name="ole_link101"></a>**public static string RemoveTown(SoftUniContext context)** and **public StartUp** class. 

Write a program that **deletes a town** with name **"Seattle"**. Also, **delete all addresses** that are in those towns. Return the **number** of **addresses** that were **deleted** in format **"{count} addresses in Seattle were deleted".** There will be **employees** living at those addresses, which will be a problem when trying to delete the addresses. So, start by setting the **AddressId** of each employee for the given address to **null**. After all of them are set to **null**, you may safely remove all the addresses from the **context** and finally remove the **given** **town**.
### **Example**

|**Output**|
| :-: |
|44 addresses in Seattle were deleted|





![](Aspose.Words.7c598c50-a9ea-420d-aef7-8c3a635922d0.015.png)![](Aspose.Words.7c598c50-a9ea-420d-aef7-8c3a635922d0.016.png)![](Aspose.Words.7c598c50-a9ea-420d-aef7-8c3a635922d0.017.png)![](Aspose.Words.7c598c50-a9ea-420d-aef7-8c3a635922d0.018.png)![](Aspose.Words.7c598c50-a9ea-420d-aef7-8c3a635922d0.019.png)![](Aspose.Words.7c598c50-a9ea-420d-aef7-8c3a635922d0.020.png)![](Aspose.Words.7c598c50-a9ea-420d-aef7-8c3a635922d0.021.png)![](Aspose.Words.7c598c50-a9ea-420d-aef7-8c3a635922d0.022.png)![](Aspose.Words.7c598c50-a9ea-420d-aef7-8c3a635922d0.023.png)


![](Aspose.Words.7c598c50-a9ea-420d-aef7-8c3a635922d0.013.png)![](Aspose.Words.7c598c50-a9ea-420d-aef7-8c3a635922d0.014.png)![](Aspose.Words.7c598c50-a9ea-420d-aef7-8c3a635922d0.024.png)![](Aspose.Words.7c598c50-a9ea-420d-aef7-8c3a635922d0.025.png)![](Aspose.Words.7c598c50-a9ea-420d-aef7-8c3a635922d0.026.png)

