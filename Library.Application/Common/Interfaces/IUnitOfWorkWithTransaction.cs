using GenericRepository;
using TS.Result;

namespace Library.Application.Common.Interfaces;

    /// <summary>
    /// Tek bir DbContext (scope) üzerinde transaction yönetimi ve
    /// SaveChanges yardımcılarını sunar.
    /// </summary>
    public interface IUnitOfWorkWithTransaction
    {
        /// <summary>Yeni bir transaction başlatır (zaten açıksa no-op).</summary>
        Task BeginTransactionAsync(CancellationToken cancellationToken = default);

        /// <summary>Açık transaction'ı commit eder ve dispose eder.</summary>
        Task CommitTransactionAsync(CancellationToken cancellationToken = default);

        /// <summary>Açık transaction'ı rollback eder ve dispose eder.</summary>
        Task RollbackTransactionAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Verilen işi transaction içinde çalıştırır; başarıda commit,
        /// hatada rollback yapar ve sonucu döner.
        /// </summary>
        Task<T> ExecuteInTransactionAsync<T>(
            Func<CancellationToken, Task<T>> action,
            CancellationToken cancellationToken = default);

        /// <summary>Değişiklikleri veritabanına yazar.</summary>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        /// <summary>Değişiklikleri veritabanına yazar (senkron).</summary>
        int SaveChanges();

        /// <summary>SaveChanges sonucunu bool olarak döner.</summary>
        Task<bool> SaveChangesAndReturnSuccessAsync(CancellationToken cancellationToken = default);

        /// <summary>SaveChanges sonucunu TS.Result ile döner.</summary>
        Task<Result<bool>> SaveChangesAsResultAsync(CancellationToken cancellationToken = default);
    }
