INSERT INTO Users (Name, Email, Password)
VALUES ('John Doe', 'john.doe@example.com', 'Password123');

SELECT Id, Name, Email
FROM Users
WHERE Email = 'john.doe@example.com' AND Password = 'Password123';
