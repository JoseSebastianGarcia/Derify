# Derify

## How to use
You have AddDerify(connection_string) and UseDerify(). Simply add both methods.
![image](https://github.com/JoseSebastianGarcia/Derify/assets/94945762/25f4aa40-5026-433e-a354-a464d2e5ae0a)

Next, you can run the test solution (you will need an sql server instance) and visit "domain/Derify".

That's all, automatically the middleware will generate an ERD from your database.

## Diagramming engine
Written manually with jquery and jqueryui


## Snapshot
![image](https://github.com/JoseSebastianGarcia/Derify/assets/94945762/e4782ef1-0bc4-4368-bd6b-25398da26c76)


CTRL+B or magnifying glass
![a](https://github.com/JoseSebastianGarcia/Derify/assets/94945762/f5217f74-32ea-4c9b-8e03-d1f95c4c9c00)


Now we can hightlight some column with certaint value
![b](https://github.com/JoseSebastianGarcia/Derify/assets/94945762/f05cbbc4-7925-4e1a-98d8-20593060c60e)


You can select many fields as you want by pressing **Ctrl button** and **left mouse click**

v2.1.6
You can share a /derify?q=FieldName URL so other people can see what you want them to see

v2.2.7
You can filter from queryString using Schemes, OmmitTables, OnlyTables 's attributes.
```url
/derify?schemas=dbo,another&ommitTables=users,roles&onlyTables=users,roles,grants
```
