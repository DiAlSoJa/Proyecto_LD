using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using LD.Application.Common.Interfaces.StandarLabel;
using LD.Domain.Entities;
using LD.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LD.Infrastructure.Services.StandardLabel
{
    public class StandardLabelService : IStandarIdService
    {
        private readonly LdProyectDbContext _context;

        public StandardLabelService(LdProyectDbContext context)
        {
            _context = context;
        }

        public async Task<Domain.Entities.StandardLabel?> GetByStandarIdStrAsync(string standarIdStr)
        {
            if (string.IsNullOrWhiteSpace(standarIdStr))
                return null;

            var normalized = standarIdStr.Trim();

            return await _context.StandardLabels
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.StandarIdStr == normalized);
        }

        public async Task<bool> IsStandarIdAssignedAsync(int standarId)
        {
            return await _context.AsnReceiptDetails
                .AsNoTracking()
                .AnyAsync(x => x.StandardId == standarId);
        }
     
        public async Task<List<string>> GenerateStandarIdsAsync(int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("La cantidad debe ser mayor a cero.");

            var now = DateTime.Now;
            var sequenceDate = now.Date;

            await using var transaction = await _context.Database.BeginTransactionAsync(System.Data.IsolationLevel.Serializable);

            try
            {
                var sequence = await _context.StandarIdSequences
                    .FirstOrDefaultAsync(x => x.SequenceDate == sequenceDate);

                if (sequence == null)
                {
                    sequence = new StandarIdSequence
                    {
                        SequenceDate = sequenceDate,
                        LastNumber = 0,
                        LastUpdatedAt = now
                    };

                    _context.StandarIdSequences.Add(sequence);
                    await _context.SaveChangesAsync();
                }

                var startNumber = sequence.LastNumber + 1;
                var endNumber = sequence.LastNumber + quantity;

                sequence.LastNumber = endNumber;
                sequence.LastUpdatedAt = now;

                var result = new List<string>();
                var labels = new List<Domain.Entities.StandardLabel>();

                for (int i = startNumber; i <= endNumber; i++)
                {
                    var standarIdStr = $"{now:yyyyMMdd}{i:0000}";
                    result.Add(standarIdStr);
                    labels.Add(new Domain.Entities.StandardLabel
                    {
                        StandarIdStr = standarIdStr,
                        CreatedAt = now,
                        CreatedByUserId = "system",
                        IsActive = true
                    });
                }

                _context.StandardLabels.AddRange(labels);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return result;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        async Task<List<Domain.Entities.StandardLabel>> IStandarIdService.AssignStandarIdsToReceiptDetailsAsync(List<int> asnReceiptDetailIds, string userId)
        {
            if (asnReceiptDetailIds == null || !asnReceiptDetailIds.Any())
                throw new ArgumentException("No se recibieron líneas para asignar StandarId.");
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentException("No se recibió el usuario para asignar StandarId.");

            var now = DateTime.Now;
            var sequenceDate = now.Date;

            await using var transaction = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable);

            try
            {
                var details = await _context.AsnReceiptDetails
                    .Include(x => x.AsnDetail)
                    .ThenInclude(x => x!.Asn)
                    .Where(x => asnReceiptDetailIds.Contains(x.AsnReceiptDetailId))
                    .OrderBy(x => x.AsnReceiptDetailId)
                    .ToListAsync();

                if (details.Count != asnReceiptDetailIds.Count)
                    throw new Exception("Una o más líneas no existen.");

                if (details.Any(x => x.StandardId != null))
                    throw new Exception("Una o más líneas ya tienen StandarId asignado.");

                var sequence = await _context.StandarIdSequences
                    .FirstOrDefaultAsync(x => x.SequenceDate == sequenceDate);

                if (sequence == null)
                {
                    sequence = new StandarIdSequence
                    {
                        SequenceDate = sequenceDate,
                        LastNumber = 0,
                        LastUpdatedAt = now
                    };

                    _context.StandarIdSequences.Add(sequence);
                    await _context.SaveChangesAsync();
                }

                int quantity = details.Count;
                int startNumber = sequence.LastNumber + 1;
                int endNumber = sequence.LastNumber + quantity;

                sequence.LastNumber = endNumber;
                sequence.LastUpdatedAt = now;

                var standarIds = new List<Domain.Entities.StandardLabel>();
                int current = startNumber;

                foreach (var detail in details)
                {
                    if (detail.AsnDetail?.Asn == null)
                        throw new Exception("No se encontró el ASN relacionado para una o más líneas.");

                    var standarId = new Domain.Entities.StandardLabel
                    {
                        StandarIdStr = $"{now:yyyyMMdd}{current:0000}",
                        PartNumber = detail.PartNumber,
                        clientId = detail.AsnDetail.Asn.ClientId,
                        projectId = detail.AsnDetail.Asn.ProjectId,
                        CreatedAt = now,
                        CreatedByUserId = userId,
                        IsActive = true
                    };

                    standarIds.Add(standarId);
                    current++;
                }

                _context.StandardLabels.AddRange(standarIds);
                await _context.SaveChangesAsync();

                for (var i = 0; i < details.Count; i++)
                {
                    details[i].StandardId = standarIds[i].StandarId;
                    details[i].LastModifiedAt = now;
                    details[i].LastModifiedByUserId = userId;
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return standarIds;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }

}
