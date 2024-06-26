
# **Exercises: Advanced Querying**
This document defines the **exercise assignments** for the [Databases Advanced - Entity Framework course @ SoftUni](https://softuni.bg/trainings/4540/entity-framework-core-june-2024)
<a name="_hlk105500510"></a>You can check your solutions in [Judge](https://judge.softuni.org/Contests/3920/Advanced-Querying)
# **BookShop System**
For the following tasks, use the **BookShop** database. You can download the complete project or create it,** but you should still use the pre-defined **Seed()** method from the project to have the same **sample** data.
1. ## **Book Shop Database**
You must create a **database** for a **book** **shop** **system**. It should look like this:

![](Aspose.Words.448fe503-92b1-4032-a4f1-f053fcd7815c.001.png)
### **Constraints**
Your **namespaces** should be:

- **BookShop** – for your **StartUp** class
- **BookShop.Data** – for your **DbContext**
- **BookShop.Models** – for your models** 
- **BookShop.Models**.**Enums** – for your models

Your **models** should be:

- **BookShopContext** – your **DbContext**
- **Author**
  - **AuthorId**
  - **FirstName** (up to **50** characters, unicode, not required)
  - **LastName** (up to **50** characters, unicode)
- **Book**
  - **BookId**
  - **Title** (up to **50** characters, unicode)
  - **Description** (up to **1000** characters, unicode)
  - **ReleaseDate** (not required)
  - **Copies** (an integer)
  - **Price**
  - **EditionType** – enum (**Normal**, **Promo**, **Gold**)
  - **AgeRestriction** – enum (**Minor**, **Teen**, **Adult**)
  - **Author**
  - **BookCategories**
- **Category**
  - **CategoryId**
  - **Name** (up to **50** characters, unicode)
  - **CategoryBooks**
- **BookCategory** – mapping entity

For the following tasks, you will be creating methods that accept a **BookShopContext** as a parameter and use it to run some queries. Create those methods inside your **StartUp** class and upload your whole solution to **Judge**.
1. ## **Age Restriction**
**NOTE**: You will need method **public static string GetBooksByAgeRestriction(BookShopContext context, string command)** and **public StartUp** class. 

Return in a **single** **string** all** book **titles**, each on a **new line,** that have **an age** **restriction**, equal to the **given** **command**. Order the titles **alphabetically**.

Read **input** from the console in your **main** **method** and call your **method** with the **necessary** **arguments**. Print the **returned** **string** to the console. **Ignore** the casing of the input.
### **Example**

|**Input**|**Output**|
| :-: | :-: |
|miNor|<p>A Confederacy of Dunces</p><p>A Farewell to Arms</p><p>A Handful of Dust</p><p>…</p>|
|teEN|<p>A Passage to India</p><p>A Scanner Darkly</p><p>A Swiftly Tilting Planet</p><p>…</p>|
1. ## **Golden Books**
**NOTE**: You will need **method public static string GetGoldenBooks(BookShopContext context)** and **public StartUp** class. 

Return in a **single** string the **titles of the golden edition books** that have **less than 5000 copies**,** each on a **new line**. Order them by **BookId** ascending.

Call the **GetGoldenBooks(BookShopContext context)** method in your **Main()** and print the returned string to the console.
### **Example**

|**Output**|
| :-: |
|<p>Behold the Man</p><p>Bury My Heart at Wounded Knee</p><p>The Cricket on the Hearth</p><p>…</p>|
1. ## **Books by Price**
**NOTE**: You will need method **public static string GetBooksByPrice(BookShopContext context)** and **public StartUp** class. 

Return in a single string all **titles and prices** **of books** with a **price higher than 40**, each on a **new** **row** in the **format** given below. Order them by **price** descending.
### **Example**

|**Output**|
| :-: |
|<p>O Pioneers! - $49.90</p><p>That Hideous Strength - $48.63</p><p>A Handful of Dust - $48.63</p><p>…</p>|
1. ## **Not Released In**
**NOTE**: You will need method <a name="ole_link4"></a><a name="ole_link5"></a>**public static string GetBooksNotReleasedIn(BookShopContext** **context, int year)** and **public StartUp** class. 

<a name="ole_link6"></a><a name="ole_link7"></a>Return in a **single** string with all **titles of books** that are **NOT released** in a given year. Order them by **bookId** ascending.
### **Example**

|**Input**|**Output**|
| :-: | :-: |
|2000|<p>Absalom</p><p>After Many a Summer Dies the Swan</p><p>Ah</p><p>…</p>|
|<a name="ole_link8"></a><a name="ole_link9"></a>1998|<p>Ah</p><p>Wilderness!</p><p>Alien Corn? (play)</p><p>…</p>|
1. ## **Book Titles by Category**
**NOTE**: You will need method <a name="ole_link10"></a><a name="ole_link11"></a>**public static string GetBooksByCategory(BookShopContext context, string input)** and **public StartUp** class. 

<a name="ole_link12"></a><a name="ole_link13"></a>Return** in a single string the **titles of books** by a given **list of categories**. The list of **categories** will be given in a single line separated by one or more spaces. Ignore casing. Order by **title** alphabetically.
### **Example**

|**Input**|**Output**|
| :-: | :-: |
|<a name="ole_link14"></a><a name="ole_link15"></a>horror mystery drama|<p>A Fanatic Heart</p><p>A Farewell to Arms</p><p>A Glass of Blessings</p><p>…</p>|
1. ## **Released Before Date**
**NOTE**: You will need method <a name="ole_link16"></a><a name="ole_link17"></a>**public static string GetBooksReleasedBefore(BookShopContext context, string date)** and **public StartUp** class. 

<a name="ole_link18"></a><a name="ole_link19"></a>Return **the title, edition type** and **price** of **all books** that are **released before a given date.** The date will be a string in the format **"dd-MM-yyyy".**

Return all of the rows in a single string, ordered by **release date (descending)**.
### **Example**

|**Input**|**Output**|
| :-: | :-: |
|<a name="ole_link22"></a>12-04-1992|<p>If I Forget Thee Jerusalem - Gold - $33.21</p><p>Oh! To be in England - Normal - $46.67</p><p>The Monkey's Raincoat - Normal - $46.93</p><p>…</p>|
|<a name="ole_link23"></a><a name="ole_link24"></a>30-12-1989|<p>A Fanatic Heart - Normal - $9.41</p><p>The Curious Incident of the Dog in the Night-Time - Normal - $23.41</p><p>The Other Side of Silence - Gold - $46.26</p><p>…</p>|
1. ## **Author Search**
**NOTE**: You will need method <a name="ole_link25"></a><a name="ole_link26"></a>**public static string GetAuthorNamesEndingIn(BookShopContext context, string input)** and **public StartUp** class. 

<a name="ole_link27"></a><a name="ole_link28"></a>Return the **full** **names** of **authors**, whose **first** **name** ends with a **given** **string**.

Return all **names** in a **single** **string**, each on a **new** **row**, ordered alphabetically.
### **Example**

|**Input**|**Output**|
| :-: | :-: |
|e|<p>George Powell</p><p>Jane Ortiz</p>|
|dy|Randy Morales|
1. ## **Book Search**
**NOTE**: You will need method <a name="ole_link29"></a><a name="ole_link30"></a>**public static string GetBookTitlesContaining(BookShopContext context, string input)** **and public StartUp** class. 

Return the **titles** of **the book**, which contain a **given** **string**. Ignore casing.

Return all **titles** in a **single** **string**, each on a **new** **row**, ordered alphabetically.
### **Example**

|**Input**|**Output**|
| :-: | :-: |
|sK|<p>A Catskill Eagle</p><p>The Daffodil Sky</p><p>The Skull Beneath the Skin</p>|
|WOR|<p>Great Work of Time</p><p>Terrible Swift Sword</p>|
1. ## **Book Search by Author**
**NOTE**: You will need method <a name="ole_link31"></a><a name="ole_link32"></a>**public static string GetBooksByAuthor(BookShopContext context, string input)** and **public StartUp** class. 

Return **all titles of books and their authors' names** for books, which are written by authors whose last names **start with the given string**.

Return a single string with each title on a new row. **Ignore** casing. Order by **BookId** ascending.
### **Example**

|**Input**|**Output**|
| :-: | :-: |
|R|<p>A Handful of Dust (Bozhidara Rysinova)</p><p>Have His Carcase (Bozhidara Rysinova)</p><p>The Heart Is a Lonely Hunter (Bozhidara Rysinova) </p><p>…</p>|
|po|<p>Postern of Fate (Stanko Popov)</p><p>Precious Bane (Stanko Popov)</p><p>The Proper Study (Stanko Popov)</p><p>…</p>|
1. ## **Count Books**
**NOTE**: You will need method <a name="ole_link35"></a><a name="ole_link36"></a>**public static int CountBooks(BookShopContext context, int lengthCheck)** and **public StartUp** class. 

Return **the number of books,** which have a **title longer than the number** given as an input.
### **Example**

|**Input**|**Output**|**Comments**|
| :-: | :-: | :-: |
|12|169|There are 169 books with longer title than 12 symbols|
|40|2|There are 2 books with longer title than 40 symbols|
1. ## **Total Book Copies**
**NOTE**: You will need method <a name="ole_link37"></a><a name="ole_link38"></a>**public static string CountCopiesByAuthor(BookShopContext context)** and **public StartUp** class. 

Return the **total number of book copies** **for each author**. Order the results **descending by total book copies**.

Return all results in a **single** **string**, each on a **new** **line**.
### **Example**

|**Output**|
| :-: |
|<p>Stanko Popov - 117778</p><p>Lyubov Ivanova - 107391</p><p>Jane Ortiz – 103673</p><p>…</p>|
1. ## **Profit by Category**
**NOTE**: You will need method <a name="ole_link39"></a><a name="ole_link40"></a>**public static string GetTotalProfitByCategory(BookShopContext** **context)** and **public StartUp** class. 

Return the **total profit of all books by category**. Profit for a book can be calculated by multiplying its **number of copies** by the **price per single book**. Order the results by **descending by total profit** for a category and **ascending by category name**. Print the total profit formatted to the **second digit**.
### **Example**

|**Output**|
| :-: |
|<p>Art $6428917.79</p><p>Fantasy $5291439.71</p><p>Adventure $5153920.77</p><p>Children's $4809746.22</p><p>…</p>|
1. ## **Most Recent Books**
**NOTE**: You will need method <a name="ole_link41"></a><a name="ole_link42"></a>**public static string GetMostRecentBooks(BookShopContext context)** and **public StartUp** class.

Get the most recent books by categories. The **categories** should be ordered by **name alphabetically**. Only take the **top 3** most recent books from each category – ordered by **release date** (descending). **Select** and **print** the **category name** and for each **book** – its **title** and **release year**.
### **Example**

|`                                                      `**Output**|
| :- |
|<p>--Action</p><p>Brandy ofthe Damned (2015)</p><p>Bonjour Tristesse (2013)</p><p>By Grand Central Station I Sat Down and Wept (2010)</p><p>--Adventure</p><p>The Cricket on the Hearth (2013)</p><p>Dance Dance Dance (2002)</p><p>Cover Her Face (2000)</p><p>…</p>|
1. ## **Increase Prices**
**NOTE**: You will need method <a name="ole_link43"></a><a name="ole_link44"></a>**public static void IncreasePrices(BookShopContext context)** and **public StartUp** class.

<a name="ole_link45"></a><a name="ole_link46"></a>**Increase the prices of all books** **released before 2010 by 5**.
1. ## **Remove Books**
**NOTE**: You will need method <a name="ole_link47"></a><a name="ole_link48"></a>**public static int RemoveBooks(BookShopContext context)** and **public** **StartUp** class.

Remove** all **books**, which have less than **4200 copies**. Return an **int** - the **number of books that were deleted** from the database.
### **Example**

|**Output**|
| :-: |
|<a name="ole_link49"></a><a name="ole_link50"></a>34|





![](Aspose.Words.448fe503-92b1-4032-a4f1-f053fcd7815c.004.png)![](Aspose.Words.448fe503-92b1-4032-a4f1-f053fcd7815c.005.png)![](Aspose.Words.448fe503-92b1-4032-a4f1-f053fcd7815c.006.png)![](Aspose.Words.448fe503-92b1-4032-a4f1-f053fcd7815c.007.png)![](Aspose.Words.448fe503-92b1-4032-a4f1-f053fcd7815c.008.png)![](Aspose.Words.448fe503-92b1-4032-a4f1-f053fcd7815c.009.png)![](Aspose.Words.448fe503-92b1-4032-a4f1-f053fcd7815c.010.png)![](Aspose.Words.448fe503-92b1-4032-a4f1-f053fcd7815c.011.png)![](Aspose.Words.448fe503-92b1-4032-a4f1-f053fcd7815c.012.png)


![](Aspose.Words.448fe503-92b1-4032-a4f1-f053fcd7815c.002.png)![](Aspose.Words.448fe503-92b1-4032-a4f1-f053fcd7815c.003.png)![](Aspose.Words.448fe503-92b1-4032-a4f1-f053fcd7815c.013.png)![](Aspose.Words.448fe503-92b1-4032-a4f1-f053fcd7815c.014.png)![](Aspose.Words.448fe503-92b1-4032-a4f1-f053fcd7815c.015.png)

