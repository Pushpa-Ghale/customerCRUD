# customerCRUD (AZURE FUNCTION)
Hello,
I heard a lot about azure function and was just curious to learn it. Its pretty cool and fast to setup.
This is one of the test project I was working on and might be helpful to super beginners to azure function.

Please note that you will have to have an azure account in order to deploy this solution.
This code does CRUD operations based on the body of the HTTP post call (this is an HTTP trigger azure funtion).

For eg:
To Insert records,
{"action":"Insert","fname":"puspa","lname":"ghale", "id": 54, "phnum": "3498729437"}

To Update Records,
{"action":"Update","fname":"puspa2","lname":"ghale2", "id": 54, "phnum": "3498729437"}

To Delete Record,
{"action":"Delete","id": 54}

Note that you will also need to have a connection string for a SQL database in order to do this CRUD operations.
For now I have added the connection string in the code itself but the best practice would be to add it to the Application Settings for the Azure funciton (just like App.config in console app)
