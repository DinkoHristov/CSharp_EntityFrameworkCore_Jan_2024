
# **Exercises: LINQ**
This document defines the **exercise assignments** for the [Databases Advanced - Entity Framework course @ SoftUni](https://softuni.bg/trainings/4540/entity-framework-core-june-2024)
You can check your solutions in [Judge](https://judge.softuni.org/Contests/3919/LINQ)
# **MusicHub**
People love listening to music, but they see that YouTube is getting older and older. You want to make people happy and you've decided to make a better version of YouTube – **MusicHub**. It's time for you to start coding. Good luck and impress us.
1. ## **MusicHub Database**
You must create a **database** for a **MusicHub**. It should look like this:

![](Aspose.Words.e7227d28-8b9a-4068-a29f-587474a54153.001.png)
### **Constraints**
Your **namespaces** should be:

- **MusicHub** – for your **StartUp** class, if you have one
- **MusicHub.Data** – for your **DbContext**
- **MusicHub.Data.Models** – for your **Models**

Your **models** should be:
#### **Song**
- <a name="ole_link57"></a><a name="ole_link58"></a>**Id** – **integer**, **Primary Key**
- **Name** – **text** with **max length 20** (**required**)
- **Duration** – **TimeSpan** (**required**)
- **CreatedOn** – <a name="ole_link12"></a><a name="ole_link13"></a>**date** (**required**)
- **Genre** – genre **enumeration** with possible values**: <a name="ole_link3"></a><a name="ole_link4"></a>"<a name="ole_link59"></a><a name="ole_link60"></a>Blues, Rap, PopMusic, Rock, Jazz" (required)**
- **AlbumId** – **integer**, **Foreign key**
- **Album** –** the **Song**'s **Album**
- **WriterId** – **integer, Foreign key (required)**
- **Writer** –** the **Song**'s **Writer**
- **Price** – **decimal** (**required**)
- <a name="ole_link5"></a><a name="ole_link6"></a>**SongPerformers** –** a **c**ollection of type **SongPerformer**
#### **Album**
- **Id** – **integer**, **Primary Key**
- **Name** – **text** with **max length 40** (**required**)
- **ReleaseDate** – **date** (**required**)
- **Price** – **calculated property** (the sum of all song prices in the album)
- **ProducerId** – **integer**, **foreign key**
- **Producer** – **the Album's Producer**
- **Songs** – a collection of all **Songs** in the **Album** 
#### **Performer**
- **Id** – **integer**, **Primary Key**
- **FirstName** – **text** with **max length 20** (**required)** 
- **LastName** – **text** with **max length 20** (**required)** 
- **Age** – **integer** (**required**)
- **NetWorth** **–** **decimal** (**required**)
- <a name="ole_link7"></a><a name="ole_link8"></a><a name="ole_link64"></a>**PerformerSongs** – a collection of type **SongPerformer**
#### **Producer**
- **Id** – **integer**, **Primary Key**
- **Name** – **text** with **max length 30** **(**required**)**
- <a name="ole_link11"></a><a name="ole_link14"></a><a name="ole_link61"></a><a name="ole_link62"></a>**Pseudonym** – **text**
- **PhoneNumber** – **text**
- **Albums** – a collection of type **Album**
#### **Writer**
- **Id** – **integer**, **Primary Key**
- **Name** – **text** with **max length 20** (required**)**
- <a name="ole_link15"></a><a name="ole_link16"></a><a name="ole_link63"></a>**Pseudonym** – **text**
- **Songs** – a collection of type **Song**
#### <a name="ole_link17"></a><a name="ole_link19"></a>**SongPerformer**
- **SongId** – **integer**, **Primary Key**
- **Song** – the performer's **Song** (**required**)
- **PerformerId** – **integer**, **Primary Key**
- **Performer** – the **Song**'s **Performer (required)**

### **Table relations**
- **One Song** can have **many Performers**
- **One Permormer** can** have **many Songs**
- **One Writer** can have **many Songs**
- **One Album** can have **many Songs**
- **One Producer** can have **many Albums**

**NOTE:** You will need a constructor, accepting **DbContextOptions** to test your solution in **Judge**!
1. ## **All Albums Produced by Given Producer**
You need to write method **string ExportAlbumsInfo(MusicHubDbContext context, int producerId)** in the **StartUp** class that receives a **ProducerId**. Export **all albums** which are **produced by** the provided **ProducerId**. For each **Album**, get the **Name**, <a name="ole_link31"></a><a name="ole_link32"></a>**ReleaseDate** in format the "<a name="ole_link75"></a><a name="ole_link76"></a>**MM/dd/yyyy**", **ProducerName**, the **Album Songs** with each **Song Name**, **Price** (**formatted to the second digit**) and the **Song WriterName**. **Sort** the **Songs** by **Song** **Name** (**descending**) and by **Writer** (**ascending**). At the end export **the Total Album Price** with exactly **two digits after the decimal place**. **Sort** the **Albums** by their **Total** **Price** (**descending**).
### **Example**

|**Output (producerId = 9)**|
| :-: |
|<p><a name="ole_link1"></a><a name="ole_link2"></a>-AlbumName: Devil's advocate</p><p><a name="ole_link9"></a><a name="ole_link10"></a>-ReleaseDate: 07/21/2018</p><p><a name="ole_link18"></a><a name="ole_link20"></a>-ProducerName: Evgeni Dimitrov</p><p><a name="ole_link21"></a><a name="ole_link22"></a>-Songs:</p><p><a name="ole_link23"></a><a name="ole_link24"></a>---#1</p><p><a name="ole_link25"></a><a name="ole_link26"></a>---SongName: Numb</p><p><a name="ole_link27"></a><a name="ole_link28"></a>---Price: 13.99</p><p><a name="ole_link29"></a><a name="ole_link30"></a>---Writer: Kara-lynn Sharpous</p><p>---#2</p><p>---SongName: Ibuprofen</p><p>---Price: 26.50</p><p>---Writer: Stanford Daykin</p><p><a name="ole_link33"></a><a name="ole_link34"></a>-AlbumPrice: 40.49</p><p>…</p>|
1. ## **Songs Above Given Duration**
You need to write method **string ExportSongsAboveDuration(MusicHubDbContext context, int duration)** in the **StartUp** class that receives **Song** duration** (**integer, in seconds**). Export the songs which are **above** the given duration. For each **Song**, export its **Name**, **Performer Full Name**, **Writer Name**, **Album** **Producer** and **Duration** (**in format**("**c**")). **Sort** the **Songs** by their **Name** (**ascending**), and then by **Writer** (**ascending**). If a **Song** has more than one **Performer**, export all performers and sort them (**ascending, alphabetically**). If there are no **Performers** for a given song, don't print the "**---Performer**" line at all.
### **Example**

|**Output (duration = 4)**|
| :-: |
|<p><a name="ole_link35"></a><a name="ole_link36"></a><a name="ole_link77"></a><a name="ole_link78"></a>-Song #1</p><p><a name="ole_link37"></a><a name="ole_link38"></a>---SongName: Away</p><p><a name="ole_link39"></a><a name="ole_link40"></a>---Writer: Norina Renihan</p><p><a name="ole_link41"></a><a name="ole_link42"></a>---Performer: Lula Zuan</p><p><a name="ole_link43"></a><a name="ole_link44"></a>---AlbumProducer: Georgi Milkov</p><p><a name="ole_link45"></a><a name="ole_link46"></a>---Duration: 00:05:35</p><p>-Song #2</p><p>---SongName: Bentasil</p><p>---Writer: Mik Jonathan</p><p>---Performer: Zabrina Amor</p><p>---AlbumProducer: Dobromir Slavchev</p><p>---Duration: 00:04:03</p><p>-Song #3</p><p>---SongName: Carvedilol</p><p>---Writer: Chloe Trayhorn</p><p>---Performer: Rhody Bettam</p><p>---Performer: Tine Althorp</p><p>---AlbumProducer: Evtim Miloshev</p><p>---Duration: 00:02:39<br>…</p>|





![](Aspose.Words.e7227d28-8b9a-4068-a29f-587474a54153.004.png)![](Aspose.Words.e7227d28-8b9a-4068-a29f-587474a54153.005.png)![](Aspose.Words.e7227d28-8b9a-4068-a29f-587474a54153.006.png)![](Aspose.Words.e7227d28-8b9a-4068-a29f-587474a54153.007.png)![](Aspose.Words.e7227d28-8b9a-4068-a29f-587474a54153.008.png)![](Aspose.Words.e7227d28-8b9a-4068-a29f-587474a54153.009.png)![](Aspose.Words.e7227d28-8b9a-4068-a29f-587474a54153.010.png)![](Aspose.Words.e7227d28-8b9a-4068-a29f-587474a54153.011.png)![](Aspose.Words.e7227d28-8b9a-4068-a29f-587474a54153.012.png)


![](Aspose.Words.e7227d28-8b9a-4068-a29f-587474a54153.002.png)![](Aspose.Words.e7227d28-8b9a-4068-a29f-587474a54153.003.png)![](Aspose.Words.e7227d28-8b9a-4068-a29f-587474a54153.013.png)![](Aspose.Words.e7227d28-8b9a-4068-a29f-587474a54153.014.png)![](Aspose.Words.e7227d28-8b9a-4068-a29f-587474a54153.015.png)

