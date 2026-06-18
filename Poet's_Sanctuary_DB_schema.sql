create database SE_PROJECT1
use SE_PROJECT1



-- 1) Users Table with Authentication and Roles
CREATE TABLE Users (
    User_ID INT IDENTITY(1,1) PRIMARY KEY,
    First_Name VARCHAR(100) NOT NULL,    --storing the Full Name
    Namee VARCHAR(100),  -- Storing the Username
    Email VARCHAR(100) NOT NULL UNIQUE,
    Password VARCHAR(255) NOT NULL,
    Role VARCHAR(20) NOT NULL DEFAULT 'User'
);

-- 2) Admin Table (separate table for admin users)
CREATE TABLE Admin (
    user_ID INT IDENTITY(1,1) PRIMARY KEY,
    First_Name VARCHAR(100) NOT NULL,  --storing username ffor admin
    Password VARCHAR(255) NOT NULL
);

alter table Admin add email varchar(30)
alter table Admin add namee varchar(50) --storing Full Name for admin

-- 3) Poets Table
CREATE TABLE Poets (
    PoetID INT PRIMARY KEY IDENTITY(1,1),
    Name VARCHAR(100) NOT NULL,
    Era VARCHAR(50),
    Bio TEXT,
    Timeline TEXT,
    ImagePath VARCHAR(255)
);

-- 4) User Preferences Table
CREATE TABLE UserPreferences (
    PreferenceID INT IDENTITY(1,1) PRIMARY KEY,
    UserID INT NOT NULL,
    PoetID INT NOT NULL,
    FOREIGN KEY (UserID) REFERENCES Users(User_ID) ON DELETE CASCADE,
    FOREIGN KEY (PoetID) REFERENCES Poets(PoetID) ON DELETE CASCADE
);

-- 5) Poetry Table
CREATE TABLE Poetry (
    PoetryID INT IDENTITY(1,1) PRIMARY KEY,
    PoetID INT NULL,
    UserID INT NULL,
    Title NVARCHAR(200) NOT NULL,
    Content TEXT NOT NULL,
    Language VARCHAR(50),
    PoetryType VARCHAR(20) NOT NULL,
    Theme VARCHAR(100),
    FOREIGN KEY (PoetID) REFERENCES Poets(PoetID) ON DELETE SET NULL,
    FOREIGN KEY (UserID) REFERENCES Users(User_ID) ON DELETE SET NULL,
    CONSTRAINT chk_PoetryType CHECK (PoetryType IN ('Nazm', 'Ghazal', 'Marsiya', 'Qasida', 'Rubai', 'Hamd', 'Naat'))
);
ALTER TABLE Poetry
DROP CONSTRAINT chk_PoetryType;
ALTER TABLE Poetry
ADD CONSTRAINT chk_PoetryType
CHECK (PoetryType IN ('Nazm', 'Ghazal', 'Marsiya', 'Qasida', 'Rubai', 'Hamd', 'Naat', 'Kafi'));

alter table poetry add top1 int

ALTER TABLE Poetry
ALTER COLUMN Title NVARCHAR(200) NOT NULL;


-- 6) Ratings Table
CREATE TABLE Ratings (
    RatingID INT IDENTITY(1,1) PRIMARY KEY,
    UserID INT,
    PoetryID INT,
    Rating INT CHECK (Rating BETWEEN 1 AND 5),
    FOREIGN KEY (UserID) REFERENCES Users(User_ID) ON DELETE CASCADE,
    FOREIGN KEY (PoetryID) REFERENCES Poetry(PoetryID) ON DELETE CASCADE
);

-- 7) Comments Table
CREATE TABLE Comments (
    CommentID INT IDENTITY(1,1) PRIMARY KEY,
    UserID INT,
    PoetryID INT,
    CommentText TEXT NOT NULL,
    Timestamp DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (UserID) REFERENCES Users(User_ID) ON DELETE CASCADE,
    FOREIGN KEY (PoetryID) REFERENCES Poetry(PoetryID) ON DELETE CASCADE
);

-- 8) Poetry Categories
CREATE TABLE Categories (
    CategoryID INT IDENTITY(1,1) PRIMARY KEY,
    Name VARCHAR(100) NOT NULL
);

CREATE TABLE PoetryCategories (
    PoetryID INT,
    CategoryID INT,
    FOREIGN KEY (PoetryID) REFERENCES Poetry(PoetryID) ON DELETE CASCADE,
    FOREIGN KEY (CategoryID) REFERENCES Categories(CategoryID) ON DELETE CASCADE,
    PRIMARY KEY (PoetryID, CategoryID)
);

-- 9) Poetry Audio
CREATE TABLE PoetryAudio (
    AudioID INT IDENTITY(1,1) PRIMARY KEY,
    PoetryID INT,
    UserID INT,
    AudioFilePath VARCHAR(255) NOT NULL,
    Duration INT,
    FOREIGN KEY (PoetryID) REFERENCES Poetry(PoetryID) ON DELETE CASCADE,
    FOREIGN KEY (UserID) REFERENCES Users(User_ID) ON DELETE CASCADE
);

-- 10) Poetry Contests
CREATE TABLE PoetryContests (
    ContestID INT IDENTITY(1,1) PRIMARY KEY,
    Title VARCHAR(200) NOT NULL,
    Theme VARCHAR(100),
    StartDate DATE NOT NULL,
    EndDate DATE NOT NULL
);

-- 11) Contest Entries
CREATE TABLE ContestEntries (
    EntryID INT IDENTITY(1,1) PRIMARY KEY,
    ContestID INT,
    UserID INT,
    PoetryID INT,
    SubmissionDate DATE DEFAULT GETDATE(),
    Votes INT DEFAULT 0,
    FOREIGN KEY (ContestID) REFERENCES PoetryContests(ContestID) ON DELETE CASCADE,
    FOREIGN KEY (UserID) REFERENCES Users(User_ID) ON DELETE CASCADE,
    FOREIGN KEY (PoetryID) REFERENCES Poetry(PoetryID) ON DELETE CASCADE
);

-- 12) Contest Voting
CREATE TABLE ContestVotes (
    VoteID INT IDENTITY(1,1) PRIMARY KEY,
    EntryID INT NOT NULL,
    UserID INT NOT NULL,
    CONSTRAINT FK_ContestVotes_Entry FOREIGN KEY (EntryID) REFERENCES ContestEntries(EntryID) ON DELETE NO ACTION,
    CONSTRAINT FK_ContestVotes_User FOREIGN KEY (UserID) REFERENCES Users(User_ID) ON DELETE NO ACTION,
    CONSTRAINT UQ_ContestVotes_UserEntry UNIQUE (UserID, EntryID)
);

-- 13) User Activity
CREATE TABLE UserActivity (
    ActivityID INT IDENTITY(1,1) PRIMARY KEY,
    UserID INT,
    Action VARCHAR(100) NOT NULL,
    Timestamp DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (UserID) REFERENCES Users(User_ID) ON DELETE CASCADE
);

-- 14) Poet Timeline Events
CREATE TABLE PoetEvents (
    EventID INT IDENTITY(1,1) PRIMARY KEY,
    PoetID INT,
    EventTitle VARCHAR(200),
    EventDate DATE,
    Description TEXT,
    FOREIGN KEY (PoetID) REFERENCES Poets(PoetID) ON DELETE CASCADE
);

-- 15) Poetry Translations
CREATE TABLE PoetryTranslations (
    TranslationID INT IDENTITY(1,1) PRIMARY KEY,
    PoetryID INT,
    Language VARCHAR(50) NOT NULL,
    TranslatedContent TEXT NOT NULL,
    FOREIGN KEY (PoetryID) REFERENCES Poetry(PoetryID) ON DELETE CASCADE
);


-- 16) Followers Table
CREATE TABLE Followers (
    FollowerID INT IDENTITY(1,1) PRIMARY KEY,
    UserID INT NOT NULL,
    FollowedUserID INT NOT NULL,
    FOREIGN KEY (UserID) REFERENCES Users(User_ID) ON DELETE NO ACTION, 
    FOREIGN KEY (FollowedUserID) REFERENCES Users(User_ID) ON DELETE NO ACTION,  
    CONSTRAINT UQ_UserFollow UNIQUE (UserID, FollowedUserID),
    CONSTRAINT CHK_NoSelfFollow CHECK (UserID <> FollowedUserID)
);


-- 17) Poetry Analytics Table
CREATE TABLE PoetryAnalytics (
    AnalyticsID INT IDENTITY(1,1) PRIMARY KEY,
    PoetryID INT,
    Views INT DEFAULT 0,
    TotalRatings INT DEFAULT 0,
    AverageRating DECIMAL(3,2) DEFAULT 0.00,
    FavoritesCount INT DEFAULT 0,
    CommentsCount INT DEFAULT 0,
    FOREIGN KEY (PoetryID) REFERENCES Poetry(PoetryID) ON DELETE CASCADE
);



-- Urdu Poets
INSERT INTO Poets (Name, Era, Bio, Timeline, ImagePath) VALUES ('Allama Iqbal', '20th Century', 'Philosopher, poet, and politician.', '1877–1938', 'images/iqbal.jpg');
INSERT INTO Poets (Name, Era, Bio, Timeline, ImagePath) VALUES ('Faiz Ahmed Faiz', '20th Century', 'Renowned Urdu poet known for revolutionary poetry.', '1911–1984', 'images/faiz.jpg');
INSERT INTO Poets (Name, Era, Bio, Timeline, ImagePath) VALUES ('Mirza Ghalib', '19th Century', 'Iconic Urdu and Persian poet.', '1797–1869', 'images/ghalib.jpg');
INSERT INTO Poets (Name, Era, Bio, Timeline, ImagePath) VALUES ('Ahmed Faraz', '20th Century', 'Famous for romantic and resistance poetry.', '1931–2008', 'images/faraz.jpg');
INSERT INTO Poets (Name, Era, Bio, Timeline, ImagePath) VALUES ('Parveen Shakir', '20th Century', 'Known for feminist and romantic poetry.', '1952–1994', 'images/parveen.jpg');
INSERT INTO Poets (Name, Era, Bio, Timeline, ImagePath) VALUES ('Josh Malihabadi', '20th Century', 'Famous for nationalistic poetry.', '1898–1982', 'images/josh.jpg');
INSERT INTO Poets (Name, Era, Bio, Timeline, ImagePath) VALUES ('Jaun Elia', '20th Century', 'Modern poet known for dark and philosophical verses.', '1931–2002', 'images/jaun.jpg');
INSERT INTO Poets (Name, Era, Bio, Timeline, ImagePath) VALUES ('Nasir Kazmi', '20th Century', 'Delicate poet of loss and nostalgia.', '1925–1972', 'images/nasir.jpg');
INSERT INTO Poets (Name, Era, Bio, Timeline, ImagePath) VALUES ('Majeed Amjad', '20th Century', 'Existential and lyrical poet.', '1914–1974', 'images/majeed.jpg');
INSERT INTO Poets (Name, Era, Bio, Timeline, ImagePath) VALUES ('Mir Taqi Mir', '18th Century', 'One of the founders of Urdu poetry.', '1723–1810', 'images/mir.jpg');

-- Punjabi Poets
INSERT INTO Poets (Name, Era, Bio, Timeline, ImagePath) VALUES ('Bulleh Shah', '17th Century', 'Mystic Punjabi Sufi poet.', '1680–1757', 'images/bulleh.jpg');
INSERT INTO Poets (Name, Era, Bio, Timeline, ImagePath) VALUES ('Shah Hussain', '16th Century', 'Punjabi Sufi poet, originator of Kafi.', '1538–1599', 'images/hussain.jpg');
INSERT INTO Poets (Name, Era, Bio, Timeline, ImagePath) VALUES ('Waris Shah', '18th Century', 'Author of Heer Ranjha.', '1722–1798', 'images/waris.jpg');
INSERT INTO Poets (Name, Era, Bio, Timeline, ImagePath) VALUES ('Sultan Bahu', '17th Century', 'Punjabi Sufi saint and poet.', '1628–1691', 'images/bahu.jpg');
INSERT INTO Poets (Name, Era, Bio, Timeline, ImagePath) VALUES ('Mian Muhammad Bakhsh', '19th Century', 'Wrote Saif-ul-Malook.', '1830–1907', 'images/bakhsh.jpg');
INSERT INTO Poets (Name, Era, Bio, Timeline, ImagePath) VALUES ('Khushal Khan Khattak', '17th Century', 'Pashto warrior-poet with Punjabi influence.', '1613–1689', 'images/khushal.jpg');
INSERT INTO Poets (Name, Era, Bio, Timeline, ImagePath) VALUES ('Najm Hosain Syed', '20th Century', 'Modern Punjabi playwright and poet.', '1935–present', 'images/najm.jpg');
INSERT INTO Poets (Name, Era, Bio, Timeline, ImagePath) VALUES ('Sharif Kunjahi', '20th Century', 'Prominent Punjabi modernist poet.', '1914–2007', 'images/sharif.jpg');
INSERT INTO Poets (Name, Era, Bio, Timeline, ImagePath) VALUES ('Munir Niazi', '20th Century', 'Famous for unique metaphors in Urdu and Punjabi.', '1928–2006', 'images/munir.jpg');
INSERT INTO Poets (Name, Era, Bio, Timeline, ImagePath) VALUES ('Baba Farid', '12th Century', 'One of the earliest Punjabi Sufi poets.', '1173–1266', 'images/farid.jpg');


truncate table poetry

INSERT INTO Poetry (PoetID, Title, Content, Language, PoetryType, Theme, top1)
VALUES (
    3,
    N'کوئی اُمید بر نہیں آتی',
    N'کوئی اُمید بر نہیں آتی  
کوئی صورت نظر نہیں آتی  
  
موت کا ایک دن معین ہے  
نیند کیوں رات بھر نہیں آتی  
  
آگے آتی تھی حالِ دل پہ ہنسی  
اب کسی بات پر نہیں آتی  
  
جانتا ہوں ثوابِ طاعت و زُہد  
پر طبیعت ادھر نہیں آتی  
  
ہے کچھ ایسی ہی بات جو چپ ہوں  
ورنہ کیا بات کر نہیں آتی  
  
کیوں نہ چیخوں کہ یاد کرتے ہیں  
میری آواز گر نہیں آتی  
  
داغ دل گر نظر نہیں آتا  
بو بھی اے چارہ گر نہیں آتی  
  
ہم وہاں ہیں جہاں سے ہم کو بھی  
کچھ ہماری خبر نہیں آتی  
  
مرتے ہیں آرزو میں مرنے کی  
موت آتی ہے پر نہیں آتی  
  
کعبہ کس منہ سے جاؤ گے غالب  
شرم تم کو مگر نہیں آتی',
    N'Urdu',
    'Ghazal',
    'Hopelessness'
);
INSERT INTO Poetry (PoetID, Title, Content, Language, PoetryType, Theme)
VALUES (
    2,
    N'ہم دیکھیں گے',
    N'ہم دیکھیں گے  
لازم ہے کہ ہم بھی دیکھیں گے  
وہ دن کہ جس کا وعدہ ہے  
جو لوحِ ازل میں لکھا ہے  
جب ظلم و ستم کے کوہِ گراں  
روئی کی طرح اُڑ جائیں گے  
ہم محکوموں کے پاؤں تلے  
یہ دھرتی دھڑ دھڑ دھڑکے گی  
اور اہلِ حکم کے سر اوپر  
جب بجلی کڑ کڑ کڑکے گی  
ہم دیکھیں گے  
لازم ہے کہ ہم بھی دیکھیں گے  
جب تخت اُچھالے جائیں گے  
جب تاج گرائے جائیں گے  
ہم اہلِ صفا مردودِ حرم  
مسند پہ بٹھائے جائیں گے  
سب تاج اُچھالے جائیں گے  
سب تخت گرائے جائیں گے  
ہم دیکھیں گے  
لازم ہے کہ ہم بھی دیکھیں گے  
بس نام رہے گا اللہ کا  
جو غائب بھی ہے، حاضر بھی  
جو منظر بھی ہے، ناظر بھی  
اُٹھے گا انا الحق کا نعرہ  
جو میں بھی ہوں، اور تم بھی ہو  
اور راج کرے گی خلقِ خدا  
جو میں بھی ہوں، اور تم بھی ہو',
    N'Urdu',
    'Nazm',
    'Revolution'
);


INSERT INTO Poetry (PoetID, Title, Content, Language, PoetryType, Theme)
VALUES (
    3,
    N'ہزاروں خواہشیں ایسی',
    N'ہزاروں خواہشیں ایسی کہ ہر خواہش پہ دم نکلے  
بہت نکلے میرے ارمان لیکن پھر بھی کم نکلے  

دلی میں گزر گئے وہ موسم، جو ہیں خوابوں کے گزر جانے  
وہ چاند راتیں، وہ ہم اور وہ تم نکلے  

دیکھا ہے دنیا نے ایک عجیب منظر، کچھ یوں  
وہ جو پلے تھا، وہ الگ تھا، جو کھلتا تھا، وہ کم نکلے  

ہمیں دیکھا تھا گزرنا سر پہ چمکتے ہوئے قمر سے  
کہ دل نے جتنا چاہا، وہ کم نکلے  

میرے دل کے اندر کی سازشوں کو میں بے حد دیکھوں  
جو پھر بھی یاد کرنی چاہییں وہ تم نکلے',
    N'Urdu',
    'Ghazal',
    'Desire'
);


INSERT INTO Poetry (PoetID, Title, Content, Language, PoetryType, Theme)
VALUES (
    1,
    N'لب پہ آتی ہے دعا',
    N'لب پہ آتی ہے دعا بن کے تمنا میری  
زندگی شمع کی مانند ہو تمنا میری  

خوابوں کی اڑان ہو، حقیقت کی روشنی ہو  
جو دلوں کی گہرائیوں میں جلن ہو تمنا میری  

ہزاروں رنگ ہیں دل میں، خوابوں کے حصار سے  
میرے دل کی صدا ہوں تمہاری ہنسی تمنا میری  

میری آنکھوں میں چمک ہو، کسی بھری دنیا میں  
تمہارا رنگ ہو، شمع کی ایک چمک تمنا میری',
    N'Urdu',
    'Nazm',
    'Hope'
);



INSERT INTO Poetry (PoetID, Title, Content, Language, PoetryType, Theme)
VALUES (
    2,
    N'ہمیں دیکھنا ہے',
    N'ہمیں دیکھنا ہے، جو چاند کی طرح چمکیں  
ہمیں دیکھنا ہے، جو راتوں کی طرح ڈوبیں  

یہ جو کھوجتے ہیں لوگ، ان کا کوئی نہیں رہتا  
ہمیں دیکھنا ہے، جو بے گلہ ہوں اور کم تر ہوں  
',
    N'Urdu',
    'Ghazal',
    'Love'
);


INSERT INTO Poetry (PoetID, Title, Content, Language, PoetryType, Theme)
VALUES (
    4,
    N'دیکھ لو',
    N'دیکھ لو، میں تمہارے لیے ہوں  
دیکھ لو، دل میں ایک تمھارے سوا نہیں ہوں  

یہ جو سنگیت ہے، تمہارے نام کی لوری  
وہ سب تمھارے خوابوں کی روشنی ہیں  
',
    N'Urdu',
    'Ghazal',
    'Love'
);


INSERT INTO Poetry (PoetID, Title, Content, Language, PoetryType, Theme)
VALUES (
    5,
    N'خوابوں کا رنگ',
    N'خوابوں کا رنگ، تمہاری آنکھوں سے آیا  
دل کا رنگ، تمہاری باتوں سے آیا  

چاند کی روشنی میں، ہم تمہارا چہرہ دیکھیں  
زندگی کی لذتیں، تمہاری ہنسی میں چھپی ہیں  
',
    N'Urdu',
    'Nazm',
    'Love'
);


INSERT INTO Poetry (PoetID, Title, Content, Language, PoetryType, Theme)
VALUES (
    6,
    N'تم یاد آنا',
    N'تم یاد آنا، یہ عشق کی حقیقت ہے  
تمہارا آنا، دل کی یہ چھپی سچائی ہے  

جب تک تمہیں جانا نہ تھا، بے حد تھے ہم  
اب تک تمہیں یاد کرنا، میری قسمت ہے  
',
    N'Urdu',
    'Ghazal',
    'Nostalgia'
);

INSERT INTO Poetry (PoetID, Title, Content, Language, PoetryType, Theme)
VALUES (
    7,
    N'یہ پل تمہارا',
    N'یہ پل تمہارا، تمہاری سادگی کی شکل ہے  
یہ تمہاری مسکراہٹ میں جو رنگ ہے، وہ رنگ ہے  

تمہاری ہر ایک نظر، چمک کی مانند ہے  
یہ پل تمہارا، تمہارا جاگتے ہوئے خواب ہیں  
',
    N'Urdu',
    'Ghazal',
    'Romance'
);


INSERT INTO Poetry (PoetID, Title, Content, Language, PoetryType, Theme)
VALUES (
    8,
    N'دیکھ لو',
    N'دیکھ لو، جو رنگ تمہارے ہیں  
وہی رنگ میری آرزو کے ہیں  

یہ جو چاند، یہ جو راتیں  
تمہارے لمس میں چھپی ہیں  
',
    N'Urdu',
    'Ghazal',
    'Love'
);


INSERT INTO Poetry (PoetID, Title, Content, Language, PoetryType, Theme)
VALUES (
    9,
    N'دیکھنا ہے',
    N'دیکھنا ہے تمہاری آنکھوں کی گہرائی  
دیکھنا ہے تمہاری ہر ایک مسکراہٹ کو  
جو دل کی گلیوں میں چھپے ہیں،  
ہمیں ان کا راز معلوم کرنا ہے  
',
    N'Urdu',
    'Nazm',
    'Love'
);


INSERT INTO Poetry (PoetID, Title, Content, Language, PoetryType, Theme)
VALUES (
    10,
    N'اگر تم چاہتے ہو',
    N'اگر تم چاہتے ہو، تو دل کو چوم لو  
اگر تم چاہتے ہو، تمہاری آنکھوں کی خوابوں میں ہم جیتیں  

یہ جو تعلق ہے، وہ تمہاری مسکراہٹوں سے جڑا  
یہ جو دل ہے، وہ تمہارے ہی بناؤ سے نیا ہے  
',
    N'Urdu',
    'Ghazal',
    'Love'
);


INSERT INTO Poetry (PoetID, Title, Content, Language, PoetryType, Theme)
VALUES (
    17,
    N'چاندنی راتیں',
    N'چاندنی راتوں میں، تمہاری مسکراہٹیں  
چاندنی راتوں میں، تمہاری باتیں  
ہماری یادیں، ہم تمہارے دریا میں  
',
    N'Punjabi',
    'Nazm',
    'Romance'
);

INSERT INTO Admin (First_Name, namee, email, Password) VALUES
('minahil','Minahil Azeem','minahil@gmail.com', 'password123'),
('ahmed','Ahmed Baig','ahmed@gmail.com', 'password123'),
('abdullah','Abdullah Qasim','abdullah@gmail.com', 'password123');


INSERT INTO Users (First_Name, Namee, Email, Password, Role) VALUES
('Ali', 'Ali Khan', 'ali@example.com', 'password123', 'User'),
('Sara', 'Sara Ali', 'sara@example.com', 'password123', 'User'),
('Usman', 'Usman Shah', 'usman@example.com', 'password123', 'User');



INSERT INTO UserPreferences (UserID, PoetID) VALUES
(1, 1),
(2, 2),
(3, 3);



INSERT INTO Ratings (UserID, PoetryID, Rating) VALUES (1, 1, 5);  
INSERT INTO Ratings (UserID, PoetryID, Rating) VALUES (2, 2, 4);  
INSERT INTO Ratings (UserID, PoetryID, Rating) VALUES (3, 3, 5); 

INSERT INTO Comments (UserID, PoetryID, CommentText) VALUES (1, 1, 'Beautifully written, full of hope!');  
INSERT INTO Comments (UserID, PoetryID, CommentText) VALUES (2, 2, 'Such a powerful revolutionary message.'); 
INSERT INTO Comments (UserID, PoetryID, CommentText) VALUES (3, 3, 'such deep longing and melancholy in these verses.');  

INSERT INTO Categories (Name) VALUES
('Romantic Poetry'),
('Philosophical Poetry'),
('Revolutionary Poetry');


INSERT INTO PoetryAudio (PoetryID, UserID, AudioFilePath, Duration) VALUES (1, 1, 'audio/iqbal_ghazal.mp3', 180); 
INSERT INTO PoetryAudio (PoetryID, UserID, AudioFilePath, Duration) VALUES (2, 2, 'audio/faiz_nazm.mp3', 210);    
INSERT INTO PoetryAudio (PoetryID, UserID, AudioFilePath, Duration) VALUES (3, 3, 'audio/ghalib_ghazal.mp3', 200);


INSERT INTO PoetryContests (Title, Theme, StartDate, EndDate) VALUES
('Poetry of Love', 'Love and Heartbreak', '2025-04-01', '2025-04-30'),
('Modern Poets Contest', 'Contemporary Issues', '2025-05-01', '2025-05-30');


INSERT INTO ContestEntries (ContestID, UserID, PoetryID) VALUES (1, 1, 1);  
INSERT INTO ContestEntries (ContestID, UserID, PoetryID) VALUES (1, 2, 2); 
INSERT INTO ContestEntries (ContestID, UserID, PoetryID) VALUES (2, 3, 3);  

INSERT INTO ContestVotes (EntryID, UserID) VALUES (1, 1);  
INSERT INTO ContestVotes (EntryID, UserID) VALUES (2, 2); 
INSERT INTO ContestVotes (EntryID, UserID) VALUES (3, 3);  


INSERT INTO UserActivity (UserID, Action) VALUES
(1, 'Login'),
(2, 'Commented on a Poem'),
(3, 'Rated a Poem');


INSERT INTO PoetEvents (PoetID, EventTitle, EventDate, Description) VALUES
(1, 'Iqbal’s Birth Anniversary', '2025-11-09', 'Celebration of Allama Iqbal’s birth anniversary.'),
(3, 'Ghalib’s Death Anniversary', '2025-02-15', 'Remembering Mirza Ghalib on his death anniversary.'),
(2, 'Faiz Ahmed Faiz Memorial', '2025-03-10', 'Paying tribute to Faiz Ahmed Faiz, one of the most influential poets of the 20th century.');

INSERT INTO PoetryTranslations (PoetryID, Language, TranslatedContent) 
VALUES (1, 'English', 'Hope is the dream of those who awaken, and the yearning of hearts that never surrender.');
INSERT INTO PoetryTranslations (PoetryID, Language, TranslatedContent) 
VALUES (2, 'English', 'We will see it, the day promised to us... that day will be written in the book of destiny.');


INSERT INTO Followers (UserID, FollowedUserID) VALUES
(1, 2),
(2, 3),
(3, 1);

select * from Users