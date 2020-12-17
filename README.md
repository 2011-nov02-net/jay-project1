## Jay-Project 1  

A web application which allows the customer to make orders for animals to purchase at store locations.  


## Technologies Used  

- ASP.NET Core - Version 3.1.0  
- .NET Standard Library - Version 2.1.0  
- Entity Framework Core - Version 5.0.0  
- Microsoft Azure App Services  
- Azure DevOps  
- Azure SQL Database 
- Sonar Cloud  

## Features  

List of features ready and TODOs for future development

- Methods to create a location and import new animals to the location's inventory  
- Methods to create a customer and update their details  
- Methods to create an order specifying the customer, location and animals in the order  
- Search function for locations by city | customers by name | orders by location or customer  
- Display all details and order history of a location or customer  
- Client-side and server-side validation  
- Logging of all events  
- Deployed on Azure Pipelines  
  
To-do list:  
- Implement user authorization and authentication  
- Add a preference for customer's next order  

## Getting Started  

Run this command in your terminal:  
git clone https://github.com/2011-nov02-net/jay-project1  

## Usage  

Initial webpage will lead you to the welcome screen. From there, navigate to any of the options in the navigation bar.  

![Welcome page](/Aqua.WebApp/wwwroot/Images/WelcomeScreen.png)

User is able to see the details of a store location and import animals to that location.  

![Location page](/Aqua.WebApp/wwwroot/Images/Location.PNG)

User can also see the details of a customer and create new customers.  

![Customer page](/Aqua.WebApp/wwwroot/Images/Customer.PNG)

User can create new orders from a location with multiple animals in it.  

![Order page](/Aqua.WebApp/wwwroot/Images/Order.PNG)

## License  
This project uses the following license: [MIT License](https://github.com/git/git-scm.com/blob/master/MIT-LICENSE.txt).
