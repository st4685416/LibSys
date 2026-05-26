using LibSys.Models;

namespace LibSys.Data
{
    public static class DatabaseSeeder
    {
       private static readonly Random Rnd = new Random();

       public static void Seed()
       {
          using var db = new LibraryContext();

          // 1. ПОВНЕ ОЧИЩЕННЯ ТА СТВОРЕННЯ БАЗИ ДАНИХ
          db.Database.EnsureDeleted();
          db.Database.EnsureCreated();

          // 2. БАЗОВІ НАЛАШТУВАННЯ СИСТЕМИ
          var settings = new SystemSettings
          {
             MaxBooksPerReader = 5,
             ShortLoanDays = 3,
             LongLoanDays = 14,
             BanDaysForShortLoan = 7,
             PenaltyBaseAmount = 50.0m,
             PenaltyPercent = 0.05m,
             NewBookAgeThreshold = 2,
             RareBookCopiesThreshold = 2
          };
          db.SystemSettings.Add(settings);
          db.SaveChanges();

          // 3. ДОВІДНИКИ: ВИДАВНИЦТВА (10 шт)
          var publishers = new[]
          {
             "А-БА-БА-ГА-ЛА-МА-ГА", "Видавництво Старого Лева", "Фоліо", "Клуб Сімейного Дозвілля",
             "Наш Формат", "Основи", "Віват", "Ранок", "Смолоскип", "Фабула"
          }.Select(name => new Publisher { Name = name }).ToList();

          db.Publishers.AddRange(publishers);

          // 4. ДОВІДНИКИ: АВТОРИ (10 шт)
          var authors = new[]
          {
             new Author { LastName = "Шевченко", FirstName = "Тарас", MiddleName = "Григорович" },
             new Author { LastName = "Франко", FirstName = "Іван", MiddleName = "Якович" },
             new Author { LastName = "Костенко", FirstName = "Ліна", MiddleName = "Василівна" },
             new Author { LastName = "Жадан", FirstName = "Сергій", MiddleName = "Вікторович" },
             new Author { LastName = "Андрухович", FirstName = "Юрій", MiddleName = "Ігорович" },
             new Author { LastName = "Забужко", FirstName = "Оксана", MiddleName = "Стефанівна" },
             new Author { LastName = "Кідрук", FirstName = "Макс", MiddleName = "Іванович" },
             new Author { LastName = "Дереш", FirstName = "Любко", Pseudonym = "Любко Дереш" },
             new Author { LastName = "Кокотюха", FirstName = "Андрій", MiddleName = "Анатолійович" },
             new Author { LastName = "Матіос", FirstName = "Оксана", MiddleName = "Стефанівна" }
          };
          db.Authors.AddRange(authors);

          // 5. ДОВІДНИКИ: ТЕГИ ТА ЖАНРИ (10 шт)
          var tags = new[]
          {
             "Класична література", "Сучасна українська проза", "Поезія", "Фантастика",
             "Історичний роман", "Детектив", "Трилер", "Біографія", "Публіцистика", "Дитяча література"
          }.Select(name => new Tag { Name = name }).ToList();

          db.Tags.AddRange(tags);

          // 6. ЧИТАЧІ (25 шт)
          var firstNamesM = new[] { "Олександр", "Андрій", "Дмитро", "Максим", "Сергій", "Іван", "Василь", "Олексій", "Михайло", "Богдан", "Юрій", "Тарас", "Роман" };
          var middleNamesM = new[] { "Олександрович", "Андрійович", "Дмитрович", "Максимович", "Сергійович", "Іванович", "Васильович", "Олексійович", "Михайлович", "Юрійович", "Тарасович" };
          
          var firstNamesF = new[] { "Марія", "Анна", "Олена", "Юлія", "Вікторія", "Тетяна", "Катерина", "Наталія", "Ірина", "Світлана", "Оксана", "Дарина", "Анастасія" };
          var middleNamesF = new[] { "Олександрівна", "Андріївна", "Дмитрівна", "Максимівна", "Сергіївна", "Іванівна", "Василівна", "Олексіївна", "Михайлівна", "Юріївна", "Тарасівна" };
          
          var lastNames = new[] { "Коваленко", "Бондаренко", "Ткаченко", "Кравченко", "Бойко", "Шевчук", "Гончар", "Мельник", "Лисенко", "Поліщук", "Козловський", "Марченко", "Ковальчук" };

          // Алфавіт для генерації пошти
          const string chars = "abcdefghijklmnopqrstuvwxyz";

          var readers = new List<Reader>();
          for (int i = 1; i <= 25; i++) 
          {
             bool isMale = Rnd.Next(2) == 0;
             int year = Rnd.Next(1960, 2008);
             int month = Rnd.Next(1, 13);
             int day = Rnd.Next(1, 29);
             var birthDate = new DateTime(year, month, day);
             long birthTimestamp = ((DateTimeOffset)birthDate).ToUnixTimeSeconds();

             // Генерація ПІБ
             string fName = isMale ? firstNamesM[Rnd.Next(firstNamesM.Length)] : firstNamesF[Rnd.Next(firstNamesF.Length)];
             string mName = isMale ? middleNamesM[Rnd.Next(middleNamesM.Length)] : middleNamesF[Rnd.Next(middleNamesF.Length)];
             string lName = lastNames[Rnd.Next(lastNames.Length)];
             
             // Генерація випадкової латини для Email (від 3 до 5 символів)
             string randomLatin = "";
             int prefixLength = Rnd.Next(3, 6);
             for (int j = 0; j < prefixLength; j++)
             {
                 randomLatin += chars[Rnd.Next(chars.Length)];
             }

             // 30% шанс отримати Email
             string email = Rnd.Next(100) < 30 ? $"{randomLatin}{year}@example.com" : null;

             readers.Add(new Reader
             {
                FirstName = fName,
                LastName = lName,
                MiddleName = mName,
                PassportNumber = Rnd.Next(100000000, 999999999).ToString(),
                Phone = $"+380{Rnd.Next(50, 99)}{Rnd.Next(1000000, 9999999)}",
                Email = email,
                BirthDate = birthTimestamp
             });
          }

          db.Readers.AddRange(readers);
          db.SaveChanges();

          // 7. КНИГИ (20 шт)
          var bookTitles = new[]
          {
             "Кобзар", "Захар Беркут", "Тіні забутих предків", "Інтернат", "Драбина",
             "Танго смерті", "Чорний ворон", "Солодка Даруся", "Музей покинутих секретів", "Бот",
             "Жорстоке небо", "Танці з кістками", "Месопотамія", "Депеш Мод",
             "Маруся", "Таємниця козацької шаблі", "Енеїда", "Лексикон таємних знань",
             "Червона зона", "Не озирайся і мовчи"
          };

          var books = new List<Book>();
          for (int i = 0; i < 20; i++)
          {
             var book = new Book
             {
                Title = bookTitles[i],
                PublicationYear = Rnd.Next(1990, 2024),
                Price = Rnd.Next(150, 800),
                PublisherId = publishers[Rnd.Next(publishers.Count)].Id
             };

             int authorsCount = Rnd.Next(1, 3);
             for (int a = 0; a < authorsCount; a++)
             {
                var author = authors[Rnd.Next(authors.Length)];
                if (!book.Authors.Contains(author)) book.Authors.Add(author);
             }

             int tagsCount = Rnd.Next(1, 4);
             for (int t = 0; t < tagsCount; t++)
             {
                var tag = tags[Rnd.Next(tags.Count)];
                if (!book.Tags.Contains(tag)) book.Tags.Add(tag);
             }

             books.Add(book);
          }

          db.Books.AddRange(books);
          db.SaveChanges();

          // 8. ПРИМІРНИКИ КНИГ – ДАТА НАДХОДЖЕННЯ В МЕЖАХ ОСТАННІХ 2 РОКІВ
          var copies = new List<BookCopy>();
          DateTime twoYearsAgo = DateTime.Today.AddYears(-2);
          foreach (var book in books)
          {
             int copyCount = Rnd.Next(3, 6);
             for (int i = 0; i < copyCount; i++)
             {
                int daysOffset = Rnd.Next(0, (DateTime.Today - twoYearsAgo).Days);
                DateTime arrival = twoYearsAgo.AddDays(daysOffset);
                copies.Add(new BookCopy
                {
                   BookId = book.Id,
                   ArrivalDate = arrival,
                   StateScore = Rnd.Next(60, 101),
                   Status = BookCopyStatus.Available
                });
             }
          }

          db.BookCopies.AddRange(copies);
          db.SaveChanges();

          // 9. ЖУРНАЛ ОПЕРАЦІЙ – ДАТИ В МЕЖАХ ОСТАННІХ 2 РОКІВ
          var copyAvailableFrom = copies.ToDictionary(c => c.Id, c => c.ArrivalDate);
          DateTime startRange = twoYearsAgo;
          DateTime endRange = DateTime.Today;

          for (int i = 0; i < 200; i++)
          {
             var reader = readers[Rnd.Next(readers.Count)];
             var copy = copies[Rnd.Next(copies.Count)];

             if (copy.Status == BookCopyStatus.Lost ||
                 copy.Status == BookCopyStatus.WrittenOff ||
                 copy.Status == BookCopyStatus.Loaned)
                continue;

             DateTime lastAvailable = copyAvailableFrom[copy.Id];
             if (lastAvailable == DateTime.MaxValue) continue;

             // Дата видачі: від max(lastAvailable, startRange) до endRange
             DateTime minIssue = lastAvailable > startRange ? lastAvailable : startRange;
             if (minIssue >= endRange) continue;

             int maxDays = (endRange - minIssue).Days;
             if (maxDays <= 0) continue;

             int issueOffset = Rnd.Next(1, maxDays + 1);
             DateTime issueDate = minIssue.AddDays(issueOffset);
             if (issueDate >= DateTime.Today) continue;

             DateTime expectedReturn = issueDate.AddDays(14);
             DateTime actualReturn = issueDate.AddDays(Rnd.Next(5, 28));

             var loan = new Loan
             {
                ReaderId = reader.Id,
                BookCopyId = copy.Id,
                IssueDate = issueDate,
                ExpectedReturnDate = expectedReturn,
                FixedPrice = copy.Book.Price,
                FixedPenaltyBaseAmount = 1.0m,
                FixedPenaltyPercent = 0.03m
             };

             int outcome = Rnd.Next(100);

             copy.Status = BookCopyStatus.Loaned;

             if (outcome < 80) // повернення
             {
                if (actualReturn < DateTime.Today)
                {
                   loan.ActualReturnDate = actualReturn;
                   loan.IsLost = false;

                   if (actualReturn > expectedReturn)
                      loan.IsLossPaid = Rnd.Next(100) <= 80; 
                   else
                      loan.IsLossPaid = true;

                   copy.Status = BookCopyStatus.Available;
                   copyAvailableFrom[copy.Id] = actualReturn;
                }
                else
                {
                   copyAvailableFrom[copy.Id] = DateTime.MaxValue;
                }
             }
             else if (outcome < 90) // втрата
             {
                if (actualReturn < DateTime.Today)
                {
                   loan.ActualReturnDate = actualReturn;
                   loan.IsLost = true;
                   loan.IsLossPaid = Rnd.Next(100) <= 80; 
                   copy.Status = BookCopyStatus.Lost;
                   copyAvailableFrom[copy.Id] = DateTime.MaxValue;
                }
             }
             else // активна видача без повернення
             {
                loan.ActualReturnDate = null;
                copyAvailableFrom[copy.Id] = DateTime.MaxValue;
             }

             db.Loans.Add(loan);
          }

          db.SaveChanges();
       }
    }
}