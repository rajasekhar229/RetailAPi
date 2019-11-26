# RetailAPi
Solution To My Retail Case Stduy

MyRetail API Solution provides the ability to:


1. Retrieve product and price information by Product Id.
2. Update the price information in the database.

End Points:

Method               Request                  
 GET              /products/{id}             
 PUT              /products/{id}       
 
 Technology Stack:
 
 1.Asp.net web api
 
 2.unit test--Nsubstitute
 
 3.Lite Db.
 
 4.Postman.
 
 5.Visual Studio.

6.Git
 
 
 Set Up instructions
 1.Git hub Download project from the following git repository https://github.com/rajasekhar229/RetailAPi.git
   Or You can clone git repository if you have 
 
 2.Clone the git project from git-bash or command prompt (You must have git setup).
 
 3. Install Visual Studio. 
 
 4. Make Sure ll the references are pointing to right folders.
 
 5 my project lite db is point to c drive , if you are using mac book point it to the right direction.

 6. Install post man for testing.

 7. install litedb studio if you  want to see the data.
 
 
 Testing
 
 1 open solution in visual studio. Build it with no errors

2. when you add products/id to your localhost url on browser.if the price and name of the product is not null from third party       server(redsky) it will be added to lite db

3.lite db will be added to your c drive if you are using macbook point it to the right folder.

4.Everytime when you hit product/id. if item exists it will be added it your litedb and it is returned.

5.if you want to test update use post man or fiddler. it will return the updatd product.

6.you have two tests in unittest project, you can run it through visual studio under test---run--all tests.


