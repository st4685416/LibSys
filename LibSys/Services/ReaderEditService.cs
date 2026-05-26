using Microsoft.EntityFrameworkCore;
using LibSys.Data;
using LibSys.Models;

namespace LibSys.Services
{
    public interface IReaderEditService
    {
        Task<Reader?> GetReaderByIdAsync(int readerId);
        Task SaveReaderAsync(Reader reader);
        Task<bool> IsPassportUniqueAsync(string passportNumber, int? excludeReaderId);
        Task<OperationResult> ValidateReaderDataAsync(Reader reader);
    }

    public class ReaderEditService : IReaderEditService
    {
        public async Task<Reader?> GetReaderByIdAsync(int readerId)
        {
            using var db = new LibraryContext();
            return await db.Readers.FindAsync(readerId);
        }

        public async Task SaveReaderAsync(Reader reader)
        {
            using var db = new LibraryContext();
            if (reader.Id == 0)
                await db.Readers.AddAsync(reader);
            else
                db.Entry(reader).State = EntityState.Modified;
            await db.SaveChangesAsync();
        }

        public async Task<bool> IsPassportUniqueAsync(string passportNumber, int? excludeReaderId)
        {
            using var db = new LibraryContext();
            return !await db.Readers.AnyAsync(r => r.PassportNumber == passportNumber && r.Id != excludeReaderId);
        }

        public Task<OperationResult> ValidateReaderDataAsync(Reader reader)
        {
            var messages = new List<string>();

            if (string.IsNullOrWhiteSpace(reader.LastName) || string.IsNullOrWhiteSpace(reader.FirstName))
                messages.Add("Прізвище та Ім'я є обов'язковими.");

            string namePattern = @"^[a-zA-Zа-яА-ЯіїєґІЇЄҐ' \-]+$";
            if (!System.Text.RegularExpressions.Regex.IsMatch(reader.LastName ?? "", namePattern) ||
                !System.Text.RegularExpressions.Regex.IsMatch(reader.FirstName ?? "", namePattern))
                messages.Add("Прізвище та ім'я можуть містити лише літери, пробіли, дефіси та апострофи.");

            if (!System.Text.RegularExpressions.Regex.IsMatch(reader.PassportNumber ?? "", @"^[a-zA-Zа-яА-ЯіїєґІЇЄҐ0-9]{5,14}$"))
                messages.Add("Некоректний формат паспорта/ID-картки. Допускаються лише літери та цифри (від 5 до 14 символів).");

            if (!System.Text.RegularExpressions.Regex.IsMatch(reader.Phone ?? "", @"^\+?\d{10,13}$"))
                messages.Add("Некоректний формат телефону! Допускаються лише цифри та опціональний знак '+' (10-13 символів).");

            if (!string.IsNullOrWhiteSpace(reader.Email) &&
                !System.Text.RegularExpressions.Regex.IsMatch(reader.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                messages.Add("Некоректний формат Email адреси!");

            if (messages.Count > 0)
                return Task.FromResult(OperationResult.Failure(string.Join("\n", messages)));

            return Task.FromResult(OperationResult.Success());
        }
    }
}