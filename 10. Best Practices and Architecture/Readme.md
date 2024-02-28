# Lab: Best Practices and Architecture

This document defines the **exercise assignments** for the "[Entity Framework Core" course @ Software University](https://softuni.bg/trainings/3221/entity-framework-core-february-2021).

# _Real Estate_ Property Ads

Create a database to hold real estate property ads using Entity Framework Core code-first approach.

The database should follow all good practices including the data normalization.

An ad should contain the following properties:

- **Size**(in square meters)
- **YardSize**(in square meters, for houses only)
- **Floor** in which the property is located
- Total number of **floors** in the building
- **District** name
- Building **year**(if no year is specified the value is 0)
- **Type** of the property (1-room apartment, 2-rooms apartment, studio, etc.)
- Type of the **building**(brick, panel, etc.)
- **Price**(in EUR)
- **Tags** for each property (e.g. OldProperty, HugeApartment, HighFloor, etc.)

Add some **console UI** for listing and filtering the ads and districts.

Implement some tagging logic for each ad.

Import the sample data given on the following links:

[https://github.com/NikolayIT/ArtificialIntelligencePlayground/raw/a844a4ee52404ede2b99c316c18772c2f24c275b/ML.NET/Regression/SofiaPropertiesPricePrediction/imot.bg-raw-data-2021-03-18.json](https://github.com/NikolayIT/ArtificialIntelligencePlayground/raw/a844a4ee52404ede2b99c316c18772c2f24c275b/ML.NET/Regression/SofiaPropertiesPricePrediction/imot.bg-raw-data-2021-03-18.json)

[https://github.com/NikolayIT/ArtificialIntelligencePlayground/raw/2ae08b43cc466e3acdb1d75ab2714dbd6f3c5aba/ML.NET/Regression/SofiaPropertiesPricePrediction/imot.bg-houses-Sofia-raw-data-2021-03-18.json](https://github.com/NikolayIT/ArtificialIntelligencePlayground/raw/2ae08b43cc466e3acdb1d75ab2714dbd6f3c5aba/ML.NET/Regression/SofiaPropertiesPricePrediction/imot.bg-houses-Sofia-raw-data-2021-03-18.json)

**Create five projects:**

### RealEstates.Data

In this project you have to create your DbContext and migrations.

### RealEstates.Models

In this project you have to implement all of you models.

### RealEstates.Services

In this project you will hold your business logic.

### RealEstates.Importer

In this project add a code to import the data from the given links. Also add some appropriate tags to each property.

### RealEstates.ConsoleApplication

In this project add some UI logic for listing and filtering the data.

![Shape3](RackMultipart20240228-1-oxbdcd_html_6f222e41d7629786.gif) ![Shape2](RackMultipart20240228-1-oxbdcd_html_5f0f2ddacbac70d2.gif) ![Shape1](RackMultipart20240228-1-oxbdcd_html_51bd00be29b85496.gif) ![Shape4](RackMultipart20240228-1-oxbdcd_html_f746d52952cd7e91.gif)[![](RackMultipart20240228-1-oxbdcd_html_3aa486326bfa75e9.png)](https://softuni.org/)

Follow us:

© SoftUni – [https://softuni.org](https://softuni.org/). Copyrighted document. Unauthorized copy, reproduction or use is not permitted.

[![](RackMultipart20240228-1-oxbdcd_html_9b17934bfedeb713.png)](https://softuni.org/)[![](RackMultipart20240228-1-oxbdcd_html_c9db196993f48ff8.png)](https://softuni.bg/)[![Software University @ Facebook](RackMultipart20240228-1-oxbdcd_html_94be3df36d913358.png)](https://www.facebook.com/softuni.org)[![](RackMultipart20240228-1-oxbdcd_html_7e8e605369b4ad74.png)](https://www.instagram.com/softuni_org)[![Software University @ Twitter](RackMultipart20240228-1-oxbdcd_html_ff9c629b0a21eb6b.png)](https://twitter.com/SoftUni1)[![Software University @ YouTube](RackMultipart20240228-1-oxbdcd_html_7db86a748da0e575.png)](https://www.youtube.com/channel/UCqvOk8tYzfRS-eDy4vs3UyA)[![](RackMultipart20240228-1-oxbdcd_html_95554caa563bbdb3.png)](https://www.linkedin.com/company/softuni/)[![](RackMultipart20240228-1-oxbdcd_html_4df51bfadcab813.png)](https://github.com/SoftUni)[![Software University: Email Us](RackMultipart20240228-1-oxbdcd_html_d7fa82ab7332f3fa.png)](mailto:info@softuni.org)

Page 1 of 1